using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common
{
    public class CommonSettings
    {
        #region 主界面

        /// <summary>
        /// 是否转换成相对路径？
        /// </summary>
        public const string MainExportScriptTips = "MainExportScriptTips";
        /// <summary>
        /// 保存失败！
        /// </summary>
        public const string MainSaveFailTips = "MainSaveFailTips";
        /// <summary>
        /// 导出脚本成功！
        /// </summary>
        public const string MainExportSuccessTips = "MainExportSuccessTips";
        /// <summary>
        /// 导出脚本失败！
        /// </summary>
        public const string MainExportFailTips = "MainExportFailTips";
        /// <summary>
        /// 编译成功！
        /// </summary>
        public const string MainCompileSuccessTips = "MainCompileSuccessTips";
        /// <summary>
        /// 编译失败！
        /// </summary>
        public const string MainCompileFailTips = "MainCompileFailTips";
        /// <summary>
        /// 编译失败！{0}
        /// </summary>
        public const string MainCompileFailExtTips = "MainCompileFailExtTips";

        #endregion

        #region 主页界面

        /// <summary>
        /// 当前文件已打开！
        /// </summary>
        public const string HomeFileHasOpened = "HomeFileHasOpened";
        /// <summary>
        /// 当前文件{0}不存在,是否从最近打开移除？
        /// </summary>
        public const string HomeFileNoExist = "HomeFileNoExist";

        #endregion

        #region 基础信息界面

        /// <summary>
        /// 英文
        /// </summary>
        public const string BaseInfoEnglish = "BaseInfoEnglish";
        /// <summary>
        /// 中文
        /// </summary>
        public const string BaseInfoChinese = "BaseInfoChinese";
        /// <summary>
        /// "应用程序名不能为空！
        /// </summary>
        public const string BaseInfoAppNameNotEmpty = "BaseInfoAppNameNotEmpty";
        /// <summary>
        /// "文件版本格式必须为X.X.X.X (X为数字)！
        /// </summary>
        public const string BaseInfoFileVersionFormat = "BaseInfoFileVersionFormat";
        /// <summary>
        /// 输出文件名称不能为空！
        /// </summary>
        public const string BaseInfoOutPutFileName = "BaseInfoOutPutFileName";
        /// <summary>
        /// 授权不存在！
        /// </summary>
        public const string BaseInfoLicenseFileNotExist = "BaseInfoLicenseFileNotExist";
        /// <summary>
        /// 应用程序图标不存在！
        /// </summary>
        public const string BaseInfoAppIconNotExist = "BaseInfoAppIconNotExist";
        /// <summary>
        /// 卸载图标不存在！
        /// </summary>
        public const string BaseInfoUnInstallAppIconNotExist = "BaseInfoUnInstallAppIconNotExist";
        /// <summary>
        /// 安装包图标不存在！
        /// </summary>
        public const string BaseInfoInstallAppIconNotExist = "BaseInfoInstallAppIconNotExist";

        #endregion

        /// <summary>
        /// 应用程序\"开始菜单\"文件夹名称不能为空!
        /// </summary>
        public const string AppIconStartMenuNameTips = "AppIconStartMenuNameTips";
    }
}
