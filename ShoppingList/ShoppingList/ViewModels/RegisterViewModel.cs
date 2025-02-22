using ReactiveUI;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, ViewModelBase> LoginCommand { get; set; } = null!;
    }
}
