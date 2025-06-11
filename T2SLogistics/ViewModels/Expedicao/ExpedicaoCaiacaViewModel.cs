using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;
using T2SLogistics.Models;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels.Expedicao
{
    public class ExpedicaoCaiacaViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ExpedicaoCaiacaViewModel()
        {

            ItemsRead = new ObservableCollection<LeiturasCaiaca>();
            LotesDisponiveis = new ObservableCollection<LoteCaiaca>();

            DataMov = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        }

        public string StampLeitura { get; set; }
        public DateTime DataMov { get; set; }

        public int NumDocInServer { get; set; }

        public ObservableCollection<LeiturasCaiaca> ItemsRead { get; set; }
        public ObservableCollection<LoteCaiaca> LotesDisponiveis { get; set; }



        public string IconReadLeitura { get => "\ue039"; }

        public string StrDataMov
        {
            get => DataMov.ToString("dd-MM-yyyy");
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

        private string _strTotalLeituras;
        public string StrTotalLeituras
        {
            get => "Total Paletes: " + ItemsRead.Count; set
            {
                _strTotalLeituras = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrTotalLeituras)));
            }
        }





        private bool _temLotes;
        public bool TemLotesDisponiveis
        {
            get => LotesDisponiveis.Count() == 0 ? false : true; set
            {
                _temLotes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TemLotesDisponiveis)));
            }
        }



        private void AddToLocalDb(IEnumerable<LoteCaiaca> lotes)
        {

            if (lotes == null)
                return;

            if (lotes.Count() <= 0)
                return;

            var _lt = new LoteCaiaca();
            _lt.AddToLocalDb(lotes);

        }

        public async Task<ObservableCollection<LoteCaiaca>> GetLotesFromApi()
        {

            try
            {
                var _lotesApi = new CaiacaApi();
                var _lotesTask = _lotesApi.GetRemoteLotesAsync();

                var _lotes = await _lotesTask;

                AddToLocalDb(_lotes);

                var _listReturn = new ObservableCollection<LoteCaiaca>();
                foreach (var l in _lotes)
                {
                    _listReturn.Add(new LoteCaiaca
                    {

                        Design = l.Design,
                        SeStamp = l.SeStamp,
                        Inactivo = l.Inactivo,
                        Lote = l.Lote,
                        NomeCliente = l.NomeCliente,
                        NumCliente = l.NumCliente,
                        Ref = l.Ref,
                        Stock = l.Stock,
                        Unidade = l.Unidade,
                        TipoMov = l.TipoMov

                    });
                }

                return _listReturn;
            }
            catch (Exception)
            {

            }

            return new ObservableCollection<LoteCaiaca>();


        }


        public List<LeiturasCaiacaDto> AddOrCloseLeituras(ExpedicaoCaiacaViewModel exp)
        {

            var _api = new CaiacaApi();
            var _leituras = new LeiturasCaiaca();

            var _result = _api.AddOrCloseSaidas(exp);

            if (_result.Count() == 0)
                _leituras.AddToLocalDb(exp.ItemsRead);
            else
                if (_result.Count() > 0)
                foreach (var l in _result)
                    if (l.NrDocInServer <= 0)
                        _leituras.AddToLocalDb(exp.ItemsRead.Where(r => r.StampLeitura == l.StampLeitura));


            return _result;

        }

        public LeiturasCaiaca GetLeituraSaida(LoteCaiaca lt)
        {
            return new LeiturasCaiaca
            {
                DataLeitura = DateTime.Now,
                Design = lt.Design,
                SeStamp = lt.SeStamp,
                IdStatus = LeiturasCaiaca.Pendente,
                Inactivo = lt.Inactivo,
                Lote = lt.Lote,
                NomeEntidade = lt.NomeCliente,
                NumEntidade = lt.NumCliente,
                Ref = lt.Ref,
                Stock = lt.Stock,
                StampLeitura = lt.StampLeitura,
                TipoMov = LeiturasCaiaca.Saidas,
                Unidade = lt.Unidade,
            };

        }

    }
}
