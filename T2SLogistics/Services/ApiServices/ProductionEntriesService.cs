using T2SLogistics.Model;
using T2SLogistics.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.ApiServices
{
    public class ProductionEntriesService:ApiBase
    {
        IRequestProvider _requestProvider;
        public ProductionEntriesService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<bool> CreateProductionEntries(ProductionEntriesRequestModel productionEntriesRequestModel)
        {
            var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(productionEntriesRequestModel);
            var response = await _requestProvider.PostAsync<bool>(ProductionEntriesKey, jsonObject);
            return response;
        }
    }
}
