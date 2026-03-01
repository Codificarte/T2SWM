using T2SLogistics.Model;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Popups;
using Microsoft.Extensions.DependencyInjection;
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
    public class OrderDetailsPageViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;

        public OrderDetailsPageViewModel(INavigationService navigationService, IServiceProvider serviceProvider) : base(navigationService)
        {
            AddProductQtyModelCommand = new Command<Operation>(ExecuteAddProductQtyModelCommand);
            ToggleExpandCommand= new Command<Order>(ExecuteToggleExpandCommand);
            _serviceProvider = serviceProvider;
        }
        private PhcOrderModel _phcOrder;
        public PhcOrderModel PhcOrder 
        {
            get=> _phcOrder;
            set => SetProperty(ref _phcOrder, value);
        }
        private ObservableCollection<Order> _ordersList;
        public ObservableCollection<Order> OrdersList
        {
            get => _ordersList;
            set => SetProperty(ref _ordersList, value);
        }
        private int _ordersCount;
        public int OrdersCount
        {
            get => _ordersCount;
            set => SetProperty(ref _ordersCount, value);
        }
        private Order _orderModel;
        public Order OrderModel
        {
            get => _orderModel;
            set => SetProperty(ref _orderModel, value);
        }
        public ICommand ToggleExpandCommand { get; }
        private void ExecuteToggleExpandCommand(Order order)
        {
            order.IsExpanded = !order.IsExpanded;
        }
        public ICommand AddProductQtyModelCommand { get; }
        private async void ExecuteAddProductQtyModelCommand(Operation operations)
        {

            OrderModel= OrdersList.FirstOrDefault(s => s.operations.Contains(operations));

            var insertProductQtyPopup = new InsertProductQtyPopup(_serviceProvider, OrderModel.refProd,
                OrderModel.description,
                OrderModel.stampLinOrderProd, operations.operationName);
            await MopupService.Instance.PushAsync(insertProductQtyPopup);
        }
        override public Task Initialise(object? parameter)
        {
            if (parameter is PhcOrderModel phc)
            {
                PhcOrder=phc;
                OrdersCount=phc.orders?.Count()??0;
                if (phc.orders != null)
                {
                    OrdersList = new ObservableCollection<Order>(phc.orders);

                }
            }
            return Task.CompletedTask;
        }
    }
}
