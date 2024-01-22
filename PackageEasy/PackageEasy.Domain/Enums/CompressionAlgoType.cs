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
    /// time:2023-03-16 23:23:29
    /// desc:CompressionAlgoType
    /// </summary>
    public enum CompressionAlgoType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// zLib
        /// </summary>
        [Description("zLib")]
        Zlib = 1,
        /// <summary>
        /// bzip2
        /// </summary>
        [Description("bzip2")]
        BZip2 = 2,
        /// <summary>
        /// LZMA
        /// </summary>
        [Description("LZMA")]
        LZMA = 3,
    }
}
