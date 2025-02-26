using Avalonia.Controls;

namespace ShoppingList.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainView mainView)
        {
            InitializeComponent();
            MyWindow.Content = mainView;
        }
    }
}