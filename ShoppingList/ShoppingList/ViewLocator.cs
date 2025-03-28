using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ShoppingList.Shared;
using System;

namespace ShoppingList
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
                return null;

            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            // If can't find the type, check in this project as well, not in ShoppingList.Shared
            if (type == null)
                name = name.Replace(".Shared", "");

            type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}