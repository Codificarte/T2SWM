using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using T2SLogistics.Models;
using T2SLogistics.Services.Api;
using T2SLogistics.Services.Scanning;

namespace T2SLogistics.ViewModels;

/// <summary>
/// Conferência por leitura de uma Receção em curso. Recebe a Receção (id) e a encomenda (number) por query
/// param, mostra previsto-vs-lido e envia cada leitura à API. As regras (lote/validade/alvéolo/quantidade)
/// são validadas no servidor — em recusa, mostra a mensagem devolvida. Não fecha a Receção (passo futuro).
/// </summary>
public partial class ReceptionReadingViewModel : ViewModelBase, IQueryAttributable
{
    private readonly IApiService _api;
    private readonly IBarcodeScanner _scanner;

    private string _receptionId = string.Empty;
    private string _number = string.Empty;

    [ObservableProperty] private string _barcode = string.Empty;
    [ObservableProperty] private int _quantity = 1;
    [ObservableProperty] private string _lote = string.Empty;
    [ObservableProperty] private bool _hasExpiry;
    [ObservableProperty] private DateTime _expiryDate = DateTime.Today;
    [ObservableProperty] private string _alveolo = string.Empty;
    [ObservableProperty] private string _observation = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasStatus))]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StatusColor))]
    private bool _statusIsError;

    public bool HasStatus => !string.IsNullOrWhiteSpace(StatusMessage);
    public Color StatusColor => StatusIsError ? Res.Color("BrandRed") : Res.Color("TextMuted");

    public ObservableCollection<ReceptionLineViewModel> Lines { get; } = new();

    public ReceptionReadingViewModel(IApiService api, IBarcodeScanner scanner)
    {
        _api = api;
        _scanner = scanner;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _receptionId = query.TryGetValue("receptionId", out var r) ? r?.ToString() ?? string.Empty : string.Empty;
        _number = query.TryGetValue("number", out var n)
            ? Uri.UnescapeDataString(n?.ToString() ?? string.Empty)
            : string.Empty;

        Title = $"Receção {_number}";
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var detail = await _api.GetOrderAsync(LogisticsModule.Orders, _number);
            Lines.Clear();
            if (detail is null)
                return;
            foreach (var line in detail.Lines)
                Lines.Add(new ReceptionLineViewModel(new ReceptionExpectedLine
                {
                    ProductRef = line.ProductRef,
                    Description = line.Description,
                    Code = line.Code,
                    Expected = line.Total,
                }));
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>Liga a escuta do scanner (chamado no OnAppearing da página).</summary>
    public void Activate()
    {
        _scanner.BarcodeScanned += OnBarcodeScanned;
        _scanner.Start();
    }

    /// <summary>Desliga a escuta do scanner (chamado no OnDisappearing da página).</summary>
    public void Deactivate()
    {
        _scanner.Stop();
        _scanner.BarcodeScanned -= OnBarcodeScanned;
    }

    private bool _processingScan;

    private void OnBarcodeScanned(string code) =>
        MainThread.BeginInvokeOnMainThread(async () => await HandleScanAsync(code));

    /// <summary>Entrada manual (Enter no campo): trata o código escrito como uma leitura — mesmo
    /// parsing GS1 via API que um scan, em vez de ir cru para o registo.</summary>
    public void SubmitManualScan() =>
        MainThread.BeginInvokeOnMainThread(async () => await HandleScanAsync(Barcode));

    // A App nunca interpreta GS1 localmente (FR-10): reencaminha o payload cru à API e preenche a
    // identificação do Artigo + Lote/Validade conforme devolvido. Cada leitura identifica um novo
    // artigo, por isso os campos derivados (Lote/Validade) são SEMPRE repostos a partir da resposta —
    // não arrastam valores da leitura anterior. Guarda de reentrância evita corridas entre scans
    // sucessivos. Conteúdo malformado → mensagem tratada.
    private async Task HandleScanAsync(string code)
    {
        if (_processingScan || string.IsNullOrWhiteSpace(code))
            return;

        try
        {
            _processingScan = true;

            var parsed = await _api.ParseScanAsync(code.Trim());
            if (parsed is null)
                return;

            if (!parsed.Success)
            {
                SetStatus(parsed.Message ?? "Não foi possível interpretar a leitura.", isError: true);
                return;
            }

            Lote = parsed.Lote ?? string.Empty;
            HasExpiry = parsed.ExpiryDate is not null;
            ExpiryDate = parsed.ExpiryDate?.Date ?? DateTime.Today;
            Barcode = parsed.ArticleCode;

            SetStatus(
                string.IsNullOrWhiteSpace(parsed.ArticleCode) ? "Leitura sem identificação de artigo." : string.Empty,
                isError: string.IsNullOrWhiteSpace(parsed.ArticleCode));
        }
        catch (Exception ex)
        {
            T2SLogistics.Helpers.AppLog.Error(nameof(HandleScanAsync), ex);
        }
        finally
        {
            _processingScan = false;
        }
    }

    [RelayCommand]
    private async Task RecordReading()
    {
        if (IsBusy)
            return;
        if (string.IsNullOrWhiteSpace(Barcode) || Quantity <= 0)
        {
            SetStatus("Código e quantidade (> 0) são obrigatórios.", isError: true);
            return;
        }

        try
        {
            IsBusy = true;

            var input = new ReceptionReadingInput
            {
                Barcode = Barcode.Trim(),
                Quantity = Quantity,
                Lote = string.IsNullOrWhiteSpace(Lote) ? null : Lote.Trim(),
                ExpiryDate = HasExpiry ? ExpiryDate.Date : null,
                Alveolo = string.IsNullOrWhiteSpace(Alveolo) ? null : Alveolo.Trim(),
                Observation = string.IsNullOrWhiteSpace(Observation) ? null : Observation.Trim(),
            };

            var result = await _api.RecordReceptionReadingAsync(_receptionId, input);

            if (!result.Success)
            {
                SetStatus(result.Message ?? "Não foi possível registar a leitura.", isError: true);
                return;
            }

            ApplyToLine(result);
            SetStatus($"✓ Registado: {result.ReceivedQuantity} un. ({result.LineStatus}).", isError: false);
            ResetInputs();
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Atualiza o progresso da linha correspondente pela ref do PHC; se não casar, ignora (a UI não regride).
    private void ApplyToLine(ReceptionReadingResult result)
    {
        var line = Lines.FirstOrDefault(l =>
            string.Equals(l.ProductRef.Trim(), result.ProductRef.Trim(), StringComparison.OrdinalIgnoreCase));
        if (line is not null)
            line.Read = result.ReceivedQuantity;
    }

    private void ResetInputs()
    {
        Barcode = string.Empty;
        Quantity = 1;
        Lote = string.Empty;
        HasExpiry = false;
        ExpiryDate = DateTime.Today;
        Alveolo = string.Empty;
        Observation = string.Empty;
    }

    private void SetStatus(string message, bool isError)
    {
        StatusMessage = message;
        StatusIsError = isError;
    }
}
