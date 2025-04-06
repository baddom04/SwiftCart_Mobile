using Avalonia.Controls;
using ShoppingList.ViewModels.ShoppingList;
using System;

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

    private async void DeleteItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(sender is not Button btn || btn.DataContext is not ShoppingItemViewModel shoppingItemViewModel)
            throw new ArgumentException(null, nameof(sender));

        bool answer = await App.MainView.ShowConfirmDialogAsync("DeleteItemQuestion");
        if (!answer) return;

        await shoppingItemViewModel.DeleteGroceryAsync();
    }

    private async void BoughtItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.DataContext is not ShoppingItemViewModel shoppingItemViewModel)
            throw new ArgumentException(null, nameof(sender));

        bool answer = await App.MainView.ShowConfirmDialogAsync("BoughtItemQuestion");
        if (!answer) return;

        await shoppingItemViewModel.DeleteGroceryAsync();
    }

    private async void AddComment_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.DataContext is not ShoppingItemViewModel shoppingItemViewModel)
            throw new ArgumentException(null, nameof(sender));

        string? answer = await App.MainView.ShowTextInputDialogAsync("NewComment", (str) => !string.IsNullOrWhiteSpace(str) && str.Length <= 255);
        if (answer is null) return;

        await shoppingItemViewModel.AddCommentAsync(answer);
    }
}