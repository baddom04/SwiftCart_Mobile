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
                    DataContext = new MainWindowViewModel(new UserAccountModel()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}