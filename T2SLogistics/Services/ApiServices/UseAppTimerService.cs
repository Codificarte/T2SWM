using T2SLogistics.Model;
using T2SLogistics.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.ApiServices
{
    public class UseAppTimerService : ApiBase
    {
        IRequestProvider _requestProvider;

        public UseAppTimerService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<CommonResponseModel<CreateTimerResponseModel>> CreateUserAppTimerAsync(CreateUsersAppTimerRequestModel createUsersAppTimerRequestModel)
        {
            var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(createUsersAppTimerRequestModel);
            var response = await _requestProvider.Post<CommonResponseModel<CreateTimerResponseModel>>(CreateUsersAppTimerKey, jsonObject);
            return response;
        }
    }
}
