using T2SLogistics.Helpers;
using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Auth;
using T2SLogistics.View.Home;
using T2SLogistics.View.Popups;
using Mopups.Services;
using System.Globalization;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.Auth
{
    public class LoginPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        ISettingsService _settingsService;
        AuthService AuthService;
        public LoginPageViewModel(INavigationService navigationService, IServiceProvider services,
            ISettingsService settingsService) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = services;
            _settingsService = settingsService;
            AuthService = _services.GetService<AuthService>();
            LoginCommand = new Command(ExecuteLoginCommand);
            SelectLanguageCommand = new Command(ExecuteSelectLanguageCommand);

            ConfigPopupCommand = new Command(ExecuteConfigPopupCommand);
            FirstAccessCommand = new Command(ExecuteFirstAccessCommand);
            Day = DateTime.Now.DayOfWeek.ToString();
            Date = DateTime.Now.ToString("dd MMMM yyyy");
            AppVersion = AppInfo.VersionString;
            // Pré-marca a caixa com a última escolha do operador.
            RememberMe = _settingsService.RememberMe;
#if DEBUG
     

            Email = "suporte@t2s.pt";
            Password = "T2S#2025!";
#endif
            if (!string.IsNullOrEmpty(settingsService.Applanguage))
            {
                if (settingsService.Applanguage == "en-US")
                {
                    SelectedLanguage = "US (English)";
                    SelectedLanguageFlag = "en_english.png";

                }
                else if (settingsService.Applanguage == "pt-PT")
                {
                    SelectedLanguage = "Português";
                    SelectedLanguageFlag = "pt_portugues";

                }
            }
            else
            {
                SelectedLanguage = "US (English)";
                SelectedLanguageFlag = "en_english.png";
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }
        public string _day;
        public string Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }
        public string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }
        private string _selectedLanguageFlag;
        public string SelectedLanguageFlag
        {
            get => _selectedLanguageFlag;
            set => SetProperty(ref _selectedLanguageFlag, value);
        }
        private string _appVersion;
        public string AppVersion
        {
            get => _appVersion;
            set => SetProperty(ref _appVersion, value);
        }
        public ICommand ConfigPopupCommand { get; }
        private async void ExecuteConfigPopupCommand()
        {
            var configurationSettingsPopup = new ConfigurationSettingsPopup();
            await MopupService.Instance.PushAsync(configurationSettingsPopup);
        }

        // 1.º acesso: operadores são provisionados SEM password. Esta entrada leva o operador
        // (com o email já escrito no login) ao ecrã de definir password (set-initial-password),
        // sem passar pelo login normal — que falharia por não haver password.
        public ICommand FirstAccessCommand { get; }
        private async void ExecuteFirstAccessCommand()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current?.MainPage?.DisplayAlert("Alert", "Please enter a email", "OK");
                return;
            }
            if (!ValidationHelper.IsValidEmail(Email))
            {
                await Application.Current?.MainPage?.DisplayAlert("Alert", "Please enter a valid email address format.", "OK");
                return;
            }

            await _navigationService.NavigateToPage<ResetNewPasswordPage>(Email);
        }

        public ICommand LoginCommand { get; }
        private async void ExecuteLoginCommand()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current?.MainPage?.DisplayAlert("Alert", "Please enter a email and password", "OK");

            }
            else if (!ValidationHelper.IsValidEmail(Email))
            {
                await Application.Current?.MainPage?.DisplayAlert("Alert", "Please enter a valid email address format.", "OK");
            }
            else
            {

                IsBusy = true;
                try
                {
                    var authModel = new AuthRequestModel
                    {
                        username = Email,
                        password = Password,
                    };
                    var authResponse = await AuthService.Login(authModel);
                    if (authResponse != null)
                    {
                        if (authResponse.mustChangePassword)
                        {
                            IsBusy = false;
                            await Application.Current?.MainPage?.DisplayAlert("Alert", LocalizationResourceManager.Instance["MustChangePasswordPrompt"], "OK");
                            await _navigationService.NavigateToPage<ResetNewPasswordPage>(Email);
                            return;
                        }
                        // O token tem de ser sempre gravado (o RequestProvider lê-o do SettingsService a
                        // cada chamada). O "Lembrar-me" controla é a persistência ENTRE sessões: o arranque
                        // (App.SetMainPage) só mantém a sessão se RememberMe estiver ligado.
                        _settingsService.AuthToken = authResponse.token;
                        _settingsService.RememberMe = RememberMe;
                        _settingsService.Username = Email;
                        _settingsService.Email = Email;
                        // UI nova: após login entra no Shell (menu principal), não na HomePage antiga.
                        Application.Current.MainPage = _services.GetService<T2SLogistics.AppShell>();
                        return;
                    }
                    else
                    {
                        IsBusy = false;
                        await Application.Current?.MainPage?.DisplayAlert("Error", "Unauthorised", "OK");

                    }
                    // Navigate to main page after successful login
                    //await _navigationService.NavigateToPage<HomePage>();
                }
                catch (Exception ex)
                {
                    // Handle login failure (e.g., show an error message)
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
        public ICommand SelectLanguageCommand { get; }
        private async void ExecuteSelectLanguageCommand()
        {
            var selectedOption = await Application.Current?.MainPage?.DisplayActionSheet("", "", "", "English", "português");


            if (selectedOption == "English")
            {
                LocalizationResourceManager.Instance.SetCulture(new CultureInfo("en-US"));
                _settingsService.Applanguage = "en-US";
                SelectedLanguage = "US (English)";
                SelectedLanguageFlag = "en_english.png";
            }
            else if (selectedOption == "português")
            {
                LocalizationResourceManager.Instance.SetCulture(new CultureInfo("pt-PT"));
                _settingsService.Applanguage = "pt-PT";
                SelectedLanguage = "Português";
                SelectedLanguageFlag = "pt_portugues.png";

            }
        }
        public override Task Initialise()
        {
           
            return base.Initialise();
        }
    }
}
