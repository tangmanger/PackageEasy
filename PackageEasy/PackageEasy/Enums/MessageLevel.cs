using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Enums
{
    /// <summary>
    /// author:TT
    /// time:2023-03-26 16:34:55
    /// desc:MessageLevel
    /// </summary>
    public enum MessageLevel
    {
        /// <summary>
        /// 信息
        /// </summary>
        Information = 0,
        /// <summary>
        /// 询问
        /// </summary>
        Question = 1,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,
        /// <summary>
        /// 是否取消
        /// </summary>
        YesNoCancel = 4
    }
}
