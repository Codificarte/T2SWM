using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.OrderSepration;
using T2SLogistics.View.Register;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.OrderSepration
{
    public class OrderSeprationPageViewModel : BaseViewModel
    {
        IServiceProvider _services;
        INavigationService _navigationService;
        private CustomerOrderService customerOrderService;
        public OrderSeprationPageViewModel(INavigationService navigationService, IServiceProvider services) : base(navigationService)
        {
            _navigationService=navigationService;
            _services=services;
            customerOrderService = _services.GetService<CustomerOrderService>();

        }
        public List<CustomersOrderModel> customersOrders { get; set; } = new List<CustomersOrderModel>();
        private ObservableCollection<CustomersOrderModel> _customersOrders;
        public ObservableCollection<CustomersOrderModel> CustomersOrders
        {
            get => _customersOrders;
            set => SetProperty(ref _customersOrders, value);
        }
        private string _searchOrderSeprationName;
        public string SearchOrderSeprationName 
        {
            get=> _searchOrderSeprationName;
            set
            {
                SetProperty(ref _searchOrderSeprationName, value);
                SearchOrderSepration();
            }
        }
        private void SearchOrderSepration()
        {
            if (!string.IsNullOrEmpty(SearchOrderSeprationName))
            {
                var filteredList = customersOrders.Where(o => o.customerName.IndexOf(SearchOrderSeprationName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                CustomersOrders = new ObservableCollection<CustomersOrderModel>(filteredList);
            }
            else
            {
                CustomersOrders = new ObservableCollection<CustomersOrderModel>(customersOrders);
            }
        }
        public ICommand OrderSeprationDetailCommand => new Command<CustomersOrderModel>(ExecuteOrderSeprationDetailCommand);
        private async void ExecuteOrderSeprationDetailCommand(CustomersOrderModel customersOrder)
        {
            await _navigationService.NavigateToPage<AskUserIdPage>(customersOrder);
        }
        public override async Task Initialise()
        {
            IsBusy = true;
            customersOrders = await customerOrderService.GetCustomerOrder();
            if (customersOrders != null)
            {
                if (customersOrders.Count > 0)
                {
                    //foreach (var order in customersOrders)
                    //{
                    //    order.ordersQty = order.orderItems.Count;
                    //}
                    CustomersOrders = new ObservableCollection<CustomersOrderModel>(customersOrders);
                }
            }
            IsBusy = false;
        }
    }
}
