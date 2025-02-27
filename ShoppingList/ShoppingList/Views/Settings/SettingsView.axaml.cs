using Avalonia.Controls;
using ShoppingList.ViewModels.Settings;

namespace ShoppingList.Views.Settings;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        Loaded += SettingsView_Loaded;
    }

    private async void SettingsView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as SettingsViewModel)!.LoadUser();
    }

    private async void Logout_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialog();
        if (!result) return;

        await (DataContext as SettingsViewModel)!.Logout();
    }

    private async void DeleteUser_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialog();
        if (!result) return;

        await (DataContext as SettingsViewModel)!.DeleteUser();
    }
}