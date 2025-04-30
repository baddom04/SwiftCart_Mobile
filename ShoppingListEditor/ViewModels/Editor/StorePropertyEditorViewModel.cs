using ReactiveUI;
using ShoppingList.Shared;
using ShoppingListEditor.Model;
using System;
using System.Reactive;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class StorePropertyEditorViewModel : ViewModelBase
    {
        public virtual bool IsUpdating => false;
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        protected readonly EditorModel _model;
        public StorePropertyEditorViewModel(EditorModel model, Action<LoggedInPages> changePage)
        {
            _model = model;
            GoBackCommand = ReactiveCommand.Create(() => changePage(LoggedInPages.Editor),
                this.WhenAnyValue(x => x.IsUpdating, isUpdating => isUpdating == true));
        }
        public void RaiseIsUpdatingPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(IsUpdating));
        }
    }
}
