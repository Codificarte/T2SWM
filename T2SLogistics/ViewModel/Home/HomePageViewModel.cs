using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Auth;
using T2SLogistics.View.Breaks;
using T2SLogistics.View.CustomerOrders;
using T2SLogistics.View.InsertProduction;
using T2SLogistics.View.Orders;
using T2SLogistics.View.OrderSepration;
using T2SLogistics.View.Register;
using T2SLogistics.ViewModel.InsertProduction;
using Microsoft.Extensions.DependencyInjection;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.Home
{
    public class HomePageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        ISettingsService _settingsService;
        IServiceProvider _services;

        public HomePageViewModel(INavigationService navigationService, IServiceProvider services
            , ISettingsService settingsService) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = services;
            _settingsService = settingsService;
            LogoutCommand = new Command(ExecuteLogoutCommand);

            ViewOrdersCommand = new Command(ExecuteViewOrdersCommand);
            InsertProductionCommand = new Command(ExecuteInsertProductionCommand);
            RegisterEntryExitPointCommand=new Command(ExecuteRegisterEntryExitPointCommand);
            OrderSeprationCommand = new Command(ExecuteOrderSeprationCommand);
            ViewExpeditionCommand =new Command(ExecuteViewExpeditionCommand);
            BreaksCommand= new Command(ExecuteBreaksCommand);

        }
        public string _day;
        public string Day
        {
            get=> _day; 
            set=>SetProperty(ref _day, value);
        }
        public string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        private string _clockTimer;

        public string ClockTimer
        {
            get=> _clockTimer;
            set => SetProperty(ref _clockTimer, value);
        }
        public ICommand LogoutCommand { get; }
        private async void ExecuteLogoutCommand()
        {
            IsBusy = true;
            _settingsService.AuthToken = string.Empty;
            _settingsService.Username = string.Empty;
            _settingsService.Email = string.Empty;
            Application.Current.MainPage = new NavigationPage(_services.GetService<LoginPage>());
            IsBusy = false;

        }
        public ICommand RegisterEntryExitPointCommand { get; }
        private async void ExecuteRegisterEntryExitPointCommand()
        {
            await _navigationService.NavigateToPage<RegisterEnteryExitPage>();
        }
        public ICommand ViewOrdersCommand { get; }

        private async void ExecuteViewOrdersCommand()
        {
            //await _navigationService.NavigateToPage<ViewOrdersPage>();
            //await _navigationService.NavigateToPage<IdentifyUsercodePage>();
            IdentifyUsercodePageViewModel identifyUsercodePageViewModel = _services.GetService<IdentifyUsercodePageViewModel>();
            var insertProductQtyPopup = new IdentifyUsercodePage(identifyUsercodePageViewModel);
            await MopupService.Instance.PushAsync(insertProductQtyPopup);

        }
        public ICommand InsertProductionCommand { get; }
        private async void ExecuteInsertProductionCommand()
        {
            await _navigationService.NavigateToPage<OrdersPage>();
        }
        public ICommand ViewExpeditionCommand { get; }
        private async void ExecuteViewExpeditionCommand()
        {
            await _navigationService.NavigateToPage<CustomerOrdersPage>();
        }
        public ICommand OrderSeprationCommand { get; }
        private async void ExecuteOrderSeprationCommand()
        {
            await _navigationService.NavigateToPage<OrderSeprationPage>();
        }
        public ICommand BreaksCommand { get; }
        private async void ExecuteBreaksCommand()
        {
            await _navigationService.NavigateToPage<ProductBreaksPage>();
        }
        public override Task Initialise()
        {
            Day = DateTime.Now.DayOfWeek.ToString();
            Date=DateTime.Now.ToString("MMMM dd, yyyy");
           

            return base.Initialise();
        }
    }
}
