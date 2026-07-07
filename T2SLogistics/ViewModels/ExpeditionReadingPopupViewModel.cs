using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using T2SLogistics.Models;
using T2SLogistics.Services.Api;
using T2SLogistics.Services.Scanning;

namespace T2SLogistics.ViewModels;

/// <summary>
/// Modal de leitura de um item na Expedição, aberto ao tocar numa linha do detalhe. Máquina de dois
/// passos: (1) ler o código do produto — QR/GS1 vai à API desdobrar (lote/validade) e valida contra a
/// linha; EAN valida que é o artigo da linha; (2) expandido — lote/alvéolo/quantidade, validados contra
/// a linha antes de devolver o input ao detalhe (que arranca a separação no 1.º registo e submete).
/// </summary>
public partial class ExpeditionReadingPopupViewModel : ObservableObject
{
    private readonly IApiService _api;
    private readonly IBarcodeScanner _scanner;
    private readonly OrderLineViewModel _line;

    public string Description { get; }
    public string ExpectedText { get; }
    public bool RequiresAlveolo { get; }

    [ObservableProperty] private string _barcode = string.Empty;
    [ObservableProperty] private string _lote = string.Empty;
    [ObservableProperty] private int _quantity = 1;
    [ObservableProperty] private string _alveolo = string.Empty;
    [ObservableProperty] private string _expiryText = string.Empty;

    [ObservableProperty] private bool _expanded;          // mostra lote/alvéolo/qtd depois de validar o produto
    [ObservableProperty] private bool _loteEditable = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasStatus))]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StatusColor))]
    private bool _statusIsError;

    public bool HasStatus => !string.IsNullOrWhiteSpace(StatusMessage);
    public Color StatusColor => StatusIsError ? Res.Color("BrandRed") : Res.Color("TextMuted");

    /// <summary>Fecho pedido pela VM: input preenchido = registar; null = cancelar.</summary>
    public event Action<SeparationReadingInput?>? CloseRequested;

    public ExpeditionReadingPopupViewModel(OrderLineViewModel line, IApiService api, IBarcodeScanner scanner)
    {
        _line = line;
        _api = api;
        _scanner = scanner;

        Description = line.Description;
        RequiresAlveolo = !string.IsNullOrWhiteSpace(line.BinLocation);

        var remaining = line.Expected - line.Separated;
        Quantity = remaining > 0 ? remaining : 1;

        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(line.Lote)) parts.Add($"Lote {line.Lote}");
        if (line.Expiry is not null) parts.Add($"Val. {line.Expiry:MM/yyyy}");
        if (!string.IsNullOrWhiteSpace(line.BinLocation)) parts.Add($"Alv. {line.BinLocation}");
        ExpectedText = string.Join("  ·  ", parts);
    }

    public void Activate()
    {
        _scanner.BarcodeScanned += OnScan;
        _scanner.Start();
    }

    public void Deactivate()
    {
        _scanner.Stop();
        _scanner.BarcodeScanned -= OnScan;
    }

    private bool _processing;

    private void OnScan(string code) =>
        MainThread.BeginInvokeOnMainThread(async () => await HandleScanAsync(code));

    /// <summary>
    /// Dispara a pesquisa/validação do PRODUTO com o código atual do campo. Chamado ao ler o código
    /// (auto, sem Enter) e no Enter manual. Só age no passo 1 (por expandir); guarda de reentrância.
    /// </summary>
    public void SubmitProductManually() =>
        MainThread.BeginInvokeOnMainThread(async () => await RunProductAsync(Barcode));

    private async Task RunProductAsync(string code)
    {
        if (_processing || Expanded || string.IsNullOrWhiteSpace(code))
            return;
        try
        {
            _processing = true;
            await HandleProductScanAsync(code.Trim());
        }
        catch (Exception ex)
        {
            T2SLogistics.Helpers.AppLog.Error(nameof(RunProductAsync), ex);
        }
        finally
        {
            _processing = false;
        }
    }

    // Scanner de stream único (hardware/DataWedge Intent): encaminha por estado — produto, depois alvéolo.
    private async Task HandleScanAsync(string code)
    {
        if (_processing || string.IsNullOrWhiteSpace(code))
            return;
        if (!Expanded)
        {
            await RunProductAsync(code);
            return;
        }
        Alveolo = code.Trim(); // no passo expandido, um scan de stream único preenche o alvéolo
    }

    private async Task HandleProductScanAsync(string code)
    {
        var parsed = await _api.ParseScanAsync(code);
        if (parsed is null || !parsed.Success)
        {
            SetStatus(parsed?.Message ?? "Não foi possível interpretar a leitura.", isError: true);
            return;
        }

        var article = parsed.ArticleCode?.Trim() ?? string.Empty;

        // Valida na BASE DE DADOS (PHC): resolve o código de barras → referência do Artigo e confirma que
        // pertence à referência da linha tocada. (bi.codigo nem sempre é o EAN real; a fonte é st.codigo.)
        SetStatus("A validar o código…", isError: false);
        var resolvedRef = await _api.ResolveArticleRefByBarcodeAsync(article);
        if (resolvedRef is null)
        {
            SetStatus("Código não reconhecido na base de dados.", isError: true);
            return;
        }
        if (!string.Equals(resolvedRef, _line.ProductRef.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            SetStatus($"Este código pertence a outro artigo ({resolvedRef}), não a esta linha.", isError: true);
            return;
        }

        Barcode = article;
        if (parsed.IsGs1)
        {
            // QR: desdobra lote/validade e valida o lote contra a linha.
            var lote = parsed.Lote?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(_line.Lote) &&
                !string.Equals(lote, _line.Lote.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                SetStatus("O lote do QR não corresponde ao da encomenda.", isError: true);
                return;
            }
            Lote = string.IsNullOrWhiteSpace(_line.Lote) ? lote : _line.Lote.Trim();
            ExpiryText = parsed.ExpiryDate is DateTime v
                ? v.ToString("MM/yyyy")
                : (_line.Expiry?.ToString("MM/yyyy") ?? string.Empty);
            LoteEditable = false;
        }
        else
        {
            // EAN: sem teclado, pré-preenche o lote esperado da linha (o operador pode ler outro para o
            // substituir — se divergir, a validação recusa). Validade também da linha.
            Lote = _line.Lote?.Trim() ?? string.Empty;
            ExpiryText = _line.Expiry?.ToString("MM/yyyy") ?? string.Empty;
            LoteEditable = true;
        }

        SetStatus(RequiresAlveolo ? "Leia o alvéolo e confirme a quantidade." : "Confirme a quantidade.", isError: false);
        Expanded = true;
    }

    [RelayCommand]
    private void Confirm()
    {
        if (!Expanded)
        {
            SetStatus("Leia primeiro o código do produto.", isError: true);
            return;
        }

        var lote = string.IsNullOrWhiteSpace(Lote) ? null : Lote.Trim();
        var alveolo = string.IsNullOrWhiteSpace(Alveolo) ? null : Alveolo.Trim();

        // Correspondência estrita contra a linha (o servidor re-valida).
        if (!string.IsNullOrWhiteSpace(_line.Lote))
        {
            if (lote is null) { SetStatus("Indique o lote.", isError: true); return; }
            if (!string.Equals(lote, _line.Lote.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                SetStatus("Lote não corresponde à encomenda.", isError: true);
                return;
            }
        }
        if (RequiresAlveolo)
        {
            if (alveolo is null) { SetStatus("Leia o alvéolo.", isError: true); return; }
            if (!string.Equals(alveolo, _line.BinLocation!.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                SetStatus("Alvéolo não corresponde à linha.", isError: true);
                return;
            }
        }
        if (Quantity <= 0) { SetStatus("Quantidade inválida.", isError: true); return; }
        if (Quantity > _line.Expected) { SetStatus("Quantidade acima do pedido.", isError: true); return; }

        // Envia a referência da linha como código: o servidor casa a linha por ref+lote de forma
        // determinística (a linha já foi escolhida pelo TAP), evitando o problema GTIN-14 vs EAN-13.
        var input = new SeparationReadingInput
        {
            Barcode = _line.ProductRef,
            Quantity = Quantity,
            Lote = lote,
            Alveolo = alveolo,
            Observation = null,
        };
        CloseRequested?.Invoke(input);
    }

    [RelayCommand]
    private void IncrementQuantity()
    {
        if (Quantity < _line.Expected)
            Quantity++;
    }

    [RelayCommand]
    private void DecrementQuantity()
    {
        if (Quantity > 1)
            Quantity--;
    }

    [RelayCommand]
    private void Cancel() => CloseRequested?.Invoke(null);

    private void SetStatus(string message, bool isError)
    {
        StatusMessage = message;
        StatusIsError = isError;
    }
}
