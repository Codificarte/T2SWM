using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Exceptions;
using T2SLogistics.Helpers;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public OrderViewModel()
        {
            //MaxWidth = Convert.ToInt32(Settings.MaxWidth);
            ItemsRead = new ObservableCollection<LeiturasViewModel>();
            Suppliers = new ObservableCollection<SupplierViewModel>();

            if (Helpers.Settings.UseMvo)
                OperationsMvo = GetOperationsMvo();

            UseMvo = Helpers.Settings.UseMvo;
            UseAlv = Helpers.Settings.UseAlveolos;
            IsMvo = false;
            IsNotMvo = true;

        }

        private ObservableCollection<string> GetOperationsMvo()
        {
            OperationsMvo = new ObservableCollection<string>
            {
                "G110 - Verificação",
                "G120 - Dispensar",
                "G140 - Exportação",
                "G150 - Amostras",
                "G170 - Bloquear",
                "G180 - Roubos"
            };

            return OperationsMvo;
        }

        public int Id { get; set; }

        public string BackOfficeIdEnc { get; set; } // Stamp Encomenda

        public int Numdoc { get; set; }
        public int Ndos { get; set; }

        public string NomeDoc { get; set; }

        public string NomeNumEnc { get; set; }

        public DateTime DataEnc { get; set; }

        public DateTime DataEntrega { get; set; }


        public string Armazem { get; set; }
        public string Requisicao { get; set; }
        public string Obs { get; set; }

        public string StrDataEnc
        {
            get => DataEnc.ToString("dd-MM-yyyy");
        }

        public bool IsClosed { get; set; }
        public bool IsAnulado { get; set; }

        public int OrderType { get; set; }   // 1 - Entradas  2 - Saídas

        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }

        public string StampLeitura { get; set; }    // Stamp do novo processo no leitor        
        public int OrdersId { get; set; }
        public string IdEntidade { get; set; }  //Nº ou stamp Cliente / Fornecedor
        public string TipoEntidade { get; set; }    // 1 - Fornecedores ; 2 - Clientes ; 3 - Entidades

        public bool Deleted { get; set; }
        //public int MaxWidth { get; set; }

        public Color ItemBgColor { get; set; }

        public virtual ObservableCollection<OrderDetailViewModel> OrderDetail { get; set; }
        public ObservableCollection<LeiturasViewModel> ItemsRead { get; set; }

        public ObservableCollection<AlveolosViewModel> Alveolos { get; set; }

        public ObservableCollection<SupplierViewModel> Suppliers { get; set; }
        public ObservableCollection<SupplierViewModel> SuppliersFilter { get; set; }

        public ObservableCollection<string> OperationsMvo { get; set; }

        public bool UseMvo { get; set; }
        public bool UseAlv { get; set; }
        //public string PdfFilePath { get; set; }

        private bool _useArmaz;
        public bool UseArmaz
        {
            get => UseAlv ? false : true; set
            {
                _useArmaz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseArmaz)));
            }
        }



        private string _pdfFilePath;
        public string PdfFilePath
        {
            get => _pdfFilePath; set
            {
                _pdfFilePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PdfFilePath)));
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


        private string _alvActual;
        public string AlvActual
        {
            get => _alvActual; set
            {
                _alvActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlvActual)));
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



        private string _leituraActual;
        public string LeituraActual
        {
            get => _leituraActual; set
            {
                _leituraActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeituraActual)));
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


        private string _titlePage;
        public string TitlePage
        {
            get => _titlePage; set
            {
                _titlePage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitlePage)));
            }
        }


        public static readonly int Open = 1; // Aguarda incio
        public static readonly int EmCurso = 2; // Já alguem pegou;
        public static readonly int Terminado = 3; // Operação terminada;
        public static readonly int Anulado = 4; // Processo anulado

        public static readonly int Entradas = 1;
        public static readonly int Saidas = 2;

        public string IconReadLeitura { get => "\ue039"; }



        private string _nome;
        public string Nome
        {
            get => _nome; set
            {
                _nome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nome)));
            }
        }



        public char FirsCharName
        {
            get => !string.IsNullOrWhiteSpace(Nome) ? Nome.ToCharArray()[0] : ' ';
        }

        public Color BackGroundColor
        {
            get => AppConstant.GetRandomColor();
        }


        public virtual async Task<IEnumerable<OrderViewModel>> GetRemoteOrdersFromApi()
        {

            var _ordersApi = new OrdersApi();
            var _ordersTask = _ordersApi.GetRemoteDataAsync();

            var _orders = await _ordersTask;

            return _orders;

        }


        public LeiturasViewModel AddEntradas(LeiturasViewModel lvm)
        {

            var _entradasApi = new LeituraEntradasApi();

            try
            {

                lvm = _entradasApi.AddOrClose(lvm);

                if (lvm.HasError)
                    throw new LeituraException("Leitura inválida \n" + lvm.ErrorMsg, UtilsForMessage.TitleException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lvm;

        }


        public LeiturasViewModel AddSaidas(LeiturasViewModel lvm)
        {

            var _saidasApi = new LeituraSaidasApi();

            try
            {

                lvm = _saidasApi.AddOrClose(lvm);

                if (lvm.HasError)
                    throw new LeituraException("Leitura inválida \n" + lvm.ErrorMsg, UtilsForMessage.TitleException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lvm;

        }


        public void PrintDocA4(OrderViewModel orderVM)
        {

            var _api = new OrdersApi();

            if (!string.IsNullOrEmpty(orderVM.PdfFilePath))
                _api.PrintDocA4(orderVM.PdfFilePath);


        }

    }
}
