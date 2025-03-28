using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.ShoppingList;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Utils;
using ShoppingList.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.ShoppingList
{
    internal class ShoppingListViewModel : HouseholdListItemViewModel
    {
        public ObservableCollection<ShoppingItemViewModel> Items { get; } = [];
        public override ReactiveCommand<Unit, Unit> HouseholdOperationCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateGroceryPageCommand { get; }

        private readonly Action<ViewModelBase> _changeToPage;
        private readonly Action<GroceryPage> _changePage;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly ShoppingListModel _model;
        private readonly Action<int, Grocery?, Action> _changeToEditingPage;
        private readonly UserAccountModel _account;

        public ShoppingListViewModel(UserAccountModel account, ShoppingListModel model, Household household, Action<ViewModelBase> changeToPage, Action<GroceryPage> changePage, Action<NotificationType, string> showNotification, Action<int, Grocery?, Action> changeToEditingPage)
        {
            _model = model;
            _name = household.Name;
            _identifier = household.Identifier;
            _changeToPage = changeToPage;
            _changePage = changePage;
            _showNotification = showNotification;
            _changeToEditingPage = changeToEditingPage;
            _account = account;
            HouseholdOperationCommand = ReactiveCommand.Create(GoToThisPage);
            GoBackCommand = ReactiveCommand.Create(() => _changePage(GroceryPage.Main));
            CreateGroceryPageCommand = ReactiveCommand.Create(() => _changeToEditingPage(household.Id, null, GoToThisPage));
        }
        private void GoToThisPage()
        {
            _changeToPage(this);
        }
        private void ShowLoading(bool isLoading) 
        {
            IsLoading = isLoading;
        }

        public async Task GetGroceriesAsync()
        {
            IsLoading = true;

            try
            {
                Items.Clear();
                Items.AddRange((await _model.GetGroceriesAsync()).Select(g => new ShoppingItemViewModel(new ShoppingItemModel(g, g.HouseholdId), _account, g, ShowLoading, _showNotification, _changeToEditingPage, GoToThisPage, GetGroceriesAsync)));
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("ShoppingListQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
