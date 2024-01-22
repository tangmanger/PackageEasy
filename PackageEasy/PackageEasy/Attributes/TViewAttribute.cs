using PackageEasy.Domain.Enums;
using PackageEasy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Attributes
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 16:09:56
    /// desc:TViewAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TViewAttribute : Attribute
    {
        public TViewAttribute(ViewType viewType, Type type, Type vmType)
        {
            ViewType = viewType;
            Type = type;
            VMType = vmType;
        }

        /// <summary>
        /// vm
        /// </summary>
        public Type VMType { get; set; }
        /// <summary>
        /// view
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 界面
        /// </summary>
        public ViewType ViewType { get; set; }
    }
}
