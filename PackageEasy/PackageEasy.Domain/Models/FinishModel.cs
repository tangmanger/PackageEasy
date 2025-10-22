using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class FinishModel
    {
        /// <summary>
        /// 安装完自动运行
        /// </summary>
        public bool IsAutoRun { get; set; }
        /// <summary>
        /// 要运行的名称
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 自述文件名称
        /// </summary>
        public string ReadMeFileName { get; set; }
        /// <summary>
        /// 启动参数
        /// </summary>
        public string RunParam { get; set; }
        /// <summary>
        /// 解除信息
        /// </summary>
        public string UninstallMsg { get; set; }
        /// <summary>
        /// 解除提示
        /// </summary>
        public string UninstallTip { get; set; }
        /// <summary>
        /// 启用进程检测
        /// </summary>
        public bool IsEnableProcess { get; set; }
        /// <summary>
        /// 进程检测提示
        /// </summary>
        public string UninstallProcessTips { get; set; }
        /// <summary>
        /// 安装提示
        /// </summary>
        public string InstallProcessTips { get; set; }
        /// <summary>
        /// 进程名
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 展示自述
        /// </summary>
        public bool IsShowReadme { get; set; }
        /// <summary>
        /// 卸载前执行脚本
        /// </summary>
        public string UnInstallApplication { get; set; }
        /// <summary>
        /// 启用卸载前事件
        /// </summary>
        public bool IsBeforeUnInstall { get; set; }
        /// <summary>
        /// 静默安装
        /// </summary>
        public bool IsQuietInstall { get; set; }
    }
}
