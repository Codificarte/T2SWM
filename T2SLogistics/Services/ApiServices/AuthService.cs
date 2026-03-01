using T2SLogistics.Model;
using T2SLogistics.Services.Interface;

namespace T2SLogistics.Services.ApiServices
{
    public class AuthService:ApiBase
    {
        IRequestProvider _requestProvider;

        public AuthService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<AuthDataModel> Login(AuthRequestModel authRequestModel)
        {
            try
            {
               
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(authRequestModel);
                var response = await _requestProvider.Post<CommonResponseModel<AuthDataModel>>(AuthLoginKey, jsonObject);
                return response.response.Data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> ResetPassword(ResetPasswordRequestModel resetPasswordRequestModel)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(resetPasswordRequestModel);
                var response = await _requestProvider.Post<string>(AuthResetPasswordKey, jsonObject);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
