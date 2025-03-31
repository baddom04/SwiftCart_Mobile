using Avalonia.Controls;
using ShoppingList.Shared.ViewModels.Login;

namespace ShoppingListEditor.Views.Login;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        Loaded += LoginView_Loaded;
    }

    private async void LoginView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not LoginViewModel viewModel) throw new System.Exception("Not correct DataContext");
        await viewModel.TryLogin();
    }
}