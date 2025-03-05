using Avalonia.Controls;
using ShoppingList.ViewModels.Social;

namespace ShoppingList.Views.Social;

public partial class ManageApplicationsView : UserControl
{
    public ManageApplicationsView()
    {
        InitializeComponent();
        Loaded += ManageApplicationsView_Loaded;
    }

    private async void ManageApplicationsView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await (DataContext as ManageApplicationsViewModel)!.LoadApplications();
    }
}