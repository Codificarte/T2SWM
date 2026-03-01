using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class ResetPasswordRequestModel
    {
        public string email { get; set; }
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}
