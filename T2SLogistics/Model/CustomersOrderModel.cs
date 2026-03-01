using T2SLogistics.Helpers;
using Newtonsoft.Json;

namespace T2SLogistics.Model
{
    public class OrderItem : ObservableObject
    {
        public int id { get; set; }
        public string productRef { get; set; }
        public string productDescription { get; set; }
        public string eanCode { get; set; }
        public string decForma { get; set; }
        public int quantity { get; set; }
        private int _quantitySep;
        public int quantitySep
        {
            get=>_quantitySep; 
            set => SetProperty(ref _quantitySep, value);
        }
        public int? pendingReadQuanty{get;set;}
        public int? customerOrderId { get; set; }
        public string phcOrderItemLinId { get; set; }
        private int _remainingQuantity;
        public int RemainingQuantity 
        { 
           get => _remainingQuantity;
              set => SetProperty(ref _remainingQuantity, value);
        }
        [JsonIgnore]
         public bool IsFullyRead=>pendingReadQuanty==quantity;
        [JsonIgnore]
        private int _status = 0;
        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
       
    }
    public class ItemsRead
    {
        public int id { get; set; }
        public string itemReadId { get; set; }
        public string phcOrderId { get; set; }
        public string phcOrderItemLinId { get; set; }
        public string productRef { get; set; }
        public string productDescription { get; set; }
        public int boxNr { get; set; }
        public string eanCode { get; set; }
        public int quantity { get; set; }
        public object userCode { get; set; }
        public DateTime dateRegist { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public int quantitySet { get; set; }
      
       

        
    }
    public class CustomersOrderModel : ObservableObject
    {
        public int id { get; set; }
        public string phcOrderId { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime dateCustomer { get; set; }
        public DateTime deliveryDate { get; set; }
        public List<OrderItem> orderItems { get; set; }
        public List<ItemsRead> itemsRead { get; set; }
        public string status { get; set; }
        public int idStatus { get; set; }
        public object requisicao { get; set; }
        public int pendingQuanty { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public int ordersQty { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        private bool _isExpanded = false;

        public bool IsExpanded 
        {
            get=> _isExpanded; 
            set => SetProperty(ref _isExpanded, value);
        }
        [System.Text.Json.Serialization.JsonIgnore]
        public int TotalBoxes { get; set; }
       
    }
}
