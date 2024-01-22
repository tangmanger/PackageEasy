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
        public abstract string CompilerName { get; }

        public abstract List<string> Build(ProjectInfoModel projectInfoModel, ViewType viewType);
        public abstract List<string> Build(ProjectInfoModel projectInfoModel);
        public abstract Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel, ViewType viewType);
        public abstract Task<List<string>> BuildAsync(ProjectInfoModel projectInfoModel);
        public abstract string GetParams<T>(T t);
        public  string OutPutFilePath { get; set; }
    }
}
