using PackageEasy.Common.Logs;
using PackageEasy.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if !DEBUG

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

#endif
        }
        protected override void OnStartup(StartupEventArgs e)
        {
     
            base.OnStartup(e);
        }
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            const string msg = "异步异常";
            try
            {
                e.SetObserved();
                Log.Write(msg, e.Exception);
            }
            catch (Exception ex)
            {
                Log.Write(msg, ex);
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Log.Write("发生CurrentDomain异常:", ex);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Write("发生异常:", e.Exception);
            e.Handled = true;
        }
    }
}
