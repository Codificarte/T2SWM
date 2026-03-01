using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class AddSeprationItemRequestModel
    {
        public int id { get; set; }
        public string phcOrderId { get; set; }
        public string phcOrderItemLinId { get; set; }
        public string productRef { get; set; }
        public string productDescription { get; set; }
        public int quantityPre { get; set; }
        public int quantitySet { get; set; }
        public int userId { get; set; }
        public string userCode { get; set; }
        public DateTime dateRegist { get; set; }
    }
}
