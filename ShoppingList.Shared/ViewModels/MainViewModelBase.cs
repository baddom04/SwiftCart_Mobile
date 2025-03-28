using ReactiveUI;

namespace ShoppingList.Shared.ViewModels;

public abstract class MainViewModelBase : ViewModelBase
{
    protected ViewModelBase _currentPage = null!;
    public ViewModelBase CurrentPage
    {
        get { return _currentPage; }
        protected set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
    }

    public abstract void ChangeToDefaultPage();
}

public abstract class MainViewModelBase<T> : MainViewModelBase where T : Enum
{
    protected Dictionary<T, ViewModelBase> _pages = [];

    protected void ChangePage(T changeSettingsPage)
    {
        CurrentPage = _pages[changeSettingsPage];
    }
}
