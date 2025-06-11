using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Exceptions;
using T2SLogistics.Helpers;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels
{
    public class LeiturasViewModel : LeituraValidator, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public LeiturasViewModel()
        {
            Lotes = new ObservableCollection<Lotes>();
            BatchExpire = new DateTime(1900, 1, 1);

        }

        public int Id { get; set; }

        public string Ref { get; set; }

        public string Description { get; set; }

        public bool UseBatch { get; set; }
        public bool NotUseBatch
        {
            get => UseBatch ? false : true;
        }

        public string IconUseBatch { get; set; }
        public string IconUseBatchColor { get => "Green"; }

        public string BatchId { get; set; }

        public string CodeReader { get; set; }

        public int NumCaixaToPL { get; set; }

        public int Quanty { get; set; }

        public bool Cativo { get; set; }

        public string NomeAlv { get; set; }
        public string StampAlv { get; set; }
        public int NumArmazem { get; set; }

        public int OrderId { get; set; }
        public int Ndos { get; set; }

        public bool IsDevolucao { get; set; }

        public bool HasAutomaticSugestAlv { get; set; } // Se tem/usa sugestão automática de alvéolos

        public string IdItemRead { get; set; }      // Guardar id por cada leitura (caso precise alterar algo já lido)
        public string StampLeitura { get; set; }    // Stamp do novo processo no leitor
        public string BackOfficeIdEnc { get; set; } // Stamp Encomenda
        public string IdBackOffice { get; set; } // Stamp Lin Encomenda

        public string Alveolo { get; set; }

        public string IdEntidade { get; set; }  //Nº ou stamp Cliente / Fornecedor
        public string TipoEntidade { get; set; }    // 1 - Fornecedores ; 2 - Clientes ; 3 - Entidades

        public int IdUser { get; set; }
        public string UserName { get; set; }

        public bool HasError { get; set; }
        public string ErrorMsg { get; set; }
        public string MsgAddDoc { get; set; }
        public int IdDocServer { get; set; }

        public string FullCode { get; set; }

        public bool IsMvo { get; set; }
        public bool IsNotMvo { get => IsMvo ? false : true; }

        public string OperationType { get; set; }
        public string TitleOperation { get; set; }

        public string ProductCode { get; set; }
        public string PackSerialNr { get; set; }

        public string LocalizacaoHab { get; set; }

        public bool HasProcCalcValidade { get; set; }
        public int YearsToExpire { get; set; }
        public decimal PercentageAcceptable { get; set; }

        public string NumDocFornec { get; set; }
        public string ObsArmazem { get; set; }

        private DateTime _batchExpire;
        public DateTime BatchExpire
        {
            get => _batchExpire; set
            {
                _batchExpire = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BatchExpire)));
            }
        }

        private string _batchExpDate;
        public string BatchExpDate
        {
            get => BatchExpire.ToString("dd-MM-yyyy"); set
            {
                _batchExpDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BatchExpDate)));
            }
        }


        public string NrReembolso { get; set; }

        public bool IsClosed { get; set; }
        public bool FromMobile { get; set; }

        public DateTime DataMov { get; set; }

        public string IconImgStatus { get; set; }
        public string IconImgcolor { get; set; }

        public int TryCountRead { get; set; } // contar quantas vezes tentei ; validar se vou ver dados à BD novamente

        public int OrdersId { get; set; }
        public int OrderType { get; set; }      // 1 - Entradas ; 2 - Saídas ; 3 - Inventário

        public string UserCode { get; set; }

        public int IdStatusLeitura { get; set; }
        public string MsgStatusLeitura { get; set; }


        private bool _isConferido;
        public bool IsConferido
        {
            get => _isConferido; set
            {
                _isConferido = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConferido)));
            }
        }

        public static readonly int CodigoValido = 0;
        public static readonly int CodigoNaoencontrado = 1;
        public static readonly int CodigoAssociadoMaisQ1Ref = 2;

        public ObservableCollection<Lotes> Lotes { get; set; }

        public Color ItemBgColor
        {
            get => Id == 1 ? Colors.WhiteSmoke : (Id == 2 ? Colors.White : Id % 2 == 0 ? Colors.White : Colors.WhiteSmoke);
        }


        public static DateTime GetBatchExpireDate(string _expDate)
        {
            if (_expDate.Length == 6)
                return new DateTime(Convert.ToInt32("20" + _expDate.Substring(0, 2).ToString()), Convert.ToInt32(_expDate.Substring(2, 2).ToString()), Convert.ToInt32(_expDate.Substring(4, 2).ToString()));

            return new DateTime(1900, 1, 1);

        }

        public override void PrepareToSave()
        {
            throw new NotImplementedException();
        }

        public override LeiturasViewModel LeituraIsValid(List<ArtigosViewModel> _listArtigos, OrderViewModel _orderVM, LeiturasViewModel _leitura)
        {

            if (_listArtigos.Count() > 1)
                throw new LeituraException("Código associado a mais do que uma referência!", UtilsForMessage.TitleException);


            if (_listArtigos.Count() == 0)
                throw new LeituraException("Código desconhecido!", UtilsForMessage.TitleException);


            var _artigosVM = _listArtigos.ToList()[0];

            if (_artigosVM.Ref == null)
                throw new LeituraException("Código desconhecido!", UtilsForMessage.TitleException);

            if (_orderVM.QttActual <= 0 || _orderVM.QttActual.ToString() == _artigosVM.Ref.ToString())
                throw new LeituraQttException("Indicar a quantidade!", UtilsForMessage.TitleException);


            if (_leitura.Lotes.Count == 0)
                foreach (var item in _artigosVM.Batchs)
                    _leitura.Lotes.Add(item);



            if (_leitura.BatchId == null)
            {
                if (_artigosVM.BatchId != null)
                {
                    _orderVM.LoteActual = _artigosVM.BatchId;
                    _leitura.BatchId = _artigosVM.BatchId;
                }

            }

            _leitura.UseBatch = _artigosVM.UseBatch;
            _leitura.Ref = _artigosVM.Ref;
            _leitura.Description = _artigosVM.Description;
            _leitura.Quanty = _orderVM.QttActual;

            if (String.IsNullOrEmpty(_leitura.ProductCode))
                _leitura.ProductCode = _artigosVM.Ref;

            if (_leitura.UseBatch && !string.IsNullOrEmpty(_orderVM.LoteActual))
            {
                var _lote = _artigosVM.Batchs.Where(l => l.BatchCode.Trim().ToUpper() == _orderVM.LoteActual.Trim().ToUpper()).FirstOrDefault();

                if (_lote != null)
                    _leitura.BatchExpire = _lote.BatchExpire;

            }


            return _leitura;

        }

        public static readonly int RecepcaoMercadoria = 1;
        public static readonly int ExpedicaoMercadoria = 2;
        public static readonly int Inventario = 3;

    }
}
