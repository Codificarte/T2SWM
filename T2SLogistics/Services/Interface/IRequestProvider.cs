using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.Interface
{
    public interface IRequestProvider
    {
        Task<T> Delete<T>(string endpoint);
        Task<T> Put<T>(string endpoint, string jsonobject);
        Task<T> Post<T>(string endpoint, string jsonobject);
        Task<bool> PostAsync<T>(string url, string jsonobject);
        // POST que devolve status + corpo crus (não lança em não-2xx) — para distinguir 2xx de 422/404 e ler a mensagem.
        Task<HttpCallResult> PostWithStatus(string endpoint, string jsonobject);
        Task<bool> DeleteAsync(string endpoint);
        Task<T> PostFormContent<T>(string endpoint, FormUrlEncodedContent form);
        Task<T> Get<T>(string endpoint);
    }
}
