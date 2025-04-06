using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class CreateHouseholdViewModel : ViewModelBase
    {
        private string _nameInput = string.Empty;
        public string NameInput
        {
            get { return _nameInput; }
            set { this.RaiseAndSetIfChanged(ref _nameInput, value); }
        }

        private string _identifierInput = string.Empty;
        public string IdentifierInput
        {
            get { return _identifierInput; }
            set { this.RaiseAndSetIfChanged(ref _identifierInput, value); }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateHouseholdCommand { get; }

        private readonly CreateHouseholdModel _model;
        private readonly Action<SocialPage> _changePage;
        private readonly Action<bool> _showLoading;
        public int HouseholdId { get; private set; }
        public CreateHouseholdViewModel(CreateHouseholdModel model, Action<SocialPage> changePage, Action<bool> showLoading)
        {
            _model = model;
            _changePage = changePage;
            _showLoading = showLoading;
            GoBackCommand = ReactiveCommand.Create(() => { ErrorMessage = null; _changePage(SocialPage.ManageHouseholds); });
            CreateHouseholdCommand = ReactiveCommand.CreateFromTask(CreateHousehold);
        }

        private async Task CreateHousehold()
        {
            if (!Validate()) return;

            _showLoading(true);

            try
            {
                if (HouseholdId == default)
                {
                    await _model.CreateHouseholdAsync(NameInput.Trim(), IdentifierInput.Trim());
                }
                else
                {
                    await _model.UpdateHouseholdAsync(HouseholdId, NameInput, IdentifierInput);
                }

                _changePage(SocialPage.ManageHouseholds);
                ErrorMessage = null;
                NameInput = string.Empty;
                IdentifierInput = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("CreateHouseholdError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(NameInput))
            {
                ErrorMessage = StringProvider.GetString("HouseholdNameMissingError");
                return false;
            }

            string trimmedName = NameInput.Trim();

            if (trimmedName.Length > 20)
            {
                ErrorMessage = StringProvider.GetString("HouseholdNameTooLongError");
                return false;
            }

            if (string.IsNullOrWhiteSpace(IdentifierInput))
            {
                ErrorMessage = StringProvider.GetString("HouseholdIdentifierMissingError");
                return false;
            }

            string trimmedIdentifier = IdentifierInput.Trim();

            if (trimmedIdentifier.Length > 20)
            {
                ErrorMessage = StringProvider.GetString("HouseholdIdentifierTooLongError");
                return false;
            }

            return true;
        }

        public void EditState(Household? household)
        {
            ErrorMessage = null;
            NameInput = household is null ? string.Empty : household.Name;
            IdentifierInput = household is null ? string.Empty : household.Identifier;
            HouseholdId = household?.Id ?? default;
        }
    }
}
