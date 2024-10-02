using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using System.Linq;

namespace ShoppingList.Views;

public partial class GroceryListView : UserControl
{
    private readonly GroceryListViewModel viewModel;
    public GroceryListView()
    {
        InitializeComponent();
        viewModel = new GroceryListViewModel();
        DataContext = viewModel;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button || !Validate()) return;

        viewModel.Save();
    }

    private bool Validate()
    {
        if (string.IsNullOrWhiteSpace(ItemName.Text))
        {
            ErrorMessage.Text = "The item's name cannot be empty!";
            return false;
        }
        else if (string.IsNullOrWhiteSpace(ItemQuantity.Text))
        {
            ErrorMessage.Text = "The item's quantity cannot be empty!";
            return false;
        }

        return true;
    }
    private async void Delete_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItemDisplay item) return;

        bool result = await App.MainView!.ShowConfirmDialog("Are you sure you want to delete this item?");

        if (!result) return;

        // TODO: send notification to users on this household
        item.Editing -= viewModel.OnEditing;
        viewModel.ShoppingList.Remove(item);
    }
    private void Bought_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItemDisplay item) return;

        // TODO: send notification to users on this household
        item.Editing -= viewModel.OnEditing;
        viewModel.ShoppingList.Remove(item);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox || textBox.Text is null) return;

        string newText = new(textBox.Text.Where(char.IsDigit).ToArray());

        if (newText != textBox.Text)
        {
            textBox.Text = newText;
            textBox.CaretIndex = newText.Length;
        }
    }
}