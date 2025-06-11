using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class LeituraInventariosApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string _urlAddLeituraInv = Helpers.Settings.UrlApiRegInventarios + "add";
        readonly string _urlCloseLeituras = Helpers.Settings.UrlApiRegInventarios + "close";


        public LeituraInventariosApi()
        {

        }

        public LeituraInventarioViewModel AddOrClose(LeituraInventarioViewModel lvm)
        {
            string url;
            if (lvm.IsClosed)
                url = _urlCloseLeituras;

            else
                url = _urlAddLeituraInv;


            lvm = AddOrCloseInApi(lvm, url);

            return lvm;
        }

        private LeituraInventarioViewModel AddOrCloseInApi(LeituraInventarioViewModel lvm, string url)
        {

            string objSerialized = JsonConvert.SerializeObject(lvm);
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");

            var response = clientApi.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                string responseString = responseContent.ReadAsStringAsync().Result;

                var _inv = new LeituraInventarioViewModel();
                _inv = JsonConvert.DeserializeObject<LeituraInventarioViewModel>(responseString);

                return _inv;
            }

            var _resultError = new LeituraInventarioViewModel();

            _resultError.HasError = true;
            return _resultError;

        }



    }
}
