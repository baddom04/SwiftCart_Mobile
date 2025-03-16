using ReactiveUI;
using System.Collections.Generic;
using System;

namespace ShoppingList.ViewModels.Shared
{
    internal abstract class MainViewModelBase<T> : ViewModelBase where T : Enum
    {
        private ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            private set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        protected readonly Dictionary<T, ViewModelBase> _pages;

        protected void ChangePage(T changeSettingsPage)
        {
            CurrentPage = _pages[changeSettingsPage];
        }
        public abstract void ChangeToDefaultPage();
    }
}
