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
    /// AssemblyFilesDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AssemblyFilesDialog : Window
    {
        public AssemblyFilesDialog()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseExecute));
            this.Owner = App.Current.MainWindow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void CloseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
