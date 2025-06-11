using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Models
{
    public class CodBarViewModel
    {
        public string Ref { get; set; }
        public string Description { get; set; }
        public string CodBar { get; set; }
        public int QttCodBar { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
