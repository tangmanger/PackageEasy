using CommunityToolkit.Mvvm.ComponentModel;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels.Dialogs
{
    public class AssemblyFilesViewModel : ObservableObject
    {
        private List<AssemblyFileModel> fileList;
        private List<DescModel<TargetDirType>> targetDirList;

        public AssemblyFilesViewModel(List<AssemblyFileModel> assemblyFiles, List<DescModel<TargetDirType>> targetDirList)
        {
            FileList = assemblyFiles;
            this.TargetDirList = targetDirList;
        }


        #region 属性

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<AssemblyFileModel> FileList
        {
            get => fileList;
            set
            {
                fileList = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 目标目录
        /// </summary>
        public List<DescModel<TargetDirType>> TargetDirList
        {
            get => targetDirList;
            set
            {
                targetDirList = value;
                OnPropertyChanged();
            }
        }


        #endregion
    }
}
