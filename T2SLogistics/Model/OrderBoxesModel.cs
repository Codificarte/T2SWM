using T2SLogistics.Helpers;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Model
{
    public class OrderBoxesModel: ObservableObject
    {
        public int Id { get; set; }
        public int BoxNumber { get; set; }
        public int ProductCount { get; set; }
        public CustomersOrderModel customersOrderModel { get; set; }
        private ObservableCollection<ItemsRead> _readItems=new ObservableCollection<ItemsRead>();
        public ObservableCollection<ItemsRead> ReadItems
        {
            get => _readItems;
            set => SetProperty(ref _readItems, value);
        }
    }
    //public class ProductBoxModel:BaseViewModel
    //{
    //    public ProductBoxModel(INavigationService navigationService) : base(navigationService)
    //    {
    //    }

    //    public int Id { get; set; }
    //    public string ProductName { get; set; }
    //    public string ProductCode { get; set; }
    //    public int Quantity { get; set; }
    //    private int _status=0;
    //    public int Status
    //    {
    //        get => _status;
    //        set => SetProperty(ref _status, value);
    //    }
    //    public string ConfirmedQuantity { get; set; }
    //}
}
