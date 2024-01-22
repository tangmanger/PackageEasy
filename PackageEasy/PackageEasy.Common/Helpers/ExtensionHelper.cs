using PackageEasy.Common.Data;
using PackageEasy.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2021/10/19 14:17:49
    /// desc:ExtensionHelper
    /// </summary>
    public static class ExtensionHelper
    {
        public static string GetDescription(this object myEnum)
        {
            Type type = myEnum.GetType();
            FieldInfo info = type.GetField(myEnum.ToString());
            DescriptionAttribute descriptionAttribute = info.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;

            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Key.GetLangText();
            }
            else
                return type.ToString();
        }
    }
}
