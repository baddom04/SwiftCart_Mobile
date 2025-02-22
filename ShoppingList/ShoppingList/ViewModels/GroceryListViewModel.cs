using ReactiveUI;
using ShoppingList.Model.Models;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace ShoppingList.ViewModels
{
    internal class GroceryListViewModel : ViewModelBase
    {
        #region Properties
        public GroceryListModel Model { get; }

        private List<ShoppingItemDisplay> _shoppingList;
        public List<ShoppingItemDisplay> ShoppingList
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

        private string? _itemFormTitle;
        public string? ItemFormTitle
        {
            get { return _itemFormTitle; }
            set { this.RaiseAndSetIfChanged(ref _itemFormTitle, value); }
        }

        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> InputModeOnCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> InputModeOffCommand { get; }
        public ReactiveCommand<ShoppingItemDisplay, Unit> DeleteItemCommand { get; }
        public ReactiveCommand<ShoppingItemDisplay, Unit> BoughtItemCommand { get; }
        public ReactiveCommand<ShoppingItemDisplay, Unit> AddCommentCommand { get; }
        #endregion

        #region Methods
        public GroceryListViewModel()
        {
            Model = new();
            Model.ErrorTypeChanged += (_, _) => OnErrorTypeChanged();
            Model.ShoppingList.CollectionChanged += ShoppingList_CollectionChanged;
            Model.EditedItemChanged += (_, _) => CurrentlyEditedItem = Model.EditedItem?.Item;

            _shoppingList = [.. Model.ShoppingList.Select(item => new ShoppingItemDisplay(item, () => OnInputModeOn((item.Clone() as ShoppingItem)!, Model.ShoppingList.IndexOf(item))))];

            InputModeOnCommand = ReactiveCommand.Create(() => OnInputModeOn(ShoppingItem.Empty, -1));
            SaveCommand = ReactiveCommand.Create(() => { Model.SaveEdit(); if(Model.IsValidItem) OnInputModeOff(); }); 
            InputModeOffCommand = ReactiveCommand.Create(OnInputModeOff);
            DeleteItemCommand = ReactiveCommand.Create<ShoppingItemDisplay>(DeleteItem);
            BoughtItemCommand = ReactiveCommand.Create<ShoppingItemDisplay>(Boughtitem);
            AddCommentCommand = ReactiveCommand.Create<ShoppingItemDisplay>(AddComment);
        }

        private void ShoppingList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ShoppingList = [.. Model.ShoppingList.Select(item => new ShoppingItemDisplay(item, () => OnInputModeOn((item.Clone() as ShoppingItem)!, Model.ShoppingList.IndexOf(item))))];
        }
        private void OnErrorTypeChanged()
        {
            ErrorMessage = Model.ErrorType switch
            {
                ItemFormErrorType.EmptyName => "The item's name cannot be empty!",
                ItemFormErrorType.EmptyQuantity => "The item's quantity cannot be empty!",
                _ => null,
            };
        }
        private void OnInputModeOn(ShoppingItem item, int index)
        {
            if (index < 0)
            {
                item.Owner = App.CurrentUser!;
                ItemFormTitle = "Add a new item!";
            }
            else
            {
                ItemFormTitle = "Edit this item!";
            }

            Model.StartEdit(item, index);
            InputMode = true;
        }
        private void OnInputModeOff()
        {
            InputMode = false;
        }
        private async void DeleteItem(ShoppingItemDisplay item)
        {
            bool result = await App.MainView!.ShowConfirmDialog("Are you sure you want to delete this item?");

            if (!result) return;

            Model.DeleteItem(item.Item);
        }
        private void Boughtitem(ShoppingItemDisplay item)
        {
            Model.DeleteItem(item.Item);
        }
        private async void AddComment(ShoppingItemDisplay display)
        {
            string? comment = await App.MainView!.ShowTextInputDialog("Comment:", (input) => !string.IsNullOrWhiteSpace(input));
            if (string.IsNullOrWhiteSpace(comment)) return;
            Model.AddComment(display.Item, App.CurrentUser!, comment);
        }
        #endregion
    }
}
