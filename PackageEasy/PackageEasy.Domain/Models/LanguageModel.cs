using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:39:21
    /// desc:LanguageModel
    /// </summary>
    public class LanguageModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 语言文本
        /// </summary>
        public string LanguageText { get; set; }
        /// <summary>
        /// 语言类型
        /// </summary>
        public LanguageType LanguageType { get; set; }

    }
}
