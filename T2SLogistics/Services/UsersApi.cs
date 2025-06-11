using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.ViewModels;

namespace T2SLogistics.Services
{
    public class UsersApi
    {

        HttpClient clientApi = new HttpClient();


        public async Task<UserAppViewModel> LoginUser(string username, string password)
        {

            var _url = Helpers.Settings.UrlApiLogin + "?username=" + username.Trim() + "&password=" + password.Trim();

            try
            {

                string resp = await clientApi.GetStringAsync(_url);
                var _user = JsonConvert.DeserializeObject<UserAppViewModel>(resp);

                return _user;

            }
            catch (Exception ex)
            {
                return new UserAppViewModel();
            }

        }

        public async Task<UserAppViewModel> ChangePassword(string username, string password)
        {

            var _url = Helpers.Settings.UrlApiForgotPassword + "?username=" + username.Trim() + "&password=" + password.Trim();

            try
            {

                string resp = await clientApi.GetStringAsync(_url);
                var _user = JsonConvert.DeserializeObject<UserAppViewModel>(resp);

                return _user;

            }
            catch (Exception ex)
            {
                return new UserAppViewModel();
            }

        }

        public async Task<IEnumerable<UserAppViewModel>> GetRemoteDataAsync()
        {
            var _list = new List<UserAppViewModel>();

            try
            {

                string resp = await clientApi.GetStringAsync(Helpers.Settings.UrlApiGetAllUsers);

                _list = JsonConvert.DeserializeObject<List<UserAppViewModel>>(resp);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _list;
        }

    }
}
