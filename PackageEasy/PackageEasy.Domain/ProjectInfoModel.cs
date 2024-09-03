using PackageEasy.Domain.Models.SaveModel;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain
{
    /// <summary>
    /// author:TT
    /// time:2023-03-16 0:09:48
    /// desc:ProjectInfoModel
    /// </summary>
    public class ProjectInfoModel
    {
        /// <summary>
        /// 额外信息
        /// </summary>
        public ExtraInfo ExtraInfo { get; set; }
        /// <summary>
        /// 基础信息
        /// </summary>
        public BaseInfoModel BaseInfo { get; set; }
        /// <summary>
        /// 组件信息
        /// </summary>
        public AssemblyInfoModel AssemblyInfo { get; set; }
        /// <summary>
        /// 快捷方式信息
        /// </summary>
        public AppIconModel AppIcon { get; set; }
        /// <summary>
        /// 脚本
        /// </summary>
        public List<string> Scripts { get; set; }
        /// <summary>
        /// 注册信息
        /// </summary>
        public RegistryModel Registry { get; set; }
        /// <summary>
        /// 多文件信息
        /// </summary>
        public List<MultiFileModel> MultiFiles { get; set; }
        /// <summary>
        /// 结束页
        /// </summary>
        public FinishModel FinishInfo { get; set; }
        /// <summary>
        /// 目标目录
        /// </summary>
        public List<TargetPathModel> TargetPaths { get; set; }
    }
}
