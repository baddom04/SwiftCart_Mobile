using Avalonia.Controls;
using ShoppingList.ViewModels;

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