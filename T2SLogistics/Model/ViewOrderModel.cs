using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class ViewOrderModel
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderDescription { get; set; }
        public bool IsAvaliable { get; set; }
    }
}
