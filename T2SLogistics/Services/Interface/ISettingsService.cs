using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.Interface
{
    public interface ISettingsService
    {
        string UserId { get; set; }
        string AuthToken { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string BaseUrl { get; set; }
        string UserCode { get; set; }
        string Applanguage { get; set; }

    }
}
