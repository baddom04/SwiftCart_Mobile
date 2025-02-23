using ReactiveUI;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, ViewModelBase> LoginCommand { get; set; } = null!;

        public string UsernameInput { get; set; } = string.Empty;
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;

        public RegisterViewModel()
        {
            
        }

    }
}
