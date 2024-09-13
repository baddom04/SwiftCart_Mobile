using ReactiveUI;
using ShoppingList.Models;
using System.Collections.ObjectModel;

namespace ShoppingList.ViewModels
{
    public class GroceryListViewModel
    {
        public ObservableCollection<ShoppingItem> ShoppingList { get; }
        public bool InputMode { get; set; }
        public ReactiveCommand<System.Reactive.Unit, bool> InputModeToggleCommand { get; set; }

        public GroceryListViewModel()
        {
            InputMode = false;

            ShoppingList = [
                new ShoppingItem("Chips", 2, Unit.Pound),
                new ShoppingItem("Matek füzet", 3),
                new ShoppingItem("Mangó szörp"),
                new ShoppingItem("Mosópor"),
                new ShoppingItem("Fogkrém", 5),
            ];

            InputModeToggleCommand = ReactiveCommand.Create(() => InputMode = !InputMode);
        }
    }
}
