using PackageEasy.Common.Data;
using PackageEasy.Domain;
using PackageEasy.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static bool CheckIsChanged(this ProjectInfoModel projectInfoModel, ProjectInfoModel oldProjectInfo)
        {

            return CheckChanged(projectInfoModel, oldProjectInfo);
        }
        public static bool CheckChanged<T>(T model, T old)
        {
            if (model == null) return true;
            var modelProperties = model.GetType().GetProperties().ToList();
            foreach (var property in modelProperties)
            {
                //不进行保存校验
                var attribute = property.GetCustomAttribute(typeof(SaveIgnoreAttribute));
                if (attribute != null)
                {
                    continue;
                }
                if (property.PropertyType == (typeof(string)) || property.PropertyType == typeof(bool)
                        || property.PropertyType == typeof(double) || property.PropertyType == typeof(int) || property.PropertyType.IsEnum || property.PropertyType == typeof(DateTime))
                {
                    var newValue = property.GetValue(model);
                    var oldValue = property.GetValue(old);
                    if (newValue != null && !newValue.Equals(oldValue))
                    {
                        return true;
                    }
                }
                else if (IsEnumerable(property.PropertyType))
                {
                    object value = property.GetValue(model, null);
                    object oldValue = property.GetValue(old, null);
                    if (value != null && oldValue != null)
                    {
                        Type objType = value.GetType();
                        Type oldType = oldValue.GetType();
                        int count = Convert.ToInt32(objType.GetProperty("Count").GetValue(value, null));
                        int oldCount = Convert.ToInt32(objType.GetProperty("Count").GetValue(oldValue, null));
                        if (count != oldCount) return true;
                        for (int i = 0; i < count; i++)
                        {


                            object listItem = objType.GetProperty("Item").GetValue(value, new object[] { i });
                            var oldListItem = oldType.GetProperty("Item").GetValue(oldValue, new object[] { i });
                            if (CheckChanged(listItem, oldListItem))
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (property is PropertyInfo)
                {

                    var data = property.GetValue(model, null);
                    var oldData = property.GetValue(old, null);
                    var flage = CheckChanged(data, oldData);
                    if (flage)
                        return true;
                }
                else
                {


                }
            }
            return false;
        }
        /// <summary>
        /// 判断是不是列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(Type type)
        {
            if (type.IsArray)
            {
                return true;
            }
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
            {
                return true;
            }
            foreach (var it in type.GetInterfaces())
                if (it.IsGenericType && typeof(IEnumerable<>) == it.GetGenericTypeDefinition())
                    return true;
            return false;
        }
        /// <summary>
        /// 获取相对目录
        /// </summary>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public static string GetWorkSpace(this ProjectInfoModel projectInfo)
        {
            if (projectInfo.BaseInfo == null) return null;
            if (!projectInfo.BaseInfo.IsUseRelativePath) return projectInfo.BaseInfo.WorkSpace;

            FileInfo fileInfo = new FileInfo(projectInfo.ExtraInfo.FilePath);
            return Path.Combine(fileInfo.Directory.FullName, projectInfo.BaseInfo.WorkSpace);
        }
    }
}
