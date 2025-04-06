using Avalonia.Controls;
using ShoppingListEditor.ViewModels;

namespace ShoppingListEditor.Views;

public partial class LoggedInView : UserControl
{
    public LoggedInView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as LoggedInViewModel)!.ToStartingPageAsync();
    }
}