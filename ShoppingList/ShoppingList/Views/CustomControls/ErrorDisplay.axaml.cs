using Avalonia;
using Avalonia.Controls;

namespace ShoppingList.Views.CustomControls;

public partial class ErrorDisplay : UserControl
{
    public ErrorDisplay()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string> ErrorMessageProperty =
        AvaloniaProperty.Register<ErrorDisplay, string>(nameof(ErrorMessage), defaultValue: "");

    public string ErrorMessage
    {
        get => GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }
}