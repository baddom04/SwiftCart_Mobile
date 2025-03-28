using ReactiveUI;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class UserListItemViewModel : ViewModelBase
    {
        private UserStatus? _status;
        public UserStatus? Status
        {
            get { return _status; }
            private set { this.RaiseAndSetIfChanged(ref _status, value); }
        }


        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public ReactiveCommand<Unit, Unit> KickUserCommand { get; }
        public ReactiveCommand<Unit, Unit> AcceptUserCommand { get; }
        public ReactiveCommand<Unit, Unit> RefuseUserCommand { get; }

        public string Name { get; }
        public string Email { get; }
        public bool IsOwner { get; }
        public bool IsMe => _account.User!.Email == Email;

        private readonly UserListItemModel _model;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly int _householdId;
        private readonly UserAccountModel _account;
        public UserListItemViewModel(bool isOwner, UserAccountModel account, int householdId, UserListItemModel model, Action<NotificationType, string> showNotification)
        {
            IsOwner = isOwner;
            _account = account;
            _model = model;
            _householdId = householdId;
            _showNotification = showNotification;
            Name = _model.User.Name;
            Email = _model.User.Email;
            KickUserCommand = ReactiveCommand.CreateFromTask(KickUserAsync);
            AcceptUserCommand = ReactiveCommand.CreateFromTask(AcceptUserAsync);
            RefuseUserCommand = ReactiveCommand.CreateFromTask(RefuseUserAsync);
        }

        private async Task RefuseUserAsync()
        {
            await DoOperation(_model.RefuseUserAsync, "RefuseUserError", UserStatus.Refused);
        }

        private async Task KickUserAsync()
        {
            await DoOperation(_model.RemoveMemberAsync, "KickUserError", UserStatus.Kicked);
        }

        private async Task AcceptUserAsync()
        {
            await DoOperation(_model.AcceptUserAsync, "AcceptUserError", UserStatus.Accepted);
        }
        private async Task DoOperation(Func<int, Task> operation, string errorKey, UserStatus resultStatus)
        {
            IsLoading = true;

            try
            {
                await operation(_householdId);
                Status = resultStatus;
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString(errorKey)}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
