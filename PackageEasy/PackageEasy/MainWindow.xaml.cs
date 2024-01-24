using PackageEasy.Common.Helpers;
using PackageEasy.Common;
using PackageEasy.Domain.Enums;
using PackageEasy.Helpers;
using PackageEasy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PackageEasy.Common.Logs;
using System.IO;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.ComponentModel;

namespace PackageEasy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.StateChanged += MainWindow_StateChanged;
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                max.Visibility = Visibility.Collapsed;
                normal.Visibility = Visibility.Visible;
            }
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                max.Visibility = Visibility.Visible;
                normal.Visibility = Visibility.Collapsed;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow_StateChanged(null, null);
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseExecute));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeExecute));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreExecute));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeExecute));
            NavigationHelper.GoTo(ViewType.Home);



        }

        private void MinimizeExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RestoreExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void MaximizeExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void CloseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            if (vm != null)
            {
                var result = vm.CloseAll();
                e.Cancel = !result;
            }
        }
        private void ItemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            scroller.ScrollToHorizontalOffset(scroller.HorizontalOffset - e.Delta);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;//窗口过程
            if (hwndSource != null)
                hwndSource.AddHook(WindowsMessgae);  //挂钩
        }
        readonly object locker = new object();
        private IntPtr WindowsMessgae(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WinMessageHelper.WM_OPENFile)
            {
                this.Activate();
            }
            if (msg == WinMessageHelper.WM_COPYDATA)
            {
                COPYDATASTRUCT cOPYDATASTRUCT = new COPYDATASTRUCT();
                cOPYDATASTRUCT = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                //Log.Write(cOPYDATASTRUCT.lpData);
                if (!string.IsNullOrWhiteSpace(cOPYDATASTRUCT.lpData) && File.Exists(cOPYDATASTRUCT.lpData))
                {
                    this.Activate();
                    Monitor.Enter(locker);

                    try
                    {


                        CacheDataHelper.OpenPath = cOPYDATASTRUCT.lpData;
                        if (CacheDataHelper.FileOpenDic.ContainsValue(CacheDataHelper.OpenPath))
                        {

                            TMessageBox.ShowMsg("", "当前文件已打开!");
                            CacheDataHelper.OpenPath = string.Empty;
                            return IntPtr.Zero;
                        }
                        Task.Run(() =>
                        {
                            Thread.Sleep(500);
                            App.Current.Dispatcher.Invoke(() =>
                            {

                                var vm = Ioc.Default.GetService<MainViewModel>();
                                vm.OpenFile(CacheDataHelper.OpenPath);
                                CacheDataHelper.OpenPath = string.Empty;
                            });
                        });

                        //this.Activate();
                        //this.WindowState = CurrentWindowState;
                    }
                    catch (Exception ex)
                    {
                        Log.Write("打开文件发生异常:" + ex.Message + ex.StackTrace);
                    }
                    finally
                    {
                        Monitor.Exit(locker);
                    }
                }

                //int a = (int)lParam;
                //byte[] s = new byte[a];
                //Marshal.Copy(wParam,s, 0, a);
                //var str=Encoding.UTF8.GetString(s);
            }
            //string[] PortNames = null;

            return IntPtr.Zero;
        }

        public const int WM_DEVICECHANGE = 0x219;          //Windows消息编号
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
    }
}
