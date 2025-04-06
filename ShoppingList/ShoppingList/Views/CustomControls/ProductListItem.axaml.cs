using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace ShoppingList.Views.CustomControls;

public partial class ProductListItem : UserControl
{
    public ProductListItem()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string> ProductNameProperty =
            AvaloniaProperty.Register<ProductListItem, string>(nameof(ProductName));
    public string ProductName
    {
        get => GetValue(ProductNameProperty);
        set => SetValue(ProductNameProperty, value);
    }

    public static readonly StyledProperty<string> BrandProperty =
        AvaloniaProperty.Register<ProductListItem, string>(nameof(Brand));
    public string Brand
    {
        get => GetValue(BrandProperty);
        set => SetValue(BrandProperty, value);
    }

    public static readonly StyledProperty<uint> PriceProperty =
        AvaloniaProperty.Register<ProductListItem, uint>(nameof(Price));
    public uint Price
    {
        get => GetValue(PriceProperty);
        set => SetValue(PriceProperty, value);
    }

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<UserListItemView, bool>(nameof(IsOpen));
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<UserListItemView, string>(nameof(Description));
    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<UserListItemView, bool>(nameof(IsSelected));
    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly StyledProperty<bool> CanChangeSelectionProperty =
        AvaloniaProperty.Register<UserListItemView, bool>(nameof(CanChangeSelection), defaultValue: true);
    public bool CanChangeSelection
    {
        get => GetValue(CanChangeSelectionProperty);
        set => SetValue(CanChangeSelectionProperty, value);
    }

    public static readonly StyledProperty<ICommand> OpenCommandProperty =
        AvaloniaProperty.Register<UserListItemView, ICommand>(nameof(OpenCommand));
    public ICommand OpenCommand
    {
        get => GetValue(OpenCommandProperty);
        set => SetValue(OpenCommandProperty, value);
    }

    public static readonly StyledProperty<object> RightContentProperty =
        AvaloniaProperty.Register<HouseholdListItemView, object>(nameof(RightContent));

    public object RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }
}