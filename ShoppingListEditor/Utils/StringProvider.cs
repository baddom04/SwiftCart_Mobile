using Avalonia;
using Avalonia.Controls;
using System.Collections.Generic;

namespace ShoppingListEditor.Utils
{
    internal class StringProvider
    {
        internal static string GetString(string key)
        {
            Application.Current!.TryFindResource(key, out var resource);
            return resource as string ?? throw new KeyNotFoundException(key);
        }
    }
}
