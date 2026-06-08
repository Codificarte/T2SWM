using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.Auth
{
    public class ResetNewPasswordPageViewModel : BaseViewModel
    {
        IServiceProvider _services;
        ISettingsService _settingsService;
        AuthService AuthService;
        INavigationService _navigationService;
        public ResetNewPasswordPageViewModel(INavigationService navigationService, IServiceProvider services,
            ISettingsService settingsService) : base(navigationService)
        {
            _navigationService= navigationService;
            _services = services;
            _settingsService = settingsService;
            AuthService = _services.GetService<AuthService>();
            ContinueResetPasswordCommand = new Command(ExecuteContinueResetPasswordCommand);
        }
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        public ICommand ContinueResetPasswordCommand { get; }
        public async void ExecuteContinueResetPasswordCommand()
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Application.Current?.MainPage?.DisplayAlert("Alert", "Please fill in all fields.", "OK");
                return;
            }
            if (NewPassword != ConfirmPassword)
            {
                await Application.Current?.MainPage?.DisplayAlert("Alert", "Passwords do not match.", "OK");
                return;
            }
            else
            {
                IsBusy = true;
                var request = new Model.SetInitialPasswordRequestModel
                {
                    username = Email,
                    newPassword = NewPassword
                };
                var (ok, error) = await AuthService.SetInitialPassword(request);
                if (ok)
                {
                    await Application.Current?.MainPage?.DisplayAlert("Success", LocalizationResourceManager.Instance["SetInitialPasswordSuccess"], "OK");
                    await _navigationService.NavigateBack();
                }
                else
                {
                    // Mostra o motivo real da API quando existe; senão, a mensagem genérica localizada.
                    var message = string.IsNullOrWhiteSpace(error)
                        ? LocalizationResourceManager.Instance["SetInitialPasswordError"]
                        : error;
                    await Application.Current?.MainPage?.DisplayAlert("Error", message, "OK");
                }
                IsBusy = false;

            }

        }
        public override Task Initialise(object? parameter)
        {
            if (parameter is string email)
            {
                Email = email;
            }


            return base.Initialise(parameter);
        }
    }
}
