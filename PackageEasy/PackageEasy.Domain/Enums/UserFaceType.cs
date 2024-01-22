using PackageEasy.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Enums
{
    /// <summary>
    /// author:TT
    /// time:2023-03-16 22:53:21
    /// desc:UserFaceType
    /// </summary>
    public enum UserFaceType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        ///    现代
        /// </summary>
        [Description("现代")]
        Morden = 1,
        /// <summary>
        /// 古典
        /// </summary>
        [Description("古典")]
        Classic = 2
    }
}
