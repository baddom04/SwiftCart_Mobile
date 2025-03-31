﻿using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Shared.ViewModels;
using ShoppingListEditor.Model;
using ShoppingListEditor.Model.Editables;
using ShoppingListEditor.Utils;
using ShoppingListEditor.ViewModels.Editor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingListEditor.ViewModels
{
    internal class LoggedInViewModel : MainViewModelBase<LoggedInPages>
    {
        public UserSettingsViewModel UserSettings { get; }
        private readonly EditorModel _model;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        public LoggedInViewModel(UserAccountModel account, EditorModel model, Action<MainPage> changePage, Action<bool> showLoading, Action<NotificationType, string> showNotification)
        {
            UserSettings = new UserSettingsViewModel(account, changePage, showLoading, showNotification);

            _model = model;
            _showLoading = showLoading;
            _showNotification = showNotification;

            _pages = new Dictionary<LoggedInPages, ViewModelBase>()
            {
                { LoggedInPages.Store, new StoreCreationViewModel(_model, ChangePage, _showLoading, _showNotification) },
                { LoggedInPages.Location, new LocationCreationViewModel() },
                { LoggedInPages.Map, new MapCreationViewModel() },
                { LoggedInPages.Editor, new EditorViewModel() },
            };
        }

        public async Task ToStartingPage()
        {
            _showLoading(true);
            try
            {
                StoreEditable? store = await _model.GetUsersStoreAsync();

                if (store is null)
                    CurrentPage = _pages[LoggedInPages.Store];
                else if (store.Location is null)
                    CurrentPage = _pages[LoggedInPages.Location];
                else if (store.Map is null)
                    CurrentPage = _pages[LoggedInPages.Map];
                else
                    CurrentPage = _pages[LoggedInPages.Editor];
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("UserStoreQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                _showLoading(false);
            }
        }

        public override void ChangeToDefaultPage()
        {
            throw new NotImplementedException();
        }
    }
    internal enum LoggedInPages
    {
        Store,
        Location,
        Map,
        Editor
    }
}
