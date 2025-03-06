using ShoppingList.Core;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdViewModel : HouseholdListItemViewModel
    {
        public HouseholdViewModel(Household household)
        {
            _name = household.Name;
			_identifier = household.Identifier;
        }
    }
}
