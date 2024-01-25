using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Interfaces
{
    /// <summary>
    /// T插件
    /// </summary>
    public interface ITPlugIn
    {
        string Uid { get; }
        /// <summary>
        /// 载入
        /// </summary>
        void Loaded();
        /// <summary>
        /// 卸载
        /// </summary>
        void Unloaded();
        /// <summary>
        /// 执行
        /// </summary>
        Tuple<bool,string> Execute();

    }
}
