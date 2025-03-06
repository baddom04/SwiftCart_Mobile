using ReactiveUI;
using ShoppingList.Core;
using System;
using System.Reactive;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdViewModel : HouseholdListItemViewModel
    {
        public ReactiveCommand<Unit, Unit> GoToHouseholdPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        private readonly Action<SocialPage> _changePage;
        private readonly Action<HouseholdViewModel> _changeToHouseholdPage;
        public HouseholdViewModel(Household household, Action<SocialPage> changePage, Action<HouseholdViewModel> changeToHouseholdPage)
        {
            _name = household.Name;
			_identifier = household.Identifier;
            _changePage = changePage;
            _changeToHouseholdPage = changeToHouseholdPage;
            GoToHouseholdPageCommand = ReactiveCommand.Create(() => _changeToHouseholdPage(this));
            GoBackCommand = ReactiveCommand.Create(() => _changePage(SocialPage.ManageHouseholds));
        }
    }
}
