using PackageEasy.Attributes;
using PackageEasy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 16:51:14
    /// desc:TViewsHelper
    /// </summary>
    public class TViewsHelper
    {
        /// <summary>
        /// 导航界面
        /// </summary>
        public static List<NaviteModel> NaviteViewList = new List<NaviteModel>();


        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            InitView();
        }
        private static void InitView()
        {
            var types = GetAlTypes();
            foreach (var item in types)
            {
                var toolAttribute =
               (TViewAttribute)Attribute.GetCustomAttribute(item, typeof(TViewAttribute));

                NaviteViewList.Add(new NaviteModel()
                {
                    Type = toolAttribute.Type,
                    ViewType = toolAttribute.ViewType,
                    VmType = toolAttribute.VMType
                });
                //if (!toolAttribute.IsViewModel)
                //    CaheDataHelper.SoundsDirectory.Add(toolAttribute.ViewType, toolAttribute.ViewType.GetDescription());


            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Type[] GetAlTypes()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(a => a.GetTypes().Where(t => t.GetCustomAttributes(false).ToList().Exists(m => m is TViewAttribute)))
              .ToArray();
            return allTypes;
        }
    }
}
