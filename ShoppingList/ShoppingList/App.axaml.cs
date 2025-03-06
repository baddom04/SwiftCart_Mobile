using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.ViewModels;
using ShoppingList.Views;

namespace ShoppingList
{
    public partial class App : Avalonia.Application
    {
        public static MainView MainView { get; private set; } = null!;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var mainViewModel = new MainViewModel(new UserAccountModel());

            MainView = new MainView(mainViewModel);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(MainView);
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = MainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}