using PackageEasy.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public abstract class BasePlugInModel : ITPlugIn
    {
        public abstract string Uid { get; }

        public abstract Tuple<bool, string> Execute();
        public abstract void Loaded();
        public abstract void Unloaded();
    }
}
