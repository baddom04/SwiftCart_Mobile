using ReactiveUI;
using ShoppingList.Core;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class ShoppingItemViewModel : ViewModelBase
    {
        //private ShoppingItem _item;
        //public ShoppingItem Item
        //{
        //    get { return _item; }
        //    set { this.RaiseAndSetIfChanged(ref _item, value); }
        //}

        //private bool _isExpanded;
        //public bool IsExpanded
        //{
        //    get { return _isExpanded; }
        //    set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        //}

        //public bool IsOwner => true;
        //public string QuantityDisplay => Item.Quantity + " " + Item.Unit.ToString();
        //public ReactiveCommand<Unit, bool> ToggleExpandedCommand { get; }
        //public ReactiveCommand<Unit, Unit> EditCommand { get; }
        //public Action Editing { get; }
        //public ShoppingItemViewModel(ShoppingItem item, Action editing)
        //{
        //    _item = item;
        //    ToggleExpandedCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
        //    Editing = editing;
        //    EditCommand = ReactiveCommand.Create(() => editing());
        //}

        public string Name { get; }
        public ShoppingItemViewModel(Grocery grocery)
        {
            Name = grocery.Name;
        }
    }
}
