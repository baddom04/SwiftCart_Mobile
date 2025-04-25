using ReactiveUI;
using ShoppingList.Shared;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class StoreCreationViewModel : StorePropertyEditor
    {
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public override bool IsUpdating => _model.Store is not null;
        public string StoreNameInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> CreateStoreCommand { get; }

        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public StoreCreationViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading) : base(model, changePage)
        {
            _changePage = changePage;
            _showLoading = showLoading;

            CreateStoreCommand = ReactiveCommand.CreateFromTask(CreateStoreAsync);

            _model.StoreChanged += OnStoreChanged;
        }

        private void OnStoreChanged()
        {
            if (_model.Store is null) return;
            StoreNameInput = _model.Store.Name;
        }

        private async Task CreateStoreAsync()
        {
            if (!Validate()) return;

            _showLoading(true);
            try
            {
                if (!IsUpdating)
                {
                    await _model.CreateStoreAsync(StoreNameInput.Trim());
                    _changePage(LoggedInPages.Location);
                }
                else
                {
                    await _model.UpdateStoreAsync(StoreNameInput.Trim(), _model.Store!.Published);
                    _changePage(LoggedInPages.Editor);
                }

                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("StoreCreationError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }
        private bool Validate()
        {
            string trimmedStoreName = StoreNameInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedStoreName))
            {
                ErrorMessage = StringProvider.GetString("StoreNameEmpty");
                return false;
            }
            else if (trimmedStoreName.Length > 50)
            {
                ErrorMessage = StringProvider.GetString("StoreNameTooLong");
                return false;
            }

            ErrorMessage = null;
            return true;
        }
    }
}
