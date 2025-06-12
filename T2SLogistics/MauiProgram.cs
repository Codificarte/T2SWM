using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace T2SLogistics
{
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
                    fonts.AddFont("Inter_Bold.ttf", "InterBold");
                    fonts.AddFont("Inter_Medium.ttf", "InterMedium");
                    fonts.AddFont("Inter_Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter_SemiBold.ttf", "InterSemiBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
