using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using ShoppingListEditor.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels
{
    internal class UserSettingsViewModel : ViewModelBase
    {
        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            private set { this.RaiseAndSetIfChanged(ref _isPaneOpen, value); }
        }
        public ReactiveCommand<Unit, bool> TogglePaneCommand { get; }

        private User? _user;
        public User? User
        {
            get { return _user; }
            private set { this.RaiseAndSetIfChanged(ref _user, value); }
        }

        private readonly UserAccountModel _account;
        private readonly Action<bool> _showLoading;
        private readonly Action<MainPage> _changePage;
        private readonly Action<NotificationType, string> _showNotification;

        public UserSettingsViewModel(UserAccountModel account, Action<MainPage> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            TogglePaneCommand = ReactiveCommand.Create(() => IsPaneOpen = !IsPaneOpen);
            _account = account;
            _changePage = changePage;
            _showLoading = showLoading;
            _showNotification = showNotification;
        }

        public async Task DeleteUser()
        {
            _showLoading(true);

            try
            {
                await _account.DeleteUserAsync();

                _changePage(MainPage.Login);
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("DeleteUserError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                _showLoading(false);
            }
        }

        public async Task Logout()
        {
            _showLoading(true);

            try
            {
                await _account.LogoutAsync();

                _changePage(MainPage.Login);
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("LogoutError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                _showLoading(false);
            }
        }
        public async Task LoadUser()
        {
            _showLoading(true);

            try
            {
                User = await _account.GetUserAsync();
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("UserQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
