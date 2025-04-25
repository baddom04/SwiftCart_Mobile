using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class CommentViewModel : ViewModelBase
    {
        public string UserName { get; }
        public string Content { get; }
        public bool IsMe { get; }
        public bool NotDeleted { get; }

        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
        private readonly ShoppingItemModel _itemModel;
        private readonly Func<Task> _getCommentsAsync;
        private readonly Action<bool> _showLoading;
        private readonly int _commentId;
        private readonly Action<NotificationType, string> _showNotification;
        public CommentViewModel(UserAccountModel account, ShoppingItemModel itemModel, Comment comment, Func<Task> getCommentsAsync, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            UserName = comment.User.Name;
            Content = comment.Content;
            IsMe = account.User!.Id == comment.UserId;
            NotDeleted = Content != "[Comment deleted]";

            _itemModel = itemModel;
            _getCommentsAsync = getCommentsAsync;
            _showLoading = showLoading;
            _commentId = comment.Id;
            _showNotification = showNotification;

            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteCommentAsync);
        }
        private async Task DeleteCommentAsync()
        {
            _showLoading(true);

            try
            {
                await _itemModel.DeleteCommentAsync(_commentId);
                await _getCommentsAsync();
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("DeleteCommentError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }
    }
}
