using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class LeituraSaidasApi
    {


        HttpClient clientApi = new HttpClient();
        readonly string _urlAdd = Helpers.Settings.UrlApiRegSaidas + "add";
        readonly string _urlClose = Helpers.Settings.UrlApiRegSaidas + "close";


        public LeiturasViewModel AddOrClose(LeiturasViewModel lvm)
        {
            string url;
            if (lvm.IsClosed)
                url = _urlClose;

            else
                url = _urlAdd;


            lvm = AddOrCloseInApi(lvm, url);

            return lvm;
        }

        private LeiturasViewModel AddOrCloseInApi(LeiturasViewModel lvm, string url)
        {

            string objSerialized = JsonConvert.SerializeObject(lvm);
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");

            var response = clientApi.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                var _saida = new LeiturasViewModel();
                _saida = JsonConvert.DeserializeObject<LeiturasViewModel>(responseString);

                return _saida;
            }

            var _resultError = new LeiturasViewModel();

            _resultError.HasError = true;
            return _resultError;

        }

    }
}
