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
    public class InventarioViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        public InventarioViewModel()
        {
            ItemsInventario = new ObservableCollection<LeituraInventarioViewModel>();

        }



        public DateTime DataInventario { get; set; }


        public decimal QttActual { get; set; }

        public string StampLeitura { get; set; }    // Stamp do novo processo no leitor        

        public int OrderType { get => LeituraInventarioViewModel.OrderTypeInventario; }      // 1 - Entradas ; 2 - Saídas ; 3 - Inventário

        public string IconReadLeitura { get => "\ue039"; }

        public ObservableCollection<LeituraInventarioViewModel> ItemsInventario { get; set; }

        private string _nomeInventario;
        public string NomeInventario
        {
            get => _nomeInventario; set
            {
                _nomeInventario = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomeInventario)));
            }
        }

        private string _alveolo;
        public string Alveolo
        {
            get => _alveolo; set
            {
                _alveolo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Alveolo)));
            }
        }


        private string _armazem;
        public string Armazem
        {
            get => _armazem; set
            {
                _armazem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Armazem)));
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

        private bool _naoUsarLote;
        public bool NaoUsarLote
        {
            get => UsarLote ? false : true; set
            {
                _naoUsarLote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NaoUsarLote)));
            }
        }


        private bool _usarAlveolos;
        public bool UsarAlveolos
        {
            get => Helpers.Settings.UseAlveolos; set
            {
                _usarAlveolos = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsarAlveolos)));
            }
        }


        private bool _usarArmazem;
        public bool UsarArmazem
        {
            get => Helpers.Settings.UseAlveolos ? false : true; set
            {
                _usarArmazem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsarArmazem)));
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

        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="inv"></param>
        /// 


        public LeituraInventarioViewModel AddOrClose(LeituraInventarioViewModel inv)
        {

            if (inv.IsClosed)
                return AddOrCloseToApi(inv);

            if (inv.Ref.Length > 18 || string.IsNullOrEmpty(inv.Ref))
                throw new LeituraRefException("Código inválido!", UtilsForMessage.TitleException);

            if (inv.UseBatch && String.IsNullOrEmpty(inv.BatchId))
                throw new LeituraLotesException("Verifique o lote!", UtilsForMessage.TitleException);

            if (inv.Qtt <= 0)
                throw new LeituraRefException("Verifique as Quantidades!", UtilsForMessage.TitleException);


            inv = AddOrCloseToApi(inv);

            return inv;

        }


        private LeituraInventarioViewModel AddOrCloseToApi(LeituraInventarioViewModel inv)
        {

            var _invApi = new LeituraInventariosApi();

            try
            {

                inv = _invApi.AddOrClose(inv);

                if (inv.HasError)
                    throw new LeituraException("Leitura inválida \n" + inv.ErrorMsg, UtilsForMessage.TitleException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return inv;

        }


    }
}

