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
        private readonly Action<ViewModelBase> _changeToPage;
        public ShoppingListViewModel(Household household, Action<ViewModelBase> changeToPage)
        {
            _name = household.Name;
            _identifier = household.Identifier;
            _changeToPage = changeToPage;
            HouseholdOperationCommand = ReactiveCommand.Create(() => _changeToPage(this));
        }
    }
}
