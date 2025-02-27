using ReactiveUI;
using ShoppingList.Model.Models;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Login
{
    internal class LoginViewModel : ViewModelBase
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

        private readonly Action<bool> _showLoading;
        private readonly Action<MainPage> _changePage;
        private readonly UserAccountModel _model;
        private bool _firstTimeLoginAttempt = true;
        public LoginViewModel(UserAccountModel model, Action<MainPage> changePage, Action<bool> showLoading)
        {
            _changePage = changePage;
            _showLoading = showLoading;
            _model = model;

            RegisterPageCommand = ReactiveCommand.Create(() => _changePage(MainPage.Register));
            LoginCommand = ReactiveCommand.Create(Login);
        }

        private async Task Login()
        {
            //if (!Validate()) return;

            _showLoading(true);

            try
            {
                await _model.LoginAsync(EmailInput, PasswordInput);

                _changePage(MainPage.Main);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("LoginError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
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

        public async Task TryLogin()
        {
            if (!_firstTimeLoginAttempt) return;

            _showLoading(true);
            try
            {
                await _model.GetUserAsync();

                _changePage(MainPage.Main);
            }
            catch
            {
                _showLoading(false);
            }
            finally
            {
                _showLoading(false);
            }

            _firstTimeLoginAttempt = false;
        }
    }
}
