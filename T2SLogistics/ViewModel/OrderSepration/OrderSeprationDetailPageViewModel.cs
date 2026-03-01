using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.DbServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Popups;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.OrderSepration
{
    public class OrderSeprationDetailPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        PhcOrderServices _phcOrderServices;
        DbService dbService;
        ISettingsService _settingsService;

        public OrderSeprationDetailPageViewModel(INavigationService navigationService, IServiceProvider services, 
            ISettingsService settingsService) : base(navigationService)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;

            IncompleteCommand = new Command<OrderItem>(ExecuteIncompleteCommand);
            CompleteCommand = new Command<OrderItem>(ExecuteCompleteCommand);
            AddReadOrderSeprationCommand = new Command(ExecuteAddReadOrderSeprationCommand);
            GoBackCommand = new Command(ExecuteGoBackCommand);
            _services = services;
            _phcOrderServices = _services.GetService<PhcOrderServices>();
            dbService = _services.GetService<DbService>();


        }
        private CustomersOrderModel _customersOrder;
        public CustomersOrderModel CustomersOrder
        {
            get => _customersOrder;
            set => SetProperty(ref _customersOrder, value);
        }
        private ObservableCollection<OrderItem> _orderItem;
        public ObservableCollection<OrderItem> OrderItem
        {
            get => _orderItem;
            set => SetProperty(ref _orderItem, value);
        }
        public string PhcOrderId { get; set; }
        public List<AddSeprationItemsRequestModel> addSeprationItemsRequestModels { get; set; }
        private List<OrderItemsSeprationLocalStorageModel> orderItemsSeprationLocalStorageModels { get; set; }
        private OrderItemsSeprationLocalStorageModel OrderItemsSeprationLocal { get; set; }
        public ICommand GoBackCommand { get; }

        private async void ExecuteGoBackCommand()
        {
            _navigationService.RemoveLastFromBackStack();
            await _navigationService.NavigateBack();

        }

        public ICommand AddReadOrderSeprationCommand { get; }
        private async void ExecuteAddReadOrderSeprationCommand()
        {

            var result = await Application.Current?.MainPage?.DisplayActionSheet("Are you confirm to close this sepration?", "Cancel", "OK");
            if (result == "OK")
            {
                await _navigationService.NavigateBack();
            }
            //    IsBusy = true;
            //    List<AddSeprationItemsRequestModel> addSeprationItemsRequestModel = new List<AddSeprationItemsRequestModel>();
            //    foreach (var item in OrderItem)
            //    {
            //        AddSeprationItemsRequestModel addSeprationItems = new AddSeprationItemsRequestModel
            //        {
            //            id = item.id,
            //            phcOrderId = item.customerOrderId == null ? "0" : item.customerOrderId.ToString(),
            //            phcOrderItemLinId = item.phcOrderItemLinId,
            //            productRef = item.productRef,
            //            productDescription = item.productDescription,
            //            //userCode = item.userCode,
            //            //boxNr = item.boxNr,
            //            quantitySet = item.quantity,
            //            quantityPre = item.quantity,
            //            //dateRegist = item.dateRegist,
            //            //eanCode = item.eanCode,
            //        };
            //        addSeprationItemsRequestModel.Add(addSeprationItems);


            //    }

            //    var isSuccess = await _phcOrderServices.AddSeprationItems(addSeprationItemsRequestModel);
            //    if (isSuccess)
            //    {
            //        IsBusy = false;
            //        await Application.Current?.MainPage?.DisplayAlert("Sucess", "Sepration items added sucessfully", "OK");

            //    }
            //    else
            //    {
            //        IsBusy = false;

            //        await Application.Current?.MainPage?.DisplayAlert("Error", "Error has been occured while adding sepration items", "OK");

            //    }
            //}
            //catch (Exception ex)
            //{
            //    IsBusy = false;

            //}
        }
        public ICommand IncompleteCommand { get; }

        private async void ExecuteIncompleteCommand(OrderItem orderItem)
        {
            IsBusy = true;
            var incompleteQuantityInputPopup = new IncompleteQuantityInputPopup(_services);
            await MopupService.Instance.PushAsync(incompleteQuantityInputPopup, animate: true);
            var inCompleteQuantity = await incompleteQuantityInputPopup.TaskCompletionSource.Task;
            if (!string.IsNullOrEmpty(inCompleteQuantity))
            {
                orderItem.quantitySep = int.Parse(inCompleteQuantity);
                AddSeprationItemRequestModel addSeprationItems = new AddSeprationItemRequestModel
                {
                    id = orderItem.id,
                    phcOrderId = PhcOrderId,
                    phcOrderItemLinId = orderItem.phcOrderItemLinId,
                    productRef = orderItem.productRef,
                    productDescription = orderItem.productDescription,
                    userId=int.Parse(_settingsService.UserId),
                    userCode = _settingsService.UserCode,
                    quantitySet = orderItem.quantitySep,
                    quantityPre = orderItem.quantity,
                    //dateRegist = itemsRead.dateRegist,
                };


                var status = await _phcOrderServices.AddSeprationItem(addSeprationItems);
                if (status)
                {
                    orderItem.Status = 2;
                    orderItem.RemainingQuantity = orderItem.quantity - orderItem.quantitySep;
                    //if (OrderItemsSeprationLocal != null)
                    //{
                    //    var localSavedItems = await dbService.GetItemsAsync<OrderItemsSeprationLocalStorageModel>();
                    //    var alreadySavedItem = localSavedItems.FirstOrDefault(s => s.phcOrderId == OrderItemsSeprationLocal.phcOrderId);
                    //    if (alreadySavedItem != null)
                    //    {
                    //        var isDelteStatus = await dbService.DeleteItemAsync<OrderItemsSeprationLocalStorageModel>(alreadySavedItem);
                    //    }
                    //}

                    //OrderItemsSeprationLocal.OrderItems.Add(orderItem);

                    //await dbService.SaveItemAsync<OrderItemsSeprationLocalStorageModel>(OrderItemsSeprationLocal);

                }
                IsBusy = false;
            }
        }
        public ICommand CompleteCommand { get; }
        private async void ExecuteCompleteCommand(OrderItem orderItem)
        {
            IsBusy = true;
            AddSeprationItemRequestModel addSeprationItems = new AddSeprationItemRequestModel
            {
                id = orderItem.id,
                phcOrderId = PhcOrderId,
                phcOrderItemLinId = orderItem.phcOrderItemLinId,
                productRef = orderItem.productRef,
                productDescription = orderItem.productDescription,
                userId=int.Parse(_settingsService.UserId),
                userCode = _settingsService.UserCode,
                quantitySet = orderItem.quantity,
                quantityPre = orderItem.quantity,
                dateRegist = DateTime.Today,
            };
            var status = await _phcOrderServices.AddSeprationItem(addSeprationItems);
            if (status)
            {
                orderItem.Status = 1;
                //if (OrderItemsSeprationLocal != null)
                //{
                //    var localSavedItems = await dbService.GetItemsAsync<OrderItemsSeprationLocalStorageModel>();
                //    var alreadySavedItem = localSavedItems.FirstOrDefault(s => s.phcOrderId == OrderItemsSeprationLocal.phcOrderId);
                //    if (alreadySavedItem != null)
                //    {
                //        var isDelteStatus = await dbService.DeleteItemAsync<OrderItemsSeprationLocalStorageModel>(alreadySavedItem);
                //    }
                //}

                //OrderItemsSeprationLocal.OrderItems.Add(orderItem);

                //await dbService.SaveItemAsync<OrderItemsSeprationLocalStorageModel>(OrderItemsSeprationLocal);

            }
            IsBusy = false;
        }
        public async override Task Initialise(object parameter)
        {
            IsBusy = true;
            if (parameter is CustomersOrderModel customersOrder)
            {

                PhcOrderId = customersOrder.phcOrderId;
                CustomersOrder = customersOrder;
                if (customersOrder.itemsRead != null)
                {
                    OrderItem = new ObservableCollection<OrderItem>(CustomersOrder.orderItems);

                }
                OrderItemsSeprationLocal = new OrderItemsSeprationLocalStorageModel();
                OrderItemsSeprationLocal.id = customersOrder.id;
                OrderItemsSeprationLocal.phcOrderId = customersOrder.phcOrderId;
                OrderItemsSeprationLocal.customerId = customersOrder.customerId;
                OrderItemsSeprationLocal.customerName = customersOrder.customerName;
                OrderItemsSeprationLocal.OrderItems = new List<OrderItem>();
                foreach (var item in OrderItem)
                {
                    if (item.quantity!=item.quantitySep&&item.quantitySep!=0)
                    {
                        item.Status = 2;
                        item.RemainingQuantity = item.quantity-item.quantitySep;
                    }
                    else if (item.quantity==item.quantitySep && item.quantitySep!=0)
                    {
                        item.Status = 1;

                    }
            }
            //var orderItemsSeprationLocal = await dbService.GetItemsAsync<OrderItemsSeprationLocalStorageModel>();
            //if (orderItemsSeprationLocal.Count > 0)
            //{
            //    var orderiItem = orderItemsSeprationLocal.FirstOrDefault(s => s.phcOrderId == customersOrder.phcOrderId);
            //    if (orderiItem!=null)
            //    {
            //        foreach (var item in OrderItem)
            //        {
            //            var savedItem = orderiItem.OrderItems.FirstOrDefault(s => s.phcOrderItemLinId == item.phcOrderItemLinId);
            //            if (savedItem != null)
            //            {
            //                item.Status = savedItem.Status;
            //                item.RemainingQuantity=savedItem.RemainingQuantity;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    orderItemsSeprationLocalStorageModels = new List<OrderItemsSeprationLocalStorageModel>();
            //}
        }
            IsBusy = false;

        }
    }
}
