using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.Dtos
{
    public class FornecedoresCaiacaDto
    {
        public string Id { get; set; }

        public int Num { get; set; }

        public string Nome { get; set; }

        public string Nome2 { get; set; }

        public bool Use2CodBar { get; set; }
        public int TipoMP { get; set; }  // Papel / Colas Verniz / Diversos


        public int NCodBarInfo { get; set; }
        public int NCodBarBobina { get; set; }
        public int PosInicio { get; set; }

        public string P1Tipo { get; set; }
        public int P1Val { get; set; }

        public string P2Tipo { get; set; }
        public int P2Val { get; set; }

        public string P3Tipo { get; set; }
        public int P3Val { get; set; }

        public string P4Tipo { get; set; }
        public int P4Val { get; set; }

        public string P5Tipo { get; set; }
        public int P5Val { get; set; }


        public bool Fechado { get; set; }
        public bool Anulado { get; set; }


        public List<ArtigosCaiaca> Artigos { get; set; }

    }
}
