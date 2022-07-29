using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Akces.Unity.App
{
    /// <summary>
    /// Logika interakcji dla klasy ExtraWindow.xaml
    /// </summary>
    public partial class ExtraWindow : Window
    {
        public ExtraWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Owner = null;
            base.OnClosing(e);
        }
    }
}
