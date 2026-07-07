using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using T2SLogistics.Models;
using T2SLogistics.Services.Api;

namespace T2SLogistics.ViewModels;

/// <summary>
/// Lista reutilizável de encomendas. O módulo (Expedição/Receção/Inventário) chega por query param
/// do Shell e define apenas o título e a ação — o ecrã é o mesmo nos três casos.
/// </summary>
public partial class MovementListViewModel : ViewModelBase, IQueryAttributable
{
    private readonly IApiService _api;
    private LogisticsModuleInfo _info = LogisticsModuleInfo.For(LogisticsModule.Orders);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ClientsBg))]
    [NotifyPropertyChangedFor(nameof(ClientsText))]
    [NotifyPropertyChangedFor(nameof(SuppliersBg))]
    [NotifyPropertyChangedFor(nameof(SuppliersText))]
    private OrderParty _party = OrderParty.Clients;

    [ObservableProperty]
    private string _query = string.Empty;

    // Estado vazio: distingue "carregou e não há nada / falhou" de "a carregar". Dá feedback + Atualizar.
    [ObservableProperty] private bool _showEmpty;

    public ObservableCollection<OrderListItemViewModel> Orders { get; } = new();

    public Color ClientsBg => Res.Color(Party == OrderParty.Clients ? "BrandRed" : "CardBg");
    public Color ClientsText => Party == OrderParty.Clients ? Colors.White : Res.Color("TextMuted");
    public Color SuppliersBg => Res.Color(Party == OrderParty.Suppliers ? "BrandRed" : "CardBg");
    public Color SuppliersText => Party == OrderParty.Suppliers ? Colors.White : Res.Color("TextMuted");

    public MovementListViewModel(IApiService api) => _api = api;

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("module", out var module))
            _info = LogisticsModuleInfo.Parse(module?.ToString());

        Title = _info.Title;
        await LoadAsync();
    }

    [RelayCommand]
    private Task ShowClients()
    {
        Party = OrderParty.Clients;
        return LoadAsync();
    }

    [RelayCommand]
    private Task ShowSuppliers()
    {
        Party = OrderParty.Suppliers;
        return LoadAsync();
    }

    partial void OnQueryChanged(string value) => _ = LoadAsync();

    private async Task LoadAsync()
    {
        // Snapshot do separador ANTES do await: os dados e os rótulos ("Enc. Cliente/Fornecedor") têm de
        // corresponder ao mesmo tipo. Não usamos IsBusy como bloqueio de reentrância — se o operador troca
        // de separador durante o pedido, deixamos o novo load correr e descartamos o resultado obsoleto.
        var party = Party;

        try
        {
            IsBusy = true;
            ShowEmpty = false;

            var data = await _api.GetOrdersAsync(_info.Module, party);

            // Trocou de separador durante o pedido → resultado obsoleto; o load do separador atual renderiza.
            if (party != Party)
                return;

            var term = Query?.Trim();
            if (!string.IsNullOrEmpty(term))
            {
                data = data.Where(o =>
                    o.Number.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    o.ClientName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            Orders.Clear();
            foreach (var order in data)
            {
                // A rota leva sempre a CHAVE (bostamp) — o detalhe faz o lookup por ela; o nº visível (obrano) fica no cartão.
                var key = order.PhcOrderId;
                Orders.Add(new OrderListItemViewModel(order, party, "Abrir",
                    () => Shell.Current.GoToAsync(
                        $"{Routes.MovementDetail}?module={_info.Module}&party={party}&number={Uri.EscapeDataString(key)}")));
            }

            ShowEmpty = Orders.Count == 0;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>Voltar a carregar (botão do estado vazio / falha de ligação).</summary>
    [RelayCommand]
    private Task Refresh() => LoadAsync();
}
