using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels.Settings;

namespace ShoppingList.Views.Settings;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as SettingsViewModel)!.LoadUserAsync();
    }

    private async void Logout_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync();
        if (!result) return;

        await (DataContext as SettingsViewModel)!.LogoutAsync();
    }

    private async void DeleteUser_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync();
        if (!result) return;

        await (DataContext as SettingsViewModel)!.DeleteUserAsync();
    }

    private async void ChangeUserName_Click(object? sender, RoutedEventArgs e)
    {
        string? result = await App.MainView.ShowTextInputDialogAsync("NewUsername", (str) => !string.IsNullOrWhiteSpace(str));
        if(result is null) return;

        await (DataContext as SettingsViewModel)!.UpdateUsernameAsync(result);
    }
}