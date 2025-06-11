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
    public class AlveolosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AlveolosViewModel()
        {

        }

        public int Id { get; set; }
        public string Alveolo { get; set; }

        public ObservableCollection<Alveolos> Alveolos { get; set; }




        public virtual async Task<IEnumerable<AlveolosViewModel>> GetRemoteAlveolosApi()
        {

            var _apiService = new AlveolosApi();
            var _alveolosTask = _apiService.GetRemoteDataAsync();

            var _list = await _alveolosTask;

            return _list;

        }


    }
}
