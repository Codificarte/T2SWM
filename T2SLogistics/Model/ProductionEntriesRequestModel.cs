using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class ProductionEntriesRequestModel
    {
        public string refProd { get; set; }
        public string description { get; set; }
        public int quanty { get; set; }
        public int quantyBreak { get; set; }
        public string userCode { get; set; }
        public string stampLinOrderProd { get; set; }
        public string operationName { get; set; }
    }
}
