using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class AddItemBreaksRequestModel
    {
        public int id { get; set; }
        public string eanCode { get; set; }
        public string productRef { get; set; }
        public string productDescription { get; set; }
        public int quantity { get; set; }
        public string nameDescription { get; set; }
        public string userCode { get; set; }
        public DateTime dateRegist { get; set; }
    }
}
