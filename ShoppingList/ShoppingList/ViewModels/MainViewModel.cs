using ReactiveUI;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Dictionary<Type, ViewModelBase> _pages;

		private ViewModelBase _currentPage;
		public ViewModelBase CurrentPage
		{
			get { return _currentPage; }
			set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
		}

        public MainViewModel()
        {
            _pages = new Dictionary<Type, ViewModelBase>()
            {
                { typeof(LoginViewModel), new LoginViewModel() },
                { typeof(RegisterViewModel), new RegisterViewModel() },
                { typeof(LoggedInViewModel), new LoggedInViewModel() },
            };

            _currentPage = _pages[typeof(LoginViewModel)];
        }
    }
}
