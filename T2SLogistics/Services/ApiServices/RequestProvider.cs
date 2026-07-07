using T2SLogistics;
using T2SLogistics.Helpers;
using T2SLogistics.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
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

        // Throttle do aviso "em migração": evita spam quando um ecrã faz várias chamadas não migradas.
        private static DateTime _lastMigrationNoticeUtc = DateTime.MinValue;
        private static readonly object _migrationNoticeLock = new object();

        // Guarda p/ não disparar vários redirects concorrentes quando várias chamadas apanham 401 ao mesmo tempo.
        private static bool _handlingSessionExpiry;

        // 401 numa chamada autenticada = sessão expirada/inválida. NUNCA mascarar como "sem dados" (o operador
        // pensaria que não há nada a tratar): limpa a sessão e volta ao Login com aviso. Ignora o próprio fluxo
        // de autenticação (login / definir password), onde o 401 significa "credenciais inválidas".
        private void HandleSessionExpired(string endpoint)
        {
            var route = (endpoint ?? string.Empty).TrimStart('/');
            if (route.StartsWith("auth", StringComparison.OrdinalIgnoreCase))
                return;
            if (_handlingSessionExpiry)
                return;
            _handlingSessionExpiry = true;

            // Limpa já a sessão (mantém BaseUrl e idioma) para não reutilizar o token inválido.
            _settingsService.AuthToken = string.Empty;
            _settingsService.Username = string.Empty;
            _settingsService.Email = string.Empty;
            _settingsService.UserId = string.Empty;
            _settingsService.UserCode = string.Empty;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var page = Application.Current?.MainPage;
                    if (page != null)
                        await page.DisplayAlert("Sessão terminada",
                            "A sua sessão expirou. Inicie sessão novamente.", "OK");

                    var login = App.serviceProvider.GetRequiredService<View.Auth.LoginPage>();
                    Application.Current.MainPage = new NavigationPage(login);
                }
                catch
                {
                    // Redirecionamento é best-effort; nunca deve rebentar o fluxo.
                }
                finally
                {
                    _handlingSessionExpiry = false;
                }
            });
        }

        public RequestProvider(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // Guarda da migração: só rotas já migradas para a nova API (ApiBase.IsRouteMigrated) podem
        // sair para a rede. Qualquer rota antiga é bloqueada AQUI (não toca na rede) e o utilizador
        // vê um aviso "em migração". Defesa estrutural — a API antiga nunca é contactada.
        private static bool IsBlocked(string endpoint)
        {
            if (ApiBase.IsRouteMigrated(endpoint))
                return false;

            NotifyFeatureNotMigrated();
            return true;
        }

        private static void NotifyFeatureNotMigrated()
        {
            lock (_migrationNoticeLock)
            {
                // Janela larga: chamadas de fundo (ex.: timer) não migradas não devem encher de
                // alertas. TODO (diferido): suprimir totalmente avisos de rotas de fundo.
                if ((DateTime.UtcNow - _lastMigrationNoticeUtc).TotalSeconds < 30)
                    return;
                _lastMigrationNoticeUtc = DateTime.UtcNow;
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var page = Application.Current?.MainPage;
                    if (page != null)
                    {
                        await page.DisplayAlert(
                            LocalizationResourceManager.Instance["MigrationNoticeTitle"],
                            LocalizationResourceManager.Instance["MigrationNoticeMessage"],
                            "OK");
                    }
                }
                catch
                {
                    // Aviso é best-effort; nunca deve rebentar o fluxo.
                }
            });
        }

        public async Task<T> Delete<T>(string endpoint)
        {
            if (IsBlocked(endpoint))
                return default;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);

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
            if (IsBlocked(endpoint))
                return false;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);

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
            if (IsBlocked(endpoint))
                return default;
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

                    AppLog.Write($"GET {httpClient.BaseAddress}{endpoint}");

                    var response = await httpClient.GetAsync(endpoint);
                    var result = await response.Content.ReadAsStringAsync();
                    AppLog.Write($"GET <- {(int)response.StatusCode} {response.ReasonPhrase} | {result?.Length ?? 0} chars");
                    if ((int)response.StatusCode == 401)
                    {
                        HandleSessionExpired(endpoint);
                        return default;
                    }
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
            if (IsBlocked(endpoint))
                return default;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);

                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    // Diagnóstico: URL completa para onde o request vai (BaseAddress + endpoint).
                    AppLog.Write($"POST {httpClient.BaseAddress}{endpoint}");

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

        public async Task<HttpCallResult> PostWithStatus(string endpoint, string jsonobject)
        {
            if (IsBlocked(endpoint))
                return new HttpCallResult { StatusCode = 0, Body = string.Empty };
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

                    AppLog.Write($"POSTSTATUS {httpClient.BaseAddress}{endpoint}");

                    var response = await httpClient.PostAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json"));
                    var resultStr = await response.Content.ReadAsStringAsync();
                    AppLog.Write($"POSTSTATUS <- {(int)response.StatusCode} {response.ReasonPhrase} | {resultStr?.Length ?? 0} chars");
                    if ((int)response.StatusCode == 401)
                        HandleSessionExpired(endpoint);
                    return new HttpCallResult { StatusCode = (int)response.StatusCode, Body = resultStr ?? string.Empty };
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(nameof(PostWithStatus), ex);
                return new HttpCallResult { StatusCode = -1, Body = string.Empty };
            }
        }

        public async Task<bool> PostAsync<T>(string url, string jsonobject)
        {
            if (IsBlocked(url))
                return false;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.BaseAddress = new Uri(_settingsService.BaseUrl);
                    if (!string.IsNullOrEmpty(_settingsService.AuthToken))
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _settingsService.AuthToken);

                    AppLog.Write($"POSTASYNC {httpClient.BaseAddress}{url}");

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
            if (IsBlocked(endpoint))
                return default;
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
            if (IsBlocked(endpoint))
                return default;
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
