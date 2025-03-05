using ShoppingList.Utils;
using System;
using System.Threading.Tasks;
using ShoppingList.Core;
using ReactiveUI;
using System.Reactive;
using ShoppingList.Model.Settings;

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
        set { this.RaiseAndSetIfChanged(ref _user, value); }
    }


    public async Task DeleteUser()
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

    public async Task Logout()
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
    public async Task LoadUser()
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

    public async Task UpdateUsername(string newUsername)
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
