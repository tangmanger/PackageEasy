using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Interfaces
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 23:16:48
    /// desc:IDataService
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// 项目key
        /// </summary>
        string ProjectKey { get; set; }
        /// <summary>
        /// 语言变化
        /// </summary>
        event Action LanguageChanged;
        /// <summary>
        /// 编译前事件
        /// </summary>
        event Action PreCompile;
        /// <summary>
        /// 创建对象
        /// </summary>
        event Action<string, string> CreateProject;
        /// <summary>
        /// 目标路径变化
        /// </summary>
        event Action TargetPathChanged;
        /// <summary>
        /// 创建实验
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        void OnCreateProject(string name, string key);
        /// <summary>
        /// 编译前事件
        /// </summary>
        void OnPreCompile();

        /// <summary>
        /// 多语言变化
        /// </summary>
        void OnLanguageChanged();

        /// <summary>
        /// 目标路径变化
        /// </summary>
        void OnTargetPathChanged();
    }
}
