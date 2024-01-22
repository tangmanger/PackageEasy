using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class InstallLanguageModel : BaseModel
    {
        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 语言代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 语言名称
        /// </summary>
        public string LanguageName { get; set; }
        /// <summary>
        /// 语言栏
        /// </summary>
        public LanguageType LanguageType { get; set; }
        /// <summary>
        /// 标志key 用来实现用户自定义多语言
        /// </summary>
        public string LanguageDisplayKey { get; set; }
    }
}
