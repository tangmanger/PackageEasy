using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class TargetPathModel : BaseModel
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 目标名称
        /// </summary>
        public string TargetPath { get; set; }
        /// <summary>
        /// 是否用户
        /// </summary>
        public bool IsUserCreated { get; set; }
        /// <summary>
        /// 创建问题
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
