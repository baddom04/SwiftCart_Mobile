using DynamicData;
using ReactiveUI;
using ShoppingList.Core;
using ShoppingList.Model.Models;
using ShoppingList.Utils;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social
{
    internal class SocialPanelViewModel : ViewModelBase
    {
        public string SearchInput { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<Unit, Unit> TurnPageForward { get; }
        public ReactiveCommand<Unit, Unit> TurnPageBackward { get; }
        public ObservableCollection<Household> Households { get; } = [];

        private bool _showLoading;
        public bool IsLoading
        {
            get { return _showLoading; }
            set { this.RaiseAndSetIfChanged(ref _showLoading, value); }
        }

        private int _page;
        public int Page
        {
            get { return _page; }
            set { this.RaiseAndSetIfChanged(ref _page, value); }
        }

        public bool EmptyHouseholds => Households.Count == 0;

        private readonly MainSocialPanelModel _households;
        private readonly Action<NotificationType, string> _showNotification;

        public SocialPanelViewModel(MainSocialPanelModel households, Action<NotificationType, string> showNotification)
        {
            _households = households;
            _showNotification = showNotification;

            Households.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyHouseholds));

            SearchCommand = ReactiveCommand.CreateFromTask(() => Search(1));
            TurnPageForward = ReactiveCommand.CreateFromTask(() => Search(Page + 1));
            TurnPageBackward = ReactiveCommand.CreateFromTask(() => Search(Page - 1));
        }

        private async Task Search(int page)
        {
            IsLoading = true;
            Page = page;

            try
            {
                Households.Clear();
                Households.AddRange(await _households.GetHouseholdsAsync(SearchInput, Page));
            }
            catch(Exception ex)
            {
                string message = $"{StringProvider.GetString("HouseholdQueryError")}{ex.Message}";
                _showNotification(NotificationType.Error, message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
