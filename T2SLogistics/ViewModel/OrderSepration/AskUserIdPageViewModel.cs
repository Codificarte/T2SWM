using T2SLogistics.Model;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.OrderSepration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.OrderSepration
{
    public class AskUserIdPageViewModel : BaseViewModel
    {
        ISettingsService _settingsService;
        INavigationService _navigationService;
        public AskUserIdPageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService)
        {
            _navigationService= navigationService;
            _settingsService = settingsService;

        }
        public string _userCode;
        public string UserCode
        {
            get => _userCode;
            set => SetProperty(ref _userCode, value);
        }
        public CustomersOrderModel CustomersOrderModel { get; set; }
        public ICommand SaveCommand => new Command(ExecuteSaveCommand);
        public async void ExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(UserCode))
            {
                await Application.Current?.MainPage?.DisplayAlert("", "Please enter the userid first", "OK");

                return;
            }
            _settingsService.UserId = UserCode;
            _settingsService.UserCode = UserCode;
            await _navigationService.NavigateToPage<OrderSeprationDetailPage>(CustomersOrderModel);

        }
        public override Task Initialise(object? parameter)
        {
            if (parameter is CustomersOrderModel customersOrderModel)
            {
                CustomersOrderModel = customersOrderModel;
            }
            return base.Initialise(parameter);
        }
    }
}
