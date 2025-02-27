﻿using ShoppingList.Utils;
using ReactiveUI;
using ShoppingList.Model.Models;
using System.Collections.Generic;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<Page, ViewModelBase> _pages;

		private ViewModelBase _currentPage;
		public ViewModelBase CurrentPage
		{
			get { return _currentPage; }
			private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
		}

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        private bool _showNotification;
        public bool ShowNotification
        {
            get { return _showNotification; }
            private set { this.RaiseAndSetIfChanged(ref _showNotification, value); }
        }

        private NotificationType? _notificationType;
        public NotificationType? NotificationType
        {
            get { return _notificationType; }
            private set { this.RaiseAndSetIfChanged(ref _notificationType, value); }
        }

        private string? _notificationMessage;
        public string? NotificationMessage
        {
            get { return _notificationMessage; }
            set { this.RaiseAndSetIfChanged(ref _notificationMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> HideNotificationCommand { get; }

        public MainViewModel(UserAccountModel userAccount)
        {
            _pages = new Dictionary<Page, ViewModelBase>()
            {
                { Page.Login, new LoginViewModel(userAccount, ChangePage, ShowLoading) },
                { Page.Register, new RegisterViewModel(userAccount, ChangePage, ShowLoading) },
                { Page.Main, new LoggedInViewModel(userAccount, ChangePage, ShowLoading, ShowNotificationDialog) },
            };

            _currentPage = _pages[Page.Login];

            HideNotificationCommand = ReactiveCommand.Create(HideNotificationDialog);
        }

        private void HideNotificationDialog()
        {
            ShowNotification = false;
            NotificationMessage = null;
            NotificationType = null;
        }

        private void ChangePage(Page page)
        {
            CurrentPage = _pages[page];
        }
        private void ShowLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
        private void ShowNotificationDialog(NotificationType type, string message)
        {
            ShowNotification = true;
            NotificationMessage = message;
            NotificationType = type;
        }
    }

    public enum Page
    {
        Login,
        Register,
        Main
    }
}
