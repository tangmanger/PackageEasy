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
    }
}
