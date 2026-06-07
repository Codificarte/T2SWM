using Mopups.Pages;
using Mopups.Services;
using T2SLogistics.Helpers;

namespace T2SLogistics.View.Popups;

public partial class ConfigurationSettingsPopup : PopupPage
{
	public ConfigurationSettingsPopup()
	{
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        envPicker.ItemsSource = ApiEnvironments.All;
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

    private void EnvPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (envPicker.SelectedItem is ApiEnvironment env)
            apiUrlInput.Text = env.Url;
    }
}