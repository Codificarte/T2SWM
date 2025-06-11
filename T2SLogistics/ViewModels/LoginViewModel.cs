using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using T2SLogistics.Services;
using T2SLogistics.Views;

namespace T2SLogistics.ViewModels
{
    public class LoginViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public Command LoginCommand { get; }

        UsersApi _apiServices = new UsersApi();

        public LoginViewModel()
        {
            IconUser = "\ue008";
            IconPassword = "\ue033";

            //LoginCommand = new Command(OnLoginClicked);

        }


        public string IconUser { get; set; }
        public string IconPassword { get; set; }


        private string _userName { get; set; }
        public string UserName
        {
            get => _userName; set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }

        private string _password { get; set; }
        public string Password
        {
            get => _password; set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private string _confirmPassword { get; set; }
        public string ConfirmPassword
        {
            get => _confirmPassword; set
            {
                _confirmPassword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConfirmPassword)));
            }
        }

        private string _message;
        public string Message
        {
            get => _message; set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning; set
            {
                _isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }


        public ICommand LoginCommand
        {

            get
            {
                return new Command(async () =>
                {

                    Message = "";

                    //if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                    if (string.IsNullOrEmpty(UserName))
                    {
                        Message = "Dados inválidos! \n Preencha o utilizador";
                        return;
                    }

                    IsRunning = true;

                    if (Password == null)
                        Password = string.Empty;

                    var _user = await _apiServices.LoginUser(UserName, Password);


                    if (_user.Id == 0 || string.IsNullOrEmpty(_user.Token))
                    {
                        Message = "Dados inválidos!";
                        return;
                    }

                    if (_user.ResetPwd)
                    {

                        IsRunning = false;
                        Helpers.Settings.UserName = UserName;
                        Application.Current.MainPage = new ResetePasswordPage(_user);
                        return;
                    }
                    else if (_user.Token == "OK" && !_user.ResetPwd)
                    {
                        Message = "Bem vindo";

                        Helpers.Settings.UserName = UserName;
                        Helpers.Settings.Password = Password;
                        Helpers.Settings.AccessToken = _user.Token;

                        IsRunning = false;

                        Application.Current.MainPage = new AppShell();
                    }


                    Message = "Não foi possível autenticar!! \n " + _user.Token.ToString();
                    IsRunning = false;

                });
            }

        }

        public ICommand ResetPasswordCommand
        {

            get
            {
                return new Command(async () =>
                {

                    Message = "";

                    if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || (Password != ConfirmPassword))
                    {
                        Message = "Dados inválidos!";
                        return;
                    }

                    IsRunning = true;

                    var _user = await _apiServices.ChangePassword(UserName, Password);

                    if (_user.Id == 0 || string.IsNullOrEmpty(_user.Token) || _user.ResetPwd)
                    {
                        Message = "Dados inválidos!";
                        return;
                    }

                    if (_user.Token == "OK" && !_user.ResetPwd)
                    {
                        Message = "Bem vindo";

                        Helpers.Settings.UserName = UserName;
                        Helpers.Settings.Password = Password;
                        Helpers.Settings.AccessToken = _user.Token;

                        IsRunning = false;

                        Application.Current.MainPage = new AppShell();
                    }
                });
            }

        }



        public async Task<IEnumerable<UserAppViewModel>> GetAllUsersRemoteApi()
        {

            var _apiService = new UsersApi();
            var _usersTask = _apiService.GetRemoteDataAsync();

            var _list = await _usersTask;

            return _list;

        }


        //private async void OnLoginClicked(object obj)
        //      {
        //          // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
        //          //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");  was original
        //          await Shell.Current.GoToAsync($"//{nameof(HomePage)}");

        //      }
    }
}
