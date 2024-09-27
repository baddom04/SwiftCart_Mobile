using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ShoppingList.Models
{
    public class MenuItem
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
