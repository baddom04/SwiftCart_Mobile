using ShoppingList.Shared;
using System;

namespace ShoppingListEditor.ViewModels.Editor.Pane
{
    internal class PanePageViewModel : ViewModelBase
    {
        public required Action GoBack { get; set; }
    }
}
