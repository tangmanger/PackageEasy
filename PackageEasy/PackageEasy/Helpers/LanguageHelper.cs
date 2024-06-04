using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy.Helpers
{
    internal class LanguageHelper
    {
        /// <summary>
        /// 多语言变化
        /// </summary>
        public static event Action LanguageChanged;
        public static List<LanguageTypeModel> LanguageTypes { get; private set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitLang()
        {
            var path = Path.Combine(DataHelper.Language, "language.json");
            if (File.Exists(path))
            {
                LanguageTypes = File.ReadAllText(path).DeserializeObject<List<LanguageTypeModel>>();
            }
            if (LanguageTypes == null)
                LanguageTypes = new List<LanguageTypeModel>();
        }
        /// <summary>
        /// 设置lang
        /// </summary>
        /// <param name="model"></param>
        public static void SetLangType(LanguageTypeModel model)
        {
            try
            {


                var currentTheme = LanguageTypes.Find(p => p.Id == ConfigHelper.Config.Lang);
                if (currentTheme != null)
                {
                    for (int i = 0; i < App.Current.Resources.MergedDictionaries.Count; i++)
                    {
                        var d = App.Current.Resources.MergedDictionaries[i];
                        if (d.Source != null && d.Source.ToString().Contains(currentTheme.FilePath))
                        {
                            App.Current.Resources.MergedDictionaries.RemoveAt(i);
                            break;
                        }
                    }
                }
                var path = Path.Combine(DataHelper.Language, model.FilePath);
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(path) });
                ConfigHelper.Config.Lang = model.Id;
                ConfigHelper.Save(true);
                LanguageChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Log.Write("设置多语言失败", ex);
            }
        }
    }
}
