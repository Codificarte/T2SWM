using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Exceptions;
using T2SLogistics.Helpers;
using T2SLogistics.Services;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Models
{
    public abstract class RecepcaoMercadoria
    {

        //ArtigosApi _artigosApi = new ArtigosApi();
        LeituraEntradasApi _artigosApi = new LeituraEntradasApi();
        AlveolosApi alveolosApi = new AlveolosApi();

        public RecepcaoMercadoria()
        {

            ItemsRead = new ObservableCollection<LeiturasViewModel>();

            if (Helpers.Settings.UseMvo)
                OperationsMvo = GetOperationsMvo();

        }


        public OrderViewModel Order { get; set; }

        public ObservableCollection<LeiturasViewModel> ItemsRead { get; set; }
        public virtual ObservableCollection<OrderDetailViewModel> ItemsPrev { get; set; }

        public ObservableCollection<string> OperationsMvo { get; set; }

        public string StampLeitura { get; set; }
        public string BackOfficeIdEnc { get; set; } // Stamp Encomenda

        public string NomeEntidade { get; set; }
        public string NomeDoc { get; set; }
        public int Ndos { get; set; }

        public DateTime DataDoc { get; set; }
        public DateTime DataEntrega { get; set; }

        public int NumDoc { get; set; }

        public string NumReq { get; set; }

        public string Obs { get; set; }

        public bool UseMvo { get; set; }
        public bool UseAlv { get; set; }


        public static readonly int Open = 1; // Aguarda incio
        public static readonly int EmCurso = 2; // Já alguem pegou;
        public static readonly int Terminado = 3; // Operação terminada;
        public static readonly int Anulado = 4; // Processo anulado

        public static readonly int Entradas = 1;
        public static readonly int Saidas = 2;

        public string IconReadLeitura { get => "\ue039"; }



        /// <summary>
        /// Methods
        /// </summary>
        /// <returns></returns>

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


        public virtual IEnumerable<ArtigosViewModel> GetInfoArtigoInDb(string _productCode)
        {

            var _artigosInDb = Task.Run(async () => await _artigosApi.GetArtigosAsync(_productCode)).Result;

            return _artigosInDb;

        }


        public virtual LeiturasViewModel GetSusgestaoAlv(LeiturasViewModel _lvm)
        {

            var _leituraVM = Task.Run(async () => await _artigosApi.GetSusgestaoAlvAsync(_lvm)).Result;

            return _leituraVM;
        }

        public virtual AlveolosViewModel GetInfoAlveoloInDb(string alv)
        {

            try
            {
                var _alv = Task.Run(async () => await alveolosApi.GetAlveoloAsync(alv)).Result;

                return _alv;
            }
            catch (Exception)
            {
                return new AlveolosViewModel();
            }


        }



        public virtual LeiturasViewModel AddOrClose(LeiturasViewModel lvm)
        {

            var _api = new LeituraEntradasApi();

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



        public virtual void PrintLabel(string codBar, int qtt, string description)
        {

            var _api = new LeituraEntradasApi();
            var lpVM = new LabelPrintViewModel();

            lpVM.Qtt = qtt;
            lpVM.CodBar = codBar;
            lpVM.Description = description;
            lpVM.DateReg = DateTime.Now;

            try
            {

                _api.PrintLabel(lpVM);

            }
            catch (Exception ex)
            {
                throw new LeituraException("Ocorreu um erro ao tentar imprimir a etiqueta\n", UtilsForMessage.TitleException);
            }


        }

        public void UpdateAlveolo(OrderDetailViewModel lvm, string stampLeitura, string newAlv)
        {

            var _api = new LeituraEntradasApi();

            try
            {

                _api.UpdateAlveolo(lvm, stampLeitura, newAlv);

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível actualizar este alvéolo!");
            }


        }






    }
}
