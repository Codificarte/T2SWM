using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.ViewModels
{
    public class LabelPrintViewModel
    {

        public int Id { get; set; }
        public string Ref { get; set; }
        public string Description { get; set; }
        public string CodBar { get; set; }
        public int Qtt { get; set; }

        public DateTime DateReg { get; set; }

        public bool IsPrinted { get; set; }

    }
}
