using CommunityToolkit.Mvvm.ComponentModel;
using PackageEasy.Domain;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Enums;
using PackageEasy.Helpers;
using PackageEasy.Models;
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
    /// time:2023-03-11 22:23:29
    /// desc:BaseProjectViewModel
    /// </summary>
    public class BaseProjectViewModel : ObservableObject, INavigateOut
    {
        /// <summary>
        /// 唯一标志
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public string ParentKey { get; set; }
        /// <summary>
        /// 页面类型
        /// </summary>
        public ViewType Type { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public ProjectInfoModel ProjectInfo { get; set; }
        /// <summary>
        /// 事件通知
        /// </summary>
        public IDataService Service { get; set; }

        /// <summary>
        /// 保存位置
        /// </summary>
        public string SavePath
        {
            get
            {
                string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", ParentKey ?? Key);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                return savePath;
            }
        }
        public string CompilePath
        {
            get
            {
                string compilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", ParentKey ?? Key);
                if (!Directory.Exists(compilePath))
                {
                    Directory.CreateDirectory(compilePath);
                }
                return compilePath;
            }
        }
        public BaseProjectViewModel()
        {
            Key = Guid.NewGuid().ToString();
            ProjectInfo = new ProjectInfoModel();
            ProjectInfo.ExtraInfo = new Domain.Models.ExtraInfo() { CreateTime = DateTime.Now };
            if (!CacheDataHelper.ProjectDic.ContainsKey(Key))
            {
                CacheDataHelper.ProjectDic.Add(Key, ProjectInfo);
            }
            Service = ServiceHelper.GetService(Key);
        }
        public BaseProjectViewModel(ViewType viewType, string key)
        {
            Key = Guid.NewGuid().ToString();
            ParentKey = key;
            Type = viewType;
            if (CacheDataHelper.ProjectDic.ContainsKey(ParentKey))
            {
                ProjectInfo = CacheDataHelper.ProjectDic[ParentKey];
            }
            Service = ServiceHelper.GetService(ParentKey);
        }
        public List<string> RefreshFileList()
        {
            List<string> strings = new List<string>();
            if (ProjectInfo.AssemblyInfo != null && ProjectInfo.AssemblyInfo.AssemblyList != null)
            {
                foreach (var item in ProjectInfo.AssemblyInfo.AssemblyList)
                {
                    if (item.FileList != null)
                    {
                        foreach (var file in item.FileList)
                        {

                            string path = $"{file.TargetPath.DisplayName}\\{file.FilePath}";
                            if (!string.IsNullOrWhiteSpace(item.SelectDir))
                            {
                                path = $"{file.TargetPath.DisplayName}{file.FilePath.Replace(item.SelectDir, "")}";
                            }
                            strings.Add(path);
                        }
                    }
                }
            }
            return strings;
        }
        /// <summary>
        /// 退出
        /// </summary>
        public virtual void NavigateOut()
        {

        }
        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        public virtual void Save()
        {

        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        public virtual void RefreshData()
        {

        }
        /// <summary>
        /// 验证数据
        /// </summary>
        public virtual bool ValidateData()
        {
            return true;
        }
        public virtual void Compile()
        {

        }
    }
}
