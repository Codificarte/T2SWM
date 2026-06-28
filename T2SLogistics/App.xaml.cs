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
            // A BaseUrl é definida exclusivamente pelo utilizador no ecrã de Configuração.
            // A app assume sempre o que lá estiver — sem substituições no arranque.
            SetMainPage();
        }
        private void SetMainPage()
        {
            // Porta de entrada por autenticação: sem token → Login; com token → Shell (menu da UI nova).
            // O Login (auth/login, já migrado) guarda o JWT e encaminha para o AppShell ao concluir.
            if (string.IsNullOrEmpty(settingsService.AuthToken))
                MainPage = new NavigationPage(serviceProvider.GetService<View.Auth.LoginPage>());
            else
                MainPage = serviceProvider.GetService<AppShell>();
        }
    }
}