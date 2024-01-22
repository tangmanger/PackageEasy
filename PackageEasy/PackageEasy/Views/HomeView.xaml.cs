using Microsoft.Xaml.Behaviors;
using PackageEasy.Attributes;
using PackageEasy.Domain.Enums;
using PackageEasy.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PackageEasy.Views
{
    /// <summary>
    /// 作者：TT
    /// 时间：2023-03-11 15:54:14
    /// 描述： HomeView.xaml 的交互逻辑、
    /// TANGMANGER
    /// </summary>
    [TView(ViewType.Home,typeof(HomeView),typeof(HomeViewModel))]
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}
