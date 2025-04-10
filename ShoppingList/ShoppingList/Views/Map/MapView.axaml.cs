using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using ShoppingList.Converters;
using ShoppingList.Core;
using ShoppingList.Shared.Converters;
using ShoppingList.ViewModels.Map;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;

namespace ShoppingList.Views.Map;

public partial class MapView : UserControl
{
    private Point _previousPoint;
    private bool _isPanning = false;

    private List<MapSegment>? _mapSegments;

    private double _zoom = 1.0;
    private readonly double _panX = 0;
    private readonly double _panY = 0;

    private readonly int _extraPadding = 400;

    private double _originalWidth;
    private double _originalHeight;

    private double _squareSize = 50;

    private readonly SegmentTypeToColorConverter _toColorConverter = new();
    public MapView()
    {
        InitializeComponent();

        MapCanvas.SizeChanged += Canvas_SizeChanged;
    }

    private void Canvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        _mapSegments = LoadMapSegments();
        if (MapCanvas.Bounds.Width > 0 && MapCanvas.Bounds.Height > 0)
        {
            MapCanvas.SizeChanged -= Canvas_SizeChanged;
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

        _originalWidth = maxX * _squareSize;
        _originalHeight = maxY * _squareSize;

        MapCanvas.Width = _originalWidth;
        MapCanvas.Height = _originalHeight;


        foreach (var segment in _mapSegments)
        {
            var border = new Border
            {
                Width = _squareSize,
                Height = _squareSize,
                Background = (IBrush)_toColorConverter.Convert(segment.Type, typeof(IBrush), null, CultureInfo.CurrentCulture)!,
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                DataContext = segment,
                CornerRadius = new CornerRadius(5),
            };


            border.PointerReleased += OnMapSegmentSelected;

            if (segment.Marked)
            {
                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(1);
                border.ZIndex = 1;

                Avalonia.Application.Current!.TryFindResource("location_regular", out var res);
                border.Child = new PathIcon
                {
                    Data = res as StreamGeometry,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = Brushes.Red,
                    Width = _squareSize / 2,
                    Height = _squareSize / 2,
                };
            }

            Canvas.SetLeft(border, _extraPadding / 2 + segment.X * _squareSize);
            Canvas.SetTop(border, _extraPadding / 2 + segment.Y * _squareSize);

            MapCanvas.Children.Add(border);
        }

        UpdateTransforms();

        CenterScrollViewerContent();
    }

    private void OnMapSegmentSelected(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not Border border || border.DataContext is not MapSegment segment) throw new ArgumentException(null, nameof(sender));
        (DataContext as MapViewModel)!.SelectedMapSegment = segment;

        if ((DataContext as MapViewModel)!.SelectedProductsOnSegment.Count != 0)
            ProductOverlay.IsPaneOpen = true;
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

        MapCanvas.Width = _originalWidth * _zoom + _extraPadding;
        MapCanvas.Height = _originalHeight * _zoom + _extraPadding;
    }
    public void ZoomInButton_Click(object sender, RoutedEventArgs e)
    {
        _zoom *= 1.2;
        UpdateTransforms();
    }

    public void ZoomOutButton_Click(object sender, RoutedEventArgs e)
    {
        _zoom /= 1.2;
        UpdateTransforms();
    }

    private void CenterScrollViewerContent()
    {
        MapScrollViewer.Offset = new Vector(_extraPadding / 2, _extraPadding / 2);
    }

    private List<MapSegment> LoadMapSegments()
    {
        return (DataContext as MapViewModel)!.MapSegments.ToList();
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
}