using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Enums;
using PackageEasy.Helpers;
using PackageEasy.Models;
using PackageEasy.NSIS;
using PackageEasy.Views.Dialogs;
using PackageEasy.Views.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 15:35:08
    /// desc:MainWindowViewModel
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataService Service;
        public MainViewModel(IDataService service)
        {
            Service = service;
            Service.CreateProject += Service_CreateProject;
        }


        #region 属性

        private string _title = "sssss";
        public string Title
        {
            get
            {
                return _title;

            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private FrameworkElement _workView;
        /// <summary>
        /// 返回工作界面
        /// </summary>
        public FrameworkElement WorkView
        {
            get
            {
                return _workView;
            }
            set
            {
                _workView = value;
                OnPropertyChanged();
            }
        }
        private bool _isActived = true;
        /// <summary>
        /// 是否是激活
        /// </summary>
        public bool IsActived
        {
            get
            {
                return _isActived;
            }
            set
            {
                _isActived = value;
                OnPropertyChanged();
            }
        }
        private List<TableModel> _tableList = new List<TableModel>();
        /// <summary>
        /// 通知
        /// </summary>
        public List<TableModel> TableList
        {
            get
            {
                return _tableList;
            }
            set
            {
                _tableList = value;
                OnPropertyChanged();
            }
        }
        private bool _isMenuCanEdit;

        /// <summary>
        /// 是否可以编辑
        /// </summary>
        public bool IsMenuCanEdit
        {
            get
            {
                return _isMenuCanEdit;

            }
            set
            {
                _isMenuCanEdit = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region 命令

        /// <summary>
        /// 返回首页
        /// </summary>
        public RelayCommand BackToHomeCommand => new RelayCommand(() =>
        {
            NavigationHelper.GoTo(ViewType.Home);
            IsActived = true;
            IsMenuCanEdit = false;
        });
        /// <summary>
        /// 切换
        /// </summary>
        public RelayCommand<TableModel> SwitchProjectCommand => new RelayCommand<TableModel>((t) =>
        {
            var projectKey = t.ProjectKey;
            if (CacheDataHelper.ProjectCahes.ContainsKey(projectKey))
            {
                WorkView = CacheDataHelper.ProjectCahes[projectKey].ProjectView;
                IsMenuCanEdit = true;
            }
        });
        /// <summary>
        /// 创建工程
        /// </summary>
        public RelayCommand NewProjectCommand => new RelayCommand(() =>
        {

            var projectViewModel = CreateProject();
            Service.OnCreateProject(projectViewModel.ProjectName, projectViewModel.Key);


        });
        /// <summary>
        /// 关闭标签页
        /// </summary>
        public RelayCommand CloseTableCommand => new RelayCommand(() =>
        {
            var table = TableList.Find(p => p.IsActive);
            if (table != null)
            {
                CloseCommand?.Execute(table);
                ServiceHelper.RemoveService(table.ProjectKey);
            }
        });
        /// <summary>
        /// 编译
        /// </summary>
        public RelayCommand CompileCommand => new RelayCommand(() =>
        {

            var table = TableList.Find(p => p.IsActive);
            if (table != null)
            {
                if (CacheDataHelper.ProjectCahes.ContainsKey(table.ProjectKey))
                {

                    var service = ServiceHelper.GetService(table.ProjectKey);
                    if (service != null)
                    {
                        service.OnPreCompile();
                    }
                    var viewCaheModel = CacheDataHelper.ProjectCahes[table.ProjectKey];
                    if (viewCaheModel != null)
                    {
                        ProjectViewModel? projectViewModel = viewCaheModel.BaseProjectViewModel as ProjectViewModel;
                        if (projectViewModel != null)
                        {
                            var result = FileHelper.Save(projectViewModel);
                            if (result)
                            {
                                table.ProjectName = projectViewModel.ProjectName;
                            }
                            if (!projectViewModel.ValidateData())
                            {
                                return;
                            }
                        }
                        NSISScript nSISScript = new NSISScript();
                        try
                        {


                            var list = nSISScript.Build(viewCaheModel.BaseProjectViewModel.ProjectInfo);
                            if (!string.IsNullOrWhiteSpace(viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.WorkSpace))
                            {
                                var path = Path.Combine(viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.WorkSpace, $"{viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.ApplicationName}.pgescript");
                                File.WriteAllLines(path, list, Encoding.Default);
                                if (File.Exists(path))
                                {
                                    var param = nSISScript.GetParams(path);
                                    Process process = new Process();

                                    process.StartInfo = new ProcessStartInfo()
                                    {
                                        Arguments = param,
                                        FileName = ConfigHelper.Config.NSISMakePath,
                                        UseShellExecute = false,
                                        CreateNoWindow = true,
                                        RedirectStandardOutput = true,
                                        RedirectStandardError = true,
                                    };
                                    bool flage = true;


                                    process.Exited += (s, e) =>
                                    {
                                        flage = false;
                                    };
                                    process.OutputDataReceived += (s, e) =>
                                    {
                                        Log.Write(e.Data ?? "");
                                        if (!string.IsNullOrWhiteSpace(e.Data) && e.Data.Contains(nSISScript.OutPutFilePath))
                                        {
                                            TMessageBox.MainShowMsg("", "编译成功!", MessageLevel.Information);
                                            if (File.Exists(nSISScript.OutPutFilePath))
                                            {
                                                FileInfo fileInfo = new FileInfo(nSISScript.OutPutFilePath);
                                                if (Directory.Exists(fileInfo.DirectoryName))
                                                {
                                                    if (fileInfo.Directory != null)
                                                    {
                                                        Process process1 = new Process();
                                                        process1.StartInfo = new ProcessStartInfo()
                                                        {
                                                            UseShellExecute = true,
                                                            FileName = fileInfo.Directory.FullName
                                                        };
                                                        process1.Start();
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            //TMessageBox.MainShowMsg("", "编译失败!", MessageLevel.Information);
                                        }
                                    };
                                    process.ErrorDataReceived += (s, e) =>
                                    {
                                        Log.Write(e.Data ?? "", LogLevelType.Error);

                                    };
                                    process.Start();
                                    //process.WaitForExit();
                                    //Log.Write($"{process.StandardOutput.ReadToEnd()}-{process.StandardError.ReadToEnd()}");
                                    Task.Run(() =>
                                    {
                                        while (true)
                                        {
                                            var proce = Process.GetProcessesByName(nSISScript.CompilerName);
                                            if (proce != null && proce.Length > 0)
                                            {
                                                process.BeginOutputReadLine();
                                                process.BeginErrorReadLine();
                                                break;
                                            }
                                            Thread.Sleep(100);
                                        }
                                    });

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TMessageBox.MainShowMsg("", "编译失败!", MessageLevel.Information);
                            Log.Write("编译失败!", ex);
                        }
                    }
                }
            }
        });



        /// <summary>
        /// 退出
        /// </summary>
        public RelayCommand ExitCommand => new RelayCommand(() =>
        {
            Environment.Exit(0);
        });
        /// <summary>
        /// 保存数据
        /// </summary>
        public RelayCommand SaveCommand => new RelayCommand(() =>
        {
            var table = TableList.Find(p => p.IsActive);
            if (table != null)
            {
                if (CacheDataHelper.ProjectCahes.ContainsKey(table.ProjectKey))
                {
                    var viewCaheModel = CacheDataHelper.ProjectCahes[table.ProjectKey];
                    if (viewCaheModel != null)
                    {
                        ProjectViewModel? projectViewModel = viewCaheModel.BaseProjectViewModel as ProjectViewModel;
                        if (projectViewModel != null)
                        {
                            var result = FileHelper.Save(projectViewModel);
                            if (result)
                            {
                                table.ProjectName = projectViewModel.ProjectName;
                                TMessageBox.ShowMsg("保存成功!");
                                SaveRecently(projectViewModel);
                            }
                            else
                            {
                                TMessageBox.ShowMsg("保存失败!");
                            }
                        }
                    }
                }
            }
        });

        /// <summary>
        /// 设置
        /// </summary>
        public RelayCommand SettingCommand => new RelayCommand(() =>
        {
            SettingControl settingControl = new SettingControl();
            ShowWindow showWindow = new ShowWindow(settingControl);
            showWindow.ShowDialog();

        });

        /// <summary>
        /// 插件
        /// </summary>
        public RelayCommand PlugInManagerCommand => new RelayCommand(() =>
        {

            PlugInManagerControl settingControl = new PlugInManagerControl();
            ShowWindow showWindow = new ShowWindow(settingControl);
            showWindow.ShowDialog();
        });

        public RelayCommand AboutCommand => new RelayCommand(() =>
        {
            AboutControl settingControl = new AboutControl();
            ShowWindow showWindow = new ShowWindow(settingControl);
            showWindow.ShowDialog();

        });


        /// <summary>
        /// 打开文件
        /// </summary>
        public RelayCommand OpenFileCommand => new RelayCommand(() =>
        {
            var projectViewModel = CreateProject();
            var result = FileHelper.OpenFile(projectViewModel);
            if (result != null && result.Item1)
            {
                projectViewModel.RefreshData();
                Service.OnCreateProject(projectViewModel.ProjectName, projectViewModel.Key);
                SaveRecently(projectViewModel);
            }
            else
            {
                if (result != null)
                    TMessageBox.ShowMsg("", result.Item2);
            }
        });

        /// <summary>
        /// 最近
        /// </summary>
        /// <param name="projectViewModel"></param>
        private void SaveRecently(ProjectViewModel projectViewModel)
        {
            string? base64Image = "";
            var iconPath = projectViewModel.ProjectInfo.BaseInfo.WorkSpace + projectViewModel.ProjectInfo.BaseInfo.InstallIconPath;
            if (!File.Exists(iconPath))
            {
                var info = Application.GetResourceStream(new Uri("/Resources/Images/tools.png", UriKind.Relative));
                using (var memoryStream = new MemoryStream())
                {
                    info.Stream.CopyTo(memoryStream);
                    using (Image image = Image.FromStream(memoryStream))
                    {
                        var tempImage = Path.Combine(DataHelper.Temp, Guid.NewGuid().ToString() + ".png");
                        image.Save(tempImage);
                        iconPath = tempImage;
                    }
                }
                base64Image = ImageHelper.ImageToBase64(iconPath);
            }
            else
            {
                var str = ImageHelper.IcoToPng(iconPath);
                if (str != null)
                    base64Image = ImageHelper.ImageToBase64(str);
            }

            CacheDataHelper.UpdateRecently(new RecentlyModel()
            {
                RecentlyName = projectViewModel.ProjectName,
                CreateTime = projectViewModel.ProjectInfo.ExtraInfo.CreateTime,
                Icon = base64Image ?? null,
                FilePath = projectViewModel.ProjectInfo.ExtraInfo.FilePath
            });
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public RelayCommand<TableModel> CloseCommand => new RelayCommand<TableModel>((t) =>
        {
            if (CacheDataHelper.ProjectCahes.ContainsKey(t.ProjectKey))
            {
                var viewCaheModel = CacheDataHelper.ProjectCahes[t.ProjectKey];
                if (viewCaheModel != null)
                {
                    ProjectViewModel? projectViewModel = viewCaheModel.BaseProjectViewModel as ProjectViewModel;
                    if (projectViewModel != null)
                    {
                        projectViewModel.Dispose();
                        CacheDataHelper.ProjectCahes.Remove(t.ProjectKey);
                        if (CacheDataHelper.FileOpenDic.ContainsKey(t.ProjectKey))
                            CacheDataHelper.FileOpenDic.Remove(t.ProjectKey);
                        ///找到当前索引
                        var index = TableList.FindIndex(p => p == t);
                        ///关掉最后一个标签，则将它的上个标签激活
                        ///如果不是最后一个标签，则将它的下个标签激活
                        var activiteId = index + 1;
                        if (activiteId >= TableList.Count)
                        {
                            activiteId = index - 1;
                        }
                        if (activiteId >= 0)
                        {
                            var table = TableList[activiteId];
                            if (t.IsActive)
                                table.IsActive = true;
                            SwitchProjectCommand?.Execute(table);
                        }
                        else
                        {
                            BackToHomeCommand?.Execute(null);
                        }
                        TableList.Remove(t);
                        TableList = new List<TableModel>(TableList);
                    }
                }
            }
        });

        /// <summary>
        /// 加载
        /// </summary>
        public RelayCommand LoadCommand => new RelayCommand(Loaded);


        #endregion

        #region 方法

        public void Loaded()
        {
            //if (string.IsNullOrWhiteSpace(ConfigHelper.Config.WorkSpace) || File.Exists(ConfigHelper.Config.WorkSpace))
            //{
            //    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            //    folderBrowserDialog.UseDescriptionForTitle = true;
            //    folderBrowserDialog.Description = "选择工作路径".GetLangText();
            //    var result = folderBrowserDialog.ShowDialog();
            //    if (result == DialogResult.OK || result == DialogResult.Yes)
            //    {
            //        if (!Directory.Exists(folderBrowserDialog.SelectedPath))
            //        {
            //            Directory.CreateDirectory(folderBrowserDialog.SelectedPath);
            //        }
            //        ConfigHelper.Config.WorkSpace = folderBrowserDialog.SelectedPath;
            //        ConfigHelper.Save(true);
            //    }
            //    else
            //    {
            //        Environment.Exit(0);
            //    }
            //}
        }
        /// <summary>
        /// 返回主页
        /// </summary>
        /// <param name="viewType"></param>
        public void GoToView(ViewType viewType)
        {

        }
        private void Service_CreateProject(string projectName, string projectKey)
        {
            var table = new TableModel() { ProjectName = projectName, ProjectKey = projectKey };
            table.IsActive = true;
            TableList.Add(table);
            TableList = new List<TableModel>(TableList);

            IsActived = false;
            foreach (var item in TableList)
            {
                if (item.ProjectKey == projectKey)
                {
                    item.IsActive = true;
                }
                else
                {
                    item.IsActive = false;
                }
            }
            //Log.Write(projectName + "----------------" + projectKey);
            if (CacheDataHelper.ProjectCahes.ContainsKey(projectKey))
            {
                WorkView = CacheDataHelper.ProjectCahes[projectKey].ProjectView;
                IsMenuCanEdit = true;
            }

        }
        void TableChanged(ViewCaheModel viewCaheModel)
        {

        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenFile(string fileName)
        {
            var projectViewModel = CreateProject();
            var result = FileHelper.Open(fileName, projectViewModel);
            if (result != null)
            {
                if (result.Item1 == true)
                {
                    projectViewModel.RefreshData();
                    Log.Write($"{fileName}---------------------------->{projectViewModel.Key}");
                    Service.OnCreateProject(projectViewModel.ProjectName, projectViewModel.Key);
                    SaveRecently(projectViewModel);
                }
                else
                {
                    TMessageBox.ShowMsg("", result.Item2);
                }
            }
            else
            {
                TMessageBox.ShowMsg("", "打开文件发生错误");
            }
        }

        #endregion
    }
}
