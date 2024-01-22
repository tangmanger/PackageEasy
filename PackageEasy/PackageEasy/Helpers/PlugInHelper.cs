using PackageEasy.Attributes;
using PackageEasy.Domain.Attributes;
using PackageEasy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageEasy.Domain.Models;
using PackageEasy.Common.Data;
using System.Reflection;
using System.IO;

namespace PackageEasy.Helpers
{
    public class PlugInHelper
    {
        public static List<PackageEasy.Domain.Models.PlugInModel> PlugInList = new List<PackageEasy.Domain.Models.PlugInModel>();
        /// <summary>
        /// 初始化插件
        /// </summary>
        public static void InitPlugIns()
        {
            var types = GetAlTypes();
            foreach (var item in types)
            {
                var toolAttribute =
               (PlugInAttribute)Attribute.GetCustomAttribute(item, typeof(PlugInAttribute));
                if (toolAttribute != null)
                    PlugInList.Add(toolAttribute.PlugIn);
                //if (!toolAttribute.IsViewModel)
                //    CaheDataHelper.SoundsDirectory.Add(toolAttribute.ViewType, toolAttribute.ViewType.GetDescription());


            }
            foreach (var item in PlugInList)
            {
                item.DisplayName = item.Name.GetLangText();
            }
        }

        private static Type[] GetAlTypes()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PackageEasy.PlugIns.dll");
            if (File.Exists(path))
            {
                Assembly assembly = Assembly.LoadFile(path);
                var allTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(false).ToList().Exists(m => m is PlugInAttribute))
                  .ToArray();
                return allTypes;
            }
            return new Type[0];
        }
    }
}
