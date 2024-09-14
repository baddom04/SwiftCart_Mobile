using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using ShoppingList.Views;

namespace ShoppingList
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new GroceryListView();
            }

            CurrentUser = new("Batki Domonkos", "Domika", "domonkosbatki98@gmail.com");

            base.OnFrameworkInitializationCompleted();
        }
    }
}