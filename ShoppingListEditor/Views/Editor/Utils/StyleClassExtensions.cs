using Avalonia.Controls;
using Avalonia;
using Avalonia.Data;
using System;
using System.Collections.Generic;

namespace ShoppingListEditor.Views.Editor.Utils
{
    public static class StyleClassExtensions
    {
        private static readonly Dictionary<string, AttachedProperty<bool>> RegisteredProperties = [];

        public static void BindStyleClass(this Control control, string styleClass, Binding binding)
        {
            if (!RegisteredProperties.TryGetValue(styleClass, out var attachedProperty))
            {
                attachedProperty = AvaloniaProperty.RegisterAttached<Control, bool>(
                    styleClass,
                    typeof(StyleClassExtensions),
                    defaultValue: false,
                    inherits: false,
                    defaultBindingMode: BindingMode.OneWay);

                attachedProperty.Changed.Subscribe(args =>
                {
                    if (args.Sender is Control ctrl)
                    {
                        bool isApplied = args.NewValue.GetValueOrDefault();
                        if (isApplied)
                        {
                            if (!ctrl.Classes.Contains(styleClass))
                                ctrl.Classes.Add(styleClass);
                        }
                        else
                        {
                            ctrl.Classes.Remove(styleClass);
                        }
                    }
                });

                RegisteredProperties[styleClass] = attachedProperty;
            }

            control.Bind(attachedProperty, binding);
        }
    }
}
