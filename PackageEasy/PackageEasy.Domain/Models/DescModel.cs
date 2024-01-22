using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-16 22:56:02
    /// desc:DescModel
    /// </summary>
    public class DescModel<T>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        ///名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        ///  描述
        /// </summary>
        public string Description { get; set; }
    }
}
