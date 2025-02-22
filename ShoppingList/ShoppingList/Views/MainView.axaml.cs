using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using ShoppingList.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Loaded += (_, _) => SetMenuIconColors(MainMenu);
        }
        public Task<bool> ShowConfirmDialog(string question)
        {
            ConfirmationView cv = new(question);
            MainGrid.Children.Add(cv);
            return cv.ShowDialog();
        }
        public Task<string?> ShowTextInputDialog(string instruction, Func<string, bool> validateInput)
        {
            TextInputView tiv = new(instruction, validateInput);
            MainGrid.Children.Add(tiv);
            return tiv.ShowDialog();
        }

        private void ListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox listBox) return;

            SetMenuIconColors(listBox);
        }
        private static void SetMenuIconColors(ListBox listBox)
        {
            foreach (var item in listBox.GetLogicalChildren())
            {
                if ((item as ListBoxItem)!.Content == listBox.SelectedItem)
                {
                    SetColorToPathIcon(item, "MainBtnBG");
                }
                else
                {
                    SetColorToPathIcon(item, "MainFG");
                }
            }
        }
        private static void SetColorToPathIcon(ILogical item, string resKey)
        {
            SolidColorBrush? brush = Application.Current!.FindResource(resKey) as SolidColorBrush;
            if (brush is null) return;

            (item.GetLogicalChildren().First() as PathIcon)!.Foreground = brush;
        }
    }
}
