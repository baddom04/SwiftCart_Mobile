using Avalonia.Controls;
using ShoppingList.ViewModels;
namespace ShoppingList;

public partial class SocialPanelView : UserControl
{
    public SocialPanelView()
    {
        InitializeComponent();
        DataContext = new SocialPanelViewModel();
    }
}