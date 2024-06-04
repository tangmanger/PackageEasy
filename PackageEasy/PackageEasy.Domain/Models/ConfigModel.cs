using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class ConfigModel
    {
        public string NSISMakePath { get; set; }
        public string NSISHelperPath { get; set; }
        /// <summary>
        /// 工作目录
        /// </summary>
        public string WorkSpace { get; set; }
        /// <summary>
        /// 主题id
        /// </summary>
        public string ThemeId { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int Lang { get; set; } =0;
    }
}
