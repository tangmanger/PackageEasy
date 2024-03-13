using PackageEasy.Common;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain;
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

        #endregion

        #region 方法

        public override void Save()
        {
            RegistryModel registryModel = new RegistryModel();
            registryModel.RegistryFormat = RegistryFormat;
            registryModel.IsAsSelected = IsAsSelected;
            registryModel.ProcessName = ProcessName;
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
            }
        }
        public override bool ValidateData()
        {
            if (!string.IsNullOrWhiteSpace(RegistryFormat) && string.IsNullOrWhiteSpace(ProcessName))
            {
                TMessageBox.ShowMsg("请填写关联的程序名称!");
                return false;
            }
            return base.ValidateData();
        }

        #endregion
    }
}
