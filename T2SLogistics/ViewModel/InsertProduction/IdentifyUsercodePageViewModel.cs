using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Orders;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.InsertProduction
{
    public class IdentifyUsercodePageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        ISettingsService _settingsService;

        PhcOrderServices PhcOrderServices;

        public IdentifyUsercodePageViewModel(INavigationService navigationService, IServiceProvider services, 
            ISettingsService settingsService) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = services;
            _settingsService= settingsService;
            PhcOrderServices = _services.GetService<PhcOrderServices>();

            SaveCommand = new Command(ExecuteSaveCommand);
        }
        private string _userCode;
        public string UserCode
        {
            get => _userCode;
            set => SetProperty(ref _userCode, value);
        }
        public ICommand SaveCommand { get; }
        public async void ExecuteSaveCommand()
        {
            if (string.IsNullOrEmpty(UserCode))
            {
                await App.Current.MainPage.DisplayAlert("Error", "User code is required", "OK");
                return;
            }
            IsBusy = true;
            var phcOrders = await PhcOrderServices.GetPhcOrdersByUserCode(UserCode);
            if (phcOrders != null)
            {
                if (phcOrders.Count==0)
                {
                    IsBusy = false;

                    await App.Current.MainPage.DisplayAlert("Error", "No orders found for the provided user code.", "OK");
                    return;
                }
                _settingsService.UserCode = UserCode;
                await MopupService.Instance.PopAsync();


                await _navigationService.NavigateToPage<OrdersPage>(phcOrders);
            }
            IsBusy = false;


        }
        public override Task Initialise()
        {
            IsBusy = false;

            return base.Initialise();

        }
    }
}
