using Avalonia;
using Avalonia.Controls;

namespace ShoppingList.Views.CustomControls;

public partial class HouseholdListItemView : UserControl
{
    public HouseholdListItemView()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<object> RightContentProperty =
            AvaloniaProperty.Register<HouseholdListItemView, object>(nameof(RightContent));

    public object RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public static readonly StyledProperty<string> HouseholdNameProperty =
            AvaloniaProperty.Register<HouseholdListItemView, string>(nameof(HouseholdName));
    public string HouseholdName
    {
        get => GetValue(HouseholdNameProperty);
        set => SetValue(HouseholdNameProperty, value);
    }

    public static readonly StyledProperty<string> IdentifierProperty =
            AvaloniaProperty.Register<HouseholdListItemView, string>(nameof(Identifier));
    public string Identifier
    {
        get => GetValue(IdentifierProperty);
        set => SetValue(IdentifierProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
            AvaloniaProperty.Register<HouseholdListItemView, bool>(nameof(IsLoading));
    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
}