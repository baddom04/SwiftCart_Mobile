using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Settings;

internal class SettingsViewModel(UserAccountModel userAccount, Action<MainPage> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<SettingsPage> changeSettingsPage) : ViewModelBase
{
    private readonly UserAccountModel _userAccount = userAccount;
    private readonly Action<bool> _showLoading = showLoading;
    private readonly Action<MainPage> _changePage = changePage;
    private readonly Action<NotificationType, string> _showNotification = showNotification;
    public ReactiveCommand<Unit, Unit> UpdatePasswordPageCommand { get; } = ReactiveCommand.Create(() => changeSettingsPage(SettingsPage.UpdatePassword));

    private User? _user;
    public User? User
    {
        get { return _user; }
        private set { this.RaiseAndSetIfChanged(ref _user, value); }
    }


    public async Task DeleteUserAsync()
    {
        _showLoading(true);

        try
        {
            await _userAccount.DeleteUserAsync();

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

    public async Task LogoutAsync()
    {
        _showLoading(true);

        try
        {
            await _userAccount.LogoutAsync();

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
    public async Task LoadUserAsync()
    {
        _showLoading(true);

        try
        {
            User = await _userAccount.GetUserAsync();
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

    public async Task UpdateUsernameAsync(string newUsername)
    {
        _showLoading(true);

        try
        {
            await _userAccount.UpdateUser(newUsername, null, null);

            User = await _userAccount.GetUserAsync(true);
        }
        catch (Exception ex)
        {
            string message = $"{StringProvider.GetString("UpdateUsernameError")}{ex.Message}";
            _showNotification(NotificationType.Error, message);
        }
        finally
        {
            _showLoading(false);
        }
    }
}
