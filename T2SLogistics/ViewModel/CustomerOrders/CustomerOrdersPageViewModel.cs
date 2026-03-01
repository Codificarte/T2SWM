using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.CustomerOrders;
using T2SLogistics.View.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.CustomerOrders
{
    public class CustomerOrdersPageViewModel : BaseViewModel
    {
        IServiceProvider _services;
        INavigationService _navigationService;
        private CustomerOrderService customerOrderService;
        public CustomerOrdersPageViewModel(INavigationService navigationService, IServiceProvider services) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = services;
            customerOrderService = _services.GetService<CustomerOrderService>();
        }
        public List<CustomersOrderModel> customersOrders { get; set; }= new List<CustomersOrderModel>();
        private ObservableCollection<CustomersOrderModel> _customersOrders;
        public ObservableCollection<CustomersOrderModel> CustomersOrders
        {
            get => _customersOrders;
            set => SetProperty(ref _customersOrders, value);
        }
        private ObservableCollection<OrderItem> _ordersItems;
        public ObservableCollection<OrderItem> OrdersItems
        {
            get => _ordersItems;
            set => SetProperty(ref _ordersItems, value);
        }
        private string _searchOrderName;
        public string SearchOrderName
        {
            get => _searchOrderName;
            set
            {
                SetProperty(ref _searchOrderName, value);
                SearchCustomerOrderList();
            }
        }
          private string _refEanCode;
        public string RefEanCode
        {
            get => _refEanCode;
            set
            {
                SetProperty(ref _refEanCode, value);
            }
        }
        private bool _viewOrderDetails = false;
        public bool ViewOrderDetails
        {
            get => _viewOrderDetails;
            set => SetProperty(ref _viewOrderDetails, value);
        }
        private async void SearchCustomerOrderList()
        {
            if (!string.IsNullOrEmpty(SearchOrderName) && customersOrders.Count>0)
            {
                var filteredList = customersOrders.Where(co => co.customerName.IndexOf(SearchOrderName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                CustomersOrders = new ObservableCollection<CustomersOrderModel>(filteredList);
            }
            else
            {
                // If the search text is empty, reset the list to the original unfiltered list
               await Initialise();
            }
        }
        public ICommand ExpandOrderItemCommand=>new Command<CustomersOrderModel>(ExecuteExpandOrderItemCommand);
        private void ExecuteExpandOrderItemCommand(CustomersOrderModel customersOrderModel)
        {
            if (customersOrderModel != null)
            {
                customersOrderModel.IsExpanded = !customersOrderModel.IsExpanded;
            }
        }
        public ICommand CustomerOrderDetailCommand=> new Command<CustomersOrderModel>(ExecuteCustomerOrderDetailCommand);
        private async void ExecuteCustomerOrderDetailCommand(CustomersOrderModel customersOrder)
        {
            await _navigationService.NavigateToPage<CustomerOrderDetailPage>(customersOrder);
        }
        public ICommand ViewCustomerOrderCommand => new Command<CustomersOrderModel>(ExecuteViewCustomerOrderCommand);
        private void ExecuteViewCustomerOrderCommand(CustomersOrderModel customersOrderModel)
        {
            if (customersOrderModel != null)
            {
                ViewOrderDetails = true;
                OrdersItems = new ObservableCollection<OrderItem>(customersOrderModel.orderItems);
            }
        }
        public async void CheckIfItemExists()
        {
            if (string.IsNullOrWhiteSpace(RefEanCode))
            {
                return;
            }

            var scannedEanCode = RefEanCode.Trim().Replace(" ", "");

            // Check all order items across all customer orders for a matching eanCode (ignoring spaces)
            var foundItem = CustomersOrders?
                .SelectMany(order => order.orderItems ?? new List<OrderItem>())
                .FirstOrDefault(item => !string.IsNullOrEmpty(item.eanCode) && 
                                       item.eanCode.Replace(" ", "").Equals(scannedEanCode, StringComparison.OrdinalIgnoreCase));

            if (foundItem != null)
            {
                await App.Current.MainPage.DisplayAlert("Item Found", $"An item with EAN Code {RefEanCode.Trim()} exists in the customer orders.", "OK");
            }

            // Clear the scanned code after processing
            RefEanCode = string.Empty;
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
