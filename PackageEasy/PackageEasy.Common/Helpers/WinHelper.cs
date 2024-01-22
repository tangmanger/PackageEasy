using PackageEasy.Common.WinAPIs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace PackageEasy.Common.Helpers
{
    public class WinHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        internal static extern IntPtr GetActiveWindow();
        public static IntPtr GetMainWindow(string processName)
        {
            var proce = Process.GetProcessesByName(processName);
            if (proce == null || proce.Length == 0)
                return IntPtr.Zero;
            else
                return proce.FirstOrDefault().MainWindowHandle;
        }
        public static Window GetActiveWindowEx(bool accurate)
        {
            Window window = null;
            try
            {
                IntPtr activeWindow = GetMainWindow("PackageEasy");
                if (activeWindow != IntPtr.Zero && HwndSource.FromHwnd(activeWindow)?.RootVisual is Window)
                {
                    window = (Window)HwndSource.FromHwnd(activeWindow).RootVisual;
                    if (window != null)
                    {
                        return window;
                        foreach (object window3 in Application.Current.Windows)
                        {
                            if (window3.Equals(window))
                            {
                                return window;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if (accurate)
            {
                return window;
            }

            try
            {
                foreach (Window window4 in Application.Current.Windows)
                {
                    if (window4.IsActive || window4.Visibility == Visibility.Visible)
                    {
                        window = window4;
                        if (window4.IsActive)
                        {
                            return window;
                        }
                    }
                }

                return window;
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.ToString());
                return window;
            }
        }
        public static Window GetActiveWindowEx()
        {
            return GetActiveWindowEx(accurate: false);
        }



        public static bool GetNoOperatePanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(NoOperatePanelProperty);
        }

        public static void SetNoOperatePanel(DependencyObject obj, bool value)
        {
            obj.SetValue(NoOperatePanelProperty, value);
        }

        // Using a DependencyProperty as the backing store for NoOperatePanel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoOperatePanelProperty =
            DependencyProperty.RegisterAttached("NoOperatePanel", typeof(bool), typeof(WinHelper), new PropertyMetadata(false, OnNoOperatePanelPropertyChanged));

        private static void OnNoOperatePanelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if ((bool)e.NewValue && window != null)
            {
                // Get this window's handle
                IntPtr hwnd = new WindowInteropHelper(window).Handle;

                // Change the extended window style to not show a window icon
                int extendedStyle = WinApi.GetWindowLong(hwnd, WinApi.GWL_EXSTYLE);
                WinApi.SetWindowLong(hwnd, WinApi.GWL_EXSTYLE, extendedStyle | WinApi.WS_EX_DLGMODALFRAME);

                // Update the window's non-client area to reflect the changes
                WinApi.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, WinApi.SWP_NOMOVE | WinApi.SWP_NOSIZE | WinApi.SWP_NOZORDER | WinApi.SWP_FRAMECHANGED);
            }
        }
    }
}
