using T2SLogistics.Helpers;
using T2SLogistics.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.ApiServices
{
    public class RequestProvider : IRequestProvider
    {
        private  ISettingsService _settingsService;
        private string _baseUrl;
        public RequestProvider(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _baseUrl = settingsService.BaseUrl;

        }
        public async Task<T> Delete<T>(string endpoint)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_baseUrl);

                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);


                    var response = await httpClient.DeleteAsync(endpoint);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new FileNotFoundException();
                    }
                    var resultStr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultStr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try 
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_baseUrl);

                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);


                    var response = await httpClient.DeleteAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    // return response;

                    return response.IsSuccessStatusCode;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<T> Get<T>(string endpoint)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    var response = await httpClient.GetAsync(endpoint);
                    var result = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<T>(result);
                    return json;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

      

        public async Task<T> Post<T>(string endpoint, string jsonobject)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);

                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    var response = await httpClient.PostAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json"));
                  
                    var resultStr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultStr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> PostAsync<T>(string url, string jsonobject)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);
                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    var response = await httpClient.PostAsync(url, new StringContent(jsonobject, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    // return response;

                    return response.IsSuccessStatusCode;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<T> PostFormContent<T>(string endpoint, FormUrlEncodedContent form)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);

                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    var response = await httpClient.PostAsync(endpoint, form);
                    var resultStr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultStr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> Put<T>(string endpoint, string jsonobject)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);

                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    var response = await httpClient.PutAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json"));
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new FileNotFoundException();
                    }
                    var resultStr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultStr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
