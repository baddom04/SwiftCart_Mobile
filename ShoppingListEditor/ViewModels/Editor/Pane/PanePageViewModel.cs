using ShoppingList.Shared;
using System;

namespace ShoppingListEditor.ViewModels.Editor.Pane
{
    internal class PanePageViewModel : ViewModelBase
    {
        public Action? GoBack { get; set; }
    }
}
