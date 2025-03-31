using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShoppingList.Shared.Model.Settings;
using ShoppingListEditor.ViewModels;
using ShoppingListEditor.Views;

namespace ShoppingListEditor
{
    public partial class App : Application
    {
        public static MainWindow MainView { get; private set; } = null!;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                MainView = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new UserAccountModel()),
                };

                desktop.MainWindow = MainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}