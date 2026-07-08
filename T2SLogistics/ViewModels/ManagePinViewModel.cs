using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using T2SLogistics.Models;
using T2SLogistics.Services.Api;

namespace T2SLogistics.ViewModels;

/// <summary>
/// Definir/Alterar PIN do operador (PDA partilhado). O operador escolhe-se de uma lista; se ainda não
/// tem PIN (<c>MustSetPin</c>) define-o (sem prova extra — risco aceite); senão altera-o exigindo o PIN
/// atual. Distinto da mudança de password.
/// </summary>
public partial class ManagePinViewModel : ViewModelBase
{
    private readonly IApiService _api;

    public ObservableCollection<OperatorSummary> Operators { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasOperator))]
    [NotifyPropertyChangedFor(nameof(IsChanging))]
    [NotifyPropertyChangedFor(nameof(ActionText))]
    private OperatorSummary? _selectedOperator;

    [ObservableProperty] private string _currentPin = string.Empty;
    [ObservableProperty] private string _newPin = string.Empty;
    [ObservableProperty] private string _confirmPin = string.Empty;

    public bool HasOperator => SelectedOperator is not null;
    // Tem PIN → alterar (pede PIN atual). Sem PIN (MustSetPin) → definir.
    public bool IsChanging => SelectedOperator is { MustSetPin: false };
    public string ActionText => IsChanging ? "Alterar PIN" : "Definir PIN";

    public ManagePinViewModel(IApiService api)
    {
        _api = api;
        Title = "PIN do operador";
    }

    public async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            var ops = await _api.GetOperatorsAsync();
            Operators.Clear();
            foreach (var o in ops)
            {
                Operators.Add(o);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Trocar de operador limpa os campos (evita reaproveitar PINs entre operadores).
    partial void OnSelectedOperatorChanged(OperatorSummary? value)
    {
        CurrentPin = string.Empty;
        NewPin = string.Empty;
        ConfirmPin = string.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedOperator is null)
        {
            await Alert("Escolha um operador.");
            return;
        }
        if (!IsNumericPin(NewPin))
        {
            await Alert("O PIN tem de ter 4 a 6 dígitos.");
            return;
        }
        if (NewPin != ConfirmPin)
        {
            await Alert("A confirmação não coincide com o novo PIN.");
            return;
        }
        if (IsChanging && string.IsNullOrEmpty(CurrentPin))
        {
            await Alert("Introduza o PIN atual.");
            return;
        }

        try
        {
            IsBusy = true;
            var result = IsChanging
                ? await _api.ChangePinAsync(SelectedOperator.Id, CurrentPin, NewPin)
                : await _api.SetInitialPinAsync(SelectedOperator.Id, NewPin);

            if (result.Ok)
            {
                await Alert(IsChanging ? "PIN alterado." : "PIN definido.");
                SelectedOperator = null;
                await LoadAsync(); // reflete a mudança de MustSetPin
            }
            else
            {
                await Alert(result.Error ?? "Não foi possível guardar o PIN.");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static bool IsNumericPin(string pin) =>
        !string.IsNullOrEmpty(pin) && pin.Length >= 4 && pin.Length <= 6 && pin.All(char.IsDigit);

    private static Task Alert(string message) =>
        Shell.Current.DisplayAlert("PIN", message, "OK");
}
