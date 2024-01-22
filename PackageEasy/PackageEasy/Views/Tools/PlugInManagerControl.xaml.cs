using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Helpers;
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

namespace PackageEasy.Views.Tools
{
    /// <summary>
    /// PlugInManagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlugInManagerControl : BaseControl
    {
        private List<PlugInModel> plugIns;

        public PlugInManagerControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        public override string Description => "插件设置".GetLangText();


        /// <summary>
        /// 插件列表
        /// </summary>
        public List<PlugInModel> PlugIns
        {
            get => plugIns;
            set
            {
                plugIns = value;
                RaisePropertyChanged();
            }
        }
        public override void Save()
        {

        }
        public override void Load()
        {
            Task.Run(() =>
            {
                PlugInHelper.InitPlugIns();
                PlugIns = PlugInHelper.PlugInList;
            });
        }


        public override void Unload()
        {

        }

        #region 命令

        /// <summary>
        /// 执行
        /// </summary>
        public RelayCommand<PlugInModel> ExecuteCommand => new RelayCommand<PlugInModel>(i =>
        {
            var instance = Activator.CreateInstance(i.PlugInType) as ITPlugIn;
            if (instance != null)
            {
                var result = instance.Execute();
                TMessageBox.ShowMsg(result.Item2);
            }

        });


        #endregion
    }
}
