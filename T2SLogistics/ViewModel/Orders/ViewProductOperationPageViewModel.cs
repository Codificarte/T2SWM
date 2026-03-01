using CommunityToolkit.Maui.Core.Extensions;
using T2SLogistics.Model;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.InsertProduction;
using T2SLogistics.View.Popups;
using T2SLogistics.View.Register;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace T2SLogistics.ViewModel.Orders
{
    public class ViewProductOperationPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _serviceProvider;

        public ViewProductOperationPageViewModel(INavigationService navigationService, IServiceProvider serviceProvider) : base(navigationService)
        {
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            AddProductQtyModelCommand = new Command<Operation>(ExecuteAddProductQtyModelCommand);
            _serviceProvider = serviceProvider;
        }

        private Order _orderModel;
        public Order OrderModel
        {
            get => _orderModel;
            set => SetProperty(ref _orderModel, value);
        }
        private ObservableCollection<Operation> _viewProductOperations;
        public ObservableCollection<Operation> ViewProductOperations
        {
            get => _viewProductOperations;
            set => SetProperty(ref _viewProductOperations, value);
        }
        public string _day;
        public string Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }
        private string _searchOperationInputText;
        public string SearchOperationInputText
        {
            get => _searchOperationInputText;
            set
            {
                SetProperty(ref _searchOperationInputText, value);
                if(string.IsNullOrEmpty(SearchOperationInputText))
                SearchOperationListCommand.Execute(null);
            }
        }
        public string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public ICommand AddProductQtyModelCommand { get; }
        private async void ExecuteAddProductQtyModelCommand(Operation operations)
        {


            var insertProductQtyPopup = new InsertProductQtyPopup(_serviceProvider, OrderModel.refProd,
                OrderModel.description,
                OrderModel.stampLinOrderProd, operations.operationName);
            await MopupService.Instance.PushAsync(insertProductQtyPopup);
        }
        public ICommand SearchOperationListCommand => new Command(ExecuteSearchOperationListCommand);
        private void ExecuteSearchOperationListCommand()
        {
            IsBusy = true;
            if (ViewProductOperations.Count != 0 && !string.IsNullOrEmpty(SearchOperationInputText))
            {
                var sortedOperationList = OrderModel.operations.Where(operationItem => operationItem.operationName.IndexOf(SearchOperationInputText,
                    StringComparison.OrdinalIgnoreCase) >= 0).ToObservableCollection();
                if (sortedOperationList.Count > 0)
                {
                    ViewProductOperations = sortedOperationList;
                }
                else
                {
                    ViewProductOperations.Clear();
                }
            }
            else
            {
                ViewProductOperations = new ObservableCollection<Operation>(OrderModel.operations);
            }
            IsBusy = false;
        }
        public override async Task Initialise(object? parameter)
        {
            if (parameter is Order Order)
            {
                OrderModel = Order;
                if (OrderModel != null)
                {
                    if (OrderModel.operations.Count > 0)
                    {
                        ViewProductOperations = new ObservableCollection<Operation>(OrderModel.operations);
                    }
                    if (OrderModel.operations.Count == 1)
                    {
                        var operations = OrderModel.operations.FirstOrDefault();
                        var insertProductQtyPopup = new InsertProductQtyPopup(_serviceProvider, OrderModel.refProd,
                      OrderModel.description,
                      OrderModel.stampLinOrderProd, operations.operationName);
                        await MopupService.Instance.PushAsync(insertProductQtyPopup);
                    }
                }
            }

            Day = DateTime.Now.DayOfWeek.ToString();
            Date = DateTime.Now.ToString("dd MMMM yyyy");

        }
    }

}
