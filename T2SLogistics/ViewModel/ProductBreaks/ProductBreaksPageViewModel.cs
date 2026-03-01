using T2SLogistics.Model;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.Popups;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.Breaks
{
    public class ProductBreaksPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        PhcOrderServices _phcOrderServices;
        ProductBreaksService _productBreaksService;
        public ProductBreaksPageViewModel(INavigationService navigationService, IServiceProvider serviceProvider) : base(navigationService)
        {
            _services = serviceProvider;
            _navigationService = navigationService;
            _phcOrderServices = _services.GetService<PhcOrderServices>();
            _productBreaksService = _services.GetService<ProductBreaksService>();

            CancelCommand = new Command(ExecuteCancelCommand);
            AddProductBreakCommand = new Command(ExecuteAddProductBreakCommand);

        }
        private ObservableCollection<OrderItem> _orderItems;
        public ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set => SetProperty(ref _orderItems, value);
        }
        private string _refEanCode;
        public string RefEanCode
        {
            get => _refEanCode;
            set => SetProperty(ref _refEanCode, value);
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
        private ItemsRead ReadValidItem { get; set; }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }
        public int BoxNr { get; set; }

        public ICommand AddProductBreakCommand { get; }
        private async void ExecuteAddProductBreakCommand()
        {
            IsBusy = true;
            if (string.IsNullOrEmpty(RefEanCode))
            {
                IsBusy = false;

                await Application.Current?.MainPage?.DisplayAlert("Error", "Please scan or input refcode first", "OK");

                return;
            }

            AddItemBreaksRequestModel addItemBreaksRequestModel = new AddItemBreaksRequestModel
            {
                id = ReadValidItem.id,
                eanCode = ReadValidItem.eanCode,
                productRef = ReadValidItem.productRef,
                productDescription = ReadValidItem.productDescription,
                nameDescription= ReadValidItem.productDescription,
                quantity = SetQuantity,
                dateRegist= CurrentDate,
            };
            var isSuccess = await _productBreaksService.AddProductBreaksItems(addItemBreaksRequestModel);
            if (isSuccess)
            {
                IsBusy = false;
                await Application.Current?.MainPage?.DisplayAlert("Sucess", "Read item saved sucessfully", "OK");
                await _navigationService.NavigateBack();
            }
            else
            {
                IsBusy = false;

                await Application.Current?.MainPage?.DisplayAlert("Error", "Error has been occured while saving read item", "OK");

            }
        }
        public ICommand CancelCommand { get; }
        private async void ExecuteCancelCommand()
        {
            try
            {
                var result = await Application.Current?.MainPage?.DisplayActionSheet("Are you sure you want to cancel", "Cancel", "OK");
                if (result == "OK")
                {
                    await _navigationService.NavigateBack();
                }
            }
            catch (Exception ex)
            {

            }

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
                        await Application.Current?.MainPage?.DisplayAlert("Error", "Code not found", "OK");

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


                    ReadValidItem = new ItemsRead
                    {
                        id = response.id,
                        eanCode = response.eanCode,
                        productRef = response.productRef,
                        productDescription = response.productDescription,
                        quantity = SetQuantity
                    };
                    OrderItems.Add(new OrderItem
                    {
                        id = response.id,
                        productRef = response.productRef,
                        productDescription = response.productDescription,
                        quantity = SetQuantity,
                        //eanCode = response.eanCode,

                    });
                }
            }
            IsBusy = false;
        }
        public override Task Initialise(object? parameter)
        {
            CurrentDate=DateTime.Today;
            OrderItems=new ObservableCollection<OrderItem>();
            return base.Initialise(parameter);
        }
    }
}
