using Avalonia.Controls;
using ShoppingList.Shared.ViewModels.Login;

namespace ShoppingListEditor.Views.Login;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is not LoginViewModel viewModel) throw new System.Exception("Not correct DataContext");
        await viewModel.TryLoginAsync();
    }
}