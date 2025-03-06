using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels.Social;
using System;

namespace ShoppingList.Views.Social;

public partial class ManageHouseholdsView : UserControl
{
    public ManageHouseholdsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        await (DataContext as ManageHouseholdsViewModel)!.LoadMyHouseholds();
    }
}