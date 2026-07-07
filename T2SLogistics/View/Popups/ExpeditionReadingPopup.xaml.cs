using Microsoft.Extensions.DependencyInjection;
using Mopups.Pages;
using Mopups.Services;
using T2SLogistics.Models;
using T2SLogistics.Services.Api;
using T2SLogistics.Services.Scanning;
using T2SLogistics.ViewModels;

namespace T2SLogistics.View.Popups;

/// <summary>
/// Modal de leitura de um item na Expedição. Devolve o input da leitura (ou null se cancelado) via
/// <see cref="Result"/>; a orquestração no servidor (arranque da separação + registo) fica no detalhe.
/// </summary>
public partial class ExpeditionReadingPopup : PopupPage
{
    private readonly ExpeditionReadingPopupViewModel _viewModel;
    private readonly TaskCompletionSource<SeparationReadingInput?> _tcs = new();

    /// <summary>Resultado da leitura: input a registar, ou null se o operador cancelou/fechou.</summary>
    public Task<SeparationReadingInput?> Result => _tcs.Task;

    public ExpeditionReadingPopup(OrderLineViewModel line)
    {
        InitializeComponent();

        var api = App.serviceProvider.GetRequiredService<IApiService>();
        var scanner = App.serviceProvider.GetRequiredService<IBarcodeScanner>();
        _viewModel = new ExpeditionReadingPopupViewModel(line, api, scanner);
        _viewModel.CloseRequested += OnCloseRequested;
        BindingContext = _viewModel;
    }

    private void OnCloseRequested(SeparationReadingInput? input)
    {
        if (!_tcs.Task.IsCompleted)
            _tcs.TrySetResult(input);
        MopupService.Instance.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Activate();

        // Foco no campo do código ao abrir (pequeno atraso p/ a folha já estar montada no Android).
        Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(250);
            ProductEntry.Focus();
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Deactivate();
        // Fecho pelo backdrop/back físico sem passar pelos botões → trata como cancelamento.
        if (!_tcs.Task.IsCompleted)
            _tcs.TrySetResult(null);
    }

    // Tocar fora da folha → cancela.
    private void OnBackdropTapped(object? sender, EventArgs e) => _viewModel.CancelCommand.Execute(null);

    // Enter no campo do código → dispara já a validação do produto (DataWedge com sufixo Enter).
    private void OnProductCompleted(object? sender, EventArgs e) => _viewModel.SubmitProductManually();

    // Ao LER o código (o scanner injeta as teclas sem Enter garantido), dispara a validação real
    // ~300ms depois da última tecla — assim a pesquisa arranca sozinha, sem esperar por Enter/botão.
    private CancellationTokenSource? _lookupDebounce;
    private void OnProductTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_viewModel.Expanded)
            return;
        _lookupDebounce?.Cancel();
        var cts = _lookupDebounce = new CancellationTokenSource();
        Dispatcher.Dispatch(async () =>
        {
            try { await Task.Delay(300, cts.Token); }
            catch { return; }
            if (!cts.IsCancellationRequested && !_viewModel.Expanded)
                _viewModel.SubmitProductManually();
        });
    }
}
