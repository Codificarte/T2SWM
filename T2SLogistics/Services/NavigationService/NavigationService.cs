using T2SLogistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.NavigationService
{
    public class NavigationService : INavigationService
    {
        readonly IServiceProvider _services;

        protected INavigation Navigation
        {
            get
            {
                INavigation? navigation = Application.Current?.MainPage?.Navigation;
                if (navigation is not null)
                    return navigation;
                else
                {
                    //This is not good!
                    if (Debugger.IsAttached)
                        Debugger.Break();
                    throw new Exception();
                }
            }
        }

        public NavigationService(IServiceProvider services)
            => _services = services;

        public Task NavigateToMainPage()
            => NavigateToPage<AppShell>();
   
        public Task NavigateBack()
        {
            if (Navigation.NavigationStack.Count > 1)
                return Navigation.PopAsync();

            throw new InvalidOperationException("No pages to navigate back to!");
        }
        public void RemoveLastFromBackStack()
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                var pageToRemove = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
                Navigation.RemovePage(pageToRemove);
            }
        }
        public Task NavigateBackTo<T>() where T : Page
        { 
            var toPage = ResolvePage<T>();
            if (toPage is not null)
            {
                //Get VM of the toPage
                var toViewModel = GetPageViewModelBase(toPage);
                if (Navigation.NavigationStack.Count > 1)
                {
                    return Navigation.PopToRootAsync();
                }
            }
            else
                throw new InvalidOperationException($"No pages to navigate back to! {typeof(T).FullName}");
            return Task.CompletedTask;
        }
        public async Task NavigateBackToPage<T>(object? parameter = null) where T : Page
        {
            var targetPage = Navigation.NavigationStack.OfType<T>().FirstOrDefault();

            if (targetPage == null)
                throw new InvalidOperationException($"No page of type {typeof(T).FullName} found in navigation stack.");

            // Get the *real* ViewModel instance currently bound to that Page
            if (targetPage.BindingContext is BaseViewModel toViewModel)
            {
                // Initialise with parameter (this triggers PropertyChanged and updates bound UI)
                if (parameter != null)
                    await toViewModel.Initialise(parameter);
                //await toViewModel.InitialiseBack(parameter);
            }

            // Pop back to that actual page (not a new instance)
            while (Navigation.NavigationStack.Last() != targetPage)
                await Navigation.PopAsync();
        }

        public async Task NavigateToPage<T>(object? parameter = null) where T : Page
        {
            var toPage = ResolvePage<T>();

            if (toPage is not null)
            {
                //Subscribe to the toPage's NavigatedTo event
                toPage.NavigatedTo += Page_NavigatedTo;

                //Get VM of the toPage
                var toViewModel = GetPageViewModelBase(toPage);

                if (toViewModel is not null)
                    await toViewModel.Initialise(parameter);

                //Navigate to requested page
                await Navigation.PushAsync(toPage, true);

                //Subscribe to the toPage's NavigatedFrom event
                toPage.NavigatedFrom += Page_NavigatedFrom;
            }
            else
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
        }

        public async Task NavigateToMainPage<T>(object? parameter = null) where T : Page
        {
            var toPage = ResolvePage<T>();

            if (toPage is not null)
            {
                //Subscribe to the toPage's NavigatedTo event
                toPage.NavigatedTo += Page_NavigatedTo;

                //Get VM of the toPage
                var toViewModel = GetPageViewModelBase(toPage);



                //Navigate to requested page
                //await Navigation.PushAsync(toPage, true);
                App.Current.MainPage = new NavigationPage(toPage);

                //Subscribe to the toPage's NavigatedFrom event
                toPage.NavigatedFrom += Page_NavigatedFrom;
            }
            else
                throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
        }
        public Task<bool> IsNavigateFrom<T>() where T : Page
        {
            var navigationStack = Navigation.NavigationStack;
           
                var previousPage = navigationStack[navigationStack.Count - 2];
                string previousPageName = previousPage.GetType().Name;
            if (previousPage is T)
                return Task.FromResult(true);

            return Task.FromResult(false);

        }
        private async void Page_NavigatedFrom(object? sender, NavigatedFromEventArgs e)
        {
            //To determine forward navigation, we look at the 2nd to last item on the NavigationStack
            //If that entry equals the sender, it means we navigated forward from the sender to another page
            bool isForwardNavigation = Navigation.NavigationStack.Count > 1
                && Navigation.NavigationStack[^2] == sender;

            if (sender is Page thisPage)
            {
                if (!isForwardNavigation)
                {
                    thisPage.NavigatedTo -= Page_NavigatedTo;
                    thisPage.NavigatedFrom -= Page_NavigatedFrom;
                }

                await CallNavigatedFrom(thisPage, isForwardNavigation);
            }
        }

        private Task CallNavigatedFrom(Page p, bool isForward)
        {
            var fromViewModel = GetPageViewModelBase(p);


            return Task.CompletedTask;
        }

        private async void Page_NavigatedTo(object? sender, NavigatedToEventArgs e)
            => await CallNavigatedTo(sender as Page);

        private Task CallNavigatedTo(Page? p)
        {
            var fromViewModel = GetPageViewModelBase(p);

            if (fromViewModel is not null)
                return fromViewModel.Initialise();
            return Task.CompletedTask;
        }

        private BaseViewModel? GetPageViewModelBase(Page? p)
            => p?.BindingContext as BaseViewModel;

        private T? ResolvePage<T>() where T : Page
            => _services.GetService<T>();
    }
}
