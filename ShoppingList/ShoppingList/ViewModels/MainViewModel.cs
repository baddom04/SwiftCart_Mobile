using ReactiveUI;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Login;
using ShoppingList.ViewModels.Register;
using System.Collections.Generic;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<MainPage, ViewModelBase> _pages;

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
            private set { this.RaiseAndSetIfChanged(ref _notificationMessage, value); }
        }

        public ReactiveCommand<Unit, Unit> HideNotificationCommand { get; }

        public MainViewModel(UserAccountModel userAccount)
        {
            _pages = new Dictionary<MainPage, ViewModelBase>()
            {
                { MainPage.Login, new LoginViewModel(userAccount, ChangeMainPage, ShowLoading) },
                { MainPage.Register, new RegisterViewModel(userAccount, ChangeMainPage, ShowLoading) },
                { MainPage.Main, new LoggedInViewModel(userAccount, ChangeMainPage, ShowLoading, ShowNotificationDialog) },
            };

            _currentPage = _pages[MainPage.Login];

            HideNotificationCommand = ReactiveCommand.Create(HideNotificationDialog);
        }

        private void HideNotificationDialog()
        {
            ShowNotification = false;
            NotificationMessage = null;
            NotificationType = null;
        }

        protected void ChangeMainPage(MainPage page)
        {
            CurrentPage = _pages[page];
        }
        protected void ShowLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
        protected void ShowNotificationDialog(NotificationType type, string message)
        {
            ShowNotification = true;
            NotificationMessage = message;
            NotificationType = type;
        }
    }

    public enum MainPage
    {
        Login,
        Register,
        Main
    }
}
