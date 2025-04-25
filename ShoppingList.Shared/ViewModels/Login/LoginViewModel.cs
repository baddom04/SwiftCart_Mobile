using ReactiveUI;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using System.Reactive;

namespace ShoppingList.Shared.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> RegisterPageCommand { get; }
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Task> LoginCommand { get; }

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
            LoginCommand = ReactiveCommand.Create(LoginAsync);
        }

        private async Task LoginAsync()
        {
            if (!Validate()) return;

            _showLoading(true);

            try
            {
                await _model.LoginAsync(EmailInput.Trim(), PasswordInput.Trim());

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

        public async Task TryLoginAsync()
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
