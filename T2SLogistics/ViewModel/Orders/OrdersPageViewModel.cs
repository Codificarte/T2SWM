using CommunityToolkit.Maui.Core.Extensions;
using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.InsertProduction;
using T2SLogistics.View.Orders;
using T2SLogistics.View.Popups;
using Microsoft.Maui.Graphics.Text;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.Orders
{
    public class OrdersPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        ISettingsService _settingsService;
        PhcOrderServices PhcOrderServices;

        public OrdersPageViewModel(INavigationService navigationService, IServiceProvider services,
            ISettingsService settingsService) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = services;
            _settingsService = settingsService;
            PhcOrderServices = _services.GetService<PhcOrderServices>();

            ViewProductCommand = new Command<PhcOrderModel>(ExecuteViewProductCommand);
            SelectProductOperationCommand = new Command<Order>(ExecuteSelectProductOperation);
        }
        private List<PhcOrderModel> phcOrders = new List<PhcOrderModel>();

        private ObservableCollection<PhcOrderModel> _ordersList;
        public ObservableCollection<PhcOrderModel> OrdersList
        {
            get => _ordersList;
            set => SetProperty(ref _ordersList, value);
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
        private bool _viewOrderDetailsVisibility = false;
        public bool ViewOrderDetailsVisibility
        {
            get => _viewOrderDetailsVisibility;
            set => SetProperty(ref _viewOrderDetailsVisibility, value);
        }
        private List<Order> productOrders = new List<Order>();

        private ObservableCollection<Order> _viewProductOrders;
        public ObservableCollection<Order> ViewProductOrders
        {
            get => _viewProductOrders;
            set => SetProperty(ref _viewProductOrders, value);
        }
        private string _productQty;
        public string ProductQty
        {
            get => _productQty;
            set => SetProperty(ref _productQty, value);
        }

        public string _searchOrderName;
        public string SearchOrderName
        {
            get => _searchOrderName;
            set
            {
                SetProperty(ref _searchOrderName, value);
                ExecuteSearchOrderListCommand();
            }
        }
        public string _searchProductOrderName;
        public string SearchProductOrderName
        {
            get => _searchProductOrderName;
            set
            {
                SetProperty(ref _searchProductOrderName, value);
                ExecuteSearchProductOrderList();
                //if (string.IsNullOrEmpty(SearchProductOrderName))
                //    SearchProductOrderListCommand.Execute(null);
            }
        }

        public ICommand ViewProductCommand { get; }
        private async void ExecuteViewProductCommand(PhcOrderModel phcOrderModel)
        {
            //ViewOrderDetailsVisibility = true;
            //if (phcOrderModel != null)
            //{
            //    if (phcOrderModel.orders.Count > 0)
            //    {
            //        ViewProductOrders = new ObservableCollection<Order>(phcOrderModel.orders);
            //        productOrders = phcOrderModel.orders;
            //    }
            //}
            await _navigationService.NavigateToPage<OrderDetailsPage>(phcOrderModel);
        }
        public ICommand SearchOrderListCommand => new Command(ExecuteSearchOrderListCommand);
        private void ExecuteSearchOrderListCommand()
        {
            IsBusy = true;
            ObservableCollection<PhcOrderModel> sortedOrderList = new ObservableCollection<PhcOrderModel>();
            if (phcOrders.Count != 0 && !string.IsNullOrEmpty(SearchOrderName))
            {
                if (int.TryParse(SearchOrderName, out int n))
                {
                    sortedOrderList = phcOrders.Where(orderItem => orderItem.id == n).ToObservableCollection();
                }
                else
                {
                    sortedOrderList = phcOrders.Where(orderItem => orderItem.familyProduct.IndexOf(SearchOrderName,
                  StringComparison.OrdinalIgnoreCase) >= 0).ToObservableCollection();
                }


                if (sortedOrderList.Count > 0)
                {
                    OrdersList = sortedOrderList;
                }
                else
                {
                    OrdersList.Clear();
                }
            }
            else
            {
                OrdersList = phcOrders.ToObservableCollection();
            }
            IsBusy = false;
        }
        public ICommand SearchProductOrderListCommand => new Command(ExecuteSearchProductOrderList);
        private void ExecuteSearchProductOrderList()
        {
            IsBusy = true;
            if (productOrders.Count != 0 && !string.IsNullOrEmpty(SearchProductOrderName))
            {
                var sortedProductOrderList = productOrders.Where(orderItem => orderItem.description.IndexOf(SearchProductOrderName,
                    StringComparison.OrdinalIgnoreCase) >= 0).ToObservableCollection();
                if (sortedProductOrderList.Count > 0)
                {
                    ViewProductOrders = sortedProductOrderList;
                }
                else
                {
                    ViewProductOrders.Clear();
                }
            }
            else
            {
                ViewProductOrders = productOrders.ToObservableCollection();
            }
            IsBusy = false;
        }
        public ICommand SelectProductOperationCommand { get; }
        private async void ExecuteSelectProductOperation(Order order)
        {
            //string operationName=string.Empty;
            //if (order.operations.Count>0)
            //{
            //    operationName = order.operations.First().operationName;
            //}
            //var insertProductQtyPopup = new InsertProductQtyPopup(_services, order.refProd,order.description,
            //    order.stampLinOrderProd, operationName);
            //await MopupService.Instance.PushAsync(insertProductQtyPopup);         
            await _navigationService.NavigateToPage<ViewProductOperationPage>(order);
        }

        //public override async Task Initialise()
        //{
        //    IsBusy = true;
        //    Day = DateTime.Now.DayOfWeek.ToString();
        //    Date = DateTime.Now.ToString("dd MMMM yyyy");
        //    phcOrders = await PhcOrderServices.GetPhcOrders();
        //    if (phcOrders != null)
        //    {
        //        if (phcOrders.Count != 0)
        //        {
        //            OrdersList = new ObservableCollection<PhcOrderModel>(phcOrders);

        //        }
        //    }

        //    IsBusy = false;
        //}
        public override async Task Initialise(object? parameter)
        {
            if (parameter is List<PhcOrderModel> phcOrderModels)
            {


                IsBusy = true;
                Day = DateTime.Now.DayOfWeek.ToString();
                Date = DateTime.Now.ToString("dd MMMM yyyy");
                phcOrders = phcOrderModels;
                if (phcOrders != null)
                {
                    if (phcOrders.Count != 0)
                    {
                        OrdersList = new ObservableCollection<PhcOrderModel>(phcOrders);

                    }
                }

                IsBusy = false;
            }
        }
    }
}
