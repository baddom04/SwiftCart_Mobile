﻿using ReactiveUI;
using ShoppingListEditor.Model;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class PublishPageViewModel : StorePropertyEditorViewModel
    {
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public override bool IsUpdating => true;
        public bool IsPublished { get; set; }
        public ReactiveCommand<Unit, Unit> SetVisibilityCommand { get; }

        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<bool> _showLoading;
        public PublishPageViewModel(EditorModel model, Action<LoggedInPages> changePage, Action<bool> showLoading) : base(model, changePage)
        {
            _changePage = changePage;
            _showLoading = showLoading;

            _model.VisibilityChanged += () => IsPublished = _model.Store?.Published ?? false;

            SetVisibilityCommand = ReactiveCommand.CreateFromTask(SetVisibilityAsync);
        }

        private async Task SetVisibilityAsync()
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
