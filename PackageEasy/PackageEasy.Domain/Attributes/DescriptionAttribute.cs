using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Attributes
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:41:11
    /// desc:DescriptionAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        public string Key { get; }

        public DescriptionAttribute(string key)
        {
            Key = key;
        }
    }
}
