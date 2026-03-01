using T2SLogistics.Model;
using T2SLogistics.Resx;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.CustomerOrders;
using T2SLogistics.View.Popups;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.CustomerOrders
{
    public class OrderReadingPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        PhcOrderServices _phcOrderServices;
        CustomerOrderService customerOrderService;
        public OrderReadingPageViewModel(INavigationService navigationService, IServiceProvider serviceProvider) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = serviceProvider;
            _phcOrderServices = _services.GetService<PhcOrderServices>();
            customerOrderService = _services.GetService<CustomerOrderService>();

            DeleteReadItemCommand = new Command<ItemsRead>(ExecuteDeleteReadItemCommand);
            CancelCommand = new Command(ExecuteCancelCommand);
            AddExpeditionReadCommand = new Command(ExecuteAddExpeditionReadCommand);

        }
        private ObservableCollection<ItemsRead> _readItems;
        public ObservableCollection<ItemsRead> ReadItems
        {
            get => _readItems;
            set => SetProperty(ref _readItems, value);
        }
        private int _boxNr;
        public int BoxNr
        {
            get => _boxNr;
            set => SetProperty(ref _boxNr, value);
        }
        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }
        private CustomersOrderModel CustomersOrderModel { get; set; }
        private string _refEanCode;
        public string RefEanCode
        {
            get => _refEanCode;
            set => SetProperty(ref _refEanCode, value);
        }
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }
        private int _setQuantity = 1;
        public int SetQuantity
        {
            get => _setQuantity;
            set => SetProperty(ref _setQuantity, value);
        }
        private bool _isAllQtyPopupSwitched = false;
        public bool IsAllQtyPopupSwitched
        {
            get => _isAllQtyPopupSwitched;
            set => SetProperty(ref _isAllQtyPopupSwitched, value);
        }
        private int _totalItemsExpected = 5;
        public int TotalItemsExpected
        {
            get => _totalItemsExpected;
            set => SetProperty(ref _totalItemsExpected, value);

        }
        private double _progressReadItems = 0;
        public double ProgressReadItems
        {
            get => _progressReadItems;
            set => SetProperty(ref _progressReadItems, value);

        }
        private int _totalItemsRead = 0;
        public int TotalItemsRead
        {
            get => _totalItemsRead;
            set => SetProperty(ref _totalItemsRead, value);

        }
        public string ItemReadID{ get; set; }

        //public ICommand DeleteReadItemCommand { get; }
        //private async void ExecuteDeleteReadItemCommand(ItemsRead item)
        //{
        //    IsBusy = true;
        //    if (ReadItems.Contains(item))
        //    {
        //       var isDeleted= await _phcOrderServices.DeleteExpeditionReadItems(item.id);
        //        IsBusy = false;
        //        if (isDeleted)
        //        {
        //            await Application.Current?.MainPage?.DisplayAlert("Success", "Read item deleted successfully", "OK");

        //            ReadItems.Remove(item);
        //            TotalItemsRead = ReadItems.Count;
        //            ProgressReadItems = (Convert.ToDouble(TotalItemsRead) / Convert.ToDouble(TotalItemsExpected));
        //        }
        //        else
        //        {

        //            await Application.Current?.MainPage?.DisplayAlert("Error", "Error has been occured while deleting read item", "OK");
        //        }

        //    }
        //    IsBusy = false;
        //}

        public ICommand DeleteReadItemCommand { get; }
        private async void ExecuteDeleteReadItemCommand(ItemsRead item)
        {
            if (item == null)
                return;

            // Pergunta de confirmação (Sim / Não)
            var confirm = await Application.Current?.MainPage?.DisplayAlert("Confirmar", "Confirma apagar esta leitura?", "Sim", "Não");
            if (!confirm)
                return;

            try
            {
                IsBusy = true;

                if (ReadItems.Contains(item))
                {
                    var isDeleted = await _phcOrderServices.DeleteExpeditionReadItems(item.id);

                    if (isDeleted)
                    {
                        await Application.Current?.MainPage?.DisplayAlert("Sucesso", "Leitura apagada com sucesso", "OK");

                        ReadItems.Remove(item);
                        TotalItemsRead = ReadItems.Count;
                        ProgressReadItems = (Convert.ToDouble(TotalItemsRead) / Convert.ToDouble(TotalItemsExpected));
                    }
                    else
                    {
                        await Application.Current?.MainPage?.DisplayAlert("Erro", "Ocorreu um erro ao apagar a leitura", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // opcional: log ex
                await Application.Current?.MainPage?.DisplayAlert("Erro", "Ocorreu um erro inesperado", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand CancelCommand { get; }
        private async void ExecuteCancelCommand()
        {
            try
            {
                var result = await Application.Current?.MainPage?.DisplayActionSheet("Confirma Cancelar", "Não", "Sim");
                if (result == "Sim")
                {
                    IsBusy = true;
                    var status = await _phcOrderServices.CancelReadExpedition(ItemReadID);
                    if (status)
                    {
                        IsBusy = false;
                        await Application.Current?.MainPage?.DisplayAlert("Processo Cancelado", $"Readings cancelled successfully.", "OK");
                        await _navigationService.NavigateBackToPage<CustomerOrdersPage>();
                    }


                }
            }
            catch (Exception ex)
            {

            }

        }
        public ICommand AddExpeditionReadCommand { get; }
        private async void ExecuteAddExpeditionReadCommand()
        {
            IsBusy = true;
           
                var updatedCustomerOrders = await customerOrderService.GetCustomerOrder();
                var updatedOrder = updatedCustomerOrders.FirstOrDefault(co => co.phcOrderId == CustomersOrderModel.phcOrderId);
                IsBusy = false;

                await _navigationService.NavigateBackToPage<CustomerOrderDetailPage>(updatedOrder);
          
            IsBusy = false;
        }
        public async Task GetRefProduct()
        {
            IsBusy = true;
            if (!string.IsNullOrEmpty(RefEanCode))
            {
                var response = await _phcOrderServices.GetRefProduct(RefEanCode);
                if (response != null)
                {
                    if (response.id == 0 && string.IsNullOrEmpty(response.eanCode) &&
                        string.IsNullOrEmpty(response.productRef) && string.IsNullOrEmpty(response.productDescription))
                    {
                        IsBusy = false;
                        await Application.Current?.MainPage?.DisplayAlert("Error", AppResources.OrderReadingPage_UnExpectedArticle, "OK");
                        RefEanCode = string.Empty;

                        return;
                    }
                    if (IsAllQtyPopupSwitched)
                    {
                        var insertQtyPopup = new IncompleteQuantityInputPopup(_services);
                        await MopupService.Instance.PushAsync(insertQtyPopup);
                        var inCompleteQuantity = await insertQtyPopup.TaskCompletionSource.Task;
                        if (!string.IsNullOrEmpty(inCompleteQuantity))
                        {
                            SetQuantity = int.Parse(inCompleteQuantity);

                        }
                    }


                    //ReadValidItem = new ItemsRead
                    //{
                    //    id = response.id,
                    //    eanCode = response.eanCode,
                    //    productRef = response.productRef,
                    //    productDescription = response.productDescription,
                    //    quantity = SetQuantity
                    //};
                    var phcOrderLinId = CustomersOrderModel.orderItems.FirstOrDefault(orderItem => orderItem.eanCode == response.eanCode);
                    AddExpeditionRequestModel addExpeditionRequestModel = new AddExpeditionRequestModel
                    {
                        id = response.id,
                        itemReadId = ItemReadID,
                        eanCode = response.eanCode,
                        productRef = response.productRef,
                        productDescription = response.productDescription,
                        boxNr = BoxNr,
                        phcOrderId = CustomersOrderModel.phcOrderId,
                        phcOrderItemLinId = phcOrderLinId != null ? phcOrderLinId.phcOrderItemLinId : string.Empty,
                        quantity = SetQuantity,


                    };
                    var expeditionResponse = await _phcOrderServices.AddExpeditionReadItems(addExpeditionRequestModel);
                    if (expeditionResponse!=null)
                    {
                        IsBusy = false;
                        //await Application.Current?.MainPage?.DisplayAlert("Sucess", "Read item saved sucessfully", "OK");
                        RefEanCode = string.Empty;
                        ReadItems.Add(new ItemsRead
                        {
                            id = expeditionResponse.id,
                            productRef = expeditionResponse.productRef,
                            productDescription = expeditionResponse.productDescription,
                            quantity = expeditionResponse.quantity,
                            boxNr = expeditionResponse.boxNr,
                            eanCode = expeditionResponse.eanCode,

                        });

                        TotalItemsRead = ReadItems.Count;
                        ProgressReadItems = (Convert.ToDouble(TotalItemsRead) / Convert.ToDouble(TotalItemsExpected));
                    }
                    else
                    {
                        IsBusy = false;

                        await Application.Current?.MainPage?.DisplayAlert("Error", "Error has been occured while saving read item", "OK");

                    }
                  
                }
            }
            IsBusy = false;
        }
        public override Task Initialise(object? parameter)
        {
            IsBusy = true;
            ItemReadID= Guid.NewGuid().ToString();
            if (parameter is CustomersOrderModel customersOrder)
            {
                CustomersOrderModel = customersOrder;
                CustomerName = CustomersOrderModel.customerName;
                CurrentDate = customersOrder.orderDate;

                BoxNr = customersOrder.TotalBoxes + 1;
                ReadItems = new ObservableCollection<ItemsRead>();
                //OrderItems = new ObservableCollection<OrderItem>(customersOrder.orderItems);
            }
            else if (parameter is OrderBoxesModel orderBoxesModel)
            {
                BoxNr = orderBoxesModel.BoxNumber;
                CustomersOrderModel = orderBoxesModel.customersOrderModel;
                CurrentDate = CustomersOrderModel.orderDate;

                CustomerName = CustomersOrderModel.customerName;
                ReadItems = new ObservableCollection<ItemsRead>();
                foreach (var item in orderBoxesModel.ReadItems)
                {
                    ReadItems.Add(new ItemsRead
                    {
                        id = item.id,
                        productRef = item.productRef,
                        productDescription = item.productDescription,
                        phcOrderId = item.phcOrderId,
                        phcOrderItemLinId = item.phcOrderItemLinId,
                        boxNr = item.boxNr,
                        quantity = item.quantity,
                        eanCode = item.eanCode,

                    });
                }

                TotalItemsRead = ReadItems.Count;
                ProgressReadItems = (Convert.ToDouble(TotalItemsRead) / Convert.ToDouble(TotalItemsExpected));
            }
            IsBusy = false;

            return base.Initialise(parameter);
        }
    }
}
