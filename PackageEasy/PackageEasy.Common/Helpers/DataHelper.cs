using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:43:54
    /// desc:DataHelper
    /// </summary>
    public class DataHelper
    {
        /// <summary>
        /// 根目录
        /// </summary>
        public static string RootDir
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        /// <summary>
        /// 数据路径
        /// </summary>
        public static string Data
        {
            get
            {
                string data = Path.Combine(RootDir, "Data");
                if (!Directory.Exists(data))
                    Directory.CreateDirectory(data);
                return data;
            }
        }
        /// <summary>
        /// 语言路径
        /// </summary>
        public static string Language
        {
            get
            {
                string path = Path.Combine(Data, "Language");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        /// <summary>
        /// 配置目录
        /// </summary>
        public static string Config
        {
            get
            {
                string path = Path.Combine(Data, "Config");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        /// <summary>
        /// 插件目录
        /// </summary>
        public static string Plugin
        {
            get
            {
                string path = Path.Combine(Data, "Plugin");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        /// <summary>
        /// 缓存目录
        /// </summary>
        public static string Temp
        {
            get
            {
                string path = Path.Combine(Path.GetTempPath(), "PackageEasy");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        /// <summary>
        /// 储存路径
        /// </summary>
        public static string Store
        {
            get
            {
                string path = Path.Combine(Data, "Store");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}
