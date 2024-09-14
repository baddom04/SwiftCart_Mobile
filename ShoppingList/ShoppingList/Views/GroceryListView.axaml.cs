using Avalonia.Controls;
using Avalonia.Input;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class GroceryListView : UserControl
{
    public bool _confirmation;

    private readonly GroceryListViewModel viewModel;
    public GroceryListView()
    {
        InitializeComponent();
        viewModel = new GroceryListViewModel();
        DataContext = viewModel;
    }
    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button) return;

        viewModel.ShoppingList.Add(new ShoppingItem(ItemName.Text!, App.CurrentUser, Int32.Parse(ItemQuantity.Text!), (Models.Unit)ItemUnit.SelectedItem!, ItemDescription.Text));
        viewModel.ToggleInputMode();
    }

    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItem item) return;

        DialogOverlay.IsVisible = true;

        while (DialogOverlay.IsVisible) await Task.Delay(100);

        if (_confirmation)
            viewModel.ShoppingList.Remove(item);
    }
    private void Confirm_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _confirmation = true;
        DialogOverlay.IsVisible = false;
    }
    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _confirmation = false;
        DialogOverlay.IsVisible = false;
    }
    private void Bought_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItem item) return;

        viewModel.ShoppingList.Remove(item);
    }
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
            return;

        if (e.Key < Key.D0 || e.Key > Key.D9)
            e.Handled = true;
    }
}