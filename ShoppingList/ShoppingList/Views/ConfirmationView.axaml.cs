using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class ConfirmationView : UserControl
{
    private TaskCompletionSource<bool>? _tcs;
    public ConfirmationView(string question)
    {
        InitializeComponent();
        DialogQuestion.Text = question;
    }
    public Task<bool> ShowDialog()
    {
        _tcs = new TaskCompletionSource<bool>();
        DialogOverlay.IsVisible = true;
        return _tcs.Task;
    }

    private void Confirm_Click(object? sender, RoutedEventArgs e)
    {
        CloseDialog(true);
    }
    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        CloseDialog(false);
    }
    private void CloseDialog(bool result)
    {
        if (!DialogOverlay.IsVisible || _tcs is null) throw new System.Exception("This should not happen :(");

        DialogOverlay.IsVisible = false;
        _tcs.SetResult(result);
    }
}