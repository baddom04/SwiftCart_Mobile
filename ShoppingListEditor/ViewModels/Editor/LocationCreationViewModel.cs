using ReactiveUI;
using ShoppingList.Shared;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class LocationCreationViewModel : StorePropertyEditor
    {
        public override bool IsUpdating => GetIsUpdating();
        private bool GetIsUpdating()
        {
            if (_model.Store is null) return false;
            else return _model.Store.Location is not null;
        }
        public string CountryInput { get; set; } = string.Empty;
        public string CityInput { get; set; } = string.Empty;
        public string ZipCodeInput { get; set; } = string.Empty;
        public string StreetInput { get; set; } = string.Empty;
        public string DetailsInput { get; set; } = string.Empty;

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> CreateLocationCommand { get; }

        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public LocationCreationViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading) : base(model, changePage)
        {
            _changePage = changePage;
            _showLoading = showLoading;

            CreateLocationCommand = ReactiveCommand.CreateFromTask(CreateStoreAsync);

            _model.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged()
        {
            if (_model.Store is null || _model.Store.Location is null) return;

            CountryInput = _model.Store.Location.Country;
            CityInput = _model.Store.Location.City;
            ZipCodeInput = _model.Store.Location.ZipCode;
            StreetInput = _model.Store.Location.Street;
            DetailsInput = _model.Store.Location.Detail;
        }

        private async Task CreateStoreAsync()
        {
            if (!Validate()) return;

            _showLoading(true);
            try
            {
                if (!IsUpdating)
                {
                    await _model.CreateLocationAsync(CountryInput.Trim(), ZipCodeInput.Trim(), CityInput.Trim(), StreetInput.Trim(), string.IsNullOrWhiteSpace(DetailsInput) ? null : DetailsInput.Trim());
                    _changePage(LoggedInPages.Map);
                }
                else
                {
                    await _model.UpdateLocationAsync(CountryInput.Trim(), ZipCodeInput.Trim(), CityInput.Trim(), StreetInput.Trim(), string.IsNullOrWhiteSpace(DetailsInput) ? null : DetailsInput.Trim());
                    _changePage(LoggedInPages.Editor);
                }

                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("LocationCreationError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }
        private bool Validate()
        {
            if (!ValidateNotEmtpy(CountryInput, "Country")
                || !ValidateNotEmtpy(CityInput, "City")
                || !ValidateNotEmtpy(ZipCodeInput, "ZipCode")
                || !ValidateNotEmtpy(StreetInput, "Street"))
            {
                return false;
            }

            string trimmedZipCode = ZipCodeInput.Trim();

            if (!int.TryParse(trimmedZipCode, out int _))
            {
                ErrorMessage = StringProvider.GetString("ZipCodeNotNumber");
                return false;
            }
            if (trimmedZipCode.Length != 4)
            {
                ErrorMessage = StringProvider.GetString("ZipCodeLengthError");
                return false;
            }

            ErrorMessage = null;
            return true;
        }
        private bool ValidateNotEmtpy(string value, string valueNameKey)
        {
            string trimmedValue = value.Trim();

            if (string.IsNullOrWhiteSpace(trimmedValue))
            {
                ErrorMessage = $"{StringProvider.GetString("SmthMissing")} {StringProvider.GetString(valueNameKey)}.";
                return false;
            }

            return true;
        }
    }
}
