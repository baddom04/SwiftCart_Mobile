using Avalonia.Controls;
using ShoppingList.ViewModels;

namespace ShoppingList;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}