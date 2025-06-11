using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ResetePasswordPage : ContentPage
{

    LoginViewModel userVM = new LoginViewModel();
    public ResetePasswordPage(UserAppViewModel _userApp)
    {
        InitializeComponent();


        userVM.UserName = _userApp.UserName;
        BindingContext = userVM;
        txtPassword.Focus();

    }
}