using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.ViewModels
{
    public class UserAppViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool ResetPwd { get; set; }
        public string Token { get; set; }
        public bool Inactivo { get; set; }
    }
}
