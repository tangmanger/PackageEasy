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
using System.Windows.Documents;
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
        /// 导入脚本
        /// </summary>
        public RelayCommand ImportScriptCommand => new RelayCommand(() =>
        {

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".pgescript"; // Default file extension
            openFileDialog.Filter = "pgescript documents|*.pgescript"; // Filter files by extension
            var result = openFileDialog.ShowDialog();
            ///用户点了关闭按钮,未选择路径
            if (result != true)
            {
                return;
            }
            var dirPath = openFileDialog.FileName;
            if (File.Exists(dirPath))
            {
                ScriptControl scriptControl = new ScriptControl(dirPath);
                ShowWindow showWindow = new ShowWindow(scriptControl);
                showWindow.ShowDialog();
            }

        });
        /// <summary>
        /// 导出脚本
        /// </summary>
        public RelayCommand ExportScriptCommand => new RelayCommand(async () =>
        {
            var table = TableList.Find(p => p.IsActive);
            if (table != null)
            {
                if (CacheDataHelper.ProjectCahes.ContainsKey(table.ProjectKey))
                {
                    var messageResult = TMessageBox.ShowMsg("是否转换成相对路径?", MessageLevel.YesNoCancel);
                    if (messageResult == TMessageBoxResult.Close) return;
                    var service = ServiceHelper.GetService(table.ProjectKey);
                    if (service != null)
                    {
                        service.OnPreCompile();
                    }
                    var viewCaheModel = CacheDataHelper.ProjectCahes[table.ProjectKey];
                    if (viewCaheModel != null)
                    {
                        string dirPath = "";
                        ProjectViewModel? projectViewModel = viewCaheModel.BaseProjectViewModel as ProjectViewModel;
                        if (projectViewModel != null)
                        {
                            Microsoft.Win32.SaveFileDialog openFileDialog = new Microsoft.Win32.SaveFileDialog();
                            openFileDialog.FileName = $"{projectViewModel.ProjectName.Replace("*", "")}.pgescript"; // Default file name
                            openFileDialog.DefaultExt = ".pgescript"; // Default file extension
                            openFileDialog.Filter = "script documents|*.pgescript"; // Filter files by extension
                            ///用户点了关闭按钮,未选择路径
                            if (openFileDialog.ShowDialog() == false)
                            {
                                return;
                            }
                            dirPath = openFileDialog.FileName;
                            var result = FileHelper.Save(projectViewModel);
                            if (result)
                            {
                                table.ProjectName = projectViewModel.ProjectName;
                            }
                            else
                            {
                                TMessageBox.ShowMsg("保存失败!");
                                return;
                            }
                            if (!projectViewModel.ValidateData())
                            {
                                return;
                            }
                        }
                        List<string> errorMsg = new List<string>();
                        Wating wating = new Wating();
                        wating.Show();
                        NSISScript nSISScript = new NSISScript();
                        try
                        {
                            List<string> list = new List<string>();
                            await Task.Run(() =>
                             {
                                 list = nSISScript.Build(viewCaheModel.BaseProjectViewModel.ProjectInfo);
                                 if (messageResult == TMessageBoxResult.OK)
                                 {
                                     var workSpace = viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.WorkSpace + "\\";
                                     List<string> lists = new List<string>();
                                     foreach (var item in list)
                                     {
                                         lists.Add(item.Replace(workSpace, string.Empty));
                                     }
                                     list = lists;
                                 }
                             });
                            wating.Close();
                            if (list != null && list.Count > 0)
                            {
                                File.WriteAllLines(dirPath, list, Encoding.Default);
                                if (File.Exists(dirPath))
                                {

                                    TMessageBox.MainShowMsg("", "导出脚本成功!", MessageLevel.Information);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            wating.Close();

                            TMessageBox.MainShowMsg("", "导出脚本失败!", MessageLevel.Information);
                            Log.Write("导出脚本失败!", ex);
                        }
                        finally
                        {

                        }
                    }
                }
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
                            else
                            {
                                TMessageBox.ShowMsg("保存失败!");
                                return;
                            }
                            if (!projectViewModel.ValidateData())
                            {
                                return;
                            }
                        }
                        List<string> errorMsg = new List<string>();
                        Wating wating = new Wating();
                        wating.Show();
                        NSISScript nSISScript = new NSISScript();
                        try
                        {


                            var list = nSISScript.Build(viewCaheModel.BaseProjectViewModel.ProjectInfo);
                            if (!string.IsNullOrWhiteSpace(viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.WorkSpace))
                            {
                                DirectoryInfo directoryInfo = new DirectoryInfo(viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.WorkSpace);
                                var path = Path.Combine(directoryInfo.Parent.FullName, $"{viewCaheModel.BaseProjectViewModel.ProjectInfo.BaseInfo.ApplicationName}.pgescript");
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
                                            App.Current.Dispatcher.Invoke(() =>
                                            {

                                                wating.Close();
                                            });
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
                                        if (!string.IsNullOrWhiteSpace(e.Data))
                                        {
                                            errorMsg.Add(e.Data);
                                            if (e.Data.ToLower().Contains("aborting creation process".ToLower()))
                                            {
                                                App.Current.Dispatcher.Invoke(() =>
                                                {

                                                    wating.Close();
                                                });
                                                string format = string.Format("编译失败!{0}".GetLangText(), $"\r{string.Join("\r", errorMsg)}");
                                                TMessageBox.MainShowMsg("", format, MessageLevel.Information);
                                                Log.Write(format);
                                            }
                                        }
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
                            wating.Close();

                            TMessageBox.MainShowMsg("", "编译失败!", MessageLevel.Information);
                            Log.Write("编译失败!", ex);
                        }
                        finally
                        {

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
        /// 另存为
        /// </summary>
        public RelayCommand SaveAsCommand => new RelayCommand(() =>
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
                            if (projectViewModel.ProjectInfo != null && projectViewModel.ProjectInfo.BaseInfo != null)
                                projectViewModel.ProjectInfo.ExtraInfo.FilePath = "";
                            if (!projectViewModel.ValidateData())
                            {
                                return;
                            }
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
                            if (!projectViewModel.ValidateData())
                            {
                                return;
                            }
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
                if (result != null && !string.IsNullOrWhiteSpace(result.Item2))
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
            CloseTips(t);
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

        /// <summary>
        /// 关闭全部
        /// </summary>
        /// <returns></returns>
        internal bool CloseAll()
        {
            int index = 0;
            int tableCount = TableList.Count;

            while (TableList.Count > 0 && index <= tableCount - 1)
            {
                var item = TableList[index];
                item.IsActive = true;
                //CloseCommand.Execute(item);
                var result = CloseTips(item);
                if (result == false)
                    break;
                if (tableCount == TableList.Count)
                {

                    index++;
                }
                else
                {
                    tableCount = TableList.Count;
                }
            }
            if (TableList.Count == 0)
            {
                Environment.Exit(0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 关闭所有标签
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool CloseTips(TableModel? t)
        {
            if (CacheDataHelper.ProjectCahes.ContainsKey(t.ProjectKey))
            {
                var viewCaheModel = CacheDataHelper.ProjectCahes[t.ProjectKey];
                if (viewCaheModel != null)
                {
                    ProjectViewModel? projectViewModel = viewCaheModel.BaseProjectViewModel as ProjectViewModel;
                    if (projectViewModel != null)
                    {
                        bool flage = true;

                        if (CacheDataHelper.OldProjectDic.ContainsKey(t.ProjectKey))
                        {
                            projectViewModel.Save();
                            var old = CacheDataHelper.OldProjectDic[t.ProjectKey];
                            if (old != null)
                            {
                                flage = projectViewModel.ProjectInfo.CheckIsChanged(old);
                            }
                        }
                        if (flage)
                        {
                            var result = TMessageBox.ShowMsg(string.Format("当前标签 {0} 未保存,是否保存?", t.ProjectName), MessageLevel.YesNoCancel);
                            if (result != TMessageBoxResult.Cancel && result != TMessageBoxResult.OK)
                            {
                                return false;
                            }
                            if (result == TMessageBoxResult.OK)
                            {
                                var flag = FileHelper.Save(projectViewModel);
                                if (flag)
                                {
                                    t.ProjectName = projectViewModel.ProjectName;
                                    //TMessageBox.ShowMsg("保存成功!");
                                    SaveRecently(projectViewModel);
                                }
                                else
                                {
                                    //TMessageBox.ShowMsg("保存失败!");
                                }
                            }
                        }

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
            return true;

        }
        #endregion
    }
}
