﻿using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Domain.Models.SaveModel;
using PackageEasy.Enums;
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

        #endregion

        #region 命令

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
            AssemblyList.Add(new AssemblyModel() { AssemblyId = id, AssemblyName = "新建组".GetLangText() });
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
                FileList = s.FileList;
        });
        /// <summary>
        /// 添加目录
        /// </summary>
        public RelayCommand AddDirCommand => new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(ProjectInfo.BaseInfo.WorkSpace))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", "工作目录为空!");
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", "请选择组!");
                return;
            }

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
            if (string.IsNullOrWhiteSpace(ProjectInfo.BaseInfo.WorkSpace))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", "工作目录为空!");
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", "请选择组!");
                return;
            }
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
                var files = GetFiles(folderBrowserDialog.SelectedPath);
                List<AssemblyFileModel> assemblyFileModels = currentAssembly.FileList;
                currentAssembly.SelectDir = folderBrowserDialog.SelectedPath;
                foreach (var file in files)
                {


                    AssemblyFileModel assemblyFileModel = new AssemblyFileModel();
                    assemblyFileModel.AssemblyId = currentAssembly.AssemblyId;
                    if (currentAssembly.SelectDir != ProjectInfo.BaseInfo.WorkSpace)
                    {
                        var path = file.Replace(currentAssembly.SelectDir, ProjectInfo.BaseInfo.WorkSpace);
                        FileInfo info = new FileInfo(path);
                        if (!Directory.Exists(info.DirectoryName))
                        {
                            if (info.DirectoryName != null)
                                Directory.CreateDirectory(info.DirectoryName);
                        }
                        File.Copy(file, path, true);
                        assemblyFileModel.FilePath = path.Replace(ProjectInfo.BaseInfo.WorkSpace, "");
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

            if (string.IsNullOrWhiteSpace(ProjectInfo.BaseInfo.WorkSpace))
            {
                Log.Write("工作目录为空!");
                TMessageBox.ShowMsg("", "工作目录为空!");
                return;
            }
            var currentAssembly = AssemblyList.Find(p => p.IsSelected == true);
            if (currentAssembly == null)
            {
                TMessageBox.ShowMsg("", "请选择组!");
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
                if (fileInfo.Directory.FullName != ProjectInfo.BaseInfo.WorkSpace)
                {
                    var path = fileInfo.FullName.Replace(fileInfo.Directory.FullName, ProjectInfo.BaseInfo.WorkSpace);
                    FileInfo info = new FileInfo(path);
                    if (!Directory.Exists(info.DirectoryName))
                    {
                        if (info.DirectoryName != null)
                            Directory.CreateDirectory(info.DirectoryName);
                    }
                    File.Copy(fileInfo.FullName, path, true);

                    assemblyFileModel.FilePath = path.Replace(ProjectInfo.BaseInfo.WorkSpace, "");

                }
                else
                {
                    assemblyFileModel.FilePath = fileInfo.FullName.Replace(ProjectInfo.BaseInfo.WorkSpace, "");
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
                TMessageBox.ShowMsg("当前没有文件列表!");
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
                            var path = ProjectInfo?.BaseInfo?.WorkSpace + file.FilePath;
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                string str = string.Format("当前文件{0}为空!".GetLangText(), file.FilePath);
                                TMessageBox.ShowMsg("", str);
                                return false;
                            }
                            if (file.IsDirectory == false && !File.Exists(path))
                            {
                                string str = string.Format("无法在工作空间找到文件{0}!".GetLangText(), file.FilePath);
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
