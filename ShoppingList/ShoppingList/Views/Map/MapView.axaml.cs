using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using ShoppingList.Core;
using System.Collections.Generic;
using System;
using ShoppingList.Core.Enums;
using System.Linq;
using ShoppingList.ViewModels.Map;

namespace ShoppingList.Views.Map;

public partial class MapView : UserControl
{
    private List<MapSegment>? _mapSegments;

    private double _zoom = 1.0;
    private readonly double _panX = 0;
    private readonly double _panY = 0;

    private double _originalWidth;
    private double _originalHeight;

    private double _squareSize = 50;
    private readonly Canvas _canvas;
    public MapView()
    {
        InitializeComponent();

        _canvas = this.FindControl<Canvas>("MapCanvas") ?? throw new Exception("Could not find Canvas");
        _canvas.SizeChanged += Canvas_SizeChanged;
    }

    private void Canvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        _mapSegments = LoadMapSegments();
        if (_canvas.Bounds.Width > 0 && _canvas.Bounds.Height > 0)
        {
            _canvas.SizeChanged -= Canvas_SizeChanged;
            RenderMapSegments();
        }
    }

    private void RenderMapSegments()
    {
        if (_mapSegments == null) throw new Exception("MapSegments are null");

        int maxX = _mapSegments.Max(m => m.X) + 1;
        int maxY = _mapSegments.Max(m => m.Y) + 1;

        _originalWidth = maxX * _squareSize;
        _originalHeight = maxY * _squareSize;
        _canvas.Width = _originalWidth;
        _canvas.Height = _originalHeight;

        double availableWidth = _canvas.Bounds.Width;
        double availableHeight = _canvas.Bounds.Height;
        _squareSize = Math.Min(availableWidth / maxX, availableHeight / maxY);

        foreach (var segment in _mapSegments)
        {
            var border = new Border
            {
                Width = _squareSize,
                Height = _squareSize,
                Background = GetBrushForSegment(segment)
            };

            Canvas.SetLeft(border, segment.X * _squareSize);
            Canvas.SetTop(border, segment.Y * _squareSize);

            _canvas.Children.Add(border);
        }

        UpdateTransforms();
    }

    private static IBrush GetBrushForSegment(MapSegment segment)
    {
        return segment.Type switch
        {
            SegmentType.Entrance => Brushes.LightGreen,
            SegmentType.CashRegister => Brushes.LightBlue,
            SegmentType.Shelf => Brushes.Beige,
            SegmentType.Fridge => Brushes.LightCyan,
            SegmentType.Wall => Brushes.Gray,
            SegmentType.Outside => Brushes.LightYellow,
            SegmentType.Empty => Brushes.White,
            _ => Brushes.White,
        };
    }
    private void UpdateTransforms()
    {
        if (_canvas.RenderTransform is TransformGroup tg)
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

        _canvas.Width = _originalWidth * _zoom;
        _canvas.Height = _originalHeight * _zoom;
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

    private List<MapSegment> LoadMapSegments()
    {
        return (DataContext as MapViewModel)!.MapSegments.ToList();
    }
}