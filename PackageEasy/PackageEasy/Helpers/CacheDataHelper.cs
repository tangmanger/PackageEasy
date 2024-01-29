using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain;
using PackageEasy.Domain.Models;
using PackageEasy.Models;
using PackageEasy.ViewModels;
using PackageEasy.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 22:38:31
    /// desc:CacheDataHelper
    /// </summary>
    public class CacheDataHelper
    {
        /// <summary>
        /// Project 缓存
        /// </summary>
        public static Dictionary<string, ViewCaheModel> ProjectCahes = new Dictionary<string, ViewCaheModel>();
        /// <summary>
        /// 项目
        /// </summary>
        public static Dictionary<string, ProjectInfoModel> ProjectDic = new Dictionary<string, ProjectInfoModel>();
        /// <summary>
        /// 已经打开的文件
        /// </summary>
        public static Dictionary<string, string> FileOpenDic { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 打开文件路径
        /// </summary>
        public static string OpenPath { get; internal set; }
        /// <summary>
        /// 最近打开的文件
        /// </summary>
        public static List<RecentlyModel> RecentlyList = new List<RecentlyModel>();
        /// <summary>
        /// 项目副本用来比较是否修改了
        /// </summary>
        public static Dictionary<string, ProjectInfoModel> OldProjectDic = new Dictionary<string, ProjectInfoModel>();
        /// <summary>
        /// 插件列表
        /// </summary>
        public static List<string> PluginList = new List<string>();

        public static int Version
        {
            get
            {
                return 1;
            }
        }

        public static string InternalVersion
        {
            get
            {
                return "2024年1月29日 11:38:13";
            }
        }
        /// <summary>
        /// 更新最近打开的文件
        /// </summary>
        /// <param name="recently"></param>
        public static void UpdateRecently(RecentlyModel recently)
        {
            var current = RecentlyList.Find(p => p.RecentlyName == recently.RecentlyName);
            if (current != null)
            {
                RecentlyList.Remove(current);
            }
            recently.UpdateTime = DateTime.Now;
            RecentlyList.Add(recently);
            RecentlyList = RecentlyList.OrderBy(p => p.UpdateTime).ToList();
            var recentlyPath = Path.Combine(DataHelper.Store, "Recently.json");
            File.WriteAllText(recentlyPath, RecentlyList.SerializeObject());
        }
        /// <summary>
        ///初始化最近打开
        /// </summary>
        private static void InitRecently()
        {
            var recentlyPath = Path.Combine(DataHelper.Store, "Recently.json");
            if (File.Exists(recentlyPath))
            {
                RecentlyList = File.ReadAllText(recentlyPath).DeserializeObject<List<RecentlyModel>>();
            }
            if (RecentlyList == null)
                RecentlyList = new List<RecentlyModel>();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            InitRecently();
            InitPlugins();
        }

        /// <summary>
        /// 最近打开
        /// </summary>
        /// <param name="recentlyName"></param>
        internal static void DeleteRecently(string recentlyName)
        {
            var current = RecentlyList.Find(p => p.RecentlyName == recentlyName);
            if (current != null)
            {
                RecentlyList.Remove(current);
                RecentlyList = RecentlyList.OrderBy(p => p.UpdateTime).ToList();
                var recentlyPath = Path.Combine(DataHelper.Store, "Recently.json");
                File.WriteAllText(recentlyPath, RecentlyList.SerializeObject());
            }
        }
        /// <summary>
        /// 插件列表
        /// </summary>
        /// <param name="pluginId"></param>
        /// <param name="isAdd"></param>
        public static void OperatePlugin(string pluginId, bool isAdd = false)
        {
            var plugin = PluginList.Find(p => p == pluginId);
            if (plugin == null)
            {
                plugin = pluginId;
            }
            if (isAdd)
            {
                if (!PluginList.Exists(c => c == plugin))
                    PluginList.Add(plugin);
            }
            else
            {
                PluginList.Remove(plugin);
            }
            var pluginPath = Path.Combine(DataHelper.Store, "Plugins.json");
            File.WriteAllText(pluginPath, PluginList.SerializeObject());
        }
        /// <summary>
        /// 初始化插件
        /// </summary>
        public static void InitPlugins()
        {
            var pluginPath = Path.Combine(DataHelper.Store, "Plugins.json");
            if (File.Exists(pluginPath))
                PluginList = File.ReadAllText(pluginPath).DeserializeObject<List<string>>();
        }
    }
}
