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
        public ReactiveCommand<Unit, Unit> InputModeOnCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public ReactiveCommand<Unit, Unit> InputModeOffCommand { get; }
        public ReactiveCommand<ShoppingItemDisplay, Unit> DeleteItemCommand { get; }
        public ReactiveCommand<ShoppingItemDisplay, Unit> BoughtItemCommand { get; }
        public ReactiveCommand<ShoppingItemDisplay, Unit> AddCommentCommand { get; }
        public GroceryListViewModel()
        {
            Model = new();
            Model.ErrorMessageChanged += (_, _) => ErrorMessage = Model.ErrorMessage;
            Model.ShoppingList.CollectionChanged += ShoppingList_CollectionChanged; ;
            Model.EditedItemChanged += (_, _) => CurrentlyEditedItem = Model.EditedItem?.Item;

            _shoppingList = [.. Model.ShoppingList.Select(item => new ShoppingItemDisplay(item))];
            ShoppingList.ToList().ForEach(display => display.Editing += OnEditing);

            InputModeOnCommand = ReactiveCommand.Create(() => OnInputModeOn(ShoppingItem.Empty, -1));
            SaveCommand = ReactiveCommand.Create(() => { Model.SaveEdit(); OnInputModeOff(); }); 
            InputModeOffCommand = ReactiveCommand.Create(OnInputModeOff);
            DeleteItemCommand = ReactiveCommand.Create<ShoppingItemDisplay>(DeleteItem);
            BoughtItemCommand = ReactiveCommand.Create<ShoppingItemDisplay>(Boughtitem);
            AddCommentCommand = ReactiveCommand.Create<ShoppingItemDisplay>(AddComment);
        }

        private void ShoppingList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ShoppingList.ForEach(display => display.Editing -= OnEditing);
            ShoppingList = [.. Model.ShoppingList.Select(item => new ShoppingItemDisplay(item))];
            ShoppingList.ForEach(display => display.Editing += OnEditing);
        }

        private void OnInputModeOn(ShoppingItem item, int index)
        {
            Model.StartEdit(item, index);
            InputMode = true;
        }
        private void OnInputModeOff()
        {
            InputMode = false;
        }
        internal void OnEditing(ShoppingItemDisplay display)
        {
            OnInputModeOn((display.Item.Clone() as ShoppingItem)!, ShoppingList.IndexOf(display));
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
    }
}
