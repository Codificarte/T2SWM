using CommunityToolkit.Maui;
using T2SLogistics.Services.ApiServices;
using T2SLogistics.Services.DbServices;
using T2SLogistics.Services.Interface;
using T2SLogistics.Services.NavigationService;
using T2SLogistics.Services.SettingsService;
using T2SLogistics.View.Auth;
using T2SLogistics.View.Breaks;
using T2SLogistics.View.CustomerOrders;
using T2SLogistics.View.Home;
using T2SLogistics.View.InsertProduction;
using T2SLogistics.View.Orders;
using T2SLogistics.View.OrderSepration;
using T2SLogistics.View.Popups;
using T2SLogistics.View.Register;
using T2SLogistics.ViewModel.Auth;
using T2SLogistics.ViewModel.Breaks;
using T2SLogistics.ViewModel.CustomerOrders;
using T2SLogistics.ViewModel.Home;
using T2SLogistics.ViewModel.InsertProduction;
using T2SLogistics.ViewModel.Orders;
using T2SLogistics.ViewModel.OrderSepration;
using T2SLogistics.ViewModel.Popups;
using T2SLogistics.ViewModel.Register;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace T2SLogistics;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseBarcodeReader();

#if DEBUG
        builder.Logging.AddDebug();
#endif
#if ANDROID
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(Entry), typeof(T2SLogistics.Platforms.Android.CustomEntryHandler));
        });
#endif
        RegisterViewsWithViewModels(builder);
        RegisterServices(builder);
        return builder.Build();
    }
    private static void RegisterViewsWithViewModels(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddTransient<OrdersPage>();
        builder.Services.AddTransient<OrdersPageViewModel>();
        builder.Services.AddTransient<InsertProductionPage>();
        builder.Services.AddTransient<OrderReadingPage>();
        builder.Services.AddTransient<OrderReadingPageViewModel>();
        builder.Services.AddTransient<InsertProductionPageViewModel>();
        builder.Services.AddTransient<RegisterEnteryExitPage>();
        builder.Services.AddTransient<RegisterEnteryExitPageViewModel>();
        builder.Services.AddTransient<ViewProductOperationPage>();
        builder.Services.AddTransient<ViewProductOperationPageViewModel>();
        builder.Services.AddTransient<ResetNewPasswordPage>();
        builder.Services.AddTransient<ResetNewPasswordPageViewModel>();
        builder.Services.AddTransient<InsertProductQtyPopupViewModel>();
        builder.Services.AddTransient<CustomerOrdersPageViewModel>();
        builder.Services.AddTransient<CustomerOrdersPage>();
        builder.Services.AddTransient<CustomerOrderDetailPage>();
        builder.Services.AddTransient<CustomerOrderDetailViewModel>();
        builder.Services.AddTransient<AuthService>();
      
        builder.Services.AddTransient<UseAppTimerService>();
        builder.Services.AddTransient<ProductionEntriesService>();
        builder.Services.AddTransient<IdentifyUsercodePage>();
        builder.Services.AddTransient<IdentifyUsercodePageViewModel>();
       
        
        builder.Services.AddTransient<OrderSeprationPage>();
        builder.Services.AddTransient<OrderSeprationPageViewModel>();
        builder.Services.AddTransient<OrderSeprationDetailPage>();
        builder.Services.AddTransient<OrderSeprationDetailPageViewModel>();
        builder.Services.AddTransient<OrderDetailsPage>();
        builder.Services.AddTransient<OrderDetailsPageViewModel>();
        builder.Services.AddTransient<ProductBreaksPage>();
        builder.Services.AddTransient<ProductBreaksPageViewModel>();
        builder.Services.AddTransient<IncompleteQuantityInputPopupViewModel>();
        builder.Services.AddTransient<AskUserIdPage>();
        builder.Services.AddTransient<AskUserIdPageViewModel>();


        builder.Services.AddTransient<PhcOrderServices>();
        builder.Services.AddTransient<CustomerOrderService>();
        builder.Services.AddTransient<ProductBreaksService>();
        builder.Services.AddTransient<DbService>();

    }
    private static void RegisterServices(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IRequestProvider, RequestProvider>();



    }
}
