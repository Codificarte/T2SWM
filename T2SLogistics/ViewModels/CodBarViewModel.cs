using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class CodBarViewModel
    {
        public string Ref { get; set; }
        public string Description { get; set; }

        public string CodBar { get; set; }
        public int QttCodBar { get; set; }


        public string IconReadLeitura { get => "\ue039"; }



        public void AddNewCodBar(CodBarViewModel refVM)
        {   

            ArtigosApi _artigosApi = new ArtigosApi();

            try
            {
                _artigosApi.AddNewCodBar(refVM);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
