using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;

namespace ShoppingList.Models
{
    internal class MenuItem : ReactiveObject
    {
        public string Title { get; }
        public StreamGeometry Icon { get; }
        public MenuItem(string title, string iconKey)
        {
            Title = title;

            Application.Current!.TryFindResource(iconKey, out var resource);
            Icon = (resource as StreamGeometry)!;
        }
    }
}
