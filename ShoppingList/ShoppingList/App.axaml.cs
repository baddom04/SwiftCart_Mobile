using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShoppingList.Core;
using ShoppingList.Views;
using ShoppingList.ViewModels;
using ShoppingList.Model.Models;
using System.Runtime.InteropServices;

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
            var accountModel = new UserAccountModel();
            var mainViewModel = new MainViewModel(accountModel);
            var mainView = new MainView(mainViewModel);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(mainView);
                MainView = mainView;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                MainView = mainView;
                singleViewPlatform.MainView = MainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}