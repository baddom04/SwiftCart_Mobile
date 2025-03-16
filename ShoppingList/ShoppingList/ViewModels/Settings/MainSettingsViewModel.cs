using ShoppingList.Model.Settings;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels.Settings
{
    internal class MainSettingsViewModel : MainViewModelBase<SettingsPage>
    {
        public MainSettingsViewModel(UserAccountModel userAccount, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<MainPage> changePage)
        {
            _pages = new Dictionary<SettingsPage, ViewModelBase>()
            {
                { SettingsPage.Main, new SettingsViewModel(userAccount, changePage, showLoading, showNotification, ChangePage) },
                { SettingsPage.UpdatePassword, new UpdatePasswordViewModel(userAccount, showLoading, ChangePage) }
            };

            _currentPage = _pages[SettingsPage.Main];
        }

        public override void ChangeToDefaultPage()
        {
            CurrentPage = _pages[SettingsPage.Main];
        }
    }

    internal enum SettingsPage
    {
        Main,
        UpdatePassword,
    }
}
