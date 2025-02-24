using ReactiveUI;
using System.Collections.Generic;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<Page, ViewModelBase> _pages;

		private ViewModelBase _currentPage;
		public ViewModelBase CurrentPage
		{
			get { return _currentPage; }
			private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
		}

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
        }

        public MainViewModel()
        {
            _pages = new Dictionary<Page, ViewModelBase>()
            {
                { Page.Login, new LoginViewModel(ChangePage, ShowLoading) },
                { Page.Register, new RegisterViewModel(ChangePage, ShowLoading) },
                { Page.Main, new LoggedInViewModel() },
            };

            _currentPage = _pages[Page.Login];
        }

        private void ChangePage(Page page)
        {
            CurrentPage = _pages[page];
        }
        private void ShowLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }

    public enum Page
    {
        Login,
        Register,
        Main
    }
}
