using Avalonia.Controls;
using ShoppingList.ViewModels;
namespace ShoppingList.Views;

public partial class SocialPanelView : UserControl
{
    public SocialPanelView()
    {
        InitializeComponent();
        DataContext = new SocialPanelViewModel();
    }
}