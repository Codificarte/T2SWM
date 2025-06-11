using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class EncomendasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public EncomendasViewModel()
        {
            Encomendas = new ObservableCollection<OrderViewModel>();

        }


        public string IconConfig { get; set; }
        public ObservableCollection<OrderViewModel> Encomendas { get; set; }
        public ObservableCollection<OrderViewModel> EncomendasFilter { get; set; }

        private string _nomeDocumento;
        public string NomeDocumentos
        {
            get => _nomeDocumento; set
            {
                _nomeDocumento = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomeDocumentos)));
            }
        }


        private string _textTemEncomendas;
        public string TextTemEncomendas
        {
            get => _textTemEncomendas; set
            {
                _textTemEncomendas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextTemEncomendas)));
            }
        }


        private bool _hasEncomendas;
        public bool HasEncomendas
        {
            get => Encomendas.Count() > 0 ? true : false; set
            {
                _hasEncomendas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasEncomendas)));
            }
        }


        private bool _semEncomendas;
        public bool SemEncomendas
        {
            get => Encomendas.Count() == 0 ? true : false; set
            {
                _semEncomendas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SemEncomendas)));
            }
        }



        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing; set
            {
                _isRefreshing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
            }
        }


        private bool _canDoEncsWithoutEntity;
        public bool CanDoEncsWithoutEntity
        {
            get => _canDoEncsWithoutEntity; set
            {
                _canDoEncsWithoutEntity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanDoEncsWithoutEntity)));
            }
        }

        public ObservableCollection<OrderViewModel> GetFiltedOrders(EncomendasViewModel orderVM, string searchName)
        {

            if (string.IsNullOrEmpty(searchName))
                return new ObservableCollection<OrderViewModel>();

            int nrIfNumeric;
            bool isNumeric = int.TryParse(searchName, out nrIfNumeric);

            if (searchName != null)
            {

                if (isNumeric)
                {

                    var _tmpList = orderVM.Encomendas.Where(o => o.Numdoc.ToString().Contains(searchName));

                    orderVM.EncomendasFilter = new ObservableCollection<OrderViewModel>();

                    foreach (var item in _tmpList)
                        orderVM.EncomendasFilter.Add(item);

                }



                if (!isNumeric && searchName.ToString().Length >= 2)
                {

                    var _tmpList = orderVM.Encomendas.Where(o => o.Nome.ToLower().ToString().Contains(searchName));

                    orderVM.EncomendasFilter = new ObservableCollection<OrderViewModel>();

                    foreach (var item in _tmpList)
                        orderVM.EncomendasFilter.Add(item);

                }

            }

            return orderVM.EncomendasFilter;

        }


        /// GetRemoteData
        /// 


        public virtual async Task<IEnumerable<OrderViewModel>> GetRemoteOrdersFromApi()
        {

            var _ordersApi = new OrdersApi();
            var _ordersTask = _ordersApi.GetRemoteDataAsync();

            var _orders = await _ordersTask;

            return _orders;

        }


        public ObservableCollection<OrderDetailViewModel> GetRemoteOrderDetailsFromApi(string idBackoffice)
        {
            var _ordersApi = new OrdersApi();

            var _orderDetails = Task.Run(async () => await _ordersApi.GetRemoteOrderDetailsFromApi(idBackoffice)).Result;

            var _list = new ObservableCollection<OrderDetailViewModel>();

            foreach (var item in _orderDetails)
                _list.Add(item);

            return _list;
        }

        public void SetLeiturasPendClosed(string idBackOffice)
        {

            var _ordersApi = new OrdersApi();
            _ordersApi.SetLeituraPendIsClosed(idBackOffice);
        }

        public ObservableCollection<LeiturasViewModel> GetRemoteLeiturasFeitasFromApi(string idBackoffice)
        {
            var _ordersApi = new OrdersApi();

            var _leituras = Task.Run(async () => await _ordersApi.GetRemoteLeiturasFeitasFromApi(idBackoffice)).Result;

            var _list = new ObservableCollection<LeiturasViewModel>();

            foreach (var item in _leituras)
                _list.Add(item);

            return _list;
        }

    }
}
