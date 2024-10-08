﻿using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain.Common;
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
            Task.Run(() =>
            {
                PlugInHelper.InitPlugIns();
                PlugIns = PlugInHelper.PlugInList;
                foreach (PlugInModel plug in PlugIns)
                {
                    if (CacheDataHelper.PluginList.Exists(C => C == plug.Uid))
                    {
                        plug.InstallState = Domain.Enums.PlugInState.Installed;
                    }
                    else
                    {
                        plug.InstallState = Domain.Enums.PlugInState.UnInstalled;
                    }
                }
                PlugIns = new List<PlugInModel>(PlugIns);
            });
        }
        public override string Description => CommonSettings.ToolPlugInManager.GetLangText();


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
                if (result.Item1)
                {
                    i.InstallState = Domain.Enums.PlugInState.Installed;
                    CacheDataHelper.OperatePlugin(i.Uid, true);
                }
                TMessageBox.ShowMsg(result.Item2);
            }

        });


        #endregion
    }
}
