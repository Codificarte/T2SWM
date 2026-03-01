using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using Mopups.Services;

namespace T2SLogistics.ViewModel.Popups
{
    public class InsertProductQtyPopupViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        ISettingsService _settingsService;
        ProductionEntriesService productionEntriesService;
        public InsertProductQtyPopupViewModel(INavigationService navigationService,
            IServiceProvider serviceProvider, ISettingsService settingsService) : base(navigationService)
        {
            _serviceProvider = serviceProvider;
            _settingsService = settingsService;
            productionEntriesService= _serviceProvider.GetService<ProductionEntriesService>();
        }
        private string _insertQty;
        public string InsertQty
        {
            get => _insertQty;
            set => SetProperty(ref _insertQty, value);
        }
        private string _qtyBreak;
        public string QtyBreak
        {
            get => _qtyBreak;
            set => SetProperty(ref _qtyBreak, value);
        }
        public async Task InsertQtyAsync(string refProd, string description,  string stampLinOrderProd, string operationName)
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(InsertQty))
            {
                await Application.Current?.MainPage?.DisplayAlert("Atenção", "Por favor, insira uma quantidade válida.", "OK");

                return;
            }
            var productionEntriesRequestModel = new Model.ProductionEntriesRequestModel
            {
                refProd = refProd,
                description = description,
                quanty = int.Parse(InsertQty),
                quantyBreak = int.Parse(QtyBreak),
                userCode = _settingsService.UserCode,
                stampLinOrderProd = stampLinOrderProd,
                operationName = operationName
            };
            var status = await productionEntriesService.CreateProductionEntries(productionEntriesRequestModel);
            if (status)
            {
                await Application.Current?.MainPage?.DisplayAlert("Concluído", "Registo de produção criado com sucesso.", "OK");
               await MopupService.Instance.PopAsync();

            }
            else
            {
                await Application.Current?.MainPage?.DisplayAlert("Erro", "Falha ao criar o registo de produção.", "OK");

            }
            IsBusy = false;
        }
    }
}
