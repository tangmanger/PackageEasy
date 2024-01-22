using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Interfaces
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 16:45:52
    /// desc:INavigateOut
    /// </summary>
    public interface INavigateIn<T>
    {
        /// <summary>
        /// 进入
        /// </summary>
        void NavigateIn(T param);


    }

    /// <summary>
    /// 无参数导航接口
    /// </summary>
    public interface INavigateIn
    {
        /// <summary>
        /// 进入
        /// </summary>
        void NavigateIn();


    }
    /// <summary>
    /// 无参数导出
    /// </summary>
    public interface INavigateOut
    {
        /// <summary>
        /// 离开
        /// </summary>
        void NavigateOut();
    }
}
