using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using ShoppingList.Converters;
using ShoppingList.Core;
using ShoppingList.ViewModels.Map;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;

namespace ShoppingList.Views.Map;

public partial class MapView : UserControl
{
    private List<MapSegment>? _mapSegments;

    private double _zoom = 1.0;
    private readonly double _panX = 0;
    private readonly double _panY = 0;

    private readonly int _extraPadding = 400;

    private double _originalWidth;
    private double _originalHeight;

    private double _squareSize = 50;
    private readonly Canvas _canvas;

    private readonly SegmentTypeToColorConverter _toColorConverter = new();
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

        double availableWidth = _canvas.Bounds.Width;
        double availableHeight = _canvas.Bounds.Height;

        _squareSize = Math.Min(availableWidth / maxX, availableHeight / maxY);

        _originalWidth = maxX * _squareSize;
        _originalHeight = maxY * _squareSize;

        _canvas.Width = _originalWidth;
        _canvas.Height = _originalHeight;


        foreach (var segment in _mapSegments)
        {
            var border = new Border
            {
                Width = _squareSize,
                Height = _squareSize,
                Background = (IBrush)_toColorConverter.Convert(segment.Type, typeof(IBrush), null, CultureInfo.CurrentCulture)!,
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                DataContext = segment
            };

            if (segment.Marked)
            {

                border.BorderBrush = Brushes.Red;
                border.BorderThickness = new Thickness(1);
                border.ZIndex = 1;

                Avalonia.Application.Current!.TryFindResource("star_regular", out var res);
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

            _canvas.Children.Add(border);
        }

        UpdateTransforms();

        CenterScrollViewerContent();
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

        _canvas.Width = _originalWidth * _zoom + _extraPadding;
        _canvas.Height = _originalHeight * _zoom + _extraPadding;
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
        var scrollViewer = this.FindControl<ScrollViewer>("MapScrollViewer");
        if (scrollViewer != null)
        {
            scrollViewer.Offset = new Vector(_extraPadding / 2, _extraPadding / 2);
        }
    }

    private List<MapSegment> LoadMapSegments()
    {
        return (DataContext as MapViewModel)!.MapSegments.ToList();
    }
}