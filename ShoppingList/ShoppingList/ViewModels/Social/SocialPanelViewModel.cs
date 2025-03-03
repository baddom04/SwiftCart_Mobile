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
        public ReactiveCommand<Unit, Unit> TurnPageForwardCommand { get; }
        public ReactiveCommand<Unit, Unit> TurnPageBackwardCommand { get; }
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

        private int _maxPage;
        public int MaxPage
        {
            get { return _maxPage; }
            private set { this.RaiseAndSetIfChanged(ref _maxPage, value); }
        }


        public bool EmptyHouseholds => Households.Count == 0;

        private readonly MainSocialPanelModel _model;
        private readonly Action<NotificationType, string> _showNotification;

        public SocialPanelViewModel(MainSocialPanelModel householdsModel, Action<NotificationType, string> showNotification)
        {
            _model = householdsModel;
            _showNotification = showNotification;

            Households.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyHouseholds));

            SearchCommand = ReactiveCommand.CreateFromTask(() => Search(1));

            TurnPageForwardCommand = ReactiveCommand.CreateFromTask(() => Search(Page + 1), 
                this.WhenAnyValue(x => x.Page, x => x.MaxPage,
            (page, maxPage) => page != maxPage));

            TurnPageBackwardCommand = ReactiveCommand.CreateFromTask(() => Search(Page - 1), 
                this.WhenAnyValue(x => x.Page, page => page != 1));
        }

        public async Task Search(int page = 1)
        {
            IsLoading = true;
            Page = page;

            try
            {
                Households.Clear();
                Households.AddRange(await _model.SearchHouseholdsAsync(SearchInput, Page));

                MaxPage = _model.MaxPage;
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
