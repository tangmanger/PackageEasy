using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models.Compares
{
    public class TargetPathCompare : IEqualityComparer<TargetPathModel>
    {
        public bool Equals(TargetPathModel? x, TargetPathModel? y)
        {
            if (x == null || y == null) return false;
            return x.DisplayName == y.DisplayName;
        }

        public int GetHashCode([DisallowNull] TargetPathModel obj)
        {
            return obj?.ToString()?.GetHashCode() ?? Guid.NewGuid().GetHashCode();
        }
    }
}
