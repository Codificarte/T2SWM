using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2SLogistics.Services.NavigationService
{
    public interface INavigationService
    {
        Task NavigateToMainPage();

        Task NavigateBack();
        Task NavigateBackToPage<T>(object? parameter = null) where T : Page;
        void RemoveLastFromBackStack();
        Task NavigateToPage<T>(object? parameter = null) where T : Page;
        Task<bool> IsNavigateFrom<T>() where T : Page;
        Task NavigateToMainPage<T>(object? parameter = null) where T : Page;
    }
}
