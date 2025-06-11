using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Interface;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels.Recepcao
{
    public class RecSandaloViewModel : RecepcaoMercadoria, INotifyPropertyChanged, IRecepcaoMercadoria<RecSandaloViewModel>
    {

        public event PropertyChangedEventHandler PropertyChanged;

        RecSandaloViewModel recVM;

        public RecSandaloViewModel()
        {

        }

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
            get => _useArmaz; set
            {
                _useArmaz = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseArmaz)));
            }
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy; set
            {
                _isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
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
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>

        public RecSandaloViewModel Add(RecSandaloViewModel entity)
        {
            throw new System.NotImplementedException();
        }

        public RecSandaloViewModel GenerateNewRec(OrderViewModel orderVM)
        {

            recVM = new RecSandaloViewModel();
            recVM.Order = orderVM;
            recVM.BackOfficeIdEnc = orderVM.BackOfficeIdEnc;
            recVM.NomeEntidade = orderVM.Nome;
            recVM.NomeDoc = orderVM.NomeDoc;
            recVM.Ndos = orderVM.Ndos;
            recVM.NumDoc = orderVM.Numdoc;
            recVM.UseMvo = Helpers.Settings.UseMvo;
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
