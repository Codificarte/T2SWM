using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Mopups.Services;
using T2SLogistics.Models;
using T2SLogistics.View.Auth;
using T2SLogistics.View.Popups;

namespace T2SLogistics.ViewModels;

public partial class MainMenuViewModel : ViewModelBase
{
    public ObservableCollection<MenuItemViewModel> Menu { get; } = new();

    public MainMenuViewModel()
    {
        Title = "Menu";

        // Glifos Tabler (ver subset em Resources/Fonts/tabler-icons.ttf). (char) evita problemas de encoding.
        string fileInvoice = ((char)0xEB67).ToString();    // Encomendas
        string clipboardCheck = ((char)0xEA6C).ToString(); // Inventários
        string barcode = ((char)0xEBC6).ToString();        // Artigos
        string settings = ((char)0xEB20).ToString();       // Definições
        string logout = ((char)0xEBA8).ToString();          // Terminar sessão (logout)

        // Encomendas: lista reutilizável com filtro Clientes/Fornecedores (expedição + receção unificadas).
        Menu.Add(new MenuItemViewModel("Encomendas", fileInvoice,
            () => Shell.Current.GoToAsync($"{Routes.Movements}?module={LogisticsModule.Orders}")));

        Menu.Add(new MenuItemViewModel("Inventários", clipboardCheck,
            () => Shell.Current.GoToAsync($"{Routes.Movements}?module={LogisticsModule.Inventory}")));

        // Ainda por construir — placeholder por agora.
        Menu.Add(new MenuItemViewModel("Artigos", barcode,
            () => Shell.Current.DisplayAlert("Artigos", "Ecrã de artigos — brevemente.", "OK")));

        // Definições: abre o mesmo popup de configuração disponível no login (ConfigurationSettingsPopup).
        Menu.Add(new MenuItemViewModel("Definições", settings,
            () => MopupService.Instance.PushAsync(new ConfigurationSettingsPopup())));

        // Terminar sessão: limpa o token/utilizador e volta ao Login (mantém BaseUrl e idioma).
        Menu.Add(new MenuItemViewModel("Terminar sessão", logout, LogoutAsync, isAccent: true));
    }

    private async Task LogoutAsync()
    {
        var confirmar = await Shell.Current.DisplayAlert(
            "Terminar sessão", "Quer terminar a sessão?", "Sim", "Não");
        if (!confirmar)
            return;

        var settings = App.settingsService;
        settings.AuthToken = string.Empty;
        settings.Username = string.Empty;
        settings.Email = string.Empty;
        settings.UserId = string.Empty;
        settings.UserCode = string.Empty;

        // Volta à raiz de Login (a BaseUrl e o idioma persistem para o próximo login).
        var login = App.serviceProvider.GetRequiredService<LoginPage>();
        Application.Current.MainPage = new NavigationPage(login);
    }
}
