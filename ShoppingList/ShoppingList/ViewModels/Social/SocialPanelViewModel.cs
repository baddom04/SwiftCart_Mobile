using DynamicData;
using ReactiveUI;
using ShoppingList.Model;
using ShoppingList.Model.Social;
using ShoppingList.Shared;
using ShoppingList.Shared.Utils;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels.Social;

internal class SocialPanelViewModel : ViewModelBase
{
    public string SearchInput { get; set; } = string.Empty;
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> TurnPageForwardCommand { get; }
    public ReactiveCommand<Unit, Unit> TurnPageBackwardCommand { get; }
    public ReactiveCommand<Unit, Unit> ManageApplicationsPageCommand { get; }
    public ReactiveCommand<Unit, Unit> ManageHouseholdsPageCommand { get; }
    public ObservableCollection<HouseholdSearchResultViewModel> Households { get; } = [];

    private bool _isLoading;
    public bool IsLoading
    {
        get { return _isLoading; }
        private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
    }

    private int _page;
    public int Page
    {
        get { return _page; }
        private set { this.RaiseAndSetIfChanged(ref _page, value); }
    }

    private int _maxPage;
    public int MaxPage
    {
        get { return _maxPage; }
        private set { this.RaiseAndSetIfChanged(ref _maxPage, value); }
    }

    public bool EmptyHouseholds => Households.Count == 0;

    private readonly SocialPanelModel _model;
    private readonly Action<NotificationType, string> _showNotification;
    private readonly Action<SocialPage> _changePage;

    public SocialPanelViewModel(SocialPanelModel householdsModel, Action<NotificationType, string> showNotification, Action<SocialPage> changePage)
    {
        _model = householdsModel;
        _showNotification = showNotification;
        _changePage = changePage;

        Households.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(EmptyHouseholds));

        SearchCommand = ReactiveCommand.CreateFromTask(() => SearchAsync(1));
        ManageApplicationsPageCommand = ReactiveCommand.Create(() => _changePage(SocialPage.ManageApplications));
        ManageHouseholdsPageCommand = ReactiveCommand.Create(() => _changePage(SocialPage.ManageHouseholds));

        TurnPageForwardCommand = ReactiveCommand.CreateFromTask(() => SearchAsync(Page + 1), 
            this.WhenAnyValue(x => x.Page, x => x.MaxPage,
        (page, maxPage) => page != maxPage));

        TurnPageBackwardCommand = ReactiveCommand.CreateFromTask(() => SearchAsync(Page - 1), 
            this.WhenAnyValue(x => x.Page, page => page != 1));
    }

    public async Task SearchAsync(int page = 1)
    {
        IsLoading = true;
        Page = page;

        try
        {
            Households.Clear();
            Households.AddRange(
                (await _model.SearchHouseholdsAsync(SearchInput, Page))
                .Select(hh => new HouseholdSearchResultViewModel(new HouseholdListItemModel(hh), _showNotification))
            );

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
