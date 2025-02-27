using ReactiveUI;
using ShoppingList.Model.Models;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Settings;

internal class SettingsViewModel : ViewModelBase
{
    public ObservableCollection<SettingGroupViewModel> SettingGroups { get; }

    private readonly UserAccountModel _userAccount;
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteUserCommand { get; }

    private readonly Action<bool> _showLoading;
    private readonly Action<Page> _changePage;
    private readonly Action<NotificationType, string> _showNotification;

    public SettingsViewModel(UserAccountModel userAccount, Action<Page> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
    {
        _userAccount = userAccount;
        _changePage = changePage;
        _showLoading = showLoading;
        _showNotification = showNotification;

        LogoutCommand = ReactiveCommand.CreateFromTask(Logout);
        DeleteUserCommand = ReactiveCommand.CreateFromTask(DeleteUser);
        SettingGroups = [new SettingGroupViewModel("AccountSettings", [
            new SingleSettingViewModel("Logout", ReactiveCommand.CreateFromTask(Logout))
        ])];
    }

    private async Task DeleteUser()
    {
        _showLoading(true);

        try
        {
            await _userAccount.DeleteUserAsync();

            _changePage(Page.Login);
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

    private async Task Logout()
    {
        _showLoading(true);

        try
        {
            await _userAccount.LogoutAsync();

            _changePage(Page.Login);
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
}
