using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class AlveolosApi
    {

        HttpClient clientApi = new HttpClient();
        readonly string url = Helpers.Settings.UrlApiAlveolos;

        public async Task<IEnumerable<AlveolosViewModel>> GetRemoteDataAsync()
        {
            var _list = new List<AlveolosViewModel>();
            var _url = url + "GetAll";

            try
            {

                string resp = await clientApi.GetStringAsync(_url);

                _list = JsonConvert.DeserializeObject<List<AlveolosViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;
        }

        public async Task<AlveolosViewModel> GetAlveoloAsync(string alveolo)
        {

            List<ArtigosViewModel> _list = new List<ArtigosViewModel>();
            var alv = new AlveolosViewModel();

            try
            {

                var _url = url + "Get?alveolo=" + alveolo.Trim();
                string resp = await clientApi.GetStringAsync(_url);

                alv = JsonConvert.DeserializeObject<AlveolosViewModel>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return alv;

        }


    }
}
