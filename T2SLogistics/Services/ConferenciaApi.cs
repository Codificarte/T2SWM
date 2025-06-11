using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class ConferenciaApi
    {
        HttpClient clientApi = new HttpClient();

        readonly string _urlAddOrUpdateConf = Helpers.Settings.UrlApiConferenciasStock + "addorupdate";

        public ConferenciaApi()
        {

        }

        public ConferenciaViewModel AddOrUpdate(ConferenciaViewModel lvm)
        {

            lvm = AddOrUpdateInApi(lvm, _urlAddOrUpdateConf);

            return lvm;
        }

        private ConferenciaViewModel AddOrUpdateInApi(ConferenciaViewModel lvm, string url)
        {

            string objSerialized = JsonConvert.SerializeObject(lvm);
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");

            var response = clientApi.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                var _conf = new ConferenciaViewModel();
                _conf = JsonConvert.DeserializeObject<ConferenciaViewModel>(responseString);

                return _conf;
            }

            var _resultError = new ConferenciaViewModel();

            _resultError.HasError = true;
            return _resultError;

        }

    }
}
