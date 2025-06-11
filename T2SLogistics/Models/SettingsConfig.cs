using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Models
{
    public class SettingsConfig
    {
        public int Id { get; set; }

        public string Nif { get; set; }

        public string CustomerName { get; set; }

        public int ConfigId { get; set; }

        public string EndPoint { get; set; }

        public string Description { get; set; }

        public bool UseMvo { get; set; }
        public bool UseAlveolos { get; set; }

        public bool ControlaAlvRec { get; set; }
        public bool SugereAlvRecep { get; set; }
        // Este campo permite que o sistema sugira os alvéolos onde o artigo pode ser guardado; 
        // No leitor o operador ao ler o artigo irá receber a informação sobre o alvéolo onde deve arrumar.


        /// <summary>
        ///  Static 
        /// </summary>




        // Users & Access
        public static readonly int IdUrlRegisterUser = 101;
        public static readonly int IdUrlUserLogin = 102;
        public static readonly int IdUrlForgotPassword = 103;
        public static readonly int IdUrlGetAllUsers = 104;

        // Logistica
        public static readonly int IdUrlGetEncomendas = 201;
        public static readonly int IdUrlAddEntradas = 202;
        public static readonly int IdUrlAddSaidas = 203;
        public static readonly int IdUrlGetAlveolos = 204;
        public static readonly int IdUrlGetProducts = 205;
        public static readonly int IdUrlGetSuppliers = 206;
        public static readonly int IdUrlGetArmazens = 207;
        public static readonly int IdUrlAddInventarios = 208;
        public static readonly int IdUrlAddRecepcaoMercadoria = 209;
        public static readonly int IdUrlAddExpedicaoMercadoria = 210;
        public static readonly int IdUrlConferenciaStock = 211;
        public static readonly int IdUrlPrintLabel = 212;
        public static readonly int IdUrlPrintA4 = 213;

    }
}
