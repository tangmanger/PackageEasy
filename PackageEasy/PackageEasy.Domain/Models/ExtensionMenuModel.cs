using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PackageEasy.Domain.Models
{
    public class ExtensionMenuModel
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string? ExtensionName { get; set; }
        /// <summary>
        /// 命令
        /// </summary>
        public ICommand? MenuCommand { get; set; }
    }
}
