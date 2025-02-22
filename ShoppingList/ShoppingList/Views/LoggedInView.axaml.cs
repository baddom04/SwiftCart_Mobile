using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using ShoppingList.ViewModels;
using System.Linq;

namespace ShoppingList;

public partial class LoggedInView : UserControl
{
    public LoggedInView()
    {
        InitializeComponent();
        DataContext = new LoggedInViewModel();
        Loaded += (_, _) => SetMenuIconColors(MainMenu);
    }
    private void ListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox listBox) return;

        SetMenuIconColors(listBox);
    }
    private static void SetMenuIconColors(ListBox listBox)
    {
        foreach (var item in listBox.GetLogicalChildren())
        {
            if ((item as ListBoxItem)!.Content == listBox.SelectedItem)
            {
                SetColorToPathIcon(item, "MainBtnBG");
            }
            else
            {
                SetColorToPathIcon(item, "MainFG");
            }
        }
    }
    private static void SetColorToPathIcon(ILogical item, string resKey)
    {
        SolidColorBrush? brush = Application.Current!.FindResource(resKey) as SolidColorBrush;
        if (brush is null) return;

        (item.GetLogicalChildren().ToList().First() as PathIcon)!.Foreground = brush;
    }
}