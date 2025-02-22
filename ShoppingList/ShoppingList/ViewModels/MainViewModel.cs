using ReactiveUI;
using System;
using System.Collections.Generic;

namespace ShoppingList.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<Type, ViewModelBase> _pages;

		private ViewModelBase _currentPage;
		public ViewModelBase CurrentPage
		{
			get { return _currentPage; }
			private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
		}

        public MainViewModel()
        {
            _pages = new Dictionary<Type, ViewModelBase>()
            {
                { typeof(LoginViewModel), new LoginViewModel() },
                { typeof(RegisterViewModel), new RegisterViewModel() },
                { typeof(LoggedInViewModel), new LoggedInViewModel() },
            };

            (_pages[typeof(LoginViewModel)] as LoginViewModel)!.RegisterCommand = 
                ReactiveCommand.Create(() => CurrentPage = _pages[typeof(RegisterViewModel)]);

            (_pages[typeof(RegisterViewModel)] as RegisterViewModel)!.LoginCommand =
                ReactiveCommand.Create(() => CurrentPage = _pages[typeof(LoginViewModel)]);

            _currentPage = _pages[typeof(LoginViewModel)];
        }
    }
}
