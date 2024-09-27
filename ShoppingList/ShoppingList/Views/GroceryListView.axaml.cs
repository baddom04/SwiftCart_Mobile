using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class GroceryListView : UserControl
{
    public bool _confirmation;

    private GroceryListViewModel viewModel;
    public GroceryListView()
    {
        InitializeComponent();
        viewModel = new GroceryListViewModel();
        DataContext = viewModel;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button) return;
        if (Validate())
        {
            viewModel.Save();
        }
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

        DialogOverlay.IsVisible = true;

        while (DialogOverlay.IsVisible) await Task.Delay(100);

        if (_confirmation)
        {
            // TODO: send notification to users
            item.Editing -= viewModel.OnEditing;
            viewModel.ShoppingList.Remove(item);
        }
    }
    private void Confirm_Click(object? sender, RoutedEventArgs e)
    {
        _confirmation = true;
        DialogOverlay.IsVisible = false;
    }
    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        _confirmation = false;
        DialogOverlay.IsVisible = false;
    }
    private void Bought_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is null || btn.Tag is not ShoppingItemDisplay item) return;

        // TODO: send notification to users
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