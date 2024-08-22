using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Enums
{
    public enum FileMenuOperateType
    {
        Install,
        QuietInstall,
        IsExistNoNeedCopy,
        IsNoNeedCopy,
        IsNoNeedDelete,
        Ignore,
        UseCustomPath
    }
}
