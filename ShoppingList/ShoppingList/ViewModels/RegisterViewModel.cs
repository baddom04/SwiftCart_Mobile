using ReactiveUI;
using System;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> LoginPageCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
        public string UsernameInput { get; set; } = string.Empty;
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;

        public RegisterViewModel(Action<Page> changePage)
        {
            LoginPageCommand = ReactiveCommand.Create(() => changePage(Page.Login));
        }

    }
}
