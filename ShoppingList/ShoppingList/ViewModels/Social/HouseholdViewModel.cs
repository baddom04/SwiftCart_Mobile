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
        public ReactiveCommand<Unit, Unit> EditHouseholdCommand { get; }
        private readonly Action<SocialPage> _changePage;
        private readonly Action<HouseholdViewModel> _changeToHouseholdPage;
        private readonly Household _household;

        public bool IsOwner { get; }
        public HouseholdViewModel(Household household, Action<SocialPage> changePage, Action<HouseholdViewModel> changeToHouseholdPage, Action<Household?> householdEditingPage)
        {
            _name = household.Name;
			_identifier = household.Identifier;
            _changePage = changePage;
            _changeToHouseholdPage = changeToHouseholdPage;
            _household = household;
            IsOwner = household.Relationship == Core.Enums.HouseholdRelationship.Owner;
            GoToHouseholdPageCommand = ReactiveCommand.Create(() => _changeToHouseholdPage(this));
            GoBackCommand = ReactiveCommand.Create(() => _changePage(SocialPage.ManageHouseholds));
            EditHouseholdCommand = ReactiveCommand.Create(() => householdEditingPage(_household));
        }
    }
}
