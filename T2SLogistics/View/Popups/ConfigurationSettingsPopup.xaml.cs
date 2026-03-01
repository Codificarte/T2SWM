using Mopups.Pages;
using Mopups.Services;

namespace T2SLogistics.View.Popups;

public partial class ConfigurationSettingsPopup : PopupPage
{
	public ConfigurationSettingsPopup()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        apiUrlInput.Text= App.settingsService.BaseUrl;
        base.OnAppearing();
    }
    private async void Save_Clicked(object sender, EventArgs e)
    {
        App.settingsService.BaseUrl= apiUrlInput.Text;
        await MopupService.Instance.PopAsync();
    }
    private void OnClose(object sender, TappedEventArgs e)
    {
        MopupService.Instance.PopAsync();

    }

    private void SetTestUrl_Clicked(object sender, EventArgs e)
    {
        apiUrlInput.Text = "https://devapi.codificarte.pt/DevApiDD/api/";
    }

    private void SetProdUrl_Clicked(object sender, EventArgs e)
    {
        apiUrlInput.Text = "http://durodesigners.createinfor.pt:44301/api/";
    }
}