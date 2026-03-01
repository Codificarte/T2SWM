using T2SLogistics.Helpers;
using T2SLogistics.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T2SLogistics.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        INavigationService _navigation;
        public BaseViewModel(INavigationService navigationService)
        {
            GoBackCommand = new Command(GoBack);
            _navigation = navigationService;
        }
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public ICommand GoBackCommand { get; }
        private async void GoBack()
        {
            await _navigation.NavigateBack();
        }
        public virtual Task Initialise() => Task.CompletedTask;
        public virtual Task Initialise(object? parameter)
           => Task.CompletedTask;
        public virtual Task InitialiseBack(object? parameter)
            => Task.CompletedTask;
       
    }
}
