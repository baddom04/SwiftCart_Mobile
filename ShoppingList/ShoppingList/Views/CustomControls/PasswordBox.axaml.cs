using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;

namespace ShoppingList.Views.CustomControls;

public partial class PasswordBox : UserControl
{
    private bool _showPassword = true;
    public PasswordBox()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PasswordInput.PasswordChar = _showPassword ? '\0' : '*';
        _showPassword = !_showPassword;
        Application.Current!.TryFindResource(_showPassword ? "eye_hide_regular" : "eye_show_regular", out var res);
        Icon.Data = res as StreamGeometry ?? throw new KeyNotFoundException();
    }

    public static readonly StyledProperty<string> PasswordProperty =
        AvaloniaProperty.Register<PasswordBox, string>(nameof(Password), defaultValue: "");

    public string Password
    {
        get => GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }
}