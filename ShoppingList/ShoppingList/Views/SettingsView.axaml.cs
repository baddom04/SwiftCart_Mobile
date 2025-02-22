using Avalonia.Controls;
using ShoppingList.ViewModels;

namespace ShoppingList.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}