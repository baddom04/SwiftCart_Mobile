using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels.Settings
{
    internal class MainSettingsViewModel : DefaultPageOnChangeViewModel
    {
		private ViewModelBase _currentPage;
		public ViewModelBase CurrentPage
		{
			get { return _currentPage; }
			private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
		}

        private readonly Dictionary<SettingsPage, ViewModelBase> _pages;
        public MainSettingsViewModel(UserAccountModel userAccount, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<MainPage> changePage)
        {
            _pages = new Dictionary<SettingsPage, ViewModelBase>()
            {
                { SettingsPage.Main, new SettingsViewModel(userAccount, changePage, showLoading, showNotification, ChangeSettingsPage) },
                { SettingsPage.UpdatePassword, new UpdatePasswordViewModel(userAccount, showLoading, ChangeSettingsPage) }
            };

            _currentPage = _pages[SettingsPage.Main];
        }

        private void ChangeSettingsPage(SettingsPage changeSettingsPage)
        {
            CurrentPage = _pages[changeSettingsPage];
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
