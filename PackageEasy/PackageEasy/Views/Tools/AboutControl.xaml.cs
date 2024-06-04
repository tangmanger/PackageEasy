using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common.Data;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain.Common;
using PackageEasy.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Windows.Forms.LinkLabel;

namespace PackageEasy.Views.Tools
{
    /// <summary>
    /// AboutControl.xaml 的交互逻辑
    /// </summary>
    public partial class AboutControl : BaseControl
    {
        private string version;

        public AboutControl()
        {
            InitializeComponent();
            DataContext = this;
            Version = CacheDataHelper.Version;
        }

        public override string Description => CommonSettings.ToolAbout.GetLangText();

        public override bool ShowSureButton => false;
        public override void Load()
        {
        }

        public override void Save()
        {
        }

        public override void Unload()
        {
        }

        #region 属性

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get => version;
            set
            {
                version = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region 命令 


        /// <summary>
        /// 导航
        /// </summary>
        public RelayCommand<Uri> NavigateCommand => new RelayCommand<Uri>((uri) =>
        {
            if (uri == null) return;
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri) { UseShellExecute = true });
            //Process.Start(uri.OriginalString);
        });

        #endregion
    }
}
