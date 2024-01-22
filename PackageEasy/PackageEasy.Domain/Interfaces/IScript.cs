using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Interfaces
{
    public interface IScript
    {
        /// <summary>
        /// 编译脚本
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        List<string> Build(ProjectInfoModel projectInfoModel, ViewType viewType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel, ViewType viewType);
        /// <summary>
        /// 编译脚本
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        List<string> Build(ProjectInfoModel projectInfoModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel);
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        string GetParams<T>(T t);
        /// <summary>
        /// 编译
        /// </summary>
        string CompilerName { get; }
    }
}
