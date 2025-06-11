using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using T2SLogistics.Dtos;
using T2SLogistics.Models;
using T2SLogistics.ViewModels.Expedicao;
using T2SLogistics.ViewModels.Recepcao;

namespace T2SLogistics.ViewModels
{
    public class SyncDataViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        //ExpedicaoCaiacaViewModel expVM;
        LeiturasCaiaca lt = new LeiturasCaiaca();
        List<LeiturasCaiaca> saidas;
        List<LeiturasCaiaca> entradas;


        public SyncDataViewModel()
        {
            LoadData();

        }


        private void LoadData()
        {

            entradas = lt.GetAllLocalData().Where(l => l.IdStatus == LeiturasCaiaca.Pendente && l.TipoMov == LeiturasCaiaca.Entradas).OrderBy(o => o.StampLeitura).ToList();
            saidas = lt.GetAllLocalData().Where(l => l.IdStatus == LeiturasCaiaca.Pendente && l.TipoMov == LeiturasCaiaca.Saidas).OrderBy(o => o.StampLeitura).ToList();


            EntradasPendentes = entradas.GroupBy(u => u.StampLeitura).Select(s => s.ToList()).Count();
            SaidasPendentes = saidas.GroupBy(u => u.StampLeitura).Select(s => s.ToList()).Count();

        }

        private int _entradasPendentes;
        public int EntradasPendentes
        {
            get => _entradasPendentes; set
            {
                _entradasPendentes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntradasPendentes)));
            }
        }

        private int _saidasPendentes;
        public int SaidasPendentes
        {
            get => _saidasPendentes; set
            {
                _saidasPendentes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SaidasPendentes)));
            }
        }

        private string _message;
        public string Message
        {
            get => _message; set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        private string _textColorMsg;
        public string TextColorMsg
        {
            get => _textColorMsg; set
            {
                _textColorMsg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextColorMsg)));
            }
        }






        public void ActualizarSincronizacao(IEnumerable<LeiturasCaiaca> _leituras)
        {

            var _itemSendSuccess = _leituras.Where(l => l.NrGuiaOnServer > 0).ToList();
            lt.AddToLocalDb(_itemSendSuccess);

        }

        public ICommand ResetLocalDb
        {

            get
            {
                return new Command(async () =>
                {

                    Message = "";


                    try
                    {


                        var fornec = new FornecedoresCaiaca();
                        var lotes = new LoteCaiaca();
                        var refs = new ArtigosCaiaca();
                        var leituras = new LeiturasCaiaca();

                        fornec.ResetLocalDb();
                        lotes.ResetLocalDb();
                        refs.ResetLocalDb();
                        leituras.ResetLocalDb();


                        Message = "Dados sincronizados!";
                        TextColorMsg = "Green";

                    }
                    catch (Exception ex)
                    {
                        TextColorMsg = "Red";
                        Message = ex.Message;
                    }


                });
            }

        }


        public ICommand ProcessSync
        {

            get
            {
                return new Command(async () =>
                {

                    Message = "";

                    LoadData();

                    try
                    {

                        if (EntradasPendentes == 0 && SaidasPendentes == 0)
                            return;

                        if (entradas.Count > 0)
                            ProcessSyncEntradas(entradas);

                        if (saidas.Count > 0)
                            ProcessSyncSaidas(saidas);

                        LoadData();
                        LoadData();

                        Message = "Dados sincronizados!";
                        TextColorMsg = "Green";

                    }
                    catch (Exception ex)
                    {
                        TextColorMsg = "Red";
                        Message = ex.Message;
                    }


                });
            }

        }


        private void ProcessSyncSaidas(List<LeiturasCaiaca> leituras)
        {

            var _resultInsApi = new List<LeiturasCaiacaDto>();

            var expVM = new ExpedicaoCaiacaViewModel();
            foreach (var item in leituras)
                expVM.ItemsRead.Add(item);

            if (expVM.ItemsRead.Count > 0)
                _resultInsApi = expVM.AddOrCloseLeituras(expVM);

            if (_resultInsApi.Where(r => r.NrDocInServer > 0).Count() > 0)
                foreach (var item in _resultInsApi.Where(r => r.NrDocInServer > 0))
                    lt.ClearReadItemsSendSuccess(item.StampLeitura);
            else
                throw new Exception("Ocorreu um erro ao tentar sincronizar!");

        }

        private void ProcessSyncEntradas(List<LeiturasCaiaca> leituras)
        {

            var recVM = new RecCaiaca1CodBarViewModel();

            var _resultInsApi = new List<LeiturasCaiacaDto>();

            foreach (var item in leituras)
                recVM.ItemsRead.Add(item);

            if (recVM.ItemsRead.Count > 0)
                _resultInsApi = recVM.AddOrCloseLeituras(recVM);

            if (_resultInsApi.Where(r => r.NrDocInServer > 0).Count() > 0)
                foreach (var item in _resultInsApi.Where(r => r.NrDocInServer > 0))
                    lt.ClearReadItemsSendSuccess(item.StampLeitura);
            else
                throw new Exception("Ocorreu um erro ao tentar sincronizar!");
        }

    }
}
