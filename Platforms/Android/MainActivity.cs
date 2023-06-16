using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SliptMath;

[Activity(Theme = "@style/Maui.SplashTheme",ScreenOrientation = ScreenOrientation.SensorPortrait, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
