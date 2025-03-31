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
        await (DataContext as UserSettingsViewModel)!.LoadUser();
    }

    private async void Logout_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialog("ConfirmLogoutQuestion");
        if (!result) return;

        await (DataContext as UserSettingsViewModel)!.Logout();
    }

    private async void DeleteUser_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialog("ConfirmDeleteUserQuestion");
        if (!result) return;

        await (DataContext as UserSettingsViewModel)!.DeleteUser();
    }
}