using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels.Map;
using System;

namespace ShoppingList.Views.Map;

public partial class StoreSettingsView : UserControl
{
    public StoreSettingsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not StoreSettingsViewModel viewModel) throw new ArgumentException(null, nameof(sender));

        await viewModel.GetMyHouseholdsAsync();
    }
}