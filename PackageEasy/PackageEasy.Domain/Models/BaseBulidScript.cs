using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public abstract class BaseBuildScript : IScript
    {
        /// <summary>
        /// 编译器名称
        /// </summary>
        public abstract string CompilerName { get; }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public abstract List<string> Build(ProjectInfoModel projectInfoModel, ViewType viewType);
        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        public abstract List<string> Build(ProjectInfoModel projectInfoModel);
        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public abstract Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel, ViewType viewType);
        /// <summary>
        /// 编译异步
        /// </summary>
        /// <param name="projectInfoModel"></param>
        /// <returns></returns>
        public abstract Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel);
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public abstract string GetParams<T>(T t);
        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutPutFilePath { get; set; }
    }
}
