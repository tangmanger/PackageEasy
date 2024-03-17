using PackageEasy.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-17 0:02:46
    /// desc:ExtraInfo
    /// </summary>
    public class ExtraInfo
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [SaveIgnore]
        public string FilePath { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 内部版本
        /// </summary>
        public string InternalVersion { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
