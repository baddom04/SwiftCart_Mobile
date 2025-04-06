using Avalonia.Controls;
using ShoppingList.ViewModels.Social;

namespace ShoppingList.Views.Social;

public partial class HouseholdView : UserControl
{
    public HouseholdView()
    {
        InitializeComponent();
    }

    private async void DeleteHoushold_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync("DeleteHouseholdQuestion");
        if (!result) return;

        (DataContext as HouseholdViewModel)!.DeleteHouseholdCommand.Execute();
    }
    private async void LeaveHoushold_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        bool result = await App.MainView.ShowConfirmDialogAsync("LeaveHouseholdQuestion");
        if (!result) return;

        (DataContext as HouseholdViewModel)!.LeaveHouseholdCommand.Execute();
    }
}