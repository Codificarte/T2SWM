using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels
{
    public class LeituraInventarioViewModel
    {
        public LeituraInventarioViewModel()
        {
            Lotes = new List<Lotes>();
        }


        public int Id { get; set; }

        public string Ref { get; set; }
        public string Description { get; set; }
        public decimal Qtt { get; set; }

        public bool UseBatch { get; set; }

        public bool NotUseBatch
        {
            get => UseBatch ? false : true;
        }


        public string BatchId { get; set; }
        public DateTime Validade { get; set; }
        public string BatchExpDate
        {
            get => Validade.ToString("dd-MM-yyyy");
        }

        public string Alveolo { get; set; }

        public string StampLeitura { get; set; }    // Stamp do novo processo no leitor        

        public List<Lotes> Lotes { get; set; }

        public string NomeInventario { get; set; }
        public DateTime DataInventario { get; set; }

        public string Armazem { get; set; }


        public int OrderType { get => OrderTypeInventario; }      // 1 - Entradas ; 2 - Saídas ; 3 - Inventário

        public bool IsClosed { get; set; }
        public bool HasError { get; set; }
        public string ErrorMsg { get; set; }
        public string MsgAddDoc { get; set; }
        public int IdDocServer { get; set; }

        public static readonly int OrderTypeInventario = 3;

    }
}
