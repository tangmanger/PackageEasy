using PackageEasy.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class RecentlyModel : BaseModel
    {
        private bool isLock;
        private string filePath;

        /// <summary>
        /// 名称
        /// </summary>
        public string RecentlyName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                IsLock = value.EndsWith(StaticStringHelper.PKY);
            }
        }

        public bool IsLock
        {
            get => isLock;
            set
            {
                isLock = value;
                RaisePropertyChanged();
            }
        }
    }
}
