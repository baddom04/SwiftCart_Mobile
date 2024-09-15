﻿using ReactiveUI;
using ShoppingList.Loaders;
using ShoppingList.Models;
using System;
using System.Collections.ObjectModel;

namespace ShoppingList.ViewModels
{
    public class GroceryListViewModel : ViewModelBase
    {
        public ObservableCollection<ShoppingItem> ShoppingList { get; }

        private bool _inputMode;
        public bool InputMode
        {
            get { return _inputMode; }
            set { this.RaiseAndSetIfChanged(ref _inputMode, value); }
        }
        public ObservableCollection<Unit> Units { get; } = [.. (Unit[])Enum.GetValues(typeof(Unit))];

        private Unit _selectedUnit = Unit.Pieces;
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set { this.RaiseAndSetIfChanged(ref _selectedUnit, value); }
        }

        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> InputModeToggleCommand { get; set; }
        public GroceryListViewModel()
        {
            InputMode = false;

            User me = App.CurrentUser;

            ShoppingList = [.. ShoppingListLoader.LoadShoppingList()];
            ShoppingList.CollectionChanged += (_, _) => ShoppingListLoader.SaveShoppingList([.. ShoppingList]);

            InputModeToggleCommand = ReactiveCommand.Create(ToggleInputMode);
        }
        public void ToggleInputMode() => InputMode = !InputMode;
    }
}
