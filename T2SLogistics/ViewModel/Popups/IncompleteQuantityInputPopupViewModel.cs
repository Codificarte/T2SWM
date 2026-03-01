using T2SLogistics.Model;
using T2SLogistics.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel.Popups
{
    public class IncompleteQuantityInputPopupViewModel:BaseViewModel
    {
        public IncompleteQuantityInputPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            Input_1_Command=new Command(ExecuteInput_1_Command);
            Input_2_Command = new Command(ExecuteInput_2_Command);
            Input_3_Command = new Command(ExecuteInput_3_Command);
            Input_4_Command = new Command(ExecuteInput_4_Command);
            Input_5_Command = new Command(ExecuteInput_5_Command);
            Input_6_Command = new Command(ExecuteInput_6_Command);
            Input_7_Command = new Command(ExecuteInput_7_Command);
            Input_8_Command = new Command(ExecuteInput_8_Command);
            Input_9_Command = new Command(ExecuteInput_9_Command);
            Input_0_Command = new Command(ExecuteInput_0_Command);
            CleanInputCommand=new Command(ExecuteCleanInputCommand);
            ConfirmCommand=new Command(ExecuteConfirmCommand);
        }
        private ItemsRead _itemsRead;
        public ItemsRead ItemsRead
        {
            get => _itemsRead;
            set => SetProperty(ref _itemsRead, value);
        }
        private string _inputQuantity="0";
        public string InputQuantity 
        {
            get=> _inputQuantity;
            set => SetProperty(ref _inputQuantity, value);
        } 
        public ICommand Input_1_Command { get; }
        private void ExecuteInput_1_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "1";
            else
                InputQuantity += "1";
        }
        public ICommand Input_2_Command { get; }
        private void ExecuteInput_2_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "2";
            else
                InputQuantity += "2";
        }
        public ICommand Input_3_Command { get; }
        private void ExecuteInput_3_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "3";
            else
                InputQuantity += "3";
        }
        public ICommand Input_4_Command { get; }
        private void ExecuteInput_4_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "4";
            else
                InputQuantity += "4";
        }
        public ICommand Input_5_Command { get; }
        private void ExecuteInput_5_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "5";
            else
                InputQuantity += "5";
        }
        public ICommand Input_6_Command { get; }
        private void ExecuteInput_6_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "6";
            else
                InputQuantity += "6";
        }
        public ICommand Input_7_Command { get; }
        private void ExecuteInput_7_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "7";
            else
                InputQuantity += "7";

        }
        public ICommand Input_8_Command { get; }
        private void ExecuteInput_8_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "8";
            else
                InputQuantity += "8";
        }
        public ICommand Input_9_Command { get; }
        private void ExecuteInput_9_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "9";
            else
                InputQuantity += "9";
        }
        public ICommand Input_0_Command { get; }
        private void ExecuteInput_0_Command()
        {
            if (InputQuantity == "0")
                InputQuantity = "0";
            else
                InputQuantity += "0";
        }
        public ICommand CleanInputCommand { get; }
        private void ExecuteCleanInputCommand()
        {
            InputQuantity = "0";
        }
        public ICommand ConfirmCommand { get; }
        private void ExecuteConfirmCommand()
        {
            
        }

        public override Task Initialise(object? parameter)
        {
            if (parameter is ItemsRead itemsRead)
            {
                ItemsRead = itemsRead;
            }
            return base.Initialise(parameter);
        }
    }
}
