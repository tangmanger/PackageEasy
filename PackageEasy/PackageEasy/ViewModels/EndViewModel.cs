using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Domain.Models.SaveModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-14 23:30:17
    /// desc:EndViewModel
    /// </summary>
    public class EndViewModel : BaseProjectViewModel, INavigateIn
    {

        public EndViewModel(ViewType viewType, string key) : base(viewType, key)
        {
            UninstallTip = "你确实要完全移除 $(^Name) ，其及所有的组件？";
            UninstallMsg = "$(^Name) 已成功地从你的计算机移除。";
            UninstallProcessTips = "卸载程序检测到 ${PRODUCT_NAME} 正在运行，是否强行关闭并继续卸载?";
            InstallProcessTips = "安装程序检测到 ${PRODUCT_NAME} 正在运行，是否强行关闭并继续安装?";
        }

        #region 属性

        private bool isAutoRun;
        private string runParam;
        private List<string> targetFilesList;
        private string applicationName;
        private string readMeName;
        private string uninstallMsg;
        private string uninstallTip;
        private bool isEnableProcess;
        private string uninstallProcessTips;
        private string installProcessTips;
        private string processName;
        private bool isShowReadme;

        /// <summary>
        /// 自动运行
        /// </summary>
        public bool IsAutoRun
        {
            get => isAutoRun;
            set
            {
                isAutoRun = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 程序启动参数
        /// </summary>
        public string RunParam
        {
            get => runParam;
            set
            {
                runParam = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 文件列表
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

        /// <summary>
        /// 启动的程序名
        /// </summary>
        public string ApplicationName
        {
            get => applicationName;
            set
            {
                applicationName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 自述文件名
        /// </summary>
        public string ReadMeName
        {
            get => readMeName;
            set
            {
                readMeName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 卸载信息
        /// </summary>
        public string UninstallMsg
        {
            get => uninstallMsg;
            set
            {
                uninstallMsg = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 解除安装tips
        /// </summary>
        public string UninstallTip
        {
            get => uninstallTip;
            set
            {
                uninstallTip = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 启用进程检测
        /// </summary>
        public bool IsEnableProcess
        {
            get => isEnableProcess;
            set
            {
                isEnableProcess = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 进程提示
        /// </summary>
        public string UninstallProcessTips
        {
            get => uninstallProcessTips;
            set
            {
                uninstallProcessTips = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 卸载提示
        /// </summary>
        public string InstallProcessTips
        {
            get => installProcessTips;
            set
            {
                installProcessTips = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 进程名
        /// </summary>
        public string ProcessName
        {
            get => processName;
            set
            {
                processName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 展示自述
        /// </summary>
        public bool IsShowReadme
        {
            get => isShowReadme;
            set
            {
                isShowReadme = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 方法

        public override void RefreshData()
        {
            TargetFilesList = RefreshFileList();
            if (TargetFilesList == null)
                TargetFilesList = new List<string>();
            if (ProjectInfo != null && ProjectInfo.FinishInfo != null)
            {
                IsAutoRun = ProjectInfo.FinishInfo.IsAutoRun;
                IsShowReadme = ProjectInfo.FinishInfo.IsShowReadme;
                ApplicationName = TargetFilesList.Find(p => p == ProjectInfo.FinishInfo.ApplicationName) ?? "";
                ReadMeName = TargetFilesList.Find(p => p == ProjectInfo.FinishInfo.ReadMeFileName) ?? "";
                RunParam = ProjectInfo.FinishInfo.RunParam;
                if (!string.IsNullOrWhiteSpace(ProjectInfo.FinishInfo.UninstallMsg))
                    UninstallMsg = ProjectInfo.FinishInfo.UninstallMsg;
                if (!string.IsNullOrWhiteSpace(ProjectInfo.FinishInfo.UninstallTip))
                    UninstallTip = ProjectInfo.FinishInfo.UninstallTip;
                if (!string.IsNullOrWhiteSpace(ProjectInfo.FinishInfo.UninstallProcessTips))
                    UninstallProcessTips = ProjectInfo.FinishInfo.UninstallProcessTips;
                if (!string.IsNullOrWhiteSpace(ProjectInfo.FinishInfo.InstallProcessTips))
                    InstallProcessTips = ProjectInfo.FinishInfo.InstallProcessTips;
                if (!string.IsNullOrWhiteSpace(ProjectInfo.FinishInfo.ProcessName))
                    ProcessName = ProjectInfo.FinishInfo.ProcessName;
                IsEnableProcess = ProjectInfo.FinishInfo.IsEnableProcess;
            }
        }
        public override void Save()
        {
            if (ProjectInfo != null)
            {
                if (ProjectInfo.FinishInfo == null)
                    ProjectInfo.FinishInfo = new FinishModel();
                FinishModel finishModel = ProjectInfo.FinishInfo;
                finishModel.ReadMeFileName = ReadMeName;
                finishModel.RunParam = RunParam;
                finishModel.ApplicationName = ApplicationName;
                finishModel.IsAutoRun = IsAutoRun;
                finishModel.UninstallMsg = UninstallMsg;
                finishModel.UninstallTip = UninstallTip;
                finishModel.IsEnableProcess = IsEnableProcess;
                finishModel.UninstallProcessTips = UninstallProcessTips;
                finishModel.InstallProcessTips = InstallProcessTips;
                finishModel.ProcessName = ProcessName;
                finishModel.IsShowReadme = IsShowReadme;
                var path = Path.Combine(SavePath, "FinishInfo.json");
                File.WriteAllText(path, finishModel.SerializeObject());
            }
        }
        public void NavigateIn()
        {
            RefreshData();
        }

        public override bool ValidateData()
        {
            if ( string.IsNullOrWhiteSpace(ApplicationName))
            {
                TMessageBox.ShowMsg("结束页面：程序不能为空!");
                return false;
            }
            if (IsEnableProcess)
            {
                if (string.IsNullOrWhiteSpace(ProcessName))
                {
                    TMessageBox.ShowMsg("检测的进程名不能空!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(UninstallProcessTips))
                {
                    TMessageBox.ShowMsg("卸载提示不能为空!");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(UninstallProcessTips))
                {
                    TMessageBox.ShowMsg("安装提示不能为空!");
                    return false;
                }
            }
            if (IsShowReadme && string.IsNullOrWhiteSpace(ReadMeName))
            {
                TMessageBox.ShowMsg("自述文件不能为空!");
                return false;
            }
            return base.ValidateData();
        }

        #endregion
    }
}
