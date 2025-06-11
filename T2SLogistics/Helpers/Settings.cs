using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Helpers
{
    public static class Settings
    {
        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion

        public static string GeneralSettings
        {
            get
            {
                return Preferences.Get(SettingsKey, SettingsDefault);
            }
            set
            {
                Preferences.Set(SettingsKey, value);
            }
        }



        #region DefaultSettings


        public static string NomeEmpresa
        {
            get
            {
                return Preferences.Get("NomeEmpresa", "");
            }
            set
            {
                Preferences.Set("NomeEmpresa", value);
            }
        }


        public static string NifEmpresa
        {
            get
            {
                return Preferences.Get("NifEmpresa", "");
            }
            set
            {
                Preferences.Set("NifEmpresa", value);
            }
        }

        public static string MaxWidth
        {
            get
            {
                return Preferences.Get("MaxWidth", "");
            }
            set
            {
                Preferences.Set("MaxWidth", value);
            }
        }

        public static string MaxHeight
        {
            get
            {
                return Preferences.Get("MaxHeight", "");
            }
            set
            {
                Preferences.Set("MaxHeight", value);
            }
        }

        public static string UrlApiBase
        {
            get
            {
                return Preferences.Get("UrlApiBase", "");
            }
            set
            {
                Preferences.Set("UrlApiBase", value);
            }
        }

        public static bool UseMvo
        {
            get
            {
                return Preferences.Get("UseMvo", false);
            }
            set
            {
                Preferences.Set("UseMvo", value);
            }
        }

        public static bool UseAlveolos
        {
            get
            {
                return Preferences.Get("UseAlveolos", false);
            }
            set
            {
                Preferences.Set("UseAlveolos", value);
            }
        }


        // Controla alvéolos na recepção
        public static bool ControlaAlvRec
        {
            get
            {
                return Preferences.Get("ControlaAlvRec", false);
            }
            set
            {
                Preferences.Set("ControlaAlvRec", value);
            }
        }


        // Este campo permite que o sistema sugira os alvéolos onde o artigo pode ser guardado; 
        // No leitor o operador ao ler o artigo irá receber a informação sobre o alvéolo onde deve arrumar.
        public static bool SugereAlvRecep
        {
            get
            {
                return Preferences.Get("SugereAlvRecep", false);
            }
            set
            {
                Preferences.Set("SugereAlvRecep", value);
            }
        }




        #endregion

        #region Settings EndPointsGetDocsFromAPI

        public static string UrlApiOrders
        {
            get
            {
                return Preferences.Get("UrlApiOrders", "");
            }
            set
            {
                Preferences.Set("UrlApiOrders", value);
            }
        }

        public static string UrlApiRegEntradas
        {
            get
            {
                return Preferences.Get("UrlApiRegEntradas", "");
            }
            set
            {
                Preferences.Set("UrlApiRegEntradas", value);
            }
        }


        public static string UrlApiConferenciasStock
        {
            get
            {
                return Preferences.Get("UrlApiConferenciasStock", "");
            }
            set
            {
                Preferences.Set("UrlApiConferenciasStock", value);
            }
        }


        public static string UrlApiRegSaidas
        {
            get
            {
                return Preferences.Get("UrlApiRegSaidas", "");
            }
            set
            {
                Preferences.Set("UrlApiRegSaidas", value);
            }
        }

        public static string UrlApiRegInventarios
        {
            get
            {
                return Preferences.Get("UrlApiRegInventarios", "");
            }
            set
            {
                Preferences.Set("UrlApiRegInventarios", value);
            }
        }

        public static string UrlApiProducts
        {
            get
            {
                return Preferences.Get("UrlApiProducts", "");
            }
            set
            {
                Preferences.Set("UrlApiProducts", value);
            }
        }

        public static string UrlApiAlveolos
        {
            get
            {
                return Preferences.Get("UrlApiAlveolos", "");
            }
            set
            {
                Preferences.Set("UrlApiAlveolos", value);
            }
        }

        public static string UrlApiArmazens
        {
            get
            {
                return Preferences.Get("UrlApiArmazens", "");
            }
            set
            {
                Preferences.Set("UrlApiArmazens", value);
            }
        }



        public static string UrlApiRecepcaoMercadoria
        {
            get
            {
                return Preferences.Get("UrlApiRecepcaoMercadoria", "");
            }
            set
            {
                Preferences.Set("UrlApiRecepcaoMercadoria", value);
            }
        }

        public static string UrlApiExpedicaoMercadoria
        {
            get
            {
                return Preferences.Get("UrlApiExpedicaoMercadoria", "");
            }
            set
            {
                Preferences.Set("UrlApiExpedicaoMercadoria", value);
            }
        }


        public static string UrlApiPrintLabel
        {
            get
            {
                return Preferences.Get("UrlApiPrintLabel", "");
            }
            set
            {
                Preferences.Set("UrlApiPrintLabel", value);
            }
        }


        public static string UrlApiPrintA4
        {
            get
            {
                return Preferences.Get("UrlApiPrintA4", "");
            }
            set
            {
                Preferences.Set("UrlApiPrintA4", value);
            }
        }


        //
        #endregion


        /// <summary>
        /// Set docs config. Where PDA knows doc type in backoffice
        /// </summary>

        #region endPointsTipoDocsAndEntities

        public static string TipoDocEncomendaFornec
        {
            get
            {
                return Preferences.Get("TipoDocEncomendaFornec", "");
            }
            set
            {
                Preferences.Set("TipoDocEncomendaFornec", value);
            }
        }

        public static string TipoDocEncomendaCliente
        {
            get
            {
                return Preferences.Get("TipoDocEncomendaCliente", "");
            }
            set
            {
                Preferences.Set("TipoDocEncomendaCliente", value);
            }
        }

        public static string TipoDocEncomendaEntidades
        {
            get
            {
                return Preferences.Get("TipoDocEncomendaEntidades", "");
            }
            set
            {
                Preferences.Set("TipoDocEncomendaEntidades", value);
            }
        }



        //public static string TipoDocRecepcaoMercadoria
        //{
        //    get
        //    {
        //        return Preferences.Get("TipoDocRecepcaoMercadoria", "");
        //    }
        //    set
        //    {
        //        Preferences.Set("TipoDocRecepcaoMercadoria", value);
        //    }

        //}

        //public static string TipoDocExpedicaoMercadoria
        //{
        //    get
        //    {
        //        return Preferences.Get("TipoDocExpedicaoMercadoria", "");
        //    }
        //    set
        //    {
        //        Preferences.Set("TipoDocExpedicaoMercadoria", value);
        //    }

        //}


        public static string UrlApiSuppliers
        {
            get
            {
                return Preferences.Get("UrlApiSuppliers", "");
            }
            set
            {
                Preferences.Set("UrlApiSuppliers", value);
            }
        }

        #endregion



        #region AppUsersLogin


        /// <summary>
        /// Users App
        /// </summary>

        public static string UserName
        {
            get
            {
                return Preferences.Get("UserName", "");
            }
            set
            {
                Preferences.Set("UserName", value);
            }
        }

        public static string Password
        {
            get
            {
                return Preferences.Get("Password", "");
            }
            set
            {
                Preferences.Set("Password", value);
            }
        }

        public static bool UserMustResetPassword
        {
            get
            {
                return Preferences.Get("UserMustResetPassword", false);
            }
            set
            {
                Preferences.Set("UserMustResetPassword", value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return Preferences.Get("AccessToken", "");
            }
            set
            {
                Preferences.Set("AccessToken", value);
            }
        }

        public static string UrlApiLogin
        {
            get
            {
                return Preferences.Get("UrlApiLogin", "");
            }
            set
            {
                Preferences.Set("UrlApiLogin", value);
            }
        }

        public static string UrlApiRegisterNewUser
        {
            get
            {
                return Preferences.Get("UrlApiRegisterNewUser", "");
            }
            set
            {
                Preferences.Set("UrlApiRegisterNewUser", value);
            }
        }

        public static string UrlApiForgotPassword
        {
            get
            {
                return Preferences.Get("UrlApiForgotPassword", "");
            }
            set
            {
                Preferences.Set("UrlApiForgotPassword", value);
            }
        }

        public static string UrlApiGetAllUsers
        {
            get
            {
                return Preferences.Get("UrlApiGetAllUsers", "");
            }
            set
            {
                Preferences.Set("UrlApiGetAllUsers", value);
            }
        }

        #endregion




    }
}
