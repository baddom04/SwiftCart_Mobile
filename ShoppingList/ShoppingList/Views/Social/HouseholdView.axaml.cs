using Avalonia.Controls;
using ShoppingList.ViewModels.Social;

namespace ShoppingList.Views.Social;

public partial class HouseholdView : UserControl
{
    public HouseholdView()
    {
        InitializeComponent();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string? result = await App.MainView.ShowTextInputDialog("NewUsername", (str) => !string.IsNullOrWhiteSpace(str));
        if (result is null) return;

        //await (DataContext as HouseholdViewModel)!.UpdateUsername(result);
    }
}