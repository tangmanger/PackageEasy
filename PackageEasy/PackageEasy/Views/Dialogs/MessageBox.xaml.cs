using log4net.Core;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Enums;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PackageEasy.Views.Dialogs
{
    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseExecute));
        }
        MessageLevel MessageLevel { get; set; }
        public MessageBox(string captionInfo, string msgInfo, MessageLevel level)
        {
            InitializeComponent();
            //Window owner = WinHelper.GetActiveWindowEx();
            //if (owner != null)
            //    Owner = owner;
            MessageLevel = level;
            this.Owner = App.Current.MainWindow;
            this.Title = captionInfo.GetLangText();
            msg.Text = msgInfo.GetLangText();
            switch (level)
            {
                case MessageLevel.Information:
                    ok.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.Question:
                    ok.Visibility = Visibility.Visible;
                    cancel.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.Warning:
                    ok.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.Error:
                    ok.Visibility = Visibility.Visible;
                    break;
                case MessageLevel.YesNoCancel:
                    ok.Visibility = Visibility.Visible;
                    cancel.Visibility = Visibility.Visible;
                    no.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseExecute));
            this.Loaded += MessageBox_Loaded;
        }

        private void MessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }

        public TMessageBoxResult MessageBoxResult { get; set; }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = TMessageBoxResult.Close;
            Close();
        }
        private void CloseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = TMessageBoxResult.OK;
            Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageLevel != MessageLevel.Question && MessageLevel != MessageLevel.YesNoCancel)
            //    MessageBoxResult = TMessageBoxResult.OK;
            //else
            MessageBoxResult = TMessageBoxResult.Cancel;
            this.Close();
        }
    }
}
