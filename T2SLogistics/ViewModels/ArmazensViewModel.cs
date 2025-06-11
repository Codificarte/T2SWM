using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Models;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class ArmazensViewModel
    {

        public virtual async Task<IEnumerable<Armazens>> GetRemoteArmazensApi()
        {

            var _apiService = new ArmazensApi();
            var _armzTask = _apiService.GetRemoteDataAsync();

            var _list = await _armzTask;

            if (_list == null)
                return new List<Armazens>();


            return _list;

        }

    }
}
