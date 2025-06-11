using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class ArtigosViewModel
    {

        ArtigosApi _artigosApi = new ArtigosApi();

        public ArtigosViewModel()
        {
            Batchs = new List<Lotes>();
            // CodBarras = new ObservableCollection<Models.CodBarViewModel>();    
        }

        public int Id { get; set; }

        public string BackOfficeId { get; set; }
        public string Ref { get; set; }
        public string Description { get; set; }

        public string CodBar { get; set; }
        public int QttCodBar { get; set; }

        public string LocalizacaoHab { get; set; }

        public bool IsService { get; set; }
        public bool UseBatch { get; set; }
        public bool UseSerialNr { get; set; }

        public bool SujReceitaMedica { get; set; }

        public decimal SellPrice1 { get; set; }
        public decimal SellPrice2 { get; set; }
        public decimal SellPrice3 { get; set; }

        public decimal LastPrice { get; set; }
        public decimal PricePond { get; set; }
        public decimal ReferencePrice { get; set; }

        public string FamilyName { get; set; }
        public string SubFamilyName { get; set; }

        public string UniType { get; set; }

        public decimal Tax { get; set; }

        public string CodeIseTax { get; set; }
        public string DescrIseTax { get; set; }

        public List<Lotes> Batchs { get; set; }
        public List<Alveolos> Alveolos { get; set; }

        //public ObservableCollection<Models.CodBarViewModel> CodBarras { get; set; }

        public string BatchId { get; set; }
        public DateTime BatchExp { get; set; }

        public bool HasProcCalcValidade { get; set; }
        public int YearsToExpire { get; set; }
        public decimal PercentageAcceptable { get; set; }

        public bool SugereLocalizacao { get; set; }



    }
}
