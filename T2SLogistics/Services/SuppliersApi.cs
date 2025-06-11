using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class SuppliersApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string url = Helpers.Settings.UrlApiSuppliers;

        public SuppliersApi()
        {

        }

        public async Task<IEnumerable<SupplierViewModel>> GetRemoteDataAsync()
        {
            var _list = new List<SupplierViewModel>();
            var _url = url + "GetAll";

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<SupplierViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;
        }

    }
}
