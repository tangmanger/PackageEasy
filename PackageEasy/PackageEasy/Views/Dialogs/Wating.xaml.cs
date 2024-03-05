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
using System.Windows.Shapes;

namespace PackageEasy.Views.Dialogs
{
    /// <summary>
    /// Wating.xaml 的交互逻辑
    /// </summary>
    public partial class Wating : Window
    {

        public bool IsRunning { get; set; }
        public Wating(bool setOwner = true)
        {
            InitializeComponent();

            if (setOwner)
            {


                Window owner = App.Current.MainWindow;//  WinHelper.GetActiveWindowEx();
                if (owner != this)
                {
                    this.Width = owner.ActualWidth;
                    this.Height = owner.ActualHeight;
                    this.WindowState = owner.WindowState;

                    this.Top = owner.Top;
                    this.Left = owner.Left;
                    this.Owner = owner;

                }
            }

            IsRunning = true;
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
