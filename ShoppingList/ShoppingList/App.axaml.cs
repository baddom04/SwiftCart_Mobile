using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShoppingList.Utils;
using ShoppingList.Views;
using ShoppingList.Model;

namespace ShoppingList
{
    public partial class App : Application
    {
        public static User? CurrentUser { get; private set; }
        public static MainView? MainView { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ServiceProvider.Register<IFileService>(new FileService());

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

            CurrentUser = new User
            {
                Id = 11,
                Email = "domonkosbatki98@gmail.com",
                Name = "Domi",
                Admin = true
            };

            base.OnFrameworkInitializationCompleted();
        }
    }
}