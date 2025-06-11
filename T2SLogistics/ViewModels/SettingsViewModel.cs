using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Helpers;
using T2SLogistics.Models;

namespace T2SLogistics.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel()
        {

            LoadDefaultSettings();
            UrlBaseApi = Settings.UrlApiBase;
            NomeEmpresa = Settings.NomeEmpresa;
            NifEmpresa = Settings.NifEmpresa;

        }

        private void LoadDefaultSettings()
        {

            ListSettingsConfig = new ObservableCollection<SettingsConfig>();
            SettingsConfig sc;

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlUserLogin;
            sc.EndPoint = Settings.UrlApiLogin;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlForgotPassword;
            sc.EndPoint = Settings.UrlApiForgotPassword;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlRegisterUser;
            sc.EndPoint = Settings.UrlApiRegisterNewUser;
            ListSettingsConfig.Add(sc);



            //Logistica
            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlGetEncomendas;
            sc.EndPoint = Settings.UrlApiOrders;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlAddEntradas;
            sc.EndPoint = Settings.UrlApiRegEntradas;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlAddSaidas;
            sc.EndPoint = Settings.UrlApiRegSaidas;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlGetAlveolos;
            sc.EndPoint = Settings.UrlApiAlveolos;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlGetProducts;
            sc.EndPoint = Settings.UrlApiProducts;
            ListSettingsConfig.Add(sc);

            sc = new SettingsConfig();
            sc.ConfigId = SettingsConfig.IdUrlGetSuppliers;
            sc.EndPoint = Settings.UrlApiSuppliers;
            ListSettingsConfig.Add(sc);


        }

        public ObservableCollection<SettingsConfig> ListSettingsConfig { get; set; }



        private string _urlBaseApi;
        public string UrlBaseApi
        {
            get => _urlBaseApi; set
            {
                _urlBaseApi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UrlBaseApi)));
            }
        }

        private string _nomeEmpresa;
        public string NomeEmpresa
        {
            get => _nomeEmpresa; set
            {
                _nomeEmpresa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NomeEmpresa)));
            }
        }

        private bool _hasMenuStocks;
        public bool HasMenuStocks
        {
            get => _hasMenuStocks; set
            {
                _hasMenuStocks = Settings.NifEmpresa == "506846148" ? true : false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasMenuStocks)));
            }
        }

        private bool _hasMenuExpedicao;
        public bool HasMenuExpedicao
        {
            get => _hasMenuExpedicao; set
            {
                _hasMenuExpedicao = Settings.NifEmpresa == "503239364" ? false : true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasMenuExpedicao)));
            }
        }


        private string _nifEmpresa;
        public string NifEmpresa
        {
            get => _nifEmpresa; set
            {
                _nifEmpresa = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NifEmpresa)));
            }
        }





        /// <summary>
        /// Methods
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>



        public async Task<IEnumerable<SettingsConfig>> GetRemoteDataAsync(string url)
        {
            List<SettingsConfig> _listUrlConfig = new List<SettingsConfig>();
            HttpClient clientApi = new HttpClient();

            try
            {

                string resp = await clientApi.GetStringAsync(url);

                _listUrlConfig = JsonConvert.DeserializeObject<List<SettingsConfig>>(resp);

            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }

            return _listUrlConfig;
        }

    }
}
