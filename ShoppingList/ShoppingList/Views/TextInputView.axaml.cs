using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading.Tasks;

namespace ShoppingList;

public partial class TextInputView : UserControl
{
    private readonly Func<string, bool> _validateInput;
    private TaskCompletionSource<string?>? _tcs;
    public TextInputView(string instruction, Func<string, bool> validateInput)
    {
        InitializeComponent();
        Instruction.Text = instruction;
        _validateInput = validateInput;
        Input.Text = string.Empty;
        Input.Focus();
    }
    public Task<string?> ShowDialog()
    {
        if (DialogOverlay.IsVisible) throw new Exception("A dialog overlay is already shown!");

        _tcs = new TaskCompletionSource<string?>();
        DialogOverlay.IsVisible = true;
        return _tcs.Task;
    }
    private void SendInput_Click(object? sender, RoutedEventArgs e)
    {
        if (_tcs is null || !DialogOverlay.IsVisible) throw new Exception("This should not happen :(");
        DialogOverlay.IsVisible = false;
        _tcs.SetResult(Input.Text!);
    }
    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        if (_tcs is null || !DialogOverlay.IsVisible) throw new Exception("This should not happen :(");
        DialogOverlay.IsVisible = false;
        _tcs.SetResult(null);
    }
    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SendInputBtn.IsEnabled = _validateInput(Input.Text!);
    }
}