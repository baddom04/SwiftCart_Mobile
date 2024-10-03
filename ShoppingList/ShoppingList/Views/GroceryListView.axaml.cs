using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels;
using System.Linq;

namespace ShoppingList.Views;

public partial class GroceryListView : UserControl
{
    public GroceryListView()
    {
        InitializeComponent();
        DataContext = new GroceryListViewModel();
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