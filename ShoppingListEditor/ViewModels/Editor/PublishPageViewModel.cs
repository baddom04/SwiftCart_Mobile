using ReactiveUI;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class PublishPageViewModel : StorePropertyEditor
    {
        public override bool IsUpdating => true;

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }
        public bool IsPublished { get; set; }
        public ReactiveCommand<Unit, Unit> UpdateVisibilityCommand { get; }

        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public PublishPageViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading) : base(model, changePage)
        {
            _changePage = changePage;
            _showLoading = showLoading;

            _model.VisibilityChanged += () => IsPublished = _model.Store?.Published ?? false;

            UpdateVisibilityCommand = ReactiveCommand.CreateFromTask(UpdateVisibilityAsync);
        }

        private async Task UpdateVisibilityAsync()
        {
            _showLoading(true);
            try
            {
                await _model.UpdateStoreAsync(_model.Store!.Name, IsPublished);
                _changePage(LoggedInPages.Editor);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("VisibilityChangeError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
