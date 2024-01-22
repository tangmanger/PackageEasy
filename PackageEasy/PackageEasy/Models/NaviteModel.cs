using PackageEasy.Domain.Enums;
using PackageEasy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 16:59:22
    /// desc:NaviteModel
    /// </summary>
    public class NaviteModel
    {
        /// <summary>
        /// 界面
        /// </summary>
        public ViewType ViewType { get; set; }
        /// <summary>
        /// 实例类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// vm类型
        /// </summary>
        public Type VmType { get; set; }
    }
}
