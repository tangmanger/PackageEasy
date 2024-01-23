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
    /// time:2023-03-15 23:31:10
    /// desc:BaseInfoModel
    /// </summary>
    public class BaseInfoModel
    {
        public string Key { get; set; }

        private string applicationName;
        private string applicationVersion;
        private string applicationPublisher;
        private string applicationUrl;
        private string appIconPath;
        private string appOutPath;
        private DescModel<CompressionAlgoType> comPressAlgo;
        private DescModel<UserFaceType> userFaceSelectItem;
        private string installIconPath;

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string ApplicationName
        {
            get => applicationName;
            set
            {
                applicationName = value;
            }
        }
        /// <summary>
        /// 应用程序版本
        /// </summary>
        public string ApplicationVersion
        {
            get => applicationVersion;
            set
            {
                applicationVersion = value;
            }
        }
        /// <summary>
        /// 出品人
        /// </summary>
        public string ApplicationPublisher
        {
            get => applicationPublisher;
            set
            {
                applicationPublisher = value;
            }
        }
        /// <summary>
        /// 网站
        /// </summary>
        public string ApplicationUrl
        {
            get => applicationUrl;
            set
            {
                applicationUrl = value;
            }
        }
        public string AppIconPath
        {
            get => appIconPath;
            set
            {
                appIconPath = value;
            }
        }
        /// <summary>
        /// app输出
        /// </summary>
        public string AppOutPath
        {
            get => appOutPath;
            set
            {
                appOutPath = value;
            }
        }
        public DescModel<CompressionAlgoType> ComPressAlgo
        {
            get => comPressAlgo;
            set
            {
                comPressAlgo = value;

            }
        }
        /// <summary>
        /// 用户选择
        /// </summary>
        public DescModel<UserFaceType> UserFaceSelectItem
        {
            get => userFaceSelectItem;
            set
            {
                userFaceSelectItem = value;
            }
        }

        /// <summary>
        /// 系统目录
        /// </summary>
        public string SystemDir { get; set; }
        /// <summary>
        /// 用户目录
        /// </summary>
        public string UserDirPath { get; set; }
        /// <summary>
        /// 允许用户更改目录
        /// </summary>
        public bool CanUserChangeDir { get; set; }
        /// <summary>
        /// 授权文件
        /// </summary>
        public string LicenseFilePath { get; set; }
        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonType ButtonType { get; set; }
        /// <summary>
        /// 工作路径
        /// </summary>
        public string WorkSpace { get; set; }
        /// <summary>
        /// 安装图标ICO
        /// </summary>
        public string InstallIconPath
        {
            get => installIconPath;
            set => installIconPath = value;
        }


        /// <summary>
        /// 卸载图标
        /// </summary>
        public string UnInstallIconPath { get; set; }
        /// <summary>
        /// 语言列表
        /// </summary>
        public List<InstallLanguageModel> LanguageList { get; set; }

        /// <summary>
        /// 显示语言
        /// </summary>
        public List<LanguageModel> DisplayLanguageList { get; set; }
        /// <summary>
        /// 语言路径
        /// </summary>
        public string LanguagePath { get; set; }
        /// <summary>
        /// 启用授权
        /// </summary>
        public bool IsLicenseChecked { get; set; }
    }
}
