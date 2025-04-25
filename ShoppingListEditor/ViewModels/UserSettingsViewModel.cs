using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using ShoppingListEditor.Model;
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

        private bool _isStoreDeletable;
        public bool IsStoreDeletable
        {
            get { return _isStoreDeletable; }
            private set { this.RaiseAndSetIfChanged(ref _isStoreDeletable, value); }
        }

        private User? _user;
        public User? User
        {
            get { return _user; }
            private set { this.RaiseAndSetIfChanged(ref _user, value); }
        }

        public ReactiveCommand<Unit, bool> TogglePaneCommand { get; }

        private readonly UserAccountModel _account;
        private readonly EditorModel _model;
        private readonly Action<bool> _showLoading;
        private readonly Action<MainPage> _changeMainPage;
        private readonly Action<LoggedInPages> _changePage;
        private readonly Action<NotificationType, string> _showNotification;
        public UserSettingsViewModel(UserAccountModel account, EditorModel model, Action<LoggedInPages> changePage, Action<MainPage> changeMainPage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            TogglePaneCommand = ReactiveCommand.Create(() => IsPaneOpen = !IsPaneOpen);
            _account = account;
            _model = model;
            _changePage = changePage;
            _changeMainPage = changeMainPage;
            _showLoading = showLoading;
            _showNotification = showNotification;

            _model.StoreChanged += () => IsStoreDeletable = _model.Store is not null;
        }
        public async Task DeleteStoreAsync()
        {
            _showLoading(true);

            try
            {
                await _model.DeleteStoreAsync();
                _changePage(LoggedInPages.Store);
            }
            catch (Exception ex)
            {
                string message = $"{StringProvider.GetString("DeleteStoreError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                _showLoading(false);
            }
        }

        public async Task DeleteUserAsync()
        {
            _showLoading(true);

            try
            {
                await _account.DeleteUserAsync();

                _changeMainPage(MainPage.Login);
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

        public async Task LogoutAsync()
        {
            _showLoading(true);

            try
            {
                await _account.LogoutAsync();

                _changeMainPage(MainPage.Login);
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
        public async Task LoadUserAsync()
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
