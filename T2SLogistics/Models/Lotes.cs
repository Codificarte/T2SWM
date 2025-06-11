using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Models
{
    public class Lotes
    {

        public string BackOfficeId { get; set; }

        public string BatchCode { get; set; }
        public string Ref { get; set; }

        public string Description { get; set; }

        public DateTime BatchExpire { get; set; }
        public DateTime DataFabrico { get; set; }

        public decimal Stock { get; set; }

    }
}
