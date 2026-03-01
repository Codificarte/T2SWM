using T2SLogistics.Helpers;

namespace T2SLogistics.Model
{
    public class Operation
    {
        public int id { get; set; }
        public string operationName { get; set; }
        public string obs { get; set; }
    }

    public class Order : ObservableObject
    {
       

        public int id { get; set; }
        public bool isStock { get; set; }
        public int qttRequest { get; set; }
        public int qttRemain{ get; set; }
        public int clientNr { get; set; }
        public object clientName { get; set; }
        public DateTime dataEnc { get; set; }
        public DateTime dataEntrega { get; set; }
        public object requisicao { get; set; }
        public string refProd { get; set; }
        public string description { get; set; }
        public bool isClosed { get; set; }
        public bool isAnulado { get; set; }
        public object statusOrder { get; set; }
        public object stampEnc { get; set; }
        public object stampClient { get; set; }
        public string stampLinOrderProd { get; set; }
        public object obs { get; set; }
        public List<Operation> operations { get; set; }
        private bool _isExpanded=false;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }
        //private ObservableCollection<Operation> _operationsCollection;
        //public ObservableCollection<Operation> operations
        //{
        //    get => _operationsCollection;
        //    set => SetProperty(ref _operationsCollection, value);
        //}

    }

    public class PhcOrderModel
    {
        public int id { get; set; }
        public string familyProduct { get; set; }
        public List<Order> orders { get; set; }
    }
}
