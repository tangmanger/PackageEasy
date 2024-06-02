using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PackageEasy.Domain.Common
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
        /// <summary>
        /// 无
        /// </summary>
        public const string BaseInfoFaceTypeNo = "BaseInfoFaceTypeNo";
        /// <summary>
        /// 现代
        /// </summary>
        public const string BaseInfoFaceTypeMorden = "BaseInfoFaceTypeMorden";
        /// <summary>
        /// 传统
        /// </summary>
        public const string BaseInfoFaceTypeClassic = "BaseInfoFaceTypeClassic";
        /// <summary>
        /// 当前语言文件格式不正确,请参考示例！
        /// </summary>
        public const string BaseInfoLangFormatError = "BaseInfoLangFormatError";
        /// <summary>
        /// 请先进行保存！
        /// </summary>
        public const string BaseInfoSaveBefore = "BaseInfoSaveBefore";

        #endregion

        #region 基础界面

        /// <summary>
        /// 新工程*
        /// </summary>
        public const string BaseViewNewProject = "BaseViewNewProject";

        #endregion


        #region 应用图标信息

        /// <summary>
        /// 应用程序\"开始菜单\"文件夹名称不能为空!
        /// </summary>
        public const string AppIconStartMenuNameTips = "AppIconStartMenuNameTips";
        /// <summary>
        /// 目标文件夹名称不能为空
        /// </summary>
        public const string AppIconTargetDirNotNull = "AppIconTargetDirNotNull";
        /// <summary>
        /// 快捷方式名称不能为空
        /// </summary>
        public const string AppIconShortcutNotNull = "AppIconShortcutNotNull";
        /// <summary>
        /// 目的文件不能为空
        /// </summary>
        public const string AppIconTargetFileNotNull = "AppIconTargetFileNotNull";
        /// <summary>
        /// 请选择要删除的项！
        /// </summary>
        public const string AppIconSelectDelItem = "AppIconSelectDelItem";

        #endregion

        #region 程序集信息

        /// <summary>
        /// 新建组
        /// </summary>
        public const string AssemblyNewSection = "AssemblyNewSection";
        /// <summary>
        /// 工作目录为空
        /// </summary>
        public const string AssemblyWorkSpaceNotNull = "AssemblyWorkSpaceNotNull";
        /// <summary>
        /// 请选择组
        /// </summary>
        public const string AssemblyChooseSection = "AssemblyChooseSection";
        /// <summary>
        /// 当前没有文件列表
        /// </summary>
        public const string AssemblyNullFileList = "AssemblyNullFileList";
        /// <summary>
        /// 当前文件{0}为空
        /// </summary>
        public const string AssemblyFileIsNull = "AssemblyFileIsNull";
        /// <summary>
        /// 无法在工作空间找到文件{0}
        /// </summary>
        public const string AssemblyFileNotExist = "AssemblyFileNotExist";


        #endregion

        #region 结束界面

        /// <summary>
        /// 你确实要完全移除 $(^Name) ，其及所有的组件？
        /// </summary>
        public const string EndUninstallTip = "EndUninstallTip";
        /// <summary>
        /// $(^Name) 已成功地从你的计算机移除。
        /// </summary>
        public const string EndUninstallMsg = "EndUninstallMsg";
        /// <summary>
        /// 卸载程序检测到 ${PRODUCT_NAME} 正在运行，是否强行关闭并继续卸载?
        /// </summary>
        public const string EndUninstallProcessTips = "EndUninstallProcessTips";
        /// <summary>
        /// 安装程序检测到 ${PRODUCT_NAME} 正在运行，是否强行关闭并继续安装?
        /// </summary>
        public const string EndInstallProcessTips = "EndInstallProcessTips";
        /// <summary>
        /// 结束页面：程序不能为空！
        /// </summary>
        public const string EndApplicationNameIsNotNull = "EndApplicationNameIsNotNull";
        /// <summary>
        /// 结束页面：检测的进程名不能空！
        /// </summary>
        public const string EndProcessNameIsNotNull = "EndProcessNameIsNotNull";
        /// <summary>
        /// 结束页面：卸载提示不能为空！
        /// </summary>
        public const string EndUninstallProcessTipsIsNotNull = "EndUninstallProcessTipsIsNotNull";
        /// <summary>
        /// 结束页面：安装提示不能为空！
        /// </summary>
        public const string EndInstallProcessTipsIsNotNull = "EndInstallProcessTipsIsNotNull";
        /// <summary>
        /// 结束页面：自述文件不能为空！
        /// </summary>
        public const string EndReadMeNameIsNotNull = "EndReadMeNameIsNotNull";

        #endregion

        #region 多语言

        /// <summary>
        /// 多语言：所属语言不能为空！
        /// </summary>
        public const string LangIsNotNull = "LangIsNotNull";
        /// <summary>
        /// 多语言：目标文件不能为空！
        /// </summary>
        public const string LangAssemblyFileIsNotNull = "LangAssemblyFileIsNotNull";
        /// <summary>
        /// 多语言：目标根目录不能为空！
        /// </summary>
        public const string LangTargetPathIsNotNull = "LangTargetPathIsNotNull";
        /// <summary>
        /// 多语言：目标目录不能为空！
        /// </summary>
        public const string LangTargetDirIsNotNull = "LangTargetDirIsNotNull";

        #endregion

        #region 注册界面

        /// <summary>
        /// 注册界面：请填写关联的程序名称！
        /// </summary>
        public const string RegistryRegistryFormatIsNotNull = "RegistryRegistryFormatIsNotNull";
        /// <summary>
        /// 注册界面：请填写密码！
        /// </summary>
        public const string RegistryPasswordIsNotNull = "RegistryPasswordIsNotNull";

        #endregion

        #region 工具

        /// <summary>
        /// 关于PackageEasy
        /// </summary>
        public const string ToolAbout = "ToolAbout";
        /// <summary>
        /// 插件设置
        /// </summary>
        public const string ToolPlugInManager = "ToolPlugInManager";
        /// <summary>
        /// 脚本预览
        /// </summary>
        public const string ToolScriptControl = "ToolScriptControl";
        /// <summary>
        /// 设置
        /// </summary>
        public const string ToolSettingControl = "ToolSettingControl";
        /// <summary>
        /// 默认主题
        /// </summary>
        public const string ThemeDefault = "ThemeDefault";
        /// <summary>
        /// 黑色主题
        /// </summary>
        public const string ThemeBlack = "ThemeBlack";
        /// <summary>
        /// 保存成功！
        /// </summary>
        public const string SaveSuccess = "SaveSuccess";
        /// <summary>
        /// 保存失败！
        /// </summary>
        public const string SaveFail = "SaveFail";
        /// <summary>
        /// 打开文件发生错误！
        /// </summary>
        public const string OpenFileError = "OpenFileError";
        /// <summary>
        /// 当前标签 {0} 未保存,是否保存？
        /// </summary>
        public const string TableNotSave = "TableNotSave";
        /// <summary>
        /// 确定
        /// </summary>
        public const string Yes = "Yes";
        /// <summary>
        /// 否
        /// </summary>
        public const string No = "No";
        /// <summary>
        /// 取消
        /// </summary>
        public const string Cancel = "Cancel";

        /// <summary>
        /// 无
        /// </summary>
        public const string Null = "Null";
        /// <summary>
        /// 简体中文
        /// </summary>
        public const string SampleChinese = "SampleChinese";
        /// <summary>
        /// 英文
        /// </summary>
        public const string English = "English";
        /// <summary>
        /// 进程检测
        /// </summary>
        public const string ProcessCheck = "ProcessCheck";
        /// <summary>
        /// 文件不存在！
        /// </summary>
        public const string FileNoExist = "FileNoExist";
        /// <summary>
        /// 安装成功！
        /// </summary>
        public const string InstallSuccess = "InstallSuccess";
        /// <summary>
        /// 设置忽略成功！
        /// </summary>
        public const string AssemblyIgnoreSuccess = "AssemblyIgnoreSuccess";
        /// <summary>
        /// 取消忽略成功！
        /// </summary>

        public const string AssemblyUnIgnoreSuccess = "AssemblyUnIgnoreSuccess";

        #endregion



    }
}
