
using ShoppingList.Models;
using System.Collections.ObjectModel;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ShoppingItem> ShoppingList { get; }

        public MainViewModel()
        {
            ShoppingList = [
                new ShoppingItem("Chips", 2, true),
                new ShoppingItem("Matek füzet", 3),
                new ShoppingItem("Mangó szörp"),
                new ShoppingItem("Mosópor"),
                new ShoppingItem("Fogkrém", 5),
            ];
        }
    }
}
