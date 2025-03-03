using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels.Social;
namespace ShoppingList.Views.Social;

public partial class SocialPanelView : UserControl
{
    public SocialPanelView()
    {
        InitializeComponent();
        Loaded += SocialPanelView_Loaded; ;
    }

    private async void SocialPanelView_Loaded(object? sender, RoutedEventArgs e)
    {
        await (DataContext as SocialPanelViewModel)!.Search();
    }
}