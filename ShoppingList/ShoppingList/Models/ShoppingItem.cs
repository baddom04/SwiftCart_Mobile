using ReactiveUI;

namespace ShoppingList.Models
{
    public class ShoppingItem : ReactiveObject
    {
        public User Owner { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Unit Unit { get; set; }
        public string? Description { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        }
        public string QuantityDisplay => Quantity + " " + Unit.ToString();
        public ReactiveCommand<System.Reactive.Unit, bool> ToggleExpandedCommand { get; }
        public ShoppingItem(string name, User owner, int quantity = 1, Unit unit = Unit.Pieces, string? description = null)
        {
            Owner = owner;
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Description = description;

            IsExpanded = false;
            ToggleExpandedCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
        }
    }
}
