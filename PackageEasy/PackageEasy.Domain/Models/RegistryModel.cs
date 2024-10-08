﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023/12/19 13:32:59
    /// desc:RegistryModel
    /// </summary>
    public class RegistryModel
    {
        /// <summary>
        /// 注册文件格式
        /// </summary>
        public string RegistryFormat { get; set; }
        /// <summary>
        /// 作为可选zujian
        /// </summary>
        public bool IsAsSelected { get; set; } = true;
        /// <summary>
        /// 注册进程名
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 是否启用密码
        /// </summary>
        public bool IsUsePassword { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
