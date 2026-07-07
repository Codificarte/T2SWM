using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using T2SLogistics.Models;
using T2SLogistics.Services.Api;
using T2SLogistics.View.Popups;

namespace T2SLogistics.ViewModels;

/// <summary>
/// Detalhe reutilizável de uma encomenda. Tal como a lista, recebe o módulo por query param;
/// só o título e o texto do botão de ação mudam entre Expedição/Receção/Inventário.
/// </summary>
public partial class MovementDetailViewModel : ViewModelBase, IQueryAttributable
{
    private readonly IApiService _api;
    private LogisticsModuleInfo _info = LogisticsModuleInfo.For(LogisticsModule.Orders);
    private OrderParty _party = OrderParty.Clients;
    private string _key = string.Empty; // bostamp — chave interna p/ chamadas à API (nunca mostrada)
    private string _separationId = string.Empty; // criado no 1.º registo (lazy), reutilizado depois

    // Botão de arranque ao fundo: só na Receção (Fornecedores). Na Expedição regista-se por TAP na linha.
    [ObservableProperty] private bool _showStartButton;

    [ObservableProperty] private string _number = string.Empty;
    [ObservableProperty] private string _clientName = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private string _dateText = string.Empty;
    [ObservableProperty] private int _linesCount;
    [ObservableProperty] private string _status = string.Empty;
    [ObservableProperty] private Color _statusBg = Colors.Transparent;
    [ObservableProperty] private Color _statusText = Colors.Transparent;
    [ObservableProperty] private string _actionText = string.Empty;

    public ObservableCollection<OrderLineViewModel> Lines { get; } = new();

    public MovementDetailViewModel(IApiService api) => _api = api;

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("module", out var module))
            _info = LogisticsModuleInfo.Parse(module?.ToString());

        if (query.TryGetValue("party", out var p) &&
            Enum.TryParse<OrderParty>(p?.ToString(), ignoreCase: true, out var party))
            _party = party;

        // Encomendas de Fornecedor → Receção (botão de arranque); Clientes → Expedição (TAP na linha → modal).
        ActionText = "Iniciar receção";
        ShowStartButton = _party == OrderParty.Suppliers;

        // O param "number" da rota é a CHAVE (bostamp) — usada no lookup, não é o nº visível.
        var key = query.TryGetValue("number", out var n)
            ? Uri.UnescapeDataString(n?.ToString() ?? string.Empty)
            : string.Empty;

        await LoadAsync(key);
    }

    private async Task LoadAsync(string key)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var detail = await _api.GetOrderAsync(_info.Module, key);
            if (detail is null)
                return;

            _key = string.IsNullOrEmpty(detail.PhcOrderId) ? key : detail.PhcOrderId;
            var label = OrderLabels.Title(_party, detail.Number); // "Enc. Cliente/Fornecedor Nº {obrano}"
            Title = label;
            Number = label;
            ClientName = detail.ClientName;
            Address = detail.Address;
            DateText = detail.Date.ToString("dd/MM/yyyy");
            LinesCount = detail.Lines.Count;
            Status = StatusVisuals.Label(detail.Status);
            StatusBg = StatusVisuals.Background(detail.Status);
            StatusText = StatusVisuals.Text(detail.Status);

            Lines.Clear();
            foreach (var line in detail.Lines)
                Lines.Add(new OrderLineViewModel(line));
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Só a Receção (Fornecedores) usa o botão de arranque; a Expedição faz-se por TAP na linha.
    [RelayCommand]
    private Task Start() => _party == OrderParty.Suppliers ? StartReceptionAsync() : Task.CompletedTask;

    /// <summary>
    /// Toca numa linha (Expedição): abre o modal de leitura. Se confirmado, cria a separação no 1.º
    /// registo (idempotente no servidor), submete a leitura e atualiza o progresso da linha.
    /// </summary>
    [RelayCommand]
    private async Task OpenLine(OrderLineViewModel? line)
    {
        if (line is null || _party != OrderParty.Clients)
            return;

        var popup = new ExpeditionReadingPopup(line);
        await MopupService.Instance.PushAsync(popup);
        var input = await popup.Result;
        if (input is null)
            return;

        try
        {
            IsBusy = true;

            if (string.IsNullOrEmpty(_separationId))
            {
                var started = await _api.StartSeparationAsync(_key);
                if (started is null)
                {
                    await Shell.Current.DisplayAlert("Expedição", "Não foi possível iniciar a separação desta encomenda.", "OK");
                    return;
                }
                _separationId = started.SeparationId;
            }

            var result = await _api.RecordSeparationReadingAsync(_separationId, input);
            if (!result.Success)
            {
                await Shell.Current.DisplayAlert("Leitura recusada", result.Message ?? "Não foi possível registar a leitura.", "OK");
                return;
            }

            line.Separated = result.SeparatedQuantity;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task StartReceptionAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var started = await _api.StartReceptionAsync(_key);
            if (started is null)
            {
                await Shell.Current.DisplayAlert("Receção", "Não foi possível iniciar a receção desta encomenda.", "OK");
                return;
            }

            // A rota leva a CHAVE (bostamp) — o ecrã de leitura recarrega as linhas por ela e mostra o obrano do detalhe.
            await Shell.Current.GoToAsync(
                $"{Routes.ReceptionReading}?receptionId={Uri.EscapeDataString(started.ReceptionId)}&number={Uri.EscapeDataString(_key)}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
