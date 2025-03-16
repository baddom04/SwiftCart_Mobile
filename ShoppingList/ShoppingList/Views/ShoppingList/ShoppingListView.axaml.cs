using Avalonia.Controls;
using ShoppingList.ViewModels.ShoppingList;

namespace ShoppingList.Views.ShoppingList;

public partial class ShoppingListView : UserControl
{
    public ShoppingListView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as ShoppingListViewModel)!.GetGroceriesAsync();
    }
}