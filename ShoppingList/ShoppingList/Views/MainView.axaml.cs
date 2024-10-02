using Avalonia.Controls;
using Avalonia.Interactivity;
using ShoppingList.ViewModels;
using System;
using System.Threading.Tasks;

namespace ShoppingList.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        public Task<bool> ShowConfirmDialog(string question)
        {
            ConfirmationView cv = new(question);
            MainGrid.Children.Add(cv);
            return cv.ShowDialog();
        }
    }
}
