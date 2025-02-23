using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShoppingList.Core;
using ShoppingList.Views;

namespace ShoppingList
{
    public partial class App : Application
    {
        public static User? CurrentUser { get; private set; }
        public static MainView MainView { get; private set; } = null!;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                MainView = (desktop.MainWindow as MainWindow)!.MainView;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                MainView = new MainView();
                singleViewPlatform.MainView = MainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}