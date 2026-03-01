using CommunityToolkit.Maui.Core.Extensions;
using T2SLogistics.Model;
using T2SLogistics.Resx;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.View.CustomerOrders;
using T2SLogistics.View.Popups;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.CustomerOrders
{
    public class CustomerOrderDetailViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IServiceProvider _services;
        PhcOrderServices _phcOrderServices;

        public CustomerOrderDetailViewModel(INavigationService navigationService, IServiceProvider services) : base(navigationService)
        {
            _navigationService = navigationService;
            _services = services;
            _phcOrderServices = _services.GetService<PhcOrderServices>();
            FinishReadingCommand = new Command(ExecuteFinishReadingCommand);

        }
        private CustomersOrderModel _customersOrder;
        public CustomersOrderModel CustomersOrder
        {
            get => _customersOrder;
            set => SetProperty(ref _customersOrder, value);
        }
        private ObservableCollection<OrderBoxesModel> _orderBoxes = new ObservableCollection<OrderBoxesModel>();
        public ObservableCollection<OrderBoxesModel> OrderBoxes
        {
            get => _orderBoxes;
            set => SetProperty(ref _orderBoxes, value);
        }
        private int _totalBoxes;
        public int TotalBoxes
        {
            get => _totalBoxes;
            set => SetProperty(ref _totalBoxes, value);

        }
        private string _observationNotes;
        public string ObservationNotes { 
            get=> _observationNotes;
            set => SetProperty(ref _observationNotes, value);
        }
        public ICommand FinishReadingCommand { get; }
        private async void ExecuteFinishReadingCommand()
        {
            try
            {
                //var selectedOption = await Application.Current?.MainPage?.DisplayActionSheet("Are you sure you want finish reading", "Cancel", "OK");
                //if (selectedOption == "OK")
                //{
                //    var observationDescriptionPopup = new ObservationDescriptionPopup();
                 
                //    await MopupService.Instance.PushAsync(observationDescriptionPopup);
                //    var observationNotes = await observationDescriptionPopup.TaskCompletionSource.Task;
                //    if (!string.IsNullOrEmpty(observationNotes))
                //    {
                //        ObservationNotes = observationNotes;
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }
        public ICommand CloseExpeditionCommand => new Command(ExecuteCloseExpeditionCommand);
        private async void ExecuteCloseExpeditionCommand()
        {
            try
            {
                var selectedOption = await Application.Current?.MainPage?.DisplayActionSheet(AppResources.CustomerOrderDetail_ConfirmationClosingTheOrder, "Cancel", "OK");
                if (selectedOption == "OK")
                {
                    IsBusy = true;
                    var observationDescriptionPopup = new ObservationDescriptionPopup();

                    await MopupService.Instance.PushAsync(observationDescriptionPopup);
                    var observationNotes = await observationDescriptionPopup.TaskCompletionSource.Task;
                    if (!string.IsNullOrEmpty(observationNotes))
                    {
                        ObservationNotes = observationNotes;
                    }
                    CloseExpeditionRequestModel closeExpeditionRequestModel = new CloseExpeditionRequestModel
                    {
                        phcOrderId = CustomersOrder.phcOrderId,
                        obs = ObservationNotes,
                    };
                    var response = await _phcOrderServices.CloseExpedition(closeExpeditionRequestModel);
                    if (response > 0)
                    {
                        IsBusy = false;
                        await Application.Current?.MainPage?.DisplayAlert(AppResources.CustomerOrderDetail_ProcessCompleted, $"{AppResources.CustomerOrderDetail_SavedData}" + Environment.NewLine + $"Doc:.{response}", "OK");

                        await _navigationService.NavigateBack();

                    }
                    else
                    {
                        IsBusy = false;


                        await Application.Current?.MainPage?.DisplayAlert("Erro", "Ocorreu um erro..", "OK");
                    }
                    IsBusy = false;
                }
                IsBusy = false;


            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
        }
        public ICommand AddingNewBoxCommand => new Command(ExecuteAddingNewBoxCommand);
        private async void ExecuteAddingNewBoxCommand()
        {
            try
            {

                await _navigationService.NavigateToPage<OrderReadingPage>(CustomersOrder);

            }
            catch (Exception ex)
            {

            }
        }
        public ICommand SelectOrderBoxCommand => new Command<OrderBoxesModel>(ExecuteSelectOrderBoxCommand);
        private async void ExecuteSelectOrderBoxCommand(OrderBoxesModel orderBoxesModel)
        {
            try
            {
                orderBoxesModel.customersOrderModel = CustomersOrder;
                await _navigationService.NavigateToPage<OrderReadingPage>(orderBoxesModel);

            }
            catch (Exception ex)
            {
            }
        }
        public override Task InitialiseBack(object? parameter)
        {
            if (parameter is CustomersOrderModel customersOrder)
            {

            }

            return base.InitialiseBack(parameter);
        }
        public override Task Initialise(object? parameter)
        {
            if (parameter is CustomersOrderModel customersOrder)
            {
                CustomersOrder = customersOrder;
                OrderBoxes = new ObservableCollection<OrderBoxesModel>();
                if (customersOrder != null)
                {
                    if (customersOrder.itemsRead != null)
                    {
                        if (customersOrder.itemsRead.Count != 0)
                        {
                            customersOrder.itemsRead = customersOrder.itemsRead.OrderByDescending(i => i.boxNr).ToList();
                            foreach (var box in customersOrder.itemsRead)
                            {
                                if (!OrderBoxes.Any(orderBox => orderBox.BoxNumber == box.boxNr))
                                {
                                    if (box.boxNr != null && box.boxNr > 0)
                                    {
                                        var readItems = customersOrder.itemsRead.Where(i => i.boxNr == box.boxNr);
                                        OrderBoxes.Add(new OrderBoxesModel
                                        {
                                            BoxNumber = (int)box.boxNr,
                                            ProductCount = box.quantity,
                                            ReadItems = new ObservableCollection<ItemsRead>(readItems),

                                        });
                                    }

                                }

                                customersOrder.TotalBoxes = OrderBoxes.FirstOrDefault().BoxNumber;
                                TotalBoxes = customersOrder.itemsRead.Count;
                            }


                        }

                    }

                }

            }
            return base.Initialise(parameter);

        }
    }
}
