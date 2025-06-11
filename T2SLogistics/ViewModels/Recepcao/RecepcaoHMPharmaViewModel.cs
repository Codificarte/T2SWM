using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels.Recepcao
{
    public class RecepcaoHMPharmaViewModel : RecepcaoMercadoria, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        RecepcaoHMPharmaViewModel recVM;

        public RecepcaoHMPharmaViewModel()
        {
            ItemsConferidos = new ObservableCollection<LeiturasViewModel>();
        }


        public ObservableCollection<LeiturasViewModel> ItemsConferidos { get; set; }

        public string NomeNumEnc
        {
            get => NomeDoc + " Nº " + NumDoc;
        }


        public string StrDataEnc
        {
            get => DataDoc.ToString("dd-MM-yyyy");
        }

        private bool _useArmaz;
        public bool UseArmaz
        {
            get => UseAlv ? false : true; set
            {
                _useArmaz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseArmaz)));
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

        //private bool _canEditQtt;
        //public bool CanEditQtt
        //{
        //    get => (IsRecMvo == false && QttManual == true) ? true : false; set
        //    {
        //        _canEditQtt = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanEditQtt)));
        //    }
        //}



        private bool _qttIsReadOnly;
        public bool QttIsReadOnly
        {
            get => _qttIsReadOnly; set
            {
                _qttIsReadOnly = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QttIsReadOnly)));
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

        private bool _notUseLote;
        public bool NotUseLote
        {
            get => _notUseLote; set
            {
                _notUseLote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotUseLote)));
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


        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>


        public RecepcaoHMPharmaViewModel GenerateNewRec(OrderViewModel orderVM)
        {
            recVM = new RecepcaoHMPharmaViewModel();
            recVM.Order = orderVM;
            recVM.BackOfficeIdEnc = orderVM.BackOfficeIdEnc;
            recVM.NomeEntidade = orderVM.Nome;
            recVM.NomeDoc = orderVM.NomeDoc;
            recVM.Ndos = orderVM.Ndos;
            recVM.NumDoc = orderVM.Numdoc;
            recVM.IsRecMvo = orderVM.IsMvo;
            recVM.IsNotRecMvo = !orderVM.IsMvo;
            recVM.QttManual = false;
            //recVM.QttIsReadOnly = true;
            recVM.UseAlv = Helpers.Settings.UseAlveolos;
            recVM.NumReq = orderVM.Requisicao;
            recVM.DataDoc = orderVM.DataEnc;
            recVM.DataEntrega = orderVM.DataEntrega;
            recVM.Obs = orderVM.Obs;
            recVM.ItemsPrev = orderVM.OrderDetail;

            return recVM;
        }
    }
}
