using ReactiveUI;
using ShoppingList.Shared;

namespace ShoppingListEditor.ViewModels.Editor
{
    internal class StorePropertyEditor : ViewModelBase
    {
        public virtual bool IsUpdating => false;
        public void RaiseIsUpdatingPropertyChanged()
        {
            this.RaisePropertyChanged(nameof(IsUpdating));
        }
    }
}
