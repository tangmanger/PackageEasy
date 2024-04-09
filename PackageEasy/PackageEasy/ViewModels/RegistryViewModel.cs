using PackageEasy.Common;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain;
using PackageEasy.Domain.Common;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;
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
    /// time:2023-03-15 22:21:02
    /// desc:RegistryViewModel
    /// </summary>
    public class RegistryViewModel : BaseProjectViewModel
    {
        public RegistryViewModel(ViewType viewType, string key) : base(viewType, key)
        {

        }
        #region 属性

        private string registryFormat;
        private bool isAsSelected = true;
        private string processName;
        private string password;
        private bool isUsePassword;

        /// <summary>
        /// 文件格式
        /// </summary>
        public string RegistryFormat
        {
            get => registryFormat;
            set
            {
                registryFormat = value;
                OnPropertyChanged(nameof(RegistryFormat));
            }
        }

        /// <summary>
        /// 注册文件格式是否可以作为独立可选组件
        /// </summary>
        public bool IsAsSelected
        {
            get => isAsSelected;
            set
            {
                isAsSelected = value;
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
        /// 密码
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 是否使用密码
        /// </summary>

        public bool IsUsePassword
        {
            get => isUsePassword;
            set
            {
                isUsePassword = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 方法

        public override void Save()
        {
            RegistryModel registryModel = new RegistryModel();
            registryModel.RegistryFormat = RegistryFormat;
            registryModel.IsAsSelected = IsAsSelected;
            registryModel.ProcessName = ProcessName;
            registryModel.Password = Password;
            registryModel.IsUsePassword = IsUsePassword;
            var registryPath = Path.Combine(SavePath, "Registry.json");
            if (ProjectInfo == null)
                ProjectInfo = new ProjectInfoModel();
            ProjectInfo.Registry = registryModel;
            File.WriteAllText(registryPath, registryModel.SerializeObject());
        }

        public override void RefreshData()
        {
            if (ProjectInfo != null && ProjectInfo.Registry != null)
            {
                RegistryFormat = ProjectInfo.Registry.RegistryFormat;
                IsAsSelected = ProjectInfo.Registry.IsAsSelected;
                ProcessName = ProjectInfo.Registry.ProcessName;
                Password = ProjectInfo.Registry.Password;
                IsUsePassword = ProjectInfo.Registry.IsUsePassword;
            }
        }
        public override bool ValidateData()
        {
            if (!string.IsNullOrWhiteSpace(RegistryFormat) && string.IsNullOrWhiteSpace(ProcessName))
            {
                TMessageBox.ShowMsg(CommonSettings.RegistryRegistryFormatIsNotNull);
                return false;
            }
            if (IsUsePassword && string.IsNullOrWhiteSpace(Password))
            {
                TMessageBox.ShowMsg(CommonSettings.RegistryPasswordIsNotNull);
                return false;
            }
            return base.ValidateData();
        }

        #endregion
    }
}
