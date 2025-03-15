using Avalonia;
using Avalonia.Controls;

namespace ShoppingList.Views.CustomControls;

public partial class UserListItemView : UserControl
{
    public UserListItemView()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<object> RightContentProperty =
            AvaloniaProperty.Register<UserListItemView, object>(nameof(RightContent));

    public object RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<string> UserNameProperty =
            AvaloniaProperty.Register<UserListItemView, string>(nameof(UserName));
    public string UserName
    {
        get => GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public static readonly StyledProperty<string> EmailProperty =
            AvaloniaProperty.Register<UserListItemView, string>(nameof(Email));
    public string Email
    {
        get => GetValue(EmailProperty);
        set => SetValue(EmailProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
            AvaloniaProperty.Register<UserListItemView, bool>(nameof(IsLoading));
    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
}