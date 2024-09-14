using Android.App;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Avalonia.Android;
using Android.Content.PM;
using Avalonia;
using Avalonia.ReactiveUI;

namespace ShoppingList.Android
{
    [Activity(
        Label = "ShoppingList.Android",
        Theme = "@style/MyTheme.NoActionBar",
        Icon = "@drawable/icon",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
    public class MainActivity : AvaloniaMainActivity<App>
    {
        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder)
                .WithInterFont()
                .UseReactiveUI();
        }
    }
}
