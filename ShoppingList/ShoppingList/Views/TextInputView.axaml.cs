using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class TextInputView : UserControl
{
    private readonly Func<string, bool> _validateInput;
    private TaskCompletionSource<string?>? _taskCompletionSource;
    public TextInputView(string instructionKey, Func<string, bool> validateInput)
    {
        InitializeComponent();
        Application.Current!.TryFindResource(instructionKey, out var res);
        Instruction.Text = res as string ?? throw new KeyNotFoundException();
        _validateInput = validateInput;
        Input.Text = string.Empty;
        Input.Focus();
    }
    public Task<string?> ShowDialog()
    {
        if (DialogOverlay.IsVisible) throw new Exception("A dialog overlay is already shown!");

        _taskCompletionSource = new TaskCompletionSource<string?>();
        DialogOverlay.IsVisible = true;
        return _taskCompletionSource.Task;
    }
    private void SendInput_Click(object? sender, RoutedEventArgs e)
    {
        if (_taskCompletionSource is null || !DialogOverlay.IsVisible) throw new Exception("This should not happen :(");
        DialogOverlay.IsVisible = false;
        _taskCompletionSource.SetResult(Input.Text!);
    }
    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        if (_taskCompletionSource is null || !DialogOverlay.IsVisible) throw new Exception("This should not happen :(");
        DialogOverlay.IsVisible = false;
        _taskCompletionSource.SetResult(null);
    }
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SendInputBtn.IsEnabled = _validateInput(Input.Text!);
    }
}