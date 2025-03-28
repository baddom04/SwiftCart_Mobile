using ReactiveUI;
using ShoppingList.Shared;
using System.Reactive;

namespace ShoppingList.ViewModels.Shared
{
    internal abstract class HouseholdListItemViewModel : ViewModelBase
    {
        protected string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            protected set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        protected string _identifier = string.Empty;
        public string Identifier
        {
            get { return _identifier; }
            protected set { this.RaiseAndSetIfChanged(ref _identifier, value); }
        }

        protected bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public abstract ReactiveCommand<Unit, Unit> HouseholdOperationCommand { get; }
    }
}
