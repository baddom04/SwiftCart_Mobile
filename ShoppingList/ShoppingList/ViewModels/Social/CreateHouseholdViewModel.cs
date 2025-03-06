using ShoppingList.Model.Social;
using System;

namespace ShoppingList.ViewModels.Social
{
    internal class CreateHouseholdViewModel : ViewModelBase
    {
        private readonly CreateHouseholdModel _model;
        public CreateHouseholdViewModel(CreateHouseholdModel model, Action<SocialPage> changePage)
        {
            _model = model;
        }
    }
}
