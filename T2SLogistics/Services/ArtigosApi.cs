using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Dtos;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class ArtigosApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string url = Helpers.Settings.UrlApiProducts;


        public async Task<IEnumerable<ArtigosCodBarViewModel>> GetAllArtigosByCodBar(string codbar)
        {

            List<ArtigosCodBarViewModel> _list = new List<ArtigosCodBarViewModel>();

            try
            {

                var _url = url + "GetRefsByCodBar?codbar=" + codbar.Trim();
                string resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<ArtigosCodBarViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;

        }

        public async Task<IEnumerable<ArtigosCodBarViewModel>> GetAllCodBarByProductCode(string productCode)
        {

            List<ArtigosCodBarViewModel> _list = new List<ArtigosCodBarViewModel>();

            try
            {

                var _url = url + "GetCodBarsByRef?productCode=" + productCode.Trim();
                string resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<ArtigosCodBarViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;

        }

        public async Task<IEnumerable<ArtigosViewModel>> GetArtigosAsync(string productCode)
        {

            List<ArtigosViewModel> _list = new List<ArtigosViewModel>();

            try
            {

                var _url = url + "Get?productCode=" + productCode.Trim();
                string resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<ArtigosViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;

        }


        public async Task<LeiturasViewModel> GetArtigosAsync(LeiturasViewModel lvm)
        {

            var _url = url + "GetInfoLeitura?_ref=" + lvm.Ref.ToString() + "&_stampLeitura=" + lvm.StampLeitura.Trim() + "&_qtt=" + lvm.Quanty.ToString();

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


        public CodBarViewModel AddNewCodBar(CodBarViewModel refVM)
        {

            var _url = url + "AddNewCodBar";

            try
            {

                string objSerialized = JsonConvert.SerializeObject(refVM);
                HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");


                var response = clientApi.PostAsync(_url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    CodBarViewModel _result;
                    _result = JsonConvert.DeserializeObject<CodBarViewModel>(responseString);

                    return _result;
                }


                throw new Exception("");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar código de barras!\n" + ex.Message);
            }

        }

        public void RemoveCodbar(string _ref, string _codbar)
        {

            var objCodbar = new CodebarRemoveDto { Ref = _ref, Codbar = _codbar };

            var _url = url + "RemoveCodbar";

            try
            {

                string objSerialized = JsonConvert.SerializeObject(objCodbar);
                HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");

                var response = clientApi.PostAsync(_url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    CodBarViewModel _result;
                    _result = JsonConvert.DeserializeObject<CodBarViewModel>(responseString);

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar código de barras!\n" + ex.Message);
            }

        }
    }
}
