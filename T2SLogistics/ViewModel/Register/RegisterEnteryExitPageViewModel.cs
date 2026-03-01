using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.OrderSepration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace T2SLogistics.ViewModel.Register
{
    public class RegisterEnteryExitPageViewModel : BaseViewModel
    {
        UseAppTimerService useAppTimerService;
        IServiceProvider _services;
        ISettingsService _settingsService;
        INavigationService _navigationService;
        public RegisterEnteryExitPageViewModel(INavigationService navigationService, 
            IServiceProvider serviceProvider, ISettingsService settingsService) : base(navigationService)
        {
            _navigationService= navigationService;
            _services = serviceProvider;
            _settingsService = settingsService;
            useAppTimerService = _services.GetService<UseAppTimerService>();
            SaveCommand = new Command(ExecuteSaveCommand); 
        }
        public string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        public string _userCode;
        public string UserCode
        {
            get => _userCode;
            set => SetProperty(ref _userCode, value);
        }
        public DateTime _dateRegist;
        public DateTime DateRegist
        {
            get => _dateRegist;
            set => SetProperty(ref _dateRegist, value);
        }
        public ICommand SaveCommand { get; }
        public async void ExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(UserCode))
            {
                await Application.Current?.MainPage?.DisplayAlert("", "Please enter the usercode first", "OK");

                return;
            }

            IsBusy = true;
            CreateUsersAppTimerRequestModel createUsersAppTimerRequestModel = new CreateUsersAppTimerRequestModel
            {
                id=0,
                userName = UserName,
                userCode = UserCode,
                dateRegist = DateRegist
            };
           var status= await useAppTimerService.CreateUserAppTimerAsync(createUsersAppTimerRequestModel);
            if(status.response.Data!=null)
            {
                IsBusy = false;

                await Application.Current?.MainPage?.DisplayAlert("Registo de Ponto",
                    $"{status.response.Data.userName}" + Environment.NewLine+
                    $"{status.response.Data.descrTipoMov}" + Environment.NewLine +
                    $"{status.response.Data.horaMov}", "OK");
               
                    _settingsService.UserCode = UserCode;
                    _settingsService.UserId=status.response.Data.id.ToString();

              
                    await _navigationService.NavigateBack();

               
            }
            else
            {
                IsBusy = false;

                await Application.Current?.MainPage?.DisplayAlert("Error", $"{status.response.Message}", "OK");

            }
            IsBusy = false;

        }
        public override Task Initialise(object parameter)
        {
           UserName= _settingsService.Username;
            DateRegist=DateTime.Now;
          
            return base.Initialise();

        }
    }
}
