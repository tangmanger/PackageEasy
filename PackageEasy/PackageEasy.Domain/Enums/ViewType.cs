using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Enums
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 16:13:38
    /// desc:ViewType
    /// </summary>
    public enum ViewType
    {
        None = 0,
        /// <summary>
        /// 主页
        /// </summary>
        Home = 1,
        /// <summary>
        /// 基础信息页
        /// </summary>
        BaseInfoView = 2,
        /// <summary>
        /// 组件界面
        /// </summary>
        AssemblyView = 3,
        /// <summary>
        /// 应用程序图标信息
        /// </summary>
        AppIconInfoView = 4,
        /// <summary>
        /// 结束界面
        /// </summary>
        EndView = 5,
        /// <summary>
        /// 注册表界面
        /// </summary>
        RegistryView = 6,
        /// <summary>
        /// 多语言处理界面
        /// </summary>
        LanguageView = 7
    }
}
