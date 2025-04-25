using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using ShoppingList.Shared.Model.Settings;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Map
{
    internal class StoreListItemViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public string Name { get; }
        public Location Location { get; }
        public ReactiveCommand<Unit, Unit> LoadStoreCommand { get; }

        private readonly StoreListItemModel _model;
        private readonly Action<ViewModelBase> _changeToPage;
        private readonly Action<MapPage> _changePage;
        private readonly Action<NotificationType, string> _showNotification;
        private readonly Action<bool> _showLoading;
        private readonly UserAccountModel _account;
        public StoreListItemViewModel(UserAccountModel account, StoreListItemModel model, Action<bool> showLoading, Action<NotificationType, string> showNotification, Action<ViewModelBase> changeToPage, Action<MapPage> changePage)
        {
            _account = account;
            _model = model;
            _showLoading = showLoading;
            _showNotification = showNotification;
            _changeToPage = changeToPage;
            _changePage = changePage;
            Name = _model.StoreWithoutMap.Name;
            Location = _model.StoreWithoutMap.Location!;
            LoadStoreCommand = ReactiveCommand.CreateFromTask(LoadStoreAsync);
        }
        private async Task LoadStoreAsync()
        {
            IsLoading = true;

            try
            {
                MapViewModel map = new(_account, new MapModel(await _model.GetFullStoreAsync()), _showLoading, _changeToPage, _changePage, _showNotification);
                if (map.MapSegments.ToList().Count == 0)
                    throw new Exception(StringProvider.GetString("NoMapSegmentsError"));
                IsLoading = false;
                _changeToPage(map);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("StoreQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
