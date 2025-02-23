using ReactiveUI;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, ViewModelBase> RegisterCommand { get; set; } = null!;

        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public LoginViewModel()
        {
            
        }
    }
}
