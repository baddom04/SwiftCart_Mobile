using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Model.Settings;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class ShoppingItemViewModel : ViewModelBase
    {
        //private ShoppingItem _item;
        //public ShoppingItem Item
        //{
        //    get { return _item; }
        //    set { this.RaiseAndSetIfChanged(ref _item, value); }
        //}

        //private bool _isExpanded;
        //public bool IsExpanded
        //{
        //    get { return _isExpanded; }
        //    set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        //}

        //public bool IsOwner => true;
        //public string QuantityDisplay => Item.Quantity + " " + Item.Unit.ToString();
        //public ReactiveCommand<Unit, bool> ToggleExpandedCommand { get; }
        //public ReactiveCommand<Unit, Unit> EditCommand { get; }
        //public Action Editing { get; }
        //public ShoppingItemViewModel(ShoppingItem item, Action editing)
        //{
        //    _item = item;
        //    ToggleExpandedCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
        //    Editing = editing;
        //    EditCommand = ReactiveCommand.Create(() => editing());
        //}

        public string Name { get; }
        public int? Quantity { get; }
        public UnitType? Unit { get; }
        public string? Description { get; }
        public string OwnerName { get; }

        private int _commentCount;
        public int CommentCount
        {
            get { return _commentCount; }
            private set { this.RaiseAndSetIfChanged(ref _commentCount, value); }
        }
        public ReactiveCommand<Unit, bool> ExpandCommand { get; }
        public ReactiveCommand<Unit, Unit> ExpandCommentsCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCommand { get; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            private set { this.RaiseAndSetIfChanged(ref _isExpanded, value); }
        }

        private bool _isCommentsExpanded;
        public bool IsCommentsExpanded
        {
            get { return _isCommentsExpanded; }
            private set { this.RaiseAndSetIfChanged(ref _isCommentsExpanded, value); }
        }

        private bool _isCommentsLoading;
        public bool IsCommentsLoading
        {
            get { return _isCommentsLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isCommentsLoading, value); }
        }

        public ObservableCollection<CommentViewModel> Comments { get; } = [];

        public bool IsOwner { get; }

        private readonly UserAccountModel _account;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly ShoppingItemModel _model;
        private readonly Func<Task> _getGroceriesAsync;
        public ShoppingItemViewModel(ShoppingItemModel model, UserAccountModel account, Grocery grocery, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<int, Grocery?, Action> changeToEditingPage, Action goToThisPage, Func<Task> getGroceriesAsync)
        {
            Name = grocery.Name;
            Quantity = grocery.Quantity;
            Unit = grocery.Unit;
            Description = grocery.Description;
            OwnerName = grocery.User.Name;
            CommentCount = grocery.CommentCount;

            _account = account;
            IsOwner = account.User!.Id == grocery.UserId;
            _showLoading = showLoading;
            _showNotification = showNotification;
            _model = model;
            _getGroceriesAsync = getGroceriesAsync;

            ExpandCommand = ReactiveCommand.Create(() => IsExpanded = !IsExpanded);
            ExpandCommentsCommand = ReactiveCommand.CreateFromTask(ExpandCommentsAsync);
            EditCommand = ReactiveCommand.Create(() => changeToEditingPage(grocery.HouseholdId, grocery, goToThisPage));
        }

        private async Task ExpandCommentsAsync()
        {
            IsCommentsExpanded = !IsCommentsExpanded;

            if (IsCommentsExpanded)
                await LoadCommentsAsync();
            else
                Comments.Clear();
        }
        private async Task LoadCommentsAsync()
        {
            IsCommentsLoading = true;
            try
            {
                Comments.Clear();
                Comments.AddRange((await _model.GetCommentsAsync())
                    .Select(c => 
                    new CommentViewModel(_account, _model, c, LoadCommentsAsync, 
                    (b) => IsCommentsLoading = b, _showNotification)));
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("LoadCommentsError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsCommentsLoading = false;
            }
        }

        public async Task DeleteGroceryAsync()
        {
            _showLoading(true);
            try
            {
                await _model.DeleteGroceryAsync();
                await _getGroceriesAsync();
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("DeleteGroceryError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }
        public async Task AddCommentAsync(string content)
        {
            IsCommentsLoading = true;
            try
            {
                await _model.CreateCommentAsync(content);
                await LoadCommentsAsync();
                CommentCount++;
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("AddCommentError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsCommentsLoading = false;
            }
        }
    }
}
