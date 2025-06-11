using System.Collections.ObjectModel;
using System.Xml.Xsl;
using T2SLogistics.Helpers;
using T2SLogistics.Models;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

public partial class SettingsPage : ContentPage
{
    SettingsViewModel settingsVM = new SettingsViewModel();

    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = settingsVM;

        if (string.IsNullOrEmpty(txtUrlApiBase.Text))
            txtUrlApiBase.Text = "http://192.168.?.?/APIT2S/api/UrlSettings/GetAll";

    }

    private void btnSaveConfig_Clicked(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(txtUrlApiBase.Text))
            GetData(txtUrlApiBase.Text.Trim());

    }

    private async void GetData(string _url)
    {
        var _aTask = settingsVM.GetRemoteDataAsync(_url);
        var _listConfig = await _aTask;


        if (_listConfig.ToList().Count > 0)
            settingsVM.ListSettingsConfig = new ObservableCollection<SettingsConfig>();

        foreach (var item in _listConfig)
            settingsVM.ListSettingsConfig.Add(item);


        if (settingsVM.ListSettingsConfig.Count() <= 0)
            return;

        Settings.UrlApiBase = txtUrlApiBase.Text.Trim();
        Settings.NomeEmpresa = _listConfig.ToList()[0].CustomerName;
        Settings.NifEmpresa = _listConfig.ToList()[0].Nif;
        Settings.UseMvo = _listConfig.ToList()[0].UseMvo;
        Settings.UseAlveolos = _listConfig.ToList()[0].UseAlveolos;
        Settings.ControlaAlvRec = _listConfig.ToList()[0].ControlaAlvRec;
        Settings.SugereAlvRecep = _listConfig.ToList()[0].SugereAlvRecep;

        try { Settings.UrlApiRegisterNewUser = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlRegisterUser).FirstOrDefault().EndPoint; } catch (Exception ex) { string e = ex.Message; }
        try { Settings.UrlApiLogin = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlUserLogin).FirstOrDefault().EndPoint; } catch (Exception ex) { string e = ex.Message; }
        try { Settings.UrlApiForgotPassword = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlForgotPassword).FirstOrDefault().EndPoint; } catch (Exception ex) { string e = ex.Message; }
        try { Settings.UrlApiGetAllUsers = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlGetAllUsers).FirstOrDefault().EndPoint; } catch (Exception ex) { string e = ex.Message; }

        Settings.UrlApiOrders = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlGetEncomendas).FirstOrDefault().EndPoint;
        Settings.UrlApiRegEntradas = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlAddEntradas).FirstOrDefault().EndPoint;
        Settings.UrlApiRegSaidas = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlAddSaidas).FirstOrDefault().EndPoint;
        Settings.UrlApiAlveolos = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlGetAlveolos).FirstOrDefault().EndPoint;
        Settings.UrlApiProducts = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlGetProducts).FirstOrDefault().EndPoint;
        Settings.UrlApiSuppliers = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlGetSuppliers).FirstOrDefault().EndPoint;
        Settings.UrlApiArmazens = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlGetArmazens).FirstOrDefault().EndPoint;
        Settings.UrlApiRegInventarios = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlAddInventarios).FirstOrDefault().EndPoint;

        Settings.UrlApiRecepcaoMercadoria = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlAddRecepcaoMercadoria).FirstOrDefault().EndPoint;
        Settings.UrlApiExpedicaoMercadoria = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlAddExpedicaoMercadoria).FirstOrDefault().EndPoint;
        Settings.UrlApiConferenciasStock = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlConferenciaStock).FirstOrDefault().EndPoint;

        try { Settings.UrlApiPrintLabel = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlPrintLabel).FirstOrDefault().EndPoint; } catch (Exception ex) { string e = ex.Message; }
        try { Settings.UrlApiPrintA4 = settingsVM.ListSettingsConfig.Where(c => c.ConfigId == SettingsConfig.IdUrlPrintA4).FirstOrDefault().EndPoint; } catch (Exception ex) { string e = ex.Message; }

        lstSettings.ItemsSource = settingsVM.ListSettingsConfig;

    }

    protected override bool OnBackButtonPressed()
    {
        App.Current.MainPage.Navigation.PopAsync();
        return true;
    }
}