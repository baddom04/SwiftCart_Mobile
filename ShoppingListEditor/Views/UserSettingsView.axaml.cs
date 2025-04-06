using Avalonia.Controls;
using Avalonia.Input;
using ShoppingListEditor.ViewModels;

namespace ShoppingListEditor.Views;

public partial class UserSettingsView : UserControl
{
    public UserSettingsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as UserSettingsViewModel)!.LoadUserAsync();
    }

    private async void Logout_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync("ConfirmLogoutQuestion");
        if (!result) return;

        await (DataContext as UserSettingsViewModel)!.LogoutAsync();
    }

    private async void DeleteUser_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync("ConfirmDeleteUserQuestion");
        if (!result) return;

        await (DataContext as UserSettingsViewModel)!.DeleteUserAsync();
    }
    private async void DeleteStore_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync("ConfirmDeleteStoreQuestion");
        if (!result) return;

        await (DataContext as UserSettingsViewModel)!.DeleteStoreAsync();
    }
}