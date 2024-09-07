using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain.Common;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels
{
    public class LanguageViewModel : BaseProjectViewModel, INavigateIn
    {

        public LanguageViewModel(ViewType viewType, string key) : base(viewType, key)
        {
            if (MultiFileList == null)
                MultiFileList = new List<MultiFileModel>();
            //TargetDirList = new List<DescModel<TargetDirType>>();
            //foreach (var target in Enum.GetValues(typeof(TargetDirType)))
            //{
            //    TargetDirType targetDirType = (TargetDirType)target;
            //    TargetDirList.Add(new DescModel<TargetDirType>()
            //    {
            //        Data = targetDirType,
            //        DisplayName = $"${targetDirType.ToString()}",
            //        Description = $"${targetDirType.ToString()}"
            //    });
            //}
            Service.TargetPathChanged += Service_TargetPathChanged;
        }




        #region 属性


        private List<MultiFileModel> multiFileList;
        private List<InstallLanguageModel> installLangList;
        private List<AssemblyFileModel> fileList;
        private List<AssemblyFileModel> dirList;
        private List<TargetPathModel> targetDirList;


        /// <summary>
        /// 多文件操作
        /// </summary>
        public List<MultiFileModel> MultiFileList
        {
            get => multiFileList;
            set
            {
                multiFileList = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// 本地文件
        /// </summary>
        public List<InstallLanguageModel> InstallLangList
        {
            get => installLangList;
            set
            {
                installLangList = value;
                OnPropertyChanged();
            }
        }
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
        /// 目录地址
        /// </summary>
        public List<AssemblyFileModel> DirList
        {
            get => dirList;
            set
            {
                dirList = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 目标目录
        /// </summary>
        public List<TargetPathModel> TargetDirList
        {
            get => targetDirList;
            set
            {
                targetDirList = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region 方法

        public override void RefreshData()
        {
            if (ProjectInfo != null)
            {
                if (ProjectInfo.MultiFiles != null)
                    MultiFileList = ProjectInfo.MultiFiles;
                InitList();

            }
        }
        public override void Save()
        {
            var multiFilePath = Path.Combine(SavePath, "MultiFiles.json");
            ProjectInfo.MultiFiles = MultiFileList;
            if (ProjectInfo.MultiFiles == null)
            {
                ProjectInfo.MultiFiles = new List<MultiFileModel>();
            }
            File.WriteAllText(multiFilePath, ProjectInfo.MultiFiles.SerializeObject());
        }

        private void InitList()
        {
            if(ProjectInfo.TargetPaths==null)
                ProjectInfo.TargetPaths=StoreHelper.ReadLocalTargetFiles();
            TargetDirList = new List<TargetPathModel>(ProjectInfo.TargetPaths);
            if (TargetDirList == null)
                TargetDirList = StoreHelper.ReadLocalTargetFiles();
            if (ProjectInfo.BaseInfo != null)
                InstallLangList = ProjectInfo.BaseInfo.LanguageList;
            if (ProjectInfo.AssemblyInfo != null && ProjectInfo.AssemblyInfo.AssemblyList != null)
            {
                var allFiles = ProjectInfo.AssemblyInfo.AssemblyList.FindAll(p => p.FileList != null).SelectMany(c => c.FileList).ToList();
                FileList = allFiles.FindAll(c => c.IsDirectory == false).ToList();
                DirList = allFiles.FindAll(c => c.IsDirectory == true).ToList();
            }
            if (MultiFileList == null)
                MultiFileList = new List<MultiFileModel>();
            foreach (var item in MultiFileList)
            {
                if (item != null && InstallLangList != null && item.Lang != null)
                    item.Lang = InstallLangList?.Find(c => c?.LanguageType == item.Lang?.LanguageType) ?? InstallLangList.FirstOrDefault();
                if (item != null && FileList != null && item.AssemblyFile != null)
                    item.AssemblyFile = FileList?.Find(c => c?.FilePath == item.AssemblyFile?.FilePath) ?? FileList.FirstOrDefault();
                if (item != null && DirList != null && item.TargetDir != null)
                    item.TargetDir = DirList?.Find(c => c?.FilePath == item.TargetDir?.FilePath) ?? DirList.FirstOrDefault();
                if (item != null && TargetDirList != null && item.TargetPath != null)
                    item.TargetPath = TargetDirList.Find(c => c.DisplayName == item.TargetPath.DisplayName) ?? TargetDirList.FirstOrDefault();
            }
        }

        public void NavigateIn()
        {
            InitList();
        }

        public override bool ValidateData()
        {
            if (MultiFileList == null) return true;
            foreach (var item in MultiFileList)
            {
                if (item.Lang == null)
                {
                    TMessageBox.ShowMsg(CommonSettings.LangIsNotNull);
                    return false;
                }
                if (item.AssemblyFile == null)
                {
                    TMessageBox.ShowMsg(CommonSettings.LangAssemblyFileIsNotNull);
                    return false;
                }
                if (item.TargetPath == null)
                {
                    TMessageBox.ShowMsg(CommonSettings.LangTargetPathIsNotNull);
                    return false;
                }
                if (item.TargetDir == null)
                {
                    TMessageBox.ShowMsg(CommonSettings.LangTargetDirIsNotNull);
                    return false;
                }
            }
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            Service.TargetPathChanged -= Service_TargetPathChanged;
        }
        private void Service_TargetPathChanged()
        {
            InitList();
        }

        #endregion


        #region 命令

        /// <summary>
        /// 新增
        /// </summary>
        public RelayCommand AddCommand => new RelayCommand(() =>
        {
            if (MultiFileList == null)
                MultiFileList = new List<MultiFileModel>();

            MultiFileModel multiFileModel = new MultiFileModel();
            multiFileModel.AssemblyFile = FileList?.FirstOrDefault() ?? null;
            multiFileModel.Lang = InstallLangList?.FirstOrDefault() ?? null;
            multiFileModel.TargetDir = DirList?.FirstOrDefault() ?? null;
            multiFileModel.TargetPath = TargetDirList.FirstOrDefault() ?? null;
            MultiFileList.Add(multiFileModel);
            MultiFileList = new List<MultiFileModel>(MultiFileList);
        });


        /// <summary>
        /// 删除
        /// </summary>
        public RelayCommand DelCommand => new RelayCommand(() =>
        {
            if (MultiFileList == null || !MultiFileList.Exists(f => f.IsSelected))
            {
                TMessageBox.ShowMsg(CommonSettings.AppIconSelectDelItem);
                return;
            }
            var allSelected = MultiFileList.FindAll(c => c.IsSelected);
            if (allSelected != null)
            {
                foreach (var item in allSelected)
                {
                    MultiFileList.Remove(item);
                }
                MultiFileList = new List<MultiFileModel>(MultiFileList);
            }

        });



        #endregion
    }
}
