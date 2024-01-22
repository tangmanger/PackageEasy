using CommunityToolkit.Mvvm.DependencyInjection;
using PackageEasy.Common.Logs;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Enums;
using PackageEasy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 16:43:40
    /// desc:NavigationHelper
    /// </summary>
    public class NavigationHelper
    {
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="name"></param>
        public static void GoTo<T>(ViewType name, T param)
        {
            var mainView = Ioc.Default.GetService<MainViewModel>();
            if (mainView != null)
            {
                if (mainView.WorkView != null)
                {
                    var naviget = mainView.WorkView.DataContext as INavigateOut;
                    if (naviget != null)
                        naviget.NavigateOut();
                }
                mainView.WorkView = GetView(name, param);
            }
        }
        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="name"></param>
        public static void GoTo(ViewType name)
        {
            var mainView = Ioc.Default.GetService<MainViewModel>();
            if (mainView != null)
            {
                if (mainView.WorkView != null)
                {
                    var naviget = mainView.WorkView.DataContext as INavigateOut;
                    if (naviget != null)
                        naviget.NavigateOut();
                }
                mainView.WorkView = GetView<object>(name, null);
            }
        }
        private static FrameworkElement GetView<T>(ViewType name, T? param)
        {

            var viewType = TViewsHelper.NaviteViewList.Find(p => p.ViewType == name);
            var navigate = Ioc.Default.GetService(viewType.VmType) as INavigateIn;
            try
            {


                if (navigate != null)
                {
                    navigate.NavigateIn();
                }
                else
                {
                    var navigateParam = Ioc.Default.GetService(viewType.VmType) as INavigateIn<T>;
                    if (navigateParam != null)
                        navigateParam.NavigateIn(param);
                }

            }
            catch (Exception ex)
            {
                Log.Write($"导航发生错误", ex);
            }
            if (name == ViewType.None) return null;
            return (FrameworkElement)Ioc.Default.GetService(viewType.Type);
        }
    }
}
