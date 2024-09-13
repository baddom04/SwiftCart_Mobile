using Avalonia.Controls;
using ShoppingList.Models;
using ShoppingList.ViewModels;

namespace ShoppingList.Views;

public partial class GroceryListView : UserControl
{
    private GroceryListViewModel viewModel;
    public GroceryListView()
    {
        InitializeComponent();
        viewModel = new GroceryListViewModel();
        DataContext = viewModel;
    }

    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItem item) return;

        var confirmationView = new ConfirmationView("Are you sure you want to delete this item?");
        await confirmationView.ShowDialog((Window)VisualRoot!);

        if (!confirmationView.DialogResult.HasValue || !confirmationView.DialogResult.Value) return;

        viewModel.ShoppingList.Remove(item);
    }
    private void Bought_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItem item) return;

        viewModel.ShoppingList.Remove(item);
    }
}