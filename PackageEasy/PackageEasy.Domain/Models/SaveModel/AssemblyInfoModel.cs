using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models.SaveModel
{
    /// <summary>
    /// author:TT
    /// time:2023-03-25 23:49:40
    /// desc:AssemblyInfoModel
    /// </summary>
    public class AssemblyInfoModel
    {
        /// <summary>
        /// 组件信息
        /// </summary>
        public List<AssemblyModel> AssemblyList { get; set; }
        /// <summary>
        /// 是否允许用户选择
        /// </summary>
        public bool IsAllowChoose { get; set; }
    }
}
