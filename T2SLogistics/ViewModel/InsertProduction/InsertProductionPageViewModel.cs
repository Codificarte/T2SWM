using T2SLogistics.Model;
using T2SLogistics.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace T2SLogistics.ViewModel.InsertProduction
{
    public class InsertProductionPageViewModel : BaseViewModel
    {
        public InsertProductionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            OpenOperationDropdown=new Command(ExecuteOpenOperationDropdown);
            SelectDropdownOperationCommand=new Command<Operation>(SelectDropdownOperation);
            IncreaseProducedQuantityCommand = new Command(IncreaseProducedQuantity);
            DecreaseProducedQuantityCommand = new Command(DecreaseProducedQuantity);
        }
        private int _quantityProduced;
        public int QuantityProduced 
        {
            get=> _quantityProduced; 
            set=>SetProperty(ref _quantityProduced,value);
        }
        private string _selectedOperation;

        public string SelectedOperation
        {
            get => _selectedOperation;
            set => SetProperty(ref _selectedOperation, value);
        }
        public string _day;
        public string Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }
        public string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        private int _selectedThickness=0;

        public int SelectedThickness
        {
            get => _selectedThickness;
            set => SetProperty(ref _selectedThickness, value);
        }
        public bool _isDropdownOpen=false;
        public bool IsDropdownOpen
        {
            get => _isDropdownOpen;
            set => SetProperty(ref _isDropdownOpen, value);
        }
        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }
        public ICommand SelectDropdownOperationCommand { get; }
        private void SelectDropdownOperation(Operation operation)
        {
            SelectedOperation = operation.operationName;
            SelectedThickness = 2;
            IsDropdownOpen = false; // Close the dropdown after selection
        }
        public ICommand OpenOperationDropdown { get; }
        private void ExecuteOpenOperationDropdown()
        {
            IsDropdownOpen = !IsDropdownOpen;
        }
        public ICommand IncreaseProducedQuantityCommand { get; }
        private void IncreaseProducedQuantity()
        {
            QuantityProduced++;
        }
        public ICommand DecreaseProducedQuantityCommand { get; }
        private void DecreaseProducedQuantity()
        {
            if (QuantityProduced > 0)
            {
                QuantityProduced--;
            }
        }
        public override Task Initialise(object? parameter)
        {
            Day = DateTime.Now.DayOfWeek.ToString();
            Date = DateTime.Now.ToString("dd MMMM yyyy");
            if (parameter is Order order)
            {
                SelectedOrder = order;
                SelectedOperation = SelectedOrder.operations[0].operationName;
                QuantityProduced = 0; // Reset quantity when navigating to this page

            }
           
            return base.Initialise();
        }
    }
}
