using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class AuthDataModel
    {
        public string token { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public bool mustChangePassword { get; set; }
        public string message { get; set; }
    }

}
