using Avalonia;
using Avalonia.Controls;
using System.Collections.Generic;

namespace ShoppingList.Utils
{
    public static class StringProvider
    {
        public static string GetString(string key)
        {
            Application.Current!.TryFindResource(key, out var resource);
            return resource as string ?? throw new KeyNotFoundException();
        }
    }
}
