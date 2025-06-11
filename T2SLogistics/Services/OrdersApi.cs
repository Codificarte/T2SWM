using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class OrdersApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string url = Helpers.Settings.UrlApiOrders;

        public async Task<IEnumerable<OrderViewModel>> GetRemoteDataAsync()
        {
            var _listOrders = new List<OrderViewModel>();
            var _url = url + "GetAll";

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _listOrders = JsonConvert.DeserializeObject<List<OrderViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _listOrders;
        }

        public async Task<IEnumerable<OrderDetailViewModel>> GetRemoteOrderDetailsFromApi(string idBackoffice)
        {
            var _listOrders = new List<OrderDetailViewModel>();
            var _url = url + "GetDetails?backOfficeId=" + idBackoffice.Trim().ToString();

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _listOrders = JsonConvert.DeserializeObject<List<OrderDetailViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _listOrders;
        }

        public void SetLeituraPendIsClosed(string idBackOffice)
        {

            string objSerialized = JsonConvert.SerializeObject("");
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");
            string _url = url + "CloseLeituraPend?backOfficeId=" + idBackOffice.Trim().ToString();
            // string _url = url + "CloseLeituraPend";


            var response = clientApi.PostAsync(_url, content).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível actualizar as leituras!");


        }

        public async Task<IEnumerable<LeiturasViewModel>> GetRemoteLeiturasFeitasFromApi(string idBackoffice)
        {
            var _listOrders = new List<LeiturasViewModel>();
            var _url = url + "GetOpenItemsRead?backOfficeId=" + idBackoffice.Trim().ToString();

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _listOrders = JsonConvert.DeserializeObject<List<LeiturasViewModel>>(resp);

            }
            catch (Exception ex)
            {
                return new List<LeiturasViewModel>();
            }

            return _listOrders;
        }


        public void PrintDocA4(string fileNamePath)
        {

            string objSerialized = JsonConvert.SerializeObject("");
            HttpContent content = new StringContent(objSerialized, Encoding.UTF8, "application/json");
            string _url = Helpers.Settings.UrlApiPrintA4 + fileNamePath.Trim().ToString();


            var response = clientApi.PostAsync(_url, content).Result;

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível actualizar as leituras!");


        }
    }
}
