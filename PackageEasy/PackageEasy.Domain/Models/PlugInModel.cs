using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class PlugInModel : BaseModel
    {
        private PlugInState installState;

        /// <summary>
        /// 插件id
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 插件类型
        /// </summary>
        public Type PlugInType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 展示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        ///插件安装状态
        /// </summary>
        public PlugInState InstallState
        {
            get => installState;
            set
            {
                installState = value;
                RaisePropertyChanged();
            }
        }
    }
}
