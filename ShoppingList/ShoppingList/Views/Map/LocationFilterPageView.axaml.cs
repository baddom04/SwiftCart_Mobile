using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels.Map;
using System;

namespace ShoppingList.Views.Map;

public partial class LocationFilterPageView : UserControl
{
    public LocationFilterPageView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        await (DataContext as LocationFilterPageViewModel)!.GetPossibleCountriesAsync();
    }
}