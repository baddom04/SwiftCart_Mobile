using Microsoft.VisualBasic;
using ReactiveUI;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class StoreCreationViewModel : ViewModelBase
    {
        public bool IsUpdating => _model.Store is not null;
        public string StoreNameInput { get; set; } = string.Empty;

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> CreateStoreCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private readonly EditorModel _model;
        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public StoreCreationViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading)
        {
            _model = model;
            _changePage = changePage;
            _showLoading = showLoading;

            CreateStoreCommand = ReactiveCommand.CreateFromTask(CreateStoreAsync);

            GoBackCommand = ReactiveCommand.Create(() => _changePage(LoggedInPages.Editor),
                this.WhenAnyValue(x => x.IsUpdating, isUpdating => isUpdating == true));

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
                    await _model.CreateStoreAsync(StoreNameInput);
                else
                    await _model.UpdateStoreAsync(StoreNameInput);

                    _changePage(LoggedInPages.Location);
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
