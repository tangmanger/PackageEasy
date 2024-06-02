using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain;
using PackageEasy.Domain.Common;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Domain.Models.SaveModel;
using PackageEasy.Enums;
using PackageEasy.ViewModels.Dialogs;
using PackageEasy.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-12 22:49:29
    /// desc:AssemblyViewModel
    /// </summary>
    public class AssemblyViewModel : BaseProjectViewModel
    {


        public AssemblyViewModel() { }
        public AssemblyViewModel(ViewType viewType, string key) : base(viewType, key)
        {
            AssemblyList = new List<AssemblyModel>();
            TargetDirList = new List<DescModel<TargetDirType>>();
            foreach (var target in Enum.GetValues(typeof(TargetDirType)))
            {
                TargetDirType targetDirType = (TargetDirType)target;
                TargetDirList.Add(new DescModel<TargetDirType>()
                {
                    Data = targetDirType,
                    DisplayName = $"${targetDirType.ToString()}",
                    Description = $"${targetDirType.ToString()}"
                });
            }
            assemblyInfoModel.AssemblyList = AssemblyList;
            ProjectInfo.AssemblyInfo = assemblyInfoModel;
        }


        #region 属性

        AssemblyInfoModel assemblyInfoModel = new AssemblyInfoModel();

        List<AssemblyFileModel> allFileList = new List<AssemblyFileModel>();
        private List<DescModel<TargetDirType>> targetDirList;
        private List<AssemblyModel> assemblyList;
        private List<AssemblyFileModel> fileList;
        private bool isAllowChoose;
        private bool isShow;
        private bool isAllChecked;
        private bool isExe;
        private bool install;
        private bool quietInstall;
        private bool isExistNoNeedCopy;
        private bool isNoNeedCopy;
        private bool isNoNeedDelete;
        private List<AssemblyFileModel> ignoreFileList;

        /// <summary>
        /// 组件信息
        /// </summary>
        public List<AssemblyModel> AssemblyList
        {
            get => assemblyList;
            set
            {
                assemblyList = value;
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
                if (value != null)
                {
                    IsAllChecked = !value.Exists(f => f.IsSelected == false);
                }
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
        /// <summary>
        /// 允许用户选择组件
        /// </summary>
        public bool IsAllowChoose
        {
            get => isAllowChoose;
            set
            {
                isAllowChoose = value;
                OnPropertyChanged();
            }
        }

        public bool IsShow
        {
            get => isShow;
            set
            {
                isShow = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        public bool IsAllChecked
        {
            get => isAllChecked;
            set
            {
                isAllChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否是exe
        /// </summary>
        public bool IsExe
        {
            get => isExe;
            set
            {
                isExe = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 安装
        /// </summary>
        public bool Install
        {
            get => install;
            set
            {
                install = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 静默安装
        /// </summary>
        public bool QuietInstall
        {
            get => quietInstall;
            set
            {
                quietInstall = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 存在则不复制
        /// </summary>
        public bool IsExistNoNeedCopy
        {
            get => isExistNoNeedCopy;
            set
            {
                isExistNoNeedCopy = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 不复制
        /// </summary>
        public bool IsNoNeedCopy
        {
            get => isNoNeedCopy;
            set
            {
                isNoNeedCopy = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 卸载后不删除
        /// </summary>
        public bool IsNoNeedDelete
        {
            get => isNoNeedDelete;
            set
            {
                isNoNeedDelete = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 忽略文件列表
        /// </summary>
        public List<AssemblyFileModel> IgnoreFileList
        {
            get => ignoreFileList;
            set
            {
                ignoreFileList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 命令

        /// <summary>
        /// 全选
        /// </summary>

        public RelayCommand FileAllCheckCommand => new RelayCommand(() =>
        {
            if (FileList != null)
            {
                foreach (var file in FileList)
                {
                    file.IsSelected = IsAllChecked;
                }
            }

        });

        /// <summary>
        /// 单个文件选择
        /// </summary>
        public RelayCommand FileCheckCommand => new RelayCommand(() =>
        {

            if (FileList != null)
            {
                IsAllChecked = !FileList.Exists(c => c.IsSelected == false);
            }
        });

        public RelayCommand AssemblyMenuCommand => new RelayCommand(() =>
        {
            IsShow = !IsShow;

        });
        /// <summary>
        /// 添加group
        /// </summary>
        public RelayCommand AddGroupCommand => new RelayCommand(() =>
        {
            int id = 0;
            if (AssemblyList.Count > 0)
            {
                id = AssemblyList.Max(c => c.AssemblyId) + 1;
            }
            AssemblyList.Add(new AssemblyModel() { AssemblyId = id, IsAutoSelected = true, AssemblyName = CommonSettings.AssemblyNewSection.GetLangText() });
            AssemblyList = new List<AssemblyModel>(AssemblyList);
        });

        /// <summary>
        /// 删除组
        /// </summary>
        public RelayCommand<AssemblyModel> DelGroupCommand => new RelayCommand<AssemblyModel>((s) =>
        {
            if (AssemblyList.Exists(g => g == s))
                AssemblyList.Remove(s);
            AssemblyList = new List<AssemblyModel>(AssemblyList);
            FileList = new List<AssemblyFileModel>();
        });
        /// <summary>
        /// 组选择
        /// </summary>
        public RelayCommand AssemblySelectedCommand => new RelayCommand(() =>
        {
            var s = AssemblyList.Find(p => p.IsSelected);
            if (s != null)
            {
                FileList = s.FileList;
                IgnoreFileList = s.IgnoreFileList;
            }
        });


        public RelayCommand<AssemblyModel> ShowDetailCommand => new RelayCommand<AssemblyModel>((s) =>
        {
            AssemblyFilesDialog assemblyFiles = new AssemblyFilesDialog();
            AssemblyFilesViewModel assemblyFileModel = new AssemblyFilesViewModel(s.FileList, TargetDirList);
            assemblyFiles.DataContext = assemblyFileModel;
            assemblyFiles.ShowDialog();

        });

        /// <summary>
        /// 添加目录
        /// </summary>
        public RelayCommand AddDirCommand1 => new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(ProjectInfo.GetWorkSpace()))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", CommonSettings.AssemblyWorkSpaceNotNull);
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", CommonSettings.AssemblyChooseSection);
                return;
            }
            if (currentAssembly.IgnoreFileList == null)
                currentAssembly.IgnoreFileList = new List<AssemblyFileModel>();
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (!Directory.Exists(folderBrowserDialog.SelectedPath))
                {
                    return;
                }
                var dirs = GetDirs(folderBrowserDialog.SelectedPath);
                List<AssemblyFileModel> assemblyFileModels = currentAssembly.FileList;
                DirectoryInfo rootInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                currentAssembly.SelectDir = rootInfo.Parent?.FullName;
                foreach (var file in dirs)
                {
                    if (currentAssembly.SelectDir == file) { continue; }
                    AssemblyFileModel assemblyFileModel = new AssemblyFileModel();
                    assemblyFileModel.AssemblyId = currentAssembly.AssemblyId;
                    DirectoryInfo fileInfo = new DirectoryInfo(file);
                    assemblyFileModel.SubPath = fileInfo?.FullName?.Replace(currentAssembly.SelectDir, "") ?? "";
                    assemblyFileModel.FilePath = assemblyFileModel.SubPath;
                    assemblyFileModel.IsDirectory = true;
                    assemblyFileModel.TargetPath = TargetDirList.FirstOrDefault() ?? new DescModel<TargetDirType>() { Data = TargetDirType.INSTDIR };
                    var current = assemblyFileModels.Find(p => p.FilePath == assemblyFileModel.FilePath);
                    if (current != null)
                    {
                        continue;
                        assemblyFileModels.Remove(current);
                    }
                    if (currentAssembly.IgnoreFileList.Exists(c => c.FilePath == assemblyFileModel.FilePath))
                        continue;
                    assemblyFileModels.Add(assemblyFileModel);
                }
            }
            if (currentAssembly.FileList == null)
                currentAssembly.FileList = new List<AssemblyFileModel>();
            FileList = new List<AssemblyFileModel>(currentAssembly.FileList);
        });

        /// <summary>
        /// 添加目录
        /// </summary>
        public RelayCommand AddDirCommand => new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(ProjectInfo.GetWorkSpace()))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", CommonSettings.AssemblyWorkSpaceNotNull);
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", CommonSettings.AssemblyChooseSection);
                return;
            }
            if (currentAssembly.IgnoreFileList == null)
                currentAssembly.IgnoreFileList = new List<AssemblyFileModel>();
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {

                if (!Directory.Exists(folderBrowserDialog.SelectedPath))
                {
                    return;
                }
                if (currentAssembly.FileList == null)
                    currentAssembly.FileList = new List<AssemblyFileModel>();
                //获取目录
                var dirs = GetDirs(folderBrowserDialog.SelectedPath);
                List<AssemblyFileModel> assemblyFileModels = currentAssembly.FileList;
                DirectoryInfo rootInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                currentAssembly.SelectDir = folderBrowserDialog.SelectedPath;
                foreach (var file in dirs)
                {
                    if (currentAssembly.SelectDir == file) { continue; }
                    AssemblyFileModel assemblyFileModel = new AssemblyFileModel();
                    assemblyFileModel.AssemblyId = currentAssembly.AssemblyId;
                    DirectoryInfo fileInfo = new DirectoryInfo(file);
                    assemblyFileModel.SubPath = fileInfo?.FullName?.Replace(currentAssembly.SelectDir, "") ?? "";
                    assemblyFileModel.FilePath = assemblyFileModel.SubPath;
                    assemblyFileModel.IsDirectory = true;
                    assemblyFileModel.TargetPath = TargetDirList.FirstOrDefault() ?? new DescModel<TargetDirType>() { Data = TargetDirType.INSTDIR };
                    var current = assemblyFileModels.Find(p => p.FilePath == assemblyFileModel.FilePath);
                    if (current != null)
                    {
                        continue;
                        assemblyFileModels.Remove(current);
                    }
                    assemblyFileModels.Add(assemblyFileModel);
                }

                var files = GetFiles(folderBrowserDialog.SelectedPath);
                currentAssembly.SelectDir = folderBrowserDialog.SelectedPath;
                foreach (var file in files)
                {


                    AssemblyFileModel assemblyFileModel = new AssemblyFileModel();
                    assemblyFileModel.AssemblyId = currentAssembly.AssemblyId;
                    if (currentAssembly.SelectDir != ProjectInfo.GetWorkSpace())
                    {
                        var path = file.Replace(currentAssembly.SelectDir, ProjectInfo.GetWorkSpace());
                        FileInfo info = new FileInfo(path);
                        if (!Directory.Exists(info.DirectoryName))
                        {
                            if (info.DirectoryName != null)
                                Directory.CreateDirectory(info.DirectoryName);
                        }
                        File.Copy(file, path, true);
                        assemblyFileModel.FilePath = path.Replace(ProjectInfo.GetWorkSpace(), "");
                    }
                    else
                    {
                        assemblyFileModel.FilePath = file.Replace(currentAssembly.SelectDir, "");
                    }
                    FileInfo fileInfo = new FileInfo(file);
                    assemblyFileModel.SubPath = fileInfo?.DirectoryName?.Replace(currentAssembly.SelectDir, "") ?? "";
                    assemblyFileModel.TargetPath = TargetDirList.FirstOrDefault() ?? new DescModel<TargetDirType>() { Data = TargetDirType.INSTDIR };
                    var current = assemblyFileModels.Find(p => p.FilePath == assemblyFileModel.FilePath);
                    if (current != null)
                    {
                        continue;
                        assemblyFileModels.Remove(current);
                    }
                    if (currentAssembly.IgnoreFileList.Exists(c => c.FilePath == assemblyFileModel.FilePath))
                        continue;
                    assemblyFileModels.Add(assemblyFileModel);
                }
            }
            if (currentAssembly.FileList == null)
                currentAssembly.FileList = new List<AssemblyFileModel>();
            FileList = new List<AssemblyFileModel>(currentAssembly.FileList);
        });
        /// <summary>
        /// 添加文件夹
        /// </summary>
        public RelayCommand AddFileByDirCommand => new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(ProjectInfo.GetWorkSpace()))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", CommonSettings.AssemblyWorkSpaceNotNull);
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", CommonSettings.AssemblyChooseSection);
                return;
            }
            if (currentAssembly.IgnoreFileList == null)
                currentAssembly.IgnoreFileList = new List<AssemblyFileModel>();
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                if (!Directory.Exists(folderBrowserDialog.SelectedPath))
                {
                    return;
                }
                if (currentAssembly.FileList == null)
                    currentAssembly.FileList = new List<AssemblyFileModel>();
                if (currentAssembly.IgnoreFileList == null)
                    currentAssembly.IgnoreFileList = new List<AssemblyFileModel>();
                var files = GetFiles(folderBrowserDialog.SelectedPath);
                List<AssemblyFileModel> assemblyFileModels = currentAssembly.FileList;
                currentAssembly.SelectDir = folderBrowserDialog.SelectedPath;
                foreach (var file in files)
                {


                    AssemblyFileModel assemblyFileModel = new AssemblyFileModel();
                    assemblyFileModel.AssemblyId = currentAssembly.AssemblyId;
                    if (currentAssembly.SelectDir != ProjectInfo.GetWorkSpace())
                    {
                        var path = file.Replace(currentAssembly.SelectDir, ProjectInfo.GetWorkSpace());
                        FileInfo info = new FileInfo(path);
                        if (!Directory.Exists(info.DirectoryName))
                        {
                            if (info.DirectoryName != null)
                                Directory.CreateDirectory(info.DirectoryName);
                        }
                        File.Copy(file, path, true);
                        assemblyFileModel.FilePath = path.Replace(ProjectInfo.GetWorkSpace(), "");
                    }
                    else
                    {
                        assemblyFileModel.FilePath = file.Replace(currentAssembly.SelectDir, "");
                    }
                    FileInfo fileInfo = new FileInfo(file);
                    assemblyFileModel.SubPath = fileInfo?.DirectoryName?.Replace(currentAssembly.SelectDir, "") ?? "";
                    assemblyFileModel.TargetPath = TargetDirList.FirstOrDefault() ?? new DescModel<TargetDirType>() { Data = TargetDirType.INSTDIR };
                    var current = assemblyFileModels.Find(p => p.FilePath == assemblyFileModel.FilePath);
                    if (current != null)
                    {
                        continue;
                        assemblyFileModels.Remove(current);
                    }
                    if (currentAssembly.IgnoreFileList.Exists(c => c.FilePath == assemblyFileModel.FilePath))
                        continue;
                    assemblyFileModels.Add(assemblyFileModel);
                }


                if (currentAssembly.FileList == null)
                    currentAssembly.FileList = new List<AssemblyFileModel>();
                FileList = new List<AssemblyFileModel>(currentAssembly.FileList);
            }
        });
        /// <summary>
        /// 添加文件
        /// </summary>
        public RelayCommand AddFileCommand => new RelayCommand(() =>
        {

            if (string.IsNullOrWhiteSpace(ProjectInfo.GetWorkSpace()))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", CommonSettings.AssemblyWorkSpaceNotNull);
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", CommonSettings.AssemblyChooseSection);
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {

                if (currentAssembly.FileList == null)
                    currentAssembly.FileList = new List<AssemblyFileModel>();
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                AssemblyFileModel assemblyFileModel = new AssemblyFileModel();

                assemblyFileModel.AssemblyId = currentAssembly.AssemblyId;
                if (fileInfo.Directory.FullName != ProjectInfo.GetWorkSpace())
                {
                    var path = fileInfo.FullName.Replace(fileInfo.Directory.FullName, ProjectInfo.GetWorkSpace());
                    FileInfo info = new FileInfo(path);
                    if (!Directory.Exists(info.DirectoryName))
                    {
                        if (info.DirectoryName != null)
                            Directory.CreateDirectory(info.DirectoryName);
                    }
                    File.Copy(fileInfo.FullName, path, true);

                    assemblyFileModel.FilePath = path.Replace(ProjectInfo.GetWorkSpace(), "");

                }
                else
                {
                    assemblyFileModel.FilePath = fileInfo.FullName.Replace(ProjectInfo.GetWorkSpace(), "");
                }
                assemblyFileModel.SubPath = fileInfo?.DirectoryName?.Replace(fileInfo.Directory.FullName, "") ?? "";
                assemblyFileModel.TargetPath = TargetDirList.FirstOrDefault() ?? new DescModel<TargetDirType>() { Data = TargetDirType.INSTDIR };
                currentAssembly.FileList.Add(assemblyFileModel);
                if (currentAssembly.FileList == null)
                    currentAssembly.FileList = new List<AssemblyFileModel>();
                FileList = new List<AssemblyFileModel>(currentAssembly.FileList);

            }

        });
        /// <summary>
        /// 删除文件
        /// </summary>
        public RelayCommand DeletedCommand => new RelayCommand(() =>
        {
            if (FileList == null || FileList.Count == 0)
            {
                TMessageBox.ShowMsg(CommonSettings.AssemblyNullFileList);
                return;
            }
            List<AssemblyFileModel> files = new List<AssemblyFileModel>();
            foreach (var item in FileList)
            {
                if (item.IsSelected)
                    files.Add(item);
            }
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    FileList.Remove(file);
                }
            }
            FileList = new List<AssemblyFileModel>(FileList);
            var s = AssemblyList.Find(p => p.IsSelected);
            if (s != null)
            {
                s.FileList = FileList;
            }
        });

        /// <summary>
        /// 打开菜单
        /// </summary>
        public RelayCommand OpenMenuCommand => new RelayCommand(() =>
        {
            if (FileList != null)
            {
                var selected = FileList.FindAll(f => f.IsSelected);
                if (selected != null && selected.Count > 0)
                {
                    IsExe = !selected.Exists(c => c.IsExe == false);
                    Install = !selected.Exists(c => c.IsNeedInstall == false);
                    QuietInstall = !selected.Exists(c => c.IsNeedQuietInstall == false);
                    IsExistNoNeedCopy = !selected.Exists(c => c.IsExistNoNeedCopy == false);
                    IsNoNeedCopy = !selected.Exists(c => c.IsNoNeedCopy == false);
                    IsNoNeedDelete = !selected.Exists(c => c.IsNoNeedDelete == false);
                    return;
                }

            }
            IsExe = false;
            Install = false;
            QuietInstall = false;
            IsExistNoNeedCopy = false;
            IsNoNeedDelete = false;
            IsNoNeedCopy = false;
        });

        /// <summary>
        /// 设置安装方式
        /// </summary>
        public RelayCommand<FileMenuOperateType> SetMenuCommand => new RelayCommand<FileMenuOperateType>((s) =>
        {
            if (FileList != null)
            {
                var selected = FileList.FindAll(f => f.IsSelected);
                if (selected == null || selected.Count == 0) return;
                if (s == FileMenuOperateType.Ignore)
                {
                    var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
                    if (currentAssembly == null) return;
                    if (currentAssembly.IgnoreFileList == null)
                        currentAssembly.IgnoreFileList = new List<AssemblyFileModel>();
                    foreach (var item in selected)
                    {
                        if (currentAssembly.IgnoreFileList.Exists(c => c.FilePath == item.FilePath))
                        {
                            continue;
                        }
                        currentAssembly.IgnoreFileList.Add(item);
                    }
                    foreach (var item in selected)
                    {
                        currentAssembly.FileList.Remove(item);
                    }
                    FileList = new List<AssemblyFileModel>(currentAssembly.FileList);
                    IgnoreFileList = new List<AssemblyFileModel>(currentAssembly.IgnoreFileList);
                    TMessageBox.ShowMsg(CommonSettings.AssemblyIgnoreSuccess);
                    return;
                }
                foreach (var item in selected)
                {
                    switch (s)
                    {
                        case FileMenuOperateType.Install:
                            item.IsNeedInstall = !item.IsNeedInstall;
                            item.IsNeedQuietInstall = false;
                            break;
                        case FileMenuOperateType.QuietInstall:
                            item.IsNeedInstall = false;
                            item.IsNeedQuietInstall = !item.IsNeedQuietInstall;
                            break;
                        case FileMenuOperateType.IsExistNoNeedCopy:
                            item.IsExistNoNeedCopy = !item.IsExistNoNeedCopy;
                            item.IsNoNeedCopy = false;

                            break;
                        case FileMenuOperateType.IsNoNeedCopy:
                            item.IsExistNoNeedCopy = false;
                            item.IsNoNeedCopy = !item.IsNoNeedCopy;
                            break;
                        case FileMenuOperateType.IsNoNeedDelete:
                            item.IsNoNeedDelete = !item.IsNoNeedDelete;
                            break;
                        default:
                            break;
                    }

                }
            }
        });

        /// <summary>
        /// 设置选中
        /// </summary>
        public RelayCommand<AssemblyModel> SetAssemblyMenuCommand => new RelayCommand<AssemblyModel>((a) =>
        {
            a.IsAutoSelected = !a.IsAutoSelected;
        });

        /// <summary>
        /// 设置忽略
        /// </summary>
        public RelayCommand<AssemblyFileModel> SetUnIgnoreCommand => new RelayCommand<AssemblyFileModel>((s) =>
        {
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null) return;
            if (currentAssembly.IgnoreFileList == null)
                currentAssembly.IgnoreFileList = new List<AssemblyFileModel>();
            currentAssembly.IgnoreFileList.Remove(s);
            currentAssembly.FileList.Add(s);

            FileList = new List<AssemblyFileModel>(currentAssembly.FileList);
            IgnoreFileList = new List<AssemblyFileModel>(currentAssembly.IgnoreFileList);
            TMessageBox.ShowMsg(CommonSettings.AssemblyIgnoreSuccess);
        });


        #endregion

        #region 方法

        public override void NavigateOut()
        {
            base.NavigateOut();
            assemblyInfoModel.AssemblyList = AssemblyList;
            assemblyInfoModel.IsAllowChoose = IsAllowChoose;
        }
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public List<string> GetFiles(string dirPath)
        {
            List<string> filesList = new List<string>();
            var files = Directory.GetFiles(dirPath);
            if (files != null && files.Length > 0)
                filesList.AddRange(files.ToList());
            var dirs = Directory.GetDirectories(dirPath);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (var dir in dirs)
                {
                    var result = GetFiles(dir);
                    filesList.AddRange(result);
                }
            }
            return filesList;
        }
        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public List<string> GetDirs(string dirPath)
        {
            List<string> dirs = new List<string>();
            dirs.Add(dirPath);
            var childDir = Directory.GetDirectories(dirPath);
            if (childDir != null)
            {
                foreach (var item in childDir)
                {
                    var ss = GetDirs(item);
                    dirs.AddRange(ss);
                }
            }
            return dirs;
        }
        public override void RefreshData()
        {
            base.RefreshData();
            assemblyInfoModel = ProjectInfo.AssemblyInfo;
            if (assemblyInfoModel == null)
                assemblyInfoModel = new AssemblyInfoModel();
            AssemblyList = assemblyInfoModel.AssemblyList ?? new List<AssemblyModel>();
            foreach (var assitem in AssemblyList)
            {
                assitem.IsSelected = false;
                if (assitem.FileList != null)
                {
                    foreach (var file in assitem.FileList)
                    {
                        file.TargetPath = TargetDirList.Find(p => p.Data == file.TargetPath.Data);
                    }
                }
                if (assitem.IgnoreFileList != null)
                {
                    foreach (var file in assitem.IgnoreFileList)
                    {
                        file.TargetPath = TargetDirList.Find(p => p.Data == file.TargetPath.Data);
                    }
                }
            }

            var first = AssemblyList.FirstOrDefault();
            if (first != null)
            {
                first.IsSelected = true;
                AssemblySelectedCommand?.Execute(null);
            }
            IsAllowChoose = assemblyInfoModel.IsAllowChoose;
        }
        public override void Save()
        {
            assemblyInfoModel.AssemblyList = AssemblyList;
            assemblyInfoModel.IsAllowChoose = IsAllowChoose;
            var assemblyPath = Path.Combine(SavePath, "AssemblyInfo.json");
            File.WriteAllText(assemblyPath, assemblyInfoModel.SerializeObject());
        }

        public override bool ValidateData()
        {
            if (AssemblyList != null && AssemblyList.Count > 0)
            {

                foreach (var assembly in AssemblyList)
                {
                    if (assembly.FileList != null)
                    {
                        foreach (var file in assembly.FileList)
                        {
                            var path = ProjectInfo.GetWorkSpace() + file.FilePath;
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                string str = string.Format(CommonSettings.AssemblyFileIsNull.GetLangText(), file.FilePath);
                                TMessageBox.ShowMsg("", str);
                                return false;
                            }
                            if (file.IsDirectory == false && !File.Exists(path))
                            {
                                string str = string.Format(CommonSettings.AssemblyFileNotExist.GetLangText(), file.FilePath);
                                TMessageBox.ShowMsg("", str);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
