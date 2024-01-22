using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Logs;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Helpers;
using PackageEasy.Models;
using PackageEasy.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 15:54:42
    /// desc:HomeViewModel
    /// </summary>
    public class HomeViewModel : BaseViewModel, INavigateIn
    {
        private readonly IDataService Service;
        public HomeViewModel(IDataService service)
        {
            Service = service;
        }

        #region 属性

        private List<RecentlyModel> recentlyList;

        /// <summary>
        /// 最近打开的文件
        /// </summary>
        public List<RecentlyModel> RecentlyList
        {
            get => recentlyList;
            set
            {
                recentlyList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 命令

        /// <summary>
        /// 创建命令
        /// </summary>
        public RelayCommand CreateCommand => new RelayCommand(() =>
        {
            var projectViewModel = CreateProject();
            Service.OnCreateProject(projectViewModel.ProjectName, projectViewModel.Key);
        });


        /// <summary>
        /// 加载
        /// </summary>
        public RelayCommand LoadedCommand => new RelayCommand(() =>
        {




            if (!string.IsNullOrWhiteSpace(CacheDataHelper.OpenPath))
            {
                var vm = Ioc.Default.GetService<MainViewModel>();
                if (CacheDataHelper.FileOpenDic.ContainsValue(CacheDataHelper.OpenPath))
                {
                    TMessageBox.ShowMsg("", "当前文件已打开");
                    CacheDataHelper.OpenPath = string.Empty;
                    return;
                }
                vm.OpenFile(CacheDataHelper.OpenPath);
                CacheDataHelper.OpenPath = string.Empty;
            }
            RecentlyList = CacheDataHelper.RecentlyList.OrderByDescending(s => s.UpdateTime).ToList();


        });

        public RelayCommand<RecentlyModel> OpenRecentlyCommand => new RelayCommand<RecentlyModel>((s) =>
        {
            if (s == null || !File.Exists(s.FilePath))
            {
                string str = string.Format("当前文件{0}不存在,是否从最近打开移除?", s.RecentlyName);
                var result = TMessageBox.ShowMsg("", str, Enums.MessageLevel.Question);
                if(result==Enums.TMessageBoxResult.OK)
                {
                    CacheDataHelper.DeleteRecently(s.RecentlyName);
                    RecentlyList = CacheDataHelper.RecentlyList.OrderByDescending(s => s.UpdateTime).ToList();
                }
                return;
            }
            if (CacheDataHelper.FileOpenDic.ContainsValue(s.FilePath))
            {
                TMessageBox.ShowMsg("", "当前文件已打开");
                return;
            }
            var vm = Ioc.Default.GetService<MainViewModel>();
            vm.OpenFile(s.FilePath);
        });

        #endregion

        #region 方法

        public void NavigateIn()
        {

        }

        #endregion
    }
}
