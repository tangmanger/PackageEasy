using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Common.WinAPIs;
using PackageEasy.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy
{
    public class Program
    {
        static System.Threading.Mutex mutex;
        [STAThread]
        static void Main(string[] args)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "PackageEasy", out ret);
            if (!ret)
            {



                var p = Process.GetProcessesByName("PackageEasy");
                if (p != null && p.Length > 0)
                {
                    var pc = Process.GetCurrentProcess();
                    var others = p.ToList().Find(P => P.Id != pc.Id);
                    if (others != null)
                    {
                        if (args.Length > 1)
                        {
                            IntPtr fileIntPtr = IntPtr.Zero;

                            var filePath = args[1];
                            byte[] buffer = Encoding.UTF8.GetBytes(filePath);
                            int len = buffer.Length;
                            //fileIntPtr=Marshal.AllocHGlobal(len);
                            COPYDATASTRUCT cds;
                            cds.dwData = (IntPtr)0;
                            cds.cbData = len + 1;
                            cds.lpData = filePath;
                            Console.WriteLine(filePath);
                            //MessageBox.Show(others.Id.ToString());
                            //WinMessageHelper.SetForegroundWindow(others.MainWindowHandle);
                            //WinMessageHelper.SendMessage((IntPtr)WinMessageHelper.HWND_BROADCAST, WinMessageHelper.WM_OPENFile, 0, ref oPYDATASTRUCT);
                            WinMessageHelper.SendMessage(others.MainWindowHandle, WinMessageHelper.WM_COPYDATA, 0, ref cds);
                            //Environment.Exit(0);
                        }
                        else
                        {
                            WinMessageHelper.SendMessage(others.MainWindowHandle, WinMessageHelper.WM_SYSCOMMAND, WinMessageHelper.SC_NOMAL, 0);
                            WinApi.SetForegroundWindow(others.MainWindowHandle);
                            //WinApi.SetActiveWindow(others.MainWindowHandle);
                        }
                    }
                }
                Environment.Exit(0);
            }
            Init();
            if (args.Length > 1)
            {
                var filePath = args[1];
                CacheDataHelper.OpenPath = filePath;

            }
            //CacheDataHelper.OpenPath = @"D:\Projects\PackageEasy\PackageEasy\PackageEasy\bin\x64\Debug\测试\PackageEasy.pge";
#if DEBUG
            MessageBox.Show($"当前是测试版软件,仅限内部测试使用!");
#endif
            CacheDataHelper.Init();
            App app = new App();//塑料制造->耗材制造

            app.InitializeComponent();
            app.Run();
        }
        static void Init()
        {
            Log.Init();
            NSISHelper.Init();
            TViewsHelper.Init();
        }
    }
}
