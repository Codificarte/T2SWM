using System.Collections.ObjectModel;
using Mopups.Services;
using T2SLogistics.Models;
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
    }
}
