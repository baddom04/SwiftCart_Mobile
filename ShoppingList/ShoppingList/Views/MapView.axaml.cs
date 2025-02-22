using Avalonia.Controls;
using ShoppingList.ViewModels;

namespace ShoppingList;

public partial class MapView : UserControl
{
    public MapView()
    {
        InitializeComponent();
        DataContext = new MapViewModel();
    }
}