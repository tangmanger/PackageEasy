using Microsoft.Win32;
using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain;
using PackageEasy.Domain.Helpers;
using PackageEasy.Domain.Models;
using PackageEasy.Domain.Models.SaveModel;
using PackageEasy.ViewModels;
using PackageEasy.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-15 23:36:34
    /// desc:FileHelper
    /// </summary>
    public class FileHelper
    {
        public static Tuple<bool, string> OpenFile(ProjectViewModel projectViewModel)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = StaticStringHelper.PGE; // Default file extension
            openFileDialog.Filter = $"pge documents|*{StaticStringHelper.PGE};*{StaticStringHelper.PKY}|pky documents|*{StaticStringHelper.PKY};*{StaticStringHelper.PGE}"; // Filter files by extension
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                if (CacheDataHelper.FileOpenDic.ContainsValue(openFileDialog.FileName))
                {
                    return new Tuple<bool, string>(false, "当前文件已打开");
                }

                return Open(openFileDialog.FileName, projectViewModel);
            }
            return new Tuple<bool, string>(false, "");
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="filePath">原路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns>原因</returns>
        static Tuple<bool, string> UnZip(string filePath, string targetPath, string password = "")
        {
            var flage = ZipHelper.UnZipFile(filePath, password, targetPath);
            if (flage.Item1 == false && Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);
            return flage;
        }
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="projectViewModel"></param>
        /// <returns></returns>
        public static Tuple<bool, string> Open(string filePath, ProjectViewModel projectViewModel)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", Guid.NewGuid().ToString().Replace("-", ""));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileInfo info = new FileInfo(filePath);
            string password = "";
            if (info.Extension.ToLower() == StaticStringHelper.PKY.ToLower())
            {
                PasswordDialog passwordDialog = new PasswordDialog();
                passwordDialog.ShowDialog();
                if (!passwordDialog.IsSuccess)
                {
                    return new Tuple<bool, string>(false, "请输入密码！");
                }
                password = passwordDialog.Password;
            }
            var result = UnZip(filePath, path, password);
            if (result.Item1 == false) return result;
            if (projectViewModel.ProjectInfo == null)
                projectViewModel.ProjectInfo = new ProjectInfoModel();
            var extraInfo = Path.Combine(path, "ExtraInfo.json");
            if (!File.Exists(extraInfo))
            {
                Log.Write($"文件{extraInfo}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.ExtraInfo = File.ReadAllText(extraInfo).DeserializeObject<ExtraInfo>();
                projectViewModel.ProjectInfo.ExtraInfo.FilePath = filePath;

            }
            var baseInfo = Path.Combine(path, "BaseInfo.json");

            FileInfo fileInfo = new FileInfo(filePath);
            if (!File.Exists(baseInfo))
            {
                Log.Write($"文件{baseInfo}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.BaseInfo = File.ReadAllText(baseInfo).DeserializeObject<BaseInfoModel>();
                //projectViewModel.ProjectInfo.BaseInfo.Key = projectViewModel.Key;
                projectViewModel.ProjectName = fileInfo.Name.Replace(fileInfo.Extension, "");

            }

            var assemblyPath = Path.Combine(path, "AssemblyInfo.json");
            if (!File.Exists(assemblyPath))
            {
                Log.Write($"文件{assemblyPath}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.AssemblyInfo = File.ReadAllText(assemblyPath).DeserializeObject<AssemblyInfoModel>();


            }
            var iconPath = Path.Combine(path, "AppInfo.json");
            if (!File.Exists(iconPath))
            {
                Log.Write($"文件{iconPath}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.AppIcon = File.ReadAllText(iconPath).DeserializeObject<AppIconModel>();
            }

            var multiFilePath = Path.Combine(path, "MultiFiles.json");
            if (!File.Exists(multiFilePath))
            {
                Log.Write($"文件{multiFilePath}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.MultiFiles = File.ReadAllText(multiFilePath).DeserializeObject<List<MultiFileModel>>();
            }
            var registryPath = Path.Combine(path, "Registry.json");
            if (!File.Exists(registryPath))
            {
                Log.Write($"文件{registryPath}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.Registry = File.ReadAllText(registryPath).DeserializeObject<RegistryModel>();
            }
            var finishPath = Path.Combine(path, "FinishInfo.json");
            if (!File.Exists(finishPath))
            {
                Log.Write($"文件{finishPath}不存在!");
            }
            else
            {
                projectViewModel.ProjectInfo.FinishInfo = File.ReadAllText(finishPath).DeserializeObject<FinishModel>();
            }
            if (!CacheDataHelper.FileOpenDic.ContainsValue(filePath))
            {
                if (CacheDataHelper.FileOpenDic.ContainsKey(projectViewModel.Key))
                    CacheDataHelper.FileOpenDic[projectViewModel.Key] = filePath;
                else
                    CacheDataHelper.FileOpenDic.Add(projectViewModel.Key, filePath);
            }
            if (!CacheDataHelper.OldProjectDic.ContainsKey(projectViewModel.Key))
                CacheDataHelper.OldProjectDic.Add(projectViewModel.Key, projectViewModel.ProjectInfo.Clone<ProjectInfoModel>());
            return new Tuple<bool, string>(true, "");
        }
        /// <summary>
        /// 异步保存
        /// </summary>
        /// <param name="projectViewModel"></param>
        /// <returns></returns>
        public static async Task<bool> SaveSync(ProjectViewModel projectViewModel)
        {
            bool result = false;
            await Task.Run(() => { result = (bool)Save(projectViewModel); });
            return result;
        }
        /// <summary>
        /// 获取指定文件的路径
        /// </summary>
        /// <param name="extension">扩展名</param>
        /// <returns></returns>
        public static string SeletcedFilePath(string extension)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = $".{extension}"; // Default file extension
            openFileDialog.Filter = $"{extension} documents|*.{extension}"; // Filter files by extension
            ///用户点了关闭按钮,未选择路径
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="projectViewModel"></param>
        /// <returns></returns>
        public static bool Save(ProjectViewModel projectViewModel)
        {
            string filePath = projectViewModel.ProjectInfo.ExtraInfo.FilePath;
            projectViewModel.Save();
            string oldFilePath = "";
            if (!File.Exists(filePath))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                if (projectViewModel.ProjectInfo.Registry != null && projectViewModel.ProjectInfo.Registry.IsUsePassword)
                {
                    openFileDialog.FileName = $"{projectViewModel.ProjectName.Replace("*", "")}{StaticStringHelper.PKY}"; // Default file name
                    openFileDialog.DefaultExt = StaticStringHelper.PKY; // Default file extension
                    openFileDialog.Filter = $"pky documents|*{StaticStringHelper.PKY}"; // Filter files by extension
                }
                else
                {
                    openFileDialog.FileName = $"{projectViewModel.ProjectName.Replace("*", "")}{StaticStringHelper.PGE}"; // Default file name
                    openFileDialog.DefaultExt = StaticStringHelper.PGE; // Default file extension
                    openFileDialog.Filter = $"pge documents|*{StaticStringHelper.PGE}"; // Filter files by extension
                }
                ///用户点了关闭按钮,未选择路径
                if (openFileDialog.ShowDialog() == false)
                {
                    return false;
                }
                filePath = openFileDialog.FileName;
                oldFilePath = filePath;
                projectViewModel.ProjectInfo.ExtraInfo.FilePath = filePath;
            }
            else
            {
                oldFilePath=filePath;
                if (projectViewModel.ProjectInfo.Registry != null && projectViewModel.ProjectInfo.Registry.IsUsePassword)
                {
                    filePath = filePath.Replace(StaticStringHelper.PGE, StaticStringHelper.PKY);
                }
                else
                {
                    filePath = filePath.Replace(StaticStringHelper.PKY, StaticStringHelper.PGE);
                }
            }
            FileInfo fileInfo = new FileInfo(filePath);
            projectViewModel.ProjectName = fileInfo.Name.Replace(fileInfo.Extension, "");
            projectViewModel.ProjectInfo.ExtraInfo.FilePath = filePath;

            if (!CacheDataHelper.OldProjectDic.ContainsKey(projectViewModel.Key))
                CacheDataHelper.OldProjectDic.Add(projectViewModel.Key, projectViewModel.ProjectInfo.Clone<ProjectInfoModel>());
            else
            {
                CacheDataHelper.OldProjectDic[projectViewModel.Key] = projectViewModel.ProjectInfo.Clone<ProjectInfoModel>();
            }
            string password = "";
            if (projectViewModel.ProjectInfo.Registry.IsUsePassword)
            {
                password = projectViewModel.ProjectInfo.Registry.Password;
            }
            var success = ZipHelper.Compress(projectViewModel.SavePath, filePath, password);
            Directory.Delete(projectViewModel.SavePath, true);
            if(oldFilePath!=filePath)
            {
                File.Delete(oldFilePath);
            }
            return success == true;
        }
    }
}
