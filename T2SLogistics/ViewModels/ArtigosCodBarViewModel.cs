using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class ArtigosCodBarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ArtigosApi _artigosApi = new ArtigosApi();

        public ArtigosCodBarViewModel()
        {
            CodBarras = new ObservableCollection<CodBarViewModel>();
        }


        public string BackOfficeId { get; set; }


        private string _ref;
        public string Ref
        {
            get => _ref; set
            {
                _ref = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ref)));
            }
        }

        private string _description;
        public string Description
        {
            get => _description; set
            {
                _description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }


        public string CodBar { get; set; }
        public int QttCodBar { get; set; }


        public ObservableCollection<CodBarViewModel> CodBarras { get; set; }



        ///


        public IEnumerable<ArtigosCodBarViewModel> GetAllArtigosByCodBar(string codbar)
        {

            var _artigosInDb = Task.Run(async () => await _artigosApi.GetAllArtigosByCodBar(codbar)).Result;

            return _artigosInDb;

        }

        public IEnumerable<ArtigosCodBarViewModel> GetAllCodBarsByProductCode(string codbar)
        {

            var _artigosInDb = Task.Run(async () => await _artigosApi.GetAllCodBarByProductCode(codbar)).Result;

            return _artigosInDb;

        }

        public void RemoveCodbar(string _ref, string _codBar)
        {
            _artigosApi.RemoveCodbar(_ref, _codBar);
        }
    }
}
