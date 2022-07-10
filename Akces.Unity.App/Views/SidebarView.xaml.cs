using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Akces.Unity.App.Views
{
    public partial class SidebarView : UserControl
    {
        private Button selectedButton;

        public SidebarView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button clickedButton))
                return;

            if (clickedButton == selectedButton)
                return;

            if (selectedButton != null) 
            {
                selectedButton.Background = Brushes.Transparent;
                selectedButton.Foreground = Brushes.Black;
                selectedButton.FontWeight = FontWeights.Normal;
                selectedButton.Focusable = true;
            }

            clickedButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0083d6"); ;
            clickedButton.Foreground = Brushes.White;
            clickedButton.FontWeight= FontWeights.SemiBold;

            selectedButton = clickedButton;
        }
    }
}
