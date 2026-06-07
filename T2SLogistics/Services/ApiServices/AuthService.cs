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
                var response = await _requestProvider.Post<AuthDataModel>(AuthLoginKey, jsonObject);
                // A nova API devolve um corpo plano. Post<T> não valida o status code, por isso um
                // 401 ({message}) desserializa para um objeto com token nulo: tratar como falha.
                if (response == null || string.IsNullOrEmpty(response.token))
                    return null;
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> SetInitialPassword(SetInitialPasswordRequestModel request)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                // set-initial-password devolve 200 sem corpo; PostAsync sinaliza sucesso via status code.
                return await _requestProvider.PostAsync<object>(AuthSetInitialPasswordKey, jsonObject);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
