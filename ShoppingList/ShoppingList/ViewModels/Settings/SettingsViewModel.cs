﻿using ShoppingList.Model.Models;
using ShoppingList.Utils;
using System;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Settings;

internal class SettingsViewModel(UserAccountModel userAccount, Action<Page> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification) : ViewModelBase
{

    private readonly UserAccountModel _userAccount = userAccount;
    private readonly Action<bool> _showLoading = showLoading;
    private readonly Action<Page> _changePage = changePage;
    private readonly Action<NotificationType, string> _showNotification = showNotification;

    public async Task DeleteUser()
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

    public async Task Logout()
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
