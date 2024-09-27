using Avalonia.Controls;
using ShoppingList.ViewModels;
using ReactiveUI;
using Avalonia.ReactiveUI;

namespace ShoppingList.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}