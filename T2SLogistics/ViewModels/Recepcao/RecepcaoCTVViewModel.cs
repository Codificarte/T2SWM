using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;
using T2SLogistics.Exceptions;
using T2SLogistics.Helpers;
using T2SLogistics.Interface;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels.Recepcao
{
    public class RecepcaoCTVViewModel : RecepcaoMercadoriaCaiaca, INotifyPropertyChanged, IRecepcaoCTV
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RecepcaoCTVViewModel()
        {
            ItemsRead = new ObservableCollection<LeiturasCaiaca>();
            Artigos = new ObservableCollection<ArtigosCaiaca>();
        }

        public int Numfornec { get; set; }
        public string NomeFornec { get; set; }

        public ArtigosCaiaca ArtigoSelected { get; set; }

        public ObservableCollection<LeiturasCaiaca> ItemsRead { get; set; }
        public ObservableCollection<ArtigosCaiaca> Artigos { get; set; }

        private string _designRefSelected;
        public string DesignRefSelected
        {
            get => _designRefSelected; set
            {
                _designRefSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignRefSelected)));
            }
        }

        private bool _canReadItems;
        public bool CanReadItems
        {
            get => _canReadItems; set
            {
                _canReadItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanReadItems)));
            }
        }


        private bool _showLotePeso;
        public bool ShowLotePeso
        {
            get => _showLotePeso; set
            {
                _showLotePeso = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowLotePeso)));
            }
        }

        private bool _notShowLotePeso;
        public bool NotShowLotePeso
        {
            get => _notShowLotePeso; set
            {
                _notShowLotePeso = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotShowLotePeso)));
            }
        }


        private int _totalLeituras;
        public int TotalLeituras
        {
            get => ItemsRead.Count; set
            {
                _totalLeituras = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLeituras)));
            }
        }

        private string _totalLido;
        public string TotalLido
        {
            get => _totalLido; set
            {
                _totalLido = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLido)));
            }
        }

        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="leitura"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public bool IsValid(LeiturasCaiaca leitura)
        {


            if (string.IsNullOrEmpty(leitura.Ref))
                throw new LeituraCaiacaRefException("Seleccione a referência", UtilsForMessage.TitleException);

            if (leitura.UsaLote && string.IsNullOrEmpty(leitura.Lote))
                throw new LeituraCaiacaLoteException("Indique o lote", UtilsForMessage.TitleException);

            if ((leitura.Unidade == ArtigosCaiaca.IsTon || leitura.Unidade == ArtigosCaiaca.IsKg) && leitura.Peso <= 0)
                throw new LeituraCaiacaPesoException("Indique o peso", UtilsForMessage.TitleException);

            if (leitura.Stock <= 0)
                throw new LeituraCaiacaQttException("Indique a quantidade", UtilsForMessage.TitleException);

            return true;

        }


        public override List<LeiturasCaiacaDto> AddOrCloseLeituras(object _rec)
        {
            RecepcaoCTVViewModel rec = new RecepcaoCTVViewModel();
            rec = (RecepcaoCTVViewModel)_rec;

            //var _api = new CaiacaApi();
            var _leituras = new LeiturasCaiaca();

            //var _result = _api.AddOrCloseEntradas(new RecCaiaca1CodBarViewModel());

            var _result = AddOrCloseEntradas(rec);

            if (_result.Count() == 0)
                _leituras.AddToLocalDb(rec.ItemsRead);
            else
                if (_result.Count() > 0)
                foreach (var l in _result)
                    if (l.NrDocInServer <= 0)
                        _leituras.AddToLocalDb(rec.ItemsRead.Where(r => r.StampLeitura == l.StampLeitura));

            return _result;
        }

        private List<LeiturasCaiacaDto> AddOrCloseEntradas(object _rec)
        {
            HttpClient clientApi = new HttpClient();
            string urlEntradas = Helpers.Settings.UrlApiRegEntradas;

            RecepcaoCTVViewModel rec = new RecepcaoCTVViewModel();
            rec = (RecepcaoCTVViewModel)_rec;

            var _url = urlEntradas + "AddRecepcaoBulkCaiaca";
            try
            {

                string objSerialized = JsonConvert.SerializeObject(rec.ItemsRead);
                HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");


                var response = clientApi.PostAsync(_url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    List<LeiturasCaiacaDto> _result;
                    _result = JsonConvert.DeserializeObject<List<LeiturasCaiacaDto>>(responseString);

                    return _result;
                }


                return new List<LeiturasCaiacaDto>();
            }
            catch (Exception)
            {
                return new List<LeiturasCaiacaDto>();
            }

        }
    }
}
