using ReactiveUI;
using System.Collections.Generic;

namespace ShoppingList.ViewModels.Social
{
    internal class MainSocialPanelViewModel : ViewModelBase
    {
        private readonly Dictionary<SocialPage, ViewModelBase> _pages;

        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        public MainSocialPanelViewModel()
        {
            _pages = new Dictionary<SocialPage, ViewModelBase>()
            {
                { SocialPage.Main, new SocialPanelViewModel() },
            };

            _currentPage = _pages[SocialPage.Main];
        }
    }

    internal enum SocialPage
    {
        Main
    }
}
