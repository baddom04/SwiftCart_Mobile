using Avalonia.Controls;
using ShoppingList.ViewModels;

namespace ShoppingList.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();

        Loaded += LoginView_Loaded;
    }

    private async void LoginView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoginViewModel viewModel = (DataContext as LoginViewModel)!;
        await viewModel.TryLogin();
    }
}