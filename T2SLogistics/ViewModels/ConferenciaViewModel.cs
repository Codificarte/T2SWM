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
    public class ConferenciaViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ConferenciaViewModel()
        {
            ItemsRead = new ObservableCollection<LeiturasViewModel>();
        }

        public string IdConferencia { get; set; }
        public string IdParent { get; set; }

        public int QttPedido { get; set; }

        private string _description;
        public string Description
        {
            get => _description; set
            {
                _description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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

        private string _validadeLote;
        public string ValidadeLote
        {
            get => _validadeLote; set
            {
                _validadeLote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidadeLote)));
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

        private string _codigoActual;
        public string CodigoActual
        {
            get => _codigoActual; set
            {
                _codigoActual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CodigoActual)));
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


        private bool _isConferido;
        public bool IsConferido
        {
            get => _isConferido; set
            {
                _isConferido = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConferido)));
            }
        }


        public bool Deleted { get; set; }
        public bool Completed { get; set; }

        public bool HasError { get; set; }
        public string ErrorMsg { get; set; }

        public ObservableCollection<LeiturasViewModel> ItemsRead { get; set; }


        public virtual ConferenciaViewModel AddOrUpdate(ConferenciaViewModel lvm)
        {

            var _api = new ConferenciaApi();

            try
            {

                lvm = _api.AddOrUpdate(lvm);

                if (lvm.HasError)
                    throw new LeituraException("Leitura inválida \n" + lvm.ErrorMsg, UtilsForMessage.TitleException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lvm;

        }

    }
}
