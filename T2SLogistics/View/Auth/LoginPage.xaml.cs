using T2SLogistics.ViewModel.Auth;

namespace T2SLogistics.View.Auth;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();
        BindingContext = loginPageViewModel;
    }
   
    private void OnTogglePasswordVisibility(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;

        var imageButton = (Button)sender;
        imageButton.ImageSource = passwordEntry.IsPassword ? "hide_password.png" : "show_password.png";
    }
}