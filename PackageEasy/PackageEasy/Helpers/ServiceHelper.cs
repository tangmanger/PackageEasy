using PackageEasy.Domain.Interfaces;
using PackageEasy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Helpers
{
    public class ServiceHelper
    {
        static Dictionary<string, IDataService> Services = new Dictionary<string, IDataService>();
        /// <summary>
        /// 获取当前的处理
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IDataService GetService(string key)
        {
            if (Services.ContainsKey(key)) return Services[key];
            IDataService service = new DataService();
            service.ProjectKey = key;
            Services.Add(key, service);
            return service;
        }
        /// <summary>
        /// 获取所有服务
        /// </summary>
        /// <returns></returns>
        public static List<IDataService> GetDataServices()
        {
            return Services.Values.ToList();
        }
        /// <summary>
        ///移除
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveService(string key)
        {
            if (Services.ContainsKey(key))
                Services.Remove(key);
        }
    }
}
