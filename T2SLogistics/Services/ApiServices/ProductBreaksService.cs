using T2SLogistics.Model;
using T2SLogistics.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.ApiServices
{
    public class ProductBreaksService : ApiBase
    {
        IRequestProvider _requestProvider;

        public ProductBreaksService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<bool> AddProductBreaksItems(AddItemBreaksRequestModel addItemBreaksRequestModel)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(addItemBreaksRequestModel);

                var response = await _requestProvider.PostAsync<bool>(AddExpeditionKey, jsonObject);
                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
