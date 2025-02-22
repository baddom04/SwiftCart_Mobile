using ReactiveUI;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, ViewModelBase> RegisterCommand { get; set; } = null!;
    }
}
