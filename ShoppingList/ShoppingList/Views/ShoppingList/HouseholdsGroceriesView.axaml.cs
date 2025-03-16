using Avalonia.Controls;
using ShoppingList.ViewModels.ShoppingList;

namespace ShoppingList.Views.ShoppingList;

public partial class HouseholdsGroceriesView : UserControl
{
    public HouseholdsGroceriesView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as HouseholdsGroceriesViewModel)!.LoadMyHouseholds();
    }
}