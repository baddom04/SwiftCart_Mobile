using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> RegisterPageCommand { get; }
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Task> LoginCommand { get; }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

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
            if (!Validate()) return;

            ShowLoading(true);

            try
            {
                IUserService userService = AppServiceProvider.Services.GetRequiredService<IUserService>();
                await userService.LoginAsync(EmailInput, PasswordInput);

                ChangePage(Page.Main);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("LoginError")}{ex.Message}";
            }
            finally
            {
                ShowLoading(false);
            }
        }
        private bool Validate()
        {
            if (ValidateEmail() && ValidatePassword())
            {
                ErrorMessage = null;
                return true;
            }
            return false;
        }
        private bool ValidateEmail()
        {
            var trimmedEmail = EmailInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedEmail))
            {
                ErrorMessage = StringProvider.GetString("EmailMissingError");
                return false;
            }

            if (trimmedEmail.EndsWith('.'))
            {
                ErrorMessage = StringProvider.GetString("EmailFormatError");
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(EmailInput);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                ErrorMessage = StringProvider.GetString("EmailFormatError");
                return false;
            }
        }
        private bool ValidatePassword()
        {
            string trimmedPassword = PasswordInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedPassword))
            {
                ErrorMessage = StringProvider.GetString("PasswordMissingError");
                return false;
            }

            if (trimmedPassword.Length < 8)
            {
                ErrorMessage = StringProvider.GetString("PasswordFormatError");
                return false;
            }

            return true;
        }
    }
}
