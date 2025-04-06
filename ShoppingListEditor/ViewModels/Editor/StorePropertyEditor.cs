using ReactiveUI;
using ShoppingList.Shared;
using ShoppingListEditor.Model;
using System;
using System.Reactive;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class StorePropertyEditor : ViewModelBase
    {
        public virtual bool IsUpdating => false;
        public void RaiseIsUpdatingPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(IsUpdating));
        }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        protected readonly EditorModel _model;
        public StorePropertyEditor(EditorModel model, Action<LoggedInPages> changePage)
        {
            _model = model;
            GoBackCommand = ReactiveCommand.Create(() => changePage(LoggedInPages.Editor),
                this.WhenAnyValue(x => x.IsUpdating, isUpdating => isUpdating == true));
        }
    }
}
