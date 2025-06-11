using T2SLogistics.ViewModels;

namespace T2SLogistics.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class LoginPage : ContentPage
{
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnResetPassword_Clicked(object sender, EventArgs e)
        {

        }

        private void btnSettings_Clicked(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new SettingsPage());

        }
}