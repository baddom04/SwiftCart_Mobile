using ReactiveUI;
using System;
using System.Reactive;

namespace ShoppingList.Models
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

        public string QuantityDisplay => Item.Quantity + " " + Item.Unit.ToString();
        public ReactiveCommand<Unit, bool> ToggleExpandedCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCommand { get; }

        public event Action<ShoppingItemDisplay>? Editing;
        public ShoppingItemDisplay(ShoppingItem item)
        {
            _item = item;
            IsExpanded = false;
            ToggleExpandedCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
            EditCommand = ReactiveCommand.Create(() => Editing?.Invoke(this));
        }
    }
}
