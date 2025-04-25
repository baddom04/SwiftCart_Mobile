using DynamicData;
using ReactiveUI;
using ShoppingList.Model.Map;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Map
{
    internal class LocationFilterViewModel : ViewModelBase
    {
        private string? _searchResult;
        public string? SearchResult
        {
            get { return _searchResult; }
            set { this.RaiseAndSetIfChanged(ref _searchResult, value); }
        }

        public string InputNameKey { get; }
        public ObservableCollection<string> Data { get; } = [];
        public bool IsEnabled => Data.Count > 0;
        public LocationProperty Type { get; }

        private readonly LocationPropertyFilter _model;
        private readonly Action<bool> _showLoading;
        private readonly Action<NotificationType, string> _showNotification;
        public LocationFilterViewModel(LocationPropertyFilter model, LocationProperty type, Action<bool> showLoading, Action<NotificationType, string> showNotification) 
        {
            _model = model;
            Type = type;
            _showLoading = showLoading;
            _showNotification = showNotification;
            InputNameKey = type.ToString();

            _model.PossiblesChanged += () =>
            {
                Data.Clear();
                Data.AddRange(_model.Possibles);
                if (!IsEnabled)
                    SearchResult = null;
            };

            Data.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(IsEnabled));
        }

        public async Task SearchAsync()
        {
            _showLoading(true);
            try
            {
                await _model.GetPossiblesAsync(SearchResult);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("LocationsSearchError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally { _showLoading(false); }
        }
        public async Task OnLocationSelected()
        {
            _showLoading(true);
            try
            {
                if(_model.Child is not null)
                    await _model.Child.GetPossiblesAsync(string.IsNullOrWhiteSpace(SearchResult) ? null : SearchResult);
            }
            catch (Exception ex)
            {
                string msg = $"{StringProvider.GetString("LocationsSearchError")}{ex.Message}";
                _showNotification(NotificationType.Error, msg);
            }
            finally { _showLoading(false); }
        }
    }
}
