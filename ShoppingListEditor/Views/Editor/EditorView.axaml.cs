using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using ShoppingList.Core.Enums;
using ShoppingList.Shared.Converters;
using ShoppingListEditor.Converters;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.ViewModels.Editor;
using ShoppingListEditor.Views.Editor.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShoppingListEditor.Views.Editor;

public partial class EditorView : UserControl
{
    private Point _previousPoint;
    private bool _isPanning = false;

    private List<MapSegmentEditable>? _mapSegments;

    private double _zoom = 1.0;
    private readonly double _panX = 0;
    private readonly double _panY = 0;

    private readonly int _extraPadding = 400;

    private double _originalWidth;
    private double _originalHeight;

    private double _squareSize = 50;

    private readonly SegmentTypeToColorConverter _toColorConverter = new();
    private readonly SegmentTypeToBoolConverter _toBoolConverter = new();

    public EditorView()
    {
        InitializeComponent();
        MapCanvas.SizeChanged += Canvas_SizeChanged;
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        (DataContext as EditorViewModel)!.MapChanged += LoadMap;
    }

    private void Canvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        MapCanvas.SizeChanged -= Canvas_SizeChanged;
        LoadMap();
    }
    private void LoadMap()
    {
        _mapSegments = LoadMapSegments();
        if (MapCanvas.Bounds.Width > 0 && MapCanvas.Bounds.Height > 0)
        {
            RenderMapSegments();
        }
    }

    private void RenderMapSegments()
    {
        if (_mapSegments == null) throw new Exception("MapSegments are null");

        int maxX = _mapSegments.Max(m => m.X) + 1;
        int maxY = _mapSegments.Max(m => m.Y) + 1;

        double availableWidth = MapCanvas.Bounds.Width;
        double availableHeight = MapCanvas.Bounds.Height;

        _squareSize = Math.Min(availableWidth / maxX, availableHeight / maxY);
        //_squareSize /= 1.3;

        _originalWidth = maxX * _squareSize;
        _originalHeight = maxY * _squareSize;

        MapCanvas.Width = _originalWidth;
        MapCanvas.Height = _originalHeight;


        foreach (var segment in _mapSegments)
        {
            var button = CreateButton(segment);

            Canvas.SetLeft(button, _extraPadding / 2 + segment.X * _squareSize);
            Canvas.SetTop(button, _extraPadding / 2 + segment.Y * _squareSize);

            MapCanvas.Children.Add(button);
        }

        UpdateTransforms();

        CenterScrollViewerContent();
    }

    private void UpdateTransforms()
    {
        if (MapCanvas.RenderTransform is TransformGroup tg)
        {
            foreach (var transform in tg.Children)
            {
                if (transform is ScaleTransform scaleTransform)
                {
                    scaleTransform.ScaleX = _zoom;
                    scaleTransform.ScaleY = _zoom;
                }
                else if (transform is TranslateTransform translateTransform)
                {
                    translateTransform.X = _panX;
                    translateTransform.Y = _panY;
                }
            }
        }

        MapCanvas.Width = (_originalWidth + _extraPadding) * _zoom;
        MapCanvas.Height = (_originalHeight + _extraPadding) * _zoom;
    }
    public void ZoomInButton_Click(object sender, RoutedEventArgs e)
    {
        UpdateZoom(1.2);
    }

    public void ZoomOutButton_Click(object sender, RoutedEventArgs e)
    {
        UpdateZoom(5d / 6d);
    }
    private void UpdateZoom(double amount)
    {
        _zoom *= amount;
        UpdateTransforms();
    }

    private void CenterScrollViewerContent()
    {
        MapScrollViewer.Offset = new Vector(_extraPadding / 2, _extraPadding / 2);
    }

    private List<MapSegmentEditable> LoadMapSegments()
    {
        return [.. (DataContext as EditorViewModel)!.MapSegments];
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Canvas canvas) throw new ArgumentException(null, nameof(sender));

        if (!e.GetCurrentPoint(null).Properties.IsMiddleButtonPressed) return;

        _isPanning = true;
        _previousPoint = e.GetPosition(MapScrollViewer);
        canvas.Cursor = new Cursor(StandardCursorType.Hand);
        e.Pointer.Capture(sender as IInputElement);
        e.Handled = true;
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isPanning) return;

        Point currentPoint = e.GetPosition(MapScrollViewer);
        Vector delta = currentPoint - _previousPoint;

        double newHorizontalOffset = MapScrollViewer.Offset.X - delta.X;
        double newVerticalOffset = MapScrollViewer.Offset.Y - delta.Y;
        MapScrollViewer.Offset = new Vector(newHorizontalOffset, newVerticalOffset);

        _previousPoint = currentPoint;
        e.Handled = true;
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not Canvas canvas) throw new ArgumentException(null, nameof(sender));
        if (!_isPanning || e.InitialPressMouseButton != MouseButton.Middle) return;

        _isPanning = false;
        canvas.Cursor = new Cursor(StandardCursorType.Arrow);
        e.Pointer.Capture(null);
        e.Handled = true;
    }

    private Button CreateButton(MapSegmentEditable segment)
    {
        var btn = new Button
        {
            Width = _squareSize,
            Height = _squareSize,
            Background = (IBrush)_toColorConverter.Convert(segment.Type, typeof(IBrush), null, CultureInfo.CurrentCulture)!,
            BorderBrush = Brushes.LightGray,
            BorderThickness = new Thickness(1),
            DataContext = segment,
        };

        foreach (SegmentType type in Enum.GetValues(typeof(SegmentType)))
        {
            btn.BindStyleClass(type.ToString(), CreateBinding(type));
        }

        return btn;
    }
    private Binding CreateBinding(SegmentType type)
    {
        return new Binding("SelectedSegmentType")
        {
            Converter = _toBoolConverter,
            ConverterParameter = type,
            Source = (DataContext as EditorViewModel)
        };
    }
}