using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.ViewModels.Expedicao
{
    public class CaixaViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public CaixaViewModel()
        {
            ItemsRead = new ObservableCollection<LeiturasViewModel>();
        }

        public int IdCaixa { get; set; }
        public string NomeCaixa { get; set; }

        public int NumCaixa { get; set; }
        public string Medidas { get; set; }

        public string NomeNumEnc { get; set; }


        private int _totalProdutos;
        public int TotalProdutos
        {
            get => _totalProdutos; set
            {
                _totalProdutos = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalProdutos)));
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


        private bool _qttIsReadOnly;
        public bool QttIsReadOnly
        {
            get => _qttIsReadOnly; set
            {
                _qttIsReadOnly = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QttIsReadOnly)));
            }
        }

        public ObservableCollection<LeiturasViewModel> ItemsRead { get; set; }

    }
}
