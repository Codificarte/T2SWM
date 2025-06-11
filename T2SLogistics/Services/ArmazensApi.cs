using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;

namespace T2SLogistics.Services
{
    public class ArmazensApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string url = Helpers.Settings.UrlApiArmazens;

        public async Task<IEnumerable<Armazens>> GetRemoteDataAsync()
        {
            var _list = new List<Armazens>();
            var _url = url + "GetAll";

            try
            {

                var resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<Armazens>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;
        }

    }
}
