using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.View;

namespace T2SLogistics
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var decorview = Window.DecorView;
            var wic = new WindowInsetsControllerCompat(Window,decorview);
            wic.AppearanceLightStatusBars = true;
        }
    }
}
