using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Settings
{
    internal class UpdatePasswordViewModel : ViewModelBase
    {
        public string CurrentPasswordInput { get; set; } = string.Empty;
        public string NewPasswordInput { get; set; } = string.Empty;
        public string NewPasswordAgainInput { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> ChangePasswordCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }


        private readonly Action<bool> _showLoading;
        private readonly Action<SettingsPage> _changePage;
        private readonly UserAccountModel _userAccount;
        public UpdatePasswordViewModel(UserAccountModel userAccount, Action<bool> showLoading, Action<SettingsPage> changePage)
        {
            ChangePasswordCommand = ReactiveCommand.CreateFromTask(ChangePassword);
            GoBackCommand = ReactiveCommand.Create(() => changePage(SettingsPage.Main));
            _showLoading = showLoading;
            _changePage = changePage;
            _userAccount = userAccount;
        }
        private async Task ChangePassword()
        {
            if (!Validate()) return;

            _showLoading(true);

            try
            {
                await _userAccount.UpdatePassword(CurrentPasswordInput, NewPasswordInput);

                _changePage(SettingsPage.Main);
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("UpdatePasswordError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(CurrentPasswordInput))
            {
                ErrorMessage = StringProvider.GetString("CurrentPasswordMissingError");
                return false;
            }


            string trimmedPassword = NewPasswordInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedPassword))
            {
                ErrorMessage = StringProvider.GetString("NewPasswordMissingError");
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewPasswordAgainInput))
            {
                ErrorMessage = StringProvider.GetString("NewPasswordAgainMissingError");
                return false;
            }

            if(NewPasswordInput != NewPasswordAgainInput)
            {
                ErrorMessage = StringProvider.GetString("PasswordsDontMatchError");
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
