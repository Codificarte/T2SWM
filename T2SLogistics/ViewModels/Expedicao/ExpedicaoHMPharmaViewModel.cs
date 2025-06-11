using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Exceptions;
using T2SLogistics.Helpers;
using T2SLogistics.Interface;
using T2SLogistics.Models;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels.Expedicao
{
    public class ExpedicaoHMPharmaViewModel : ExpedicaoMercadoria, INotifyPropertyChanged, IExpedicaoMercadoria<ExpedicaoViewModel>
    {

        public event PropertyChangedEventHandler PropertyChanged;

        ExpedicaoHMPharmaViewModel expVM;

        public ExpedicaoHMPharmaViewModel()
        {
            Caixas = new ObservableCollection<CaixaViewModel>();
        }

        public string NomeNumEnc
        {
            get => NomeDoc + " Nº " + NumDoc;
        }


        public string StrDataEnc
        {
            get => DataDoc.ToString("dd-MM-yyyy");
        }

        public ObservableCollection<CaixaViewModel> Caixas { get; set; }

        private bool _useArmaz;
        public bool UseArmaz
        {
            get => UseAlv ? false : true; set
            {
                _useArmaz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseArmaz)));
            }
        }

        private bool _isMvo;
        public bool IsMvo
        {
            get => _isMvo; set
            {
                _isMvo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMvo)));
            }
        }

        private bool _isNotMvo;
        public bool IsNotMvo
        {
            get => IsMvo ? true : false; set
            {
                _isNotMvo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotMvo)));
            }
        }

        private bool _isRecMvo;
        public bool IsRecMvo
        {
            get => _isRecMvo; set
            {
                _isRecMvo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRecMvo)));
            }
        }

        private bool _isNotRecMvo;
        public bool IsNotRecMvo
        {
            get => _isNotRecMvo; set
            {
                _isNotRecMvo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotRecMvo)));
            }
        }


        private string _operationMvo;
        public string OperationMvo
        {
            get => _operationMvo; set
            {
                _operationMvo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperationMvo)));
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

        private bool _cannotReadItems;
        public bool CannotReadItems
        {
            get => _cannotReadItems; set
            {
                _cannotReadItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CannotReadItems)));
            }
        }

        private string _statusEnc;
        public string StatusEnc
        {
            get => _statusEnc; set
            {
                _statusEnc = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusEnc)));
            }
        }

        private int _totalCaixas;
        public int TotalCaixas
        {
            get => _totalCaixas; set
            {
                _totalCaixas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCaixas)));
            }
        }

        private int _totalLeituras;
        public int TotalLeituras
        {
            get => _totalLeituras; set
            {
                _totalLeituras = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalLeituras)));
            }
        }

        private string _strTotalLeituras;
        public string StrTotalLeituras
        {
            get => _strTotalLeituras; set
            {
                _strTotalLeituras = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrTotalLeituras)));
            }
        }

        private string _strTotalCaixas;
        public string StrTotalCaixas
        {
            get => _strTotalCaixas; set
            {
                _strTotalCaixas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrTotalCaixas)));
            }
        }



        /// Leitura Acual
        /// 

        private bool _usarLote;
        public bool UsarLote
        {
            get => _usarLote; set
            {
                _usarLote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsarLote)));
            }
        }

        private string _loteActual;
        public string LoteActual
        {
            get => _loteActual; set
            {
                _loteActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoteActual)));
            }
        }

        private DateTime _loteValidadeActual;
        public DateTime LoteValidadeActual
        {
            get => _loteValidadeActual; set
            {
                _loteValidadeActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoteValidadeActual)));
            }
        }


        private DateTime _loteValidadeMin;
        public DateTime LoteValidadeMin
        {
            get => _loteValidadeMin; set
            {
                _loteValidadeMin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoteValidadeMin)));
            }
        }

        private string _alvActual;
        public string AlvActual
        {
            get => _alvActual; set
            {
                _alvActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlvActual)));
            }
        }

        private string _codigoActual;
        public string CodigoActual
        {
            get => _codigoActual; set
            {
                _codigoActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CodigoActual)));
            }
        }



        private int _qttActual;
        public int QttActual
        {
            get => _qttActual; set
            {
                _qttActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QttActual)));
            }
        }


        private bool _qttManual;
        public bool QttManual
        {
            get => _qttManual; set
            {
                _qttManual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QttManual)));
            }
        }

        private bool _useAutomacticAlveolo;
        public bool UseAutomacticAlveolo
        {
            get => _useAutomacticAlveolo; set
            {
                _useAutomacticAlveolo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseAutomacticAlveolo)));
            }
        }


        public ExpedicaoHMPharmaViewModel GenerateNewRec(OrderViewModel orderVM)
        {

            expVM = new ExpedicaoHMPharmaViewModel();
            expVM.Order = orderVM;
            expVM.BackOfficeIdEnc = orderVM.BackOfficeIdEnc;
            expVM.NomeEntidade = orderVM.Nome.Trim();
            expVM.NomeDoc = orderVM.NomeDoc.Trim();
            expVM.Ndos = orderVM.Ndos;
            expVM.NumDoc = orderVM.Numdoc;
            expVM.UseMvo = Helpers.Settings.UseMvo;
            expVM.UseAlv = Helpers.Settings.UseAlveolos;

            expVM.IsRecMvo = orderVM.IsMvo;
            expVM.IsNotRecMvo = !orderVM.IsMvo;
            expVM.QttManual = false;

            expVM.NumReq = orderVM.Requisicao;
            expVM.DataDoc = orderVM.DataEnc;
            expVM.DataEntrega = orderVM.DataEntrega;
            expVM.Obs = orderVM.Obs;

            expVM.ItemsPrev = orderVM.OrderDetail;

            return expVM;

        }

        public override LeiturasViewModel AddOrClose(LeiturasViewModel lvm)
        {
            var _api = new LeituraSaidasApi();

            try
            {

                lvm = _api.AddOrClose(lvm);

                if (lvm.HasError)
                    throw new LeituraException("Leitura inválida \n" + lvm.ErrorMsg, UtilsForMessage.TitleException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lvm;
        }

        public ExpedicaoViewModel Add(ExpedicaoViewModel entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
