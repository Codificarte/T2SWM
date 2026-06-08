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
        // Devolve (Ok, Error): Ok=true em sucesso; senão Error traz o motivo real da API quando existe.
        public async Task<(bool Ok, string Error)> SetInitialPassword(SetInitialPasswordRequestModel request)
        {
            try
            {
                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                // 200 sem corpo → desserializa para null = sucesso. 400 → corpo com message/errors = falha.
                var result = await _requestProvider.Post<SetInitialPasswordResultModel>(AuthSetInitialPasswordKey, jsonObject);
                if (result == null)
                    return (true, null);

                var error = result.errors != null && result.errors.Count > 0
                    ? string.Join("\n", result.errors)
                    : result.message;
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }
    }
}
