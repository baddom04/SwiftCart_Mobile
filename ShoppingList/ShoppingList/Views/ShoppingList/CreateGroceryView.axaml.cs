using Avalonia.Controls;
using System.Linq;

namespace ShoppingList.Views.ShoppingList;

public partial class CreateGroceryView : UserControl
{
    public CreateGroceryView()
    {
        InitializeComponent();
    }

    private void OnQuantityTextChanged(object? sender, TextChangedEventArgs e)
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