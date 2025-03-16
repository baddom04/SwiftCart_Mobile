using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.ViewModels.Shared;
using System;
using System.Reactive;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class ShoppingListViewModel : HouseholdListItemViewModel
    {
        public override ReactiveCommand<Unit, Unit> HouseholdOperationCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateGroceryPageCommand { get; }

        private readonly Action<ViewModelBase> _changeToPage;
        private readonly Action<GroceryPage> _changePage;
        public ShoppingListViewModel(Household household, Action<ViewModelBase> changeToPage, Action<GroceryPage> changePage)
        {
            _name = household.Name;
            _identifier = household.Identifier;
            _changeToPage = changeToPage;
            _changePage = changePage;
            HouseholdOperationCommand = ReactiveCommand.Create(() => _changeToPage(this));
            GoBackCommand = ReactiveCommand.Create(() => _changePage(GroceryPage.Main));
        }
    }
}
