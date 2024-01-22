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
    }
}
