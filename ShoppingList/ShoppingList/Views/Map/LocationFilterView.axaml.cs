using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Map;
using System.Threading.Tasks;

namespace ShoppingList.Views.Map;

public partial class LocationFilterView : UserControl
{
    public LocationFilterView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        LocationAutoComplete.AttachedToVisualTree += LocationAutoComplete_AttachedToVisualTree;
        LocationAutoComplete.DetachedFromVisualTree += LocationAutoComplete_DetachedFromVisualTree;
    }

    private void LocationAutoComplete_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        //LocationAutoComplete.SelectionChanged += AutoCompleteBox_SelectionChanged;
        Task.Run(SubscribeToEvent);
    }
    private async void SubscribeToEvent()
    {
        await Task.Delay(500);
        LocationAutoComplete.SelectionChanged += AutoCompleteBox_SelectionChanged;
    }

    private void LocationAutoComplete_DetachedFromVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        LocationAutoComplete.SelectionChanged -= AutoCompleteBox_SelectionChanged;
    }


    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        LocationFilterViewModel viewModel = (DataContext as LocationFilterViewModel)!;
        LocationFilterLabel.Text = StringProvider.GetString(viewModel.InputNameKey);
    }
    private async void AutoCompleteBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not LocationFilterViewModel viewModel) return;
        await viewModel.OnLocationSelected();
    }
}