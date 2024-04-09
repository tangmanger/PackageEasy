using PackageEasy.Domain.Attributes;
using PackageEasy.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Enums
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:39:55
    /// desc:LanguageType
    /// </summary>
    public enum LanguageType
    {
        [Description(CommonSettings.Null)]
        None = 0,
        /// <summary>
        /// 简体中文
        /// </summary>
        [Description(CommonSettings.SampleChinese)]
        Zh_CN = 1,
        /// <summary>
        /// 英文
        /// </summary>
        [Description(CommonSettings.English)]
        En_SH = 2,

    }
}
