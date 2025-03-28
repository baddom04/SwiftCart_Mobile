using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ShoppingList.Utils
{
    internal class MenuIcon
    {
        public string Title { get; }
        public StreamGeometry Icon { get; }
        public MenuIcon(string title, string iconKey)
        {
            Title = title;

            Application.Current!.TryFindResource(iconKey, out var resource);
            Icon = (resource as StreamGeometry)!;
        }
    }
}
