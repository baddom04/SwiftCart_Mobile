using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services;
using ShoppingList.Persistor.Services.Interfaces;
using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> RegisterPageCommand { get; }
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Task> LoginCommand { get; }

        private Action<bool> ShowLoading { get; }
        private Action<Page> ChangePage { get; }
        public LoginViewModel(Action<Page> changePage, Action<bool> showLoading)
        {
            ChangePage = changePage;
            ShowLoading = showLoading;

            RegisterPageCommand = ReactiveCommand.Create(() => ChangePage(Page.Register));
            LoginCommand = ReactiveCommand.Create(Login);
        }

        private async Task Login()
        {
            ShowLoading(true);

            IUserService userService = AppServiceProvider.Services.GetRequiredService<IUserService>();
            await userService.LoginAsync(EmailInput, PasswordInput);

            ShowLoading(false);
            ChangePage(Page.Main);
        }
    }
}
