using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models.SaveModel
{
    /// <summary>
    /// author:TT
    /// time:2023-04-05 17:33:11
    /// desc:AppIconModel
    /// </summary>
    public class AppIconModel
    {
        private string startMenuName;
        private bool isCanChangeStartMenuName;
        private bool isCreateWebUrl;
        private bool isCreateUnInstall;
        private ObservableCollection<AppIconInfoModel> appIconInfoList;

        /// <summary>
        /// 启动目录名称
        /// </summary>
        public string StartMenuName
        {
            get => startMenuName;
            set
            {
                startMenuName = value;
            }
        }
        /// <summary>
        /// /允许用户改变开始菜单
        /// </summary>
        public bool IsCanChangeStartMenuName
        {
            get => isCanChangeStartMenuName;
            set
            {
                isCanChangeStartMenuName = value;
            }
        }

        /// <summary>
        /// 创建互联网快捷方式
        /// </summary>
        public bool IsCreateWebUrl
        {
            get => isCreateWebUrl;
            set
            {
                isCreateWebUrl = value;
            }
        }
        /// <summary>
        /// 创建卸载快捷键
        /// </summary>
        public bool IsCreateUnInstall
        {
            get => isCreateUnInstall;
            set
            {
                isCreateUnInstall = value;
            }
        }

        /// <summary>
        /// 快捷方式列表
        /// </summary>
        public ObservableCollection<AppIconInfoModel> AppIconInfoList
        {
            get => appIconInfoList;
            set
            {
                appIconInfoList = value;
            }
        }

    }
}
