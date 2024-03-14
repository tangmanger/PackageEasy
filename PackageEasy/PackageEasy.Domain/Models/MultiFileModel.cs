using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// 多文件操作
    /// </summary>
    public class MultiFileModel : BaseModel
    {
        private bool isSelected;
        private InstallLanguageModel lang;
        private AssemblyFileModel assemblyFile;
        private AssemblyFileModel targetDir;
        private DescModel<TargetDirType> targetPath;

        /// <summary>
        /// 选择
        /// </summary>
        [JsonIgnore]
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
        /// 多语言标志
        /// </summary>
        public InstallLanguageModel Lang
        {
            get => lang;

            set
            {
                lang = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 程序集文件
        /// </summary>
        public AssemblyFileModel AssemblyFile
        {
            get => assemblyFile;
            set
            {
                assemblyFile = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 目标目录
        /// </summary>
        public AssemblyFileModel TargetDir
        {
            get => targetDir;
            set
            {
                targetDir = value;
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
    }
}
