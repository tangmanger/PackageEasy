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

        #endregion

        #region 方法

        public override void Save()
        {
            RegistryModel registryModel = new RegistryModel();
            registryModel.RegistryFormat = RegistryFormat;
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
            }
        }

        #endregion
    }
}
