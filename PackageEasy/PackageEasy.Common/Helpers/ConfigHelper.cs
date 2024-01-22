using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    public class ConfigHelper
    {
        static ConfigModel _config;
        /// <summary>
        /// 配置文件
        /// </summary>
        public static ConfigModel Config
        {
            get
            {
                if (_config == null)
                {
                    var filePath = Path.Combine(DataHelper.Config, "general.json");
                    if (File.Exists(filePath))
                        _config = File.ReadAllText(filePath).DeserializeObject<ConfigModel>();
                    if (_config == null)
                        _config = new ConfigModel();
                }
                return _config;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="isRefresh"></param>
        public static void Save(bool isRefresh)
        {
            var data = Config.SerializeObject();
            var filePath = Path.Combine(DataHelper.Config, "general.json");
            File.WriteAllText(filePath, data);
            if (isRefresh)
            {
                if (File.Exists(filePath))
                    _config = File.ReadAllText(filePath).DeserializeObject<ConfigModel>();
            }
        }
    }
}
