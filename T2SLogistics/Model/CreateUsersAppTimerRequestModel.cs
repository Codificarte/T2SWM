using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class CreateUsersAppTimerRequestModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string userCode { get; set; }
        public DateTime dateRegist { get; set; }
    }
}
