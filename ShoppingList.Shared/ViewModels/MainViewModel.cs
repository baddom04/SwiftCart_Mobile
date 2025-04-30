using ReactiveUI;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels.Login;
using ShoppingList.Shared.ViewModels.Register;
using System.Reactive;

namespace ShoppingList.Shared.ViewModels
{
    public abstract class MainViewModel : MainViewModelBase<MainPage>
    {
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
                { MainPage.Login, new LoginViewModel(userAccount, ChangePage, ShowLoading) },
                { MainPage.Register, new RegisterViewModel(userAccount, ChangePage, ShowLoading) },
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

        public override void ChangeToDefaultPage()
        {
            throw new NotImplementedException();
        }
    }

    public enum MainPage
    {
        Login,
        Register,
        Main
    }
}
