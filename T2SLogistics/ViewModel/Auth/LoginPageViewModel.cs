using T2SLogistics.Helpers;
using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Auth;
using T2SLogistics.View.Home;
using T2SLogistics.View.Popups;
using Mopups.Services;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Day = DateTime.Now.DayOfWeek.ToString();
            Date = DateTime.Now.ToString("dd MMMM yyyy");
            AppVersion = AppInfo.VersionString;
#if DEBUG
            //Email = "ddulla@t2s.pt";
            //Password = "Dev1#2025!";

            Email = "fabrica1@durodesigners.com";
            Password = "DuroD#2025!";
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
                        email = Email,
                        password = Password,
                    };
                    var authResponse = await AuthService.Login(authModel);
                    if (authResponse != null)
                    {
                        if (authResponse.mustChangePassword)
                        {
                            IsBusy = false;
                            await Application.Current?.MainPage?.DisplayAlert("Alert", authResponse.message, "OK");
                            await _navigationService.NavigateToPage<ResetNewPasswordPage>(Email);
                            return;
                        }
                        _settingsService.AuthToken = authResponse.token;
                        _settingsService.Username = authResponse.username;
                        _settingsService.Email = authResponse.email;
                        Application.Current.MainPage =new NavigationPage(_services.GetService<HomePage>());
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
