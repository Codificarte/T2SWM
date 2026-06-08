using T2SLogistics.ViewModel.Auth;

namespace T2SLogistics.View.Auth;

public partial class ResetNewPasswordPage : ContentPage
{
	public ResetNewPasswordPage(ResetNewPasswordPageViewModel resetNewPasswordPageViewModel)
	{
		InitializeComponent();
		BindingContext = resetNewPasswordPageViewModel;
    }

    private void OnToggleNewPasswordVisibility(object sender, EventArgs e)
    {
        newPasswordEntry.IsPassword = !newPasswordEntry.IsPassword;
        ((Button)sender).ImageSource = newPasswordEntry.IsPassword ? "hide_password.png" : "show_password.png";
    }

    private void OnToggleConfirmPasswordVisibility(object sender, EventArgs e)
    {
        confirmPasswordEntry.IsPassword = !confirmPasswordEntry.IsPassword;
        ((Button)sender).ImageSource = confirmPasswordEntry.IsPassword ? "hide_password.png" : "show_password.png";
    }
}