using PackageEasy.ViewModels.Dialogs;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace PackageEasy.Views.Dialogs
{
    /// <summary>
    /// author:TT
    /// time:2024-05-20 10:00:00
    /// desc:编译进度窗口
    /// </summary>
    public partial class CompileProgressWindow : Window
    {
        public CompileProgressWindow(string projectName)
        {
            InitializeComponent();
            ViewModel = new CompileProgressViewModel(projectName);
            ViewModel.CloseRequested += ViewModel_CloseRequested;
            ViewModel.Logs.CollectionChanged += Logs_CollectionChanged;
            logList.Loaded += LogList_Loaded;
            this.Owner = App.Current.MainWindow;
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// 视图模型
        /// </summary>
        public CompileProgressViewModel ViewModel { get; }

        /// <summary>
        /// 是否已有待执行的滚动请求
        /// </summary>
        private bool scrollPending;

        private void ViewModel_CloseRequested(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Logs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RequestScrollToEnd();
        }

        private void LogList_Loaded(object sender, RoutedEventArgs e)
        {
            RequestScrollToEnd();
        }

        /// <summary>
        /// 请求滚动到最后一行（合并同一轮内的多次请求，延迟到布局完成后再滚动）
        /// </summary>
        private void RequestScrollToEnd()
        {
            if (scrollPending || ViewModel.Logs.Count == 0)
            {
                return;
            }
            scrollPending = true;
            Dispatcher.BeginInvoke(new Action(ScrollToEnd), DispatcherPriority.Background);
        }

        /// <summary>
        /// 滚动到最后一行
        /// </summary>
        private void ScrollToEnd()
        {
            scrollPending = false;
            if (!logList.IsLoaded || ViewModel.Logs.Count == 0)
            {
                return;
            }
            try
            {
                logList.ScrollIntoView(ViewModel.Logs[ViewModel.Logs.Count - 1]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            ViewModel.CloseRequested -= ViewModel_CloseRequested;
            ViewModel.Logs.CollectionChanged -= Logs_CollectionChanged;
            logList.Loaded -= LogList_Loaded;
            scrollPending = false;
            base.OnClosed(e);
        }
    }
}
