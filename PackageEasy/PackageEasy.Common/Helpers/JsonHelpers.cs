using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:53:01
    /// desc:JsonHelpers
    /// </summary>
    public static class JsonHelpers
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(this object obj, Formatting formatting=Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
