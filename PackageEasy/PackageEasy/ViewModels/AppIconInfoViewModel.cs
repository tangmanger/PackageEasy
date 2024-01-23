using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Domain.Models.SaveModel;
using PackageEasy.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-13 22:56:14
    /// desc:AppIconInfoViewModel
    /// </summary>
    public class AppIconInfoViewModel : BaseProjectViewModel, INavigateIn
    {

        public AppIconInfoViewModel(ViewType viewType, string key) : base(viewType, key)
        {
            AppIconInfoList = new ObservableCollection<AppIconInfoModel>();
            IsCanDeleted = AppIconInfoList.Any(p => p.IsSelected);
            IconPathDir = new List<DescModel<TargetDirType>>
            {
                new DescModel<TargetDirType>()
                {
                    Data = TargetDirType.ICONS_GROUP,
                    DisplayName = $"${TargetDirType.ICONS_GROUP.ToString()}",
                    Description = $"${TargetDirType.ICONS_GROUP.ToString()}"
                },
                new DescModel<TargetDirType>()
                {
                    Data = TargetDirType.DESKTOP,
                    DisplayName = $"${TargetDirType.DESKTOP.ToString()}",
                    Description = $"${TargetDirType.DESKTOP.ToString()}"
                },
                new DescModel<TargetDirType>()
                {
                    Data = TargetDirType.STARTMENU,
                    DisplayName = $"${TargetDirType.STARTMENU.ToString()}",
                    Description = $"${TargetDirType.STARTMENU.ToString()}"
                },
                new DescModel<TargetDirType>()
                {
                    Data = TargetDirType.SMPROGRAMS,
                    DisplayName = $"${TargetDirType.SMPROGRAMS.ToString()}",
                    Description = $"${TargetDirType.SMPROGRAMS.ToString()}"
                },
                new DescModel<TargetDirType>()
                {
                    Data = TargetDirType.QUICKLAUNCH,
                    DisplayName = $"${TargetDirType.QUICKLAUNCH.ToString()}",
                    Description = $"${TargetDirType.QUICKLAUNCH.ToString()}"
                }
            };

        }

        #region 属性

        private string startMenuName;
        private bool isCanChangeStartMenuName;
        private bool isCreateWebUrl = true;
        private bool isCreateUnInstall = true;
        private ObservableCollection<AppIconInfoModel> appIconInfoList;
        private bool isCanDeleted;
        private List<DescModel<TargetDirType>> iconPathDir;
        private List<string> targetFilesList;

        /// <summary>
        /// 启动目录名称
        /// </summary>
        public string StartMenuName
        {
            get => startMenuName;
            set
            {
                startMenuName = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// /允许用户改变开始菜单
        /// </summary>
        public bool IsCanChangeStartMenuName
        {
            get => isCanChangeStartMenuName;
            set
            {
                isCanChangeStartMenuName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 创建互联网快捷方式
        /// </summary>
        public bool IsCreateWebUrl
        {
            get => isCreateWebUrl;
            set
            {
                isCreateWebUrl = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 创建卸载快捷键
        /// </summary>
        public bool IsCreateUnInstall
        {
            get => isCreateUnInstall;
            set
            {
                isCreateUnInstall = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 快捷方式列表
        /// </summary>
        public ObservableCollection<AppIconInfoModel> AppIconInfoList
        {
            get => appIconInfoList;
            set
            {
                appIconInfoList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public bool IsCanDeleted
        {
            get => isCanDeleted;
            set
            {
                isCanDeleted = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 快捷方式目录
        /// </summary>
        public List<DescModel<TargetDirType>> IconPathDir
        {
            get => iconPathDir;
            set
            {
                iconPathDir = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 目标
        /// </summary>
        public List<string> TargetFilesList
        {
            get => targetFilesList;
            set
            {
                targetFilesList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 命令

        /// <summary>
        /// 添加命令
        /// </summary>
        public RelayCommand AddCommand => new RelayCommand(() =>
        {
            AppIconInfoModel appIconInfoModel = new AppIconInfoModel();
            appIconInfoModel.IconDir = IconPathDir.Find(p => p.Data == TargetDirType.DESKTOP);
            AppIconInfoList.Add(appIconInfoModel);
        });

        /// <summary>
        /// 删除
        /// </summary>
        public RelayCommand<AppIconInfoModel> DelCommand => new RelayCommand<AppIconInfoModel>((s) =>
        {
            if (AppIconInfoList != null && s != null)
            {
                AppIconInfoList.Remove(s);
            }
            IsCanDeleted = AppIconInfoList.Any(p => p.IsSelected);
        });

        /// <summary>
        /// 删除选中的
        /// </summary>
        public RelayCommand DelSelectCommand => new RelayCommand(() =>
        {
            if (AppIconInfoList == null || !AppIconInfoList.Any(x => x.IsSelected == true))
            {
                TMessageBox.ShowMsg("请选择要删除的项");
                return;
            }
            var selectedItem = AppIconInfoList.ToList().FindAll(c => c.IsSelected == true);
            if (selectedItem != null)
            {
                foreach (var item in selectedItem)
                {
                    AppIconInfoList.Remove(item);
                }
            }
            IsCanDeleted = AppIconInfoList.Any(p => p.IsSelected);
        });


        /// <summary>
        /// 是否选中
        /// </summary>

        public RelayCommand AppIconCheckCommand => new RelayCommand(() =>
        {
            IsCanDeleted = AppIconInfoList.Any(p => p.IsSelected);
        });

        #endregion

        #region 方法

        public override void RefreshData()
        {
            if (ProjectInfo != null && ProjectInfo.AppIcon != null)
            {
                TargetFilesList = RefreshFileList();
                if (string.IsNullOrWhiteSpace(ProjectInfo.AppIcon.StartMenuName))
                    ProjectInfo.AppIcon.StartMenuName = ProjectInfo.BaseInfo.ApplicationName;
                StartMenuName = ProjectInfo.AppIcon.StartMenuName;

                AppIconInfoList = ProjectInfo?.AppIcon?.AppIconInfoList ?? null;
                IsCanChangeStartMenuName = ProjectInfo.AppIcon.IsCanChangeStartMenuName;
                IsCreateUnInstall = ProjectInfo.AppIcon.IsCreateUnInstall;
                IsCreateWebUrl = ProjectInfo.AppIcon.IsCreateWebUrl;
                if (AppIconInfoList != null)
                {

                    foreach (var appIcon in AppIconInfoList)
                    {
                        if (IconPathDir.Exists(f => f.Data == appIcon?.IconDir?.Data))
                            appIcon.IconDir = IconPathDir.Find(x => x.Data == appIcon.IconDir.Data);
                        if (!string.IsNullOrWhiteSpace(appIcon.FilePath))
                        {
                            appIcon.FilePath = TargetFilesList.Find(x => x == appIcon.FilePath) ?? "";
                        }
                    }
                }
            }

        }

        public override void Save()
        {
            if (ProjectInfo.AppIcon == null)
                ProjectInfo.AppIcon = new AppIconModel();
            AppIconModel appIconModel = ProjectInfo.AppIcon;

            appIconModel.StartMenuName = StartMenuName;
            appIconModel.AppIconInfoList = AppIconInfoList;
            appIconModel.IsCanChangeStartMenuName = IsCanChangeStartMenuName;
            appIconModel.IsCreateUnInstall = IsCreateUnInstall;
            appIconModel.IsCreateWebUrl = IsCreateWebUrl;
            var path = Path.Combine(SavePath, "AppInfo.json");
            File.WriteAllText(path, appIconModel.SerializeObject());
        }

        public void NavigateIn()
        {
            RefreshData();
        }

        public override bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(StartMenuName))
            {
                TMessageBox.ShowMsg("应用程序\"开始菜单\"文件夹名称不能为空!");
                return false;
            }
            if (AppIconInfoList != null && AppIconInfoList.Count > 0)
            {
                foreach (var item in AppIconInfoList)
                {
                    if (item.IconDir == null)
                    {
                        TMessageBox.ShowMsg("目标文件夹名称不能为空!");
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(item.ShortcutPath))
                    {
                        TMessageBox.ShowMsg("快捷方式名称不能为空!");
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(item.FilePath))
                    {
                        TMessageBox.ShowMsg("目的文件不能为空!");
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}
