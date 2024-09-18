using Avalonia.Controls;
using ShoppingList.ViewModels;

namespace ShoppingList.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}