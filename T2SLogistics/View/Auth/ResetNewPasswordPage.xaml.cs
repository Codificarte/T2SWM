using T2SLogistics.ViewModel.Auth;

namespace T2SLogistics.View.Auth;

public partial class ResetNewPasswordPage : ContentPage
{
	public ResetNewPasswordPage(ResetNewPasswordPageViewModel resetNewPasswordPageViewModel)
	{
		InitializeComponent();
		BindingContext = resetNewPasswordPageViewModel;
    }

}