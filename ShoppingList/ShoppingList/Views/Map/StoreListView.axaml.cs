using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ShoppingList.ViewModels.Map;

namespace ShoppingList.Views.Map;

public partial class StoreListView : UserControl
{
    public StoreListView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as StoreListViewModel)!.Search();
    }
}