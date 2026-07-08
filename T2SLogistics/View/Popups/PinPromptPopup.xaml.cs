using System.Linq;
using Mopups.Pages;
using Mopups.Services;

namespace T2SLogistics.View.Popups;

/// <summary>
/// Popup que pede o PIN do operador antes de uma ação (imprimir / abrir leituras). Devolve o PIN
/// introduzido via <see cref="Completion"/>, ou <c>null</c> se cancelado. A validação de negócio
/// (o PIN pertence a um operador) é feita no servidor.
/// </summary>
public partial class PinPromptPopup : PopupPage
{
    public TaskCompletionSource<string?> Completion { get; } = new();

    public PinPromptPopup(string? message = null)
    {
        InitializeComponent();
        if (!string.IsNullOrWhiteSpace(message))
        {
            MessageLabel.Text = message;
            MessageLabel.IsVisible = true;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PinEntry.Focus();
    }

    private async void OnCancel(object? sender, EventArgs e)
    {
        Completion.TrySetResult(null);
        await MopupService.Instance.PopAsync();
    }

    private async void OnConfirm(object? sender, EventArgs e)
    {
        var pin = PinEntry.Text?.Trim() ?? string.Empty;
        if (pin.Length < 4 || pin.Length > 6 || !pin.All(char.IsDigit))
        {
            ErrorLabel.Text = "O PIN tem de ter 4 a 6 dígitos.";
            ErrorLabel.IsVisible = true;
            return;
        }

        Completion.TrySetResult(pin);
        await MopupService.Instance.PopAsync();
    }
}
