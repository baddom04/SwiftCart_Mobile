using ReactiveUI;
using ShoppingList.Core;
using System;
using System.Reactive;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdViewModel : HouseholdListItemViewModel
    {
        public ReactiveCommand<Unit, Unit> GoToHouseholdPageCommand { get; }
        private readonly Action<SocialPage> _changePage;
        public HouseholdViewModel(Household household, Action<SocialPage> changePage)
        {
            _name = household.Name;
			_identifier = household.Identifier;
            _changePage = changePage;
            GoToHouseholdPageCommand = ReactiveCommand.Create(() => _changePage(SocialPage.HouseholdPage));
        }
    }
}
