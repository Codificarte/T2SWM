using T2SLogistics.ViewModel.InsertProduction;
using Mopups.Pages;
using Mopups.Services;

namespace T2SLogistics.View.InsertProduction;

public partial class IdentifyUsercodePage : PopupPage
{
	public IdentifyUsercodePage(IdentifyUsercodePageViewModel identifyUsercodePageViewModel)
	{
		InitializeComponent();
		BindingContext = identifyUsercodePageViewModel;
    }
    protected override void OnAppearing()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            UserIdEntry.Focus();
        });
        base.OnAppearing();
    }
    protected override void OnDisappearing()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            UserIdEntry.Unfocus();
        });
        base.OnDisappearing();

    }
    private void OnClose(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();

    }
   
}