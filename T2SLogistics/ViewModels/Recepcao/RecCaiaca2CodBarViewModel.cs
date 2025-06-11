using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;
using T2SLogistics.Interface;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels.Recepcao
{
    public class RecCaiaca2CodBarViewModel : RecepcaoMercadoriaCaiaca, INotifyPropertyChanged, IRecepcaoCaiaca2CodBar
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RecCaiaca2CodBarViewModel()
        {
            ItemsRead = new ObservableCollection<LeiturasCaiaca>();
            DataMov = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TiposDePapel = new ObservableCollection<TipoPapelCaiaca>();
            Artigos = new ObservableCollection<ArtigosCaiaca>();
            ArtigoSelected = new ArtigosCaiaca();
        }

        public int Numfornec { get; set; }
        public string NomeFornec { get; set; }

        public ArtigosCaiaca ArtigoSelected { get; set; }
        public ObservableCollection<TipoPapelCaiaca> TiposDePapel { get; set; }
        public ObservableCollection<LeiturasCaiaca> ItemsRead { get; set; }
        public ObservableCollection<ArtigosCaiaca> Artigos { get; set; }



        private bool _isEditingItemRead;
        public bool IsEditingItemRead
        {
            get => _isEditingItemRead; set
            {
                _isEditingItemRead = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditingItemRead)));
            }
        }

        private bool _isAdditinItems;
        public bool IsAdditingItems
        {
            get => IsEditingItemRead ? false : true; set
            {
                _isAdditinItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdditingItems)));
            }
        }


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


        private string _totalLido;
        public string TotalLido
        {
            get => _totalLido; set
            {
                _totalLido = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLido)));
            }
        }




        public override List<LeiturasCaiacaDto> AddOrCloseLeituras(object _rec)
        {

            RecCaiaca2CodBarViewModel rec = new RecCaiaca2CodBarViewModel();
            rec = (RecCaiaca2CodBarViewModel)_rec;

            var _leituras = new LeiturasCaiaca();
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

            RecCaiaca2CodBarViewModel rec = new RecCaiaca2CodBarViewModel();
            rec = (RecCaiaca2CodBarViewModel)_rec;

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

        public bool IsValid(LeiturasCaiaca leitura)
        {

            if (string.IsNullOrEmpty(leitura.InfoBobiba2CodBar) || string.IsNullOrEmpty(leitura.Lote))
                return false;

            return true;

        }

        public LeiturasCaiaca SeparaInfoArtigo(string _codBar, RecCaiaca2CodBarViewModel recVM)
        {

            try
            {
                var f = new FornecedoresCaiaca();
                var _lt = new LeiturasCaiaca();
                f = f.GetAllLocalData().Where(i => i.Num == recVM.Numfornec).FirstOrDefault();

                if (f.P1Tipo == FornecedoresCaiaca.InfoGramagem
                    && f.P2Tipo == FornecedoresCaiaca.InfoDiametro
                    && f.P3Tipo == FornecedoresCaiaca.InfoPeso
                    && f.P4Tipo == FornecedoresCaiaca.InfoComprimento)
                    _lt = GetInfoRefDefault(_codBar, recVM);

                return _lt;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        private LeiturasCaiaca GetInfoRefDefault(string itemLido, RecCaiaca2CodBarViewModel recVM)
        {

            var _listArtigos = new List<ArtigosCaiaca>();
            var _ref = new ArtigosCaiaca();
            if (recVM.Artigos == null || recVM.Artigos.Count == 0)
                _listArtigos = _ref.GetAllLocalData().Where(l => l.NumFornec == recVM.Numfornec && l.TipoPapel == recVM.ArtigoSelected.TipoPapel).ToList();

            var _gramagem = itemLido.Substring(0, 3).ToString().Trim().ToString().Substring(1, 2);

            var _largura_dec = (Convert.ToDecimal(itemLido.Substring(3, 4).ToString().Trim()) / 10).ToString().Replace(".", ",");
            var _largura_int = (Convert.ToInt32(itemLido.Substring(3, 4).ToString().Trim()) / 10).ToString();


            var _peso = itemLido.Substring(7, 4).ToString().Trim();
            var _comp = itemLido.Substring(11, 5).ToString().Trim();

            _ref = _listArtigos.Where(a => a.TipoPapel == recVM.ArtigoSelected.TipoPapel
                    && (a.Largura == _largura_dec || a.Largura == _largura_int)
                    && a.Gramagem == _gramagem).FirstOrDefault();

            var _lt = new LeiturasCaiaca();

            _lt.Ref = _ref.Ref;
            _lt.DesignLcb = _ref.DesignLcb;
            _lt.Stock = 1;
            _lt.Peso = Convert.ToDecimal(_peso);
            _lt.UsaLote = _ref.Usalote;
            _lt.Unidade = _ref.UnidLcb;

            _lt.InfoBobiba2CodBar = itemLido;

            return _lt;

        }

        public ObservableCollection<TipoPapelCaiaca> LoadTiposPapel()
        {
            var _list = new ObservableCollection<TipoPapelCaiaca>();
            var _tmpArtigos = GetArtigosFornec(Numfornec);

            var _tmpTiposPapel = _tmpArtigos.GroupBy(u => u.TipoPapel).Select(s => s.ToList());

            foreach (var r in _tmpTiposPapel)
            {
                TipoPapelCaiaca tp = new TipoPapelCaiaca();
                tp.Id++;
                tp.Tipo = r[0].TipoPapel;
                _list.Add(tp);
            }

            return _list;
        }
    }

}
