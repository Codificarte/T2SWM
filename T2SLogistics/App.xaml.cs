using T2SLogistics.Helpers;
using T2SLogistics.Services.Interface;
using T2SLogistics.View.Auth;
using T2SLogistics.View.Home;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace T2SLogistics
{
    public partial class App : Application
    {
        public static IServiceProvider serviceProvider;
        public static ISettingsService settingsService;

        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;

            serviceProvider =
#if ANDROID
  MauiApplication.Current.Services;
#elif IOS || MACCATALYST
    MauiUIApplicationDelegate.Current.Services;
#else
    null;
#endif
            Application.Current.Resources["DefaultStringResources"] = new Resx.AppResources();
            settingsService = serviceProvider.GetService<ISettingsService>();
            if (!string.IsNullOrEmpty(settingsService.Applanguage))
            {
                LocalizationResourceManager.Instance.SetCulture(new CultureInfo(settingsService.Applanguage));

            }
            if (string.IsNullOrEmpty(settingsService.BaseUrl))
            {
                settingsService.BaseUrl = AppConstants.ApiBaseUrl;
            }
            SetMainPage();
        }
        private void SetMainPage()
        {
            if (string.IsNullOrEmpty(settingsService.AuthToken))
            {
                MainPage = new NavigationPage(serviceProvider.GetService<LoginPage>());

            }
            else
            {
                MainPage = new NavigationPage(serviceProvider.GetService<HomePage>());

            }

        }
    }
}