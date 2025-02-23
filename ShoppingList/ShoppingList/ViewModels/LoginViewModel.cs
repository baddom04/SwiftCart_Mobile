using ReactiveUI;
using System;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> RegisterPageCommand { get; }
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        private Action<bool> ShowLoading { get; }
        private Action<Page> ChangePage { get; }
        public LoginViewModel(Action<Page> changePage, Action<bool> showLoading)
        {
            ChangePage = changePage;
            ShowLoading = showLoading;

            RegisterPageCommand = ReactiveCommand.Create(() => ChangePage(Page.Register));
        }
    }
}
