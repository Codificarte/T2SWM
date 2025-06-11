using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class LeituraEntradasApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string _urlAddEntradas = Helpers.Settings.UrlApiRegEntradas + "add";
        readonly string _urlCloseEntradas = Helpers.Settings.UrlApiRegEntradas + "close";
        readonly string _urlUpdateAlv = Helpers.Settings.UrlApiRegEntradas + "UpdateAlv";
        readonly string _urlPrintLabel = Helpers.Settings.UrlApiPrintLabel;

        readonly string _urlGetProducts = Helpers.Settings.UrlApiProducts;

        public LeituraEntradasApi()
        {

        }

        public LeiturasViewModel AddOrClose(LeiturasViewModel lvm)
        {
            string url;
            if (lvm.IsClosed)
                url = _urlCloseEntradas;

            else
                url = _urlAddEntradas;

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

                var _entrada = new LeiturasViewModel();
                _entrada = JsonConvert.DeserializeObject<LeiturasViewModel>(responseString);

                return _entrada;
            }

            var _resultError = new LeiturasViewModel();

            _resultError.HasError = true;
            return _resultError;

        }



        /// Get Information about ProductCode On Entries
        /// 

        public async Task<IEnumerable<ArtigosViewModel>> GetArtigosAsync(string productCode)
        {

            List<ArtigosViewModel> _list = new List<ArtigosViewModel>();

            try
            {

                var _url = _urlGetProducts + "Get?productCode=" + productCode.Trim();
                string resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<ArtigosViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;

        }


        public async Task<LeiturasViewModel> GetSusgestaoAlvAsync(LeiturasViewModel lvm)
        {

            var _url = Helpers.Settings.UrlApiRegEntradas + "GetSusgestaoAlv?_ref=" + lvm.Ref.ToString() + "&_stampLeitura=" + lvm.StampLeitura.Trim() + "&_qtt=" + lvm.Quanty.ToString() + "&_idBackOffice=" + lvm.BackOfficeIdEnc.ToString();

            if (lvm.BatchId != null)
                _url += "&_lote=" + lvm.BatchId.ToString();

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                var _list = JsonConvert.DeserializeObject<LeiturasViewModel>(resp);

                return _list;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void UpdateAlveolo(OrderDetailViewModel lvm, string stampLeitura, string newAlv)
        {

            string objSerialized = JsonConvert.SerializeObject(lvm);
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");
            string url = _urlUpdateAlv
                + "?stampLeitura=" + stampLeitura.Trim().ToString()
                + "&refPhc=" + lvm.Ref.Trim()
                + "&oldAlv=" + lvm.Alveolo.Trim()
                + "&newAlv=" + newAlv.Trim();

            var response = clientApi.PostAsync(url, content).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível actualizar este alvéolo!");

        }

        public void PrintLabel(LabelPrintViewModel lpVM)
        {
            string objSerialized = JsonConvert.SerializeObject(lpVM);
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");

            string url = _urlPrintLabel;
            var response = clientApi.PostAsync(url, content).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível imprimir a etiqueta!");
        }
    }
}
