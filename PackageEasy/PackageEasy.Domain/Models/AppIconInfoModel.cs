using PackageEasy.Domain.Attributes;
using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-04-05 17:02:15
    /// desc:AppIconInfoModel
    /// </summary>
    public class AppIconInfoModel : BaseModel
    {
        private bool isSelected;
        private string shortcutPath;
        private string filePath;
        private DescModel<TargetDirType> iconDir;
        private string appIconCutShotParam;

        /// <summary>
        /// 快捷方式
        /// </summary>
        public string ShortcutPath
        {
            get => shortcutPath;
            set
            {
                shortcutPath = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 目录
        /// </summary>
        public DescModel<TargetDirType> IconDir
        {
            get => iconDir;
            set
            {
                iconDir = value;
                RaisePropertyChanged();
            }

        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {

            get => filePath;
            set
            {
                filePath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 选择
        /// </summary>
        [JsonIgnore]
        [SaveIgnore]
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
        /// 快捷方式启动参数
        /// </summary>
        public string AppIconCutShotParam
        {
            get => appIconCutShotParam;
            set
            {
                appIconCutShotParam = value;
                RaisePropertyChanged();
            }
        }
    }
}
