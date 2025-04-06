using Avalonia.Controls;
using ShoppingListEditor.ViewModels.Editor.Pane;
using System;

namespace ShoppingListEditor.Views.Editor.Pane;

public partial class SectionPaneView : UserControl
{
    private readonly Func<string, bool> _validateSectionName;
    public SectionPaneView()
    {
        InitializeComponent();
        _validateSectionName = (str) => !string.IsNullOrWhiteSpace(str) && str.Length <= 20;
    }

    private async void AddSection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn) throw new ArgumentException(null, nameof(sender));

        string? newName = await App.MainView.ShowTextInputDialogAsync("SectionName", _validateSectionName);
        if (newName is null) return;

        await (DataContext as SectionPaneViewModel)!.AddSectionAsync(newName);
    }

    private async void UpdateSection_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.DataContext is not SectionViewModel vm) throw new ArgumentException(null, nameof(sender));

        string? newName = await App.MainView.ShowTextInputDialogAsync("SectionName", _validateSectionName);
        if (newName is null) return;

        await vm.UpdateSectionAsync(newName);
    }
}