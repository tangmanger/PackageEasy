using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;


namespace PackageEasy.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlugInAttribute : Attribute
    {

        /// <summary>
        /// 插件
        /// </summary>
        public PlugInModel PlugIn { get; set; }
   
        /// <summary>
        /// 插件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="icon"></param>
        /// <param name="type"></param>
        public PlugInAttribute(string name, string icon, Type type,string uid)
        {
            PlugIn = new PlugInModel();
            PlugIn.Name = name;
            PlugIn.Icon = icon;
            PlugIn.PlugInType = type;
            PlugIn.Uid = uid;
        }
    }
}
