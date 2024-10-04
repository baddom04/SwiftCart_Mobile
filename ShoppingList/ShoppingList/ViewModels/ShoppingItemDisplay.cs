using ReactiveUI;
using System;
using System.Reactive;
using ShoppingList.Utils;

namespace ShoppingList.ViewModels
{
    internal class ShoppingItemDisplay : ReactiveObject
    {
        private ShoppingItem _item;
        public ShoppingItem Item
        {
            get { return _item; }
            set { this.RaiseAndSetIfChanged(ref _item, value); }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        }

        public bool IsOwner => App.CurrentUser!.Equals(Item.Owner);
        public string QuantityDisplay => Item.Quantity + " " + Item.Unit.ToString();
        public ReactiveCommand<Unit, bool> ToggleExpandedCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCommand { get; }
        public Action Editing { get; }
        public ShoppingItemDisplay(ShoppingItem item, Action editing)
        {
            _item = item;
            ToggleExpandedCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
            Editing = editing;
            EditCommand = ReactiveCommand.Create(() => editing());

        }
    }
}
