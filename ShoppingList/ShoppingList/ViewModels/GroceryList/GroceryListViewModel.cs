using ReactiveUI;
using ShoppingList.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ShoppingList.Model.Temp;
using ShoppingList.Model;
using ShoppingList.ViewModels.Shared;

namespace ShoppingList.ViewModels.GroceryList
{
    internal class GroceryListViewModel : DefaultPageOnChangeViewModel
    {
        #region Properties
        public GroceryListModel Model { get; }

        private List<ShoppingItemViewModel> _shoppingList;
        public List<ShoppingItemViewModel> ShoppingList
        {
            get { return _shoppingList; }
            set { this.RaiseAndSetIfChanged(ref _shoppingList, value); }
        }

        private bool _inputMode;
        public bool InputMode
        {
            get { return _inputMode; }
            set { this.RaiseAndSetIfChanged(ref _inputMode, value); }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        private ShoppingItem? _currentlyEditedItem;
        public ShoppingItem? CurrentlyEditedItem
        {
            get { return _currentlyEditedItem; }
            set { this.RaiseAndSetIfChanged(ref _currentlyEditedItem, value); }
        }

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> InputModeOnCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> InputModeOffCommand { get; }
        public ReactiveCommand<ShoppingItemViewModel, Unit> BoughtItemCommand { get; }
        #endregion

        #region Methods
        public GroceryListViewModel()
        {
            Model = new();
            Model.ErrorTypeChanged += (_, _) => OnErrorTypeChanged();
            Model.ShoppingList.CollectionChanged += ShoppingList_CollectionChanged;
            Model.EditedItemChanged += (_, _) => CurrentlyEditedItem = Model.EditedItem?.Item;

            _shoppingList = [.. Model.ShoppingList.Select(item => new ShoppingItemViewModel(item, () => OnInputModeOn((item.Clone() as ShoppingItem)!, Model.ShoppingList.IndexOf(item))))];

            InputModeOnCommand = ReactiveCommand.Create(() => OnInputModeOn(ShoppingItem.Empty, -1));
            SaveCommand = ReactiveCommand.Create(() => { Model.SaveEdit(); if (Model.IsValidItem) OnInputModeOff(); });
            InputModeOffCommand = ReactiveCommand.Create(OnInputModeOff);
            BoughtItemCommand = ReactiveCommand.Create<ShoppingItemViewModel>(Boughtitem);
        }

        private void ShoppingList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ShoppingList = [.. Model.ShoppingList.Select(item => new ShoppingItemViewModel(item, () => OnInputModeOn((item.Clone() as ShoppingItem)!, Model.ShoppingList.IndexOf(item))))];
        }
        private void OnErrorTypeChanged()
        {
            ErrorMessage = Model.ErrorType switch
            {
                ItemFormErrorType.EmptyName => "The item's name cannot be empty!",
                _ => null,
            };
        }
        private void OnInputModeOn(ShoppingItem item, int index)
        {
            if (index < 0)
            {
                //item.UserId = App.CurrentUser!.Id;
            }

            Model.StartEdit(item, index);
            InputMode = true;
        }
        private void OnInputModeOff()
        {
            InputMode = false;
        }
        private void Boughtitem(ShoppingItemViewModel item)
        {
            Model.DeleteItem(item.Item);
        }

        public override void ChangeToDefaultPage()
        {
            // TODO: implement
        }
        #endregion
    }
}
