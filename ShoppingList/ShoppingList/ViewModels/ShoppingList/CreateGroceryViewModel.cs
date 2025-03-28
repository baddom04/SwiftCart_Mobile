using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Shared;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class CreateGroceryViewModel : ViewModelBase
    {
        public string NameInput { get; set; } = string.Empty;
        public string? QuantityInput { get; set; }
        public UnitType UnitInput { get; set; } = UnitType.none;
        public string? DescriptionInput { get; set; }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public bool Updating { get; set; }

        private readonly Action<bool> _showLoading;
        private readonly CreateGroceryModel _model;
        private int _householdId;
        private int? _groceryId;

        public ReactiveCommand<Unit, Unit> CreateGroceryCommand { get; }

        private Action _goBackAction = null!;
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; private set; } = null!;

        public List<UnitType> UnitTypes { get; } = [.. (UnitType[])Enum.GetValues(typeof(UnitType))];

        public CreateGroceryViewModel(CreateGroceryModel model, Action<bool> showLoading)
        {
            _model = model;
            _showLoading = showLoading;
            CreateGroceryCommand = ReactiveCommand.CreateFromTask(CreateGroceryAsync);
        }
        private async Task CreateGroceryAsync()
        {
            if (!Validate()) return;

            _showLoading(true);

            try
            {
                int? quantity = string.IsNullOrWhiteSpace(QuantityInput) ? null : Int32.Parse(QuantityInput);

                if(Updating)
                    await _model.UpdateGroceryAsync(_householdId, _groceryId!.Value, NameInput, quantity, UnitInput == UnitType.none ? null : UnitInput, string.IsNullOrWhiteSpace(DescriptionInput) ? null : DescriptionInput);
                else
                    await _model.CreateGroceryAsync(_householdId, NameInput, quantity, UnitInput == UnitType.none ? null : UnitInput, string.IsNullOrWhiteSpace(DescriptionInput) ? null : DescriptionInput);

                ErrorMessage = null;
                _goBackAction();
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("GroceryCreationError")}{ex.Message}";
                ErrorMessage = msg;
            }
            finally 
            {
                _showLoading(false);
            }
        }

        private bool Validate()
        {
            string trimmedName = NameInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedName))
                return Failed("EmptyNameError");
            if(trimmedName.Length > 20)
                return Failed("TooLongNameError");

            if ((string.IsNullOrWhiteSpace(QuantityInput) && UnitInput != UnitType.none) || (!string.IsNullOrWhiteSpace(QuantityInput) && UnitInput == UnitType.none))
                return Failed("BothMustBeNullError");

            if (DescriptionInput is not null && DescriptionInput.Length > 255)
                return Failed("TooLongDescriptionError");

            return true;
        }
        private bool Failed(string messageKey)
        {
            ErrorMessage = StringProvider.GetString(messageKey);
            return false;
        }

        public void Initialize(int householdId, Grocery? grocery, Action goBackAction)
        {
            Updating = grocery is not null;
            _goBackAction = goBackAction;
            GoBackCommand = ReactiveCommand.Create(_goBackAction);
            ErrorMessage = null;
            _householdId = householdId;
            _groceryId = grocery?.Id;
            NameInput = grocery?.Name ?? string.Empty;
            DescriptionInput = grocery?.Description;
            QuantityInput = grocery?.Quantity.ToString();
            UnitInput = grocery is null || grocery.Unit is null ? UnitType.none : grocery.Unit.Value;
        }
    }
}
