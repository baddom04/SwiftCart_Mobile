using ReactiveUI;
using ShoppingList.Model.Settings;
using ShoppingList.Model.Social;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class UserListItemViewModel : ViewModelBase
    {

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
        public bool IsMe => _account.User!.Email == Email;

        private readonly UserListItemModel _model;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly int _householdId;
        private readonly UserAccountModel _account;
        public UserListItemViewModel(UserAccountModel account, int householdId, UserListItemModel model, Action<NotificationType, string> showNotification)
        {
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
            await DoOperation(_model.RefuseUserAsync, "RefuseUserError");
        }

        private async Task KickUserAsync()
        {
            await DoOperation(_model.RemoveMemberAsync, "KickUserError");
        }

        private async Task AcceptUserAsync()
        {
            await DoOperation(_model.AcceptUserAsync, "AcceptUserError");
        }
        private async Task<bool> DoOperation(Func<int, Task> operation, string errorKey)
        {
            IsLoading = true;

            try
            {
                await operation(_householdId);
                return true;
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString(errorKey)}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
