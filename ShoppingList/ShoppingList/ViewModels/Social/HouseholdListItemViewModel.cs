using ReactiveUI;
using ShoppingList.Core;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class HouseholdListItemViewModel : ViewModelBase
    {
        public Household Household { get; private set; }
        public ReactiveCommand<Unit, Unit> ApplyCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public HouseholdListItemViewModel(Household household)
        {
            Household = household;
            ApplyCommand = ReactiveCommand.CreateFromTask(Apply);
        }

        private async Task Apply()
        {
            IsLoading = true;
            await Task.Delay(5000);
            IsLoading = false;
        }
    }
}
