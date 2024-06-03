using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-24 23:34:39
    /// desc:AssemblyFileModel
    /// </summary>
    public class AssemblyFileModel : BaseModel
    {
        private DescModel<TargetDirType> targetPath;
        private string filePath;
        private int assemblyId;
        private int fileId;
        private bool isSelected;
        private bool isNeedQuietInstall;
        private bool isNeedInstall;
        private bool isExe;
        private bool isNoNeedCopy;
        private bool isExistNoNeedCopy;
        private bool isNoNeedDelete;

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 文件id
        /// </summary>
        public int FileId
        {
            get => fileId;
            set => fileId = value;
        }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public int AssemblyId
        {
            get => assemblyId;
            set
            {
                assemblyId = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                if (!string.IsNullOrEmpty(value) && (value.ToLower().EndsWith(".exe")|| value.ToLower().EndsWith(".bat")))
                {
                    IsExe = true;
                }
                else
                {
                    isExe = false;
                }
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 目标路径
        /// </summary>
        public DescModel<TargetDirType> TargetPath
        {
            get => targetPath;
            set
            {
                targetPath = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 子路径
        /// </summary>
        public string SubPath
        {
            get;
            set;
        }
        /// <summary>
        /// 安装
        /// </summary>
        public bool IsNeedInstall
        {
            get => isNeedInstall;
            set
            {
                isNeedInstall = value;
                if (value)
                {
                    IsNeedQuietInstall = false;
                }
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 静默安装
        /// </summary>
        public bool IsNeedQuietInstall
        {
            get => isNeedQuietInstall;
            set
            {
                isNeedQuietInstall = value;
                if (value)
                {
                    IsNeedInstall = false;
                }
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 是否可执行文件
        /// </summary>
        public bool IsExe
        {
            get => isExe;
            set
            {
                isExe = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 是否需要复制到指定目录
        /// </summary>
        public bool IsNoNeedCopy
        {
            get => isNoNeedCopy;
            set
            {
                isNoNeedCopy = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 是否是目录
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// 如果存在则不复制
        /// </summary>
        public bool IsExistNoNeedCopy
        {
            get => isExistNoNeedCopy;
            set
            {
                isExistNoNeedCopy = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 不用删除
        /// </summary>
        public bool IsNoNeedDelete
        {
            get => isNoNeedDelete;
            set
            {
                isNoNeedDelete = value;
                RaisePropertyChanged();
            }
        }
    }
}
