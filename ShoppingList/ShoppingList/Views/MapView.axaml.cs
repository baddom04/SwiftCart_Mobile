using Avalonia.Controls;
using ShoppingList.ViewModels;

namespace ShoppingList.Views;

public partial class MapView : UserControl
{
    public MapView()
    {
        InitializeComponent();
        DataContext = new MapViewModel();
    }
}