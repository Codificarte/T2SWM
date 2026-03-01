using CommunityToolkit.Maui.Views;
using T2SLogistics.Model;
using T2SLogistics.ViewModel.Popups;
using Mopups.Pages;
using Mopups.Services;

namespace T2SLogistics.View.Popups;

public partial class IncompleteQuantityInputPopup : PopupPage
{
    IncompleteQuantityInputPopupViewModel incompleteQuantityInputPopupViewModel;
    public IncompleteQuantityInputPopup(IServiceProvider _services)
	{
		InitializeComponent();
		BindingContext = incompleteQuantityInputPopupViewModel = _services.GetService<IncompleteQuantityInputPopupViewModel>();
        incompleteQuantityInputPopupViewModel.Initialise();
        TaskCompletionSource = new TaskCompletionSource<string>();

    }
    private void OnClose(object sender, EventArgs e)
    {
        TaskCompletionSource.SetResult(string.Empty);
        MopupService.Instance.PopAsync();

    }
    public TaskCompletionSource<string> TaskCompletionSource { get; set; }

    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        TaskCompletionSource.SetResult(incompleteQuantityInputPopupViewModel.InputQuantity);
        MopupService.Instance.PopAsync();

    }
}