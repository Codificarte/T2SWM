using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2SLogistics.Services;

namespace T2SLogistics.ViewModels
{
    public class SupplierViewModel
    {

        public SupplierViewModel()
        {

        }

        public int Id { get; set; }

        public string BackOfficeId { get; set; }

        public int Num { get; set; }
        public int Estab { get; set; }

        public string Name { get; set; }

        public string VatNr { get; set; }

        public string Address { get; set; }
        public string Local { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }


        public Color ItemBgColor { get; set; }



        public virtual async Task<IEnumerable<SupplierViewModel>> GetRemoteSuppliersApi()
        {

            var _ordersApi = new SuppliersApi();
            var _ordersTask = _ordersApi.GetRemoteDataAsync();

            var _orders = await _ordersTask;

            return _orders;

        }


        public ObservableCollection<SupplierViewModel> GetFiltedOrders(ObservableCollection<SupplierViewModel> _suppliers, string searchName)
        {

            if (string.IsNullOrEmpty(searchName))
                return new ObservableCollection<SupplierViewModel>();

            int nrIfNumeric;
            bool isNumeric = int.TryParse(searchName, out nrIfNumeric);

            if (searchName != null)
            {

                if (isNumeric)
                {

                    var _tmpList = _suppliers.Where(o => o.Num.ToString().Contains(searchName));

                    var suppliers = new ObservableCollection<SupplierViewModel>();

                    foreach (var item in _tmpList)
                        suppliers.Add(item);

                    return suppliers;

                }



                if (!isNumeric && searchName.ToString().Length >= 2)
                {

                    var _tmpList = _suppliers.Where(o => o.Name.ToLower().ToString().Contains(searchName));

                    var suppliers = new ObservableCollection<SupplierViewModel>();

                    foreach (var item in _tmpList)
                        suppliers.Add(item);

                    return suppliers;

                }

            }

            return new ObservableCollection<SupplierViewModel>();

        }


    }
}
