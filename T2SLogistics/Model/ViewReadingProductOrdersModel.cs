using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class ViewReadingProductOrdersModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int Status{ get; set; }
        public int OrdersCount{ get; set; }
        public int ReadCount { get; set; }
        public int Lack{ get; set; }
    }
}
