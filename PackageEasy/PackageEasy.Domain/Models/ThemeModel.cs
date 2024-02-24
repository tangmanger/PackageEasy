using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class ThemeModel
    {
        /// <summary>
        /// 主题文件名称
        /// </summary>
        public string ThemeName { get; set; }
        /// <summary>
        /// 主题描述
        /// </summary>
        public string ThemeDescription { get; set; }
        /// <summary>
        /// 主题id
        /// </summary>
        public string ThemeId { get; set; }
    }
}
