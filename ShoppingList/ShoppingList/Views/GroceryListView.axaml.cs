using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ShoppingList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Views;

public partial class GroceryListView : UserControl
{
    private GroceryListViewModel _viewModel = null!;
    public GroceryListView()
    {
        InitializeComponent();

        Loaded += (s, e) =>
        {
            _viewModel = (DataContext as GroceryListViewModel)!;
        };
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
    private void Display_Click(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border border) return;
        ShoppingItemDisplay display = (border.DataContext as ShoppingItemDisplay)!;

        display.IsExpanded = !display.IsExpanded;
    }

    private async void DeleteItem_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not ShoppingItemDisplay itemDisplay)
            throw new ArgumentException("Invalid argument", nameof(sender));

        Application.Current!.TryFindResource("DeleteGroceryConfirmQuestion", out var res);
        bool result = await App.MainView.ShowConfirmDialog(res as string ?? throw new KeyNotFoundException());

        if (!result) return;

        _viewModel.Model.DeleteItem(itemDisplay.Item);
    }

    private async void Comment_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not ShoppingItemDisplay itemDisplay)
            throw new ArgumentException("Invalid argument", nameof(sender));

        string? comment = await App.MainView.ShowTextInputDialog("Comment", (input) => !string.IsNullOrWhiteSpace(input));
        if (string.IsNullOrWhiteSpace(comment)) return;
        _viewModel.Model.AddComment(itemDisplay.Item, App.CurrentUser!, comment);
    }
}