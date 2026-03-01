using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class AddExpeditionReadItemResponseModel
    {
        public int id { get; set; }
        public string itemReadId { get; set; }
        public string phcOrderId { get; set; }
        public string phcOrderItemLinId { get; set; }
        public string productRef { get; set; }
        public string productDescription { get; set; }
        public int boxNr { get; set; }
        public string eanCode { get; set; }
        public int quantity { get; set; }
        public object userCode { get; set; }
        public DateTime dateRegist { get; set; }
    }
}
