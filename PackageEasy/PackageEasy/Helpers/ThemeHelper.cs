using PackageEasy.Common.Helpers;
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
    public class ThemeHelper
    {
        public static List<ThemeModel> Themes = new List<ThemeModel>();
        /// <summary>
        /// 初始化主题
        /// </summary>
        public static void InitTheme()
        {
            Themes = new List<ThemeModel>();
            var themeFile = Path.Combine(DataHelper.Themes, "ThemeConfig.json");

            if (!string.IsNullOrWhiteSpace(themeFile) && File.Exists(themeFile))
            {
                var themes = File.ReadAllText(themeFile).DeserializeObject<List<ThemeModel>>();
                if (themes != null)
                {
                    foreach (var theme in themes)
                    {
                        var filePath = Path.Combine(DataHelper.Themes, theme.ThemeName);
                        if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                            Themes.Add(theme);
                    }
                }
            }
         
        }
        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="themeModel"></param>
        public static void UpdateTheme(ThemeModel themeModel)
        {
            var currentTheme = Themes.Find(p => p.ThemeId == ConfigHelper.Config.ThemeId);
            if (currentTheme != null)
            {
                for (int i = 0; i < App.Current.Resources.MergedDictionaries.Count; i++)
                {
                    var d = App.Current.Resources.MergedDictionaries[i];
                    if (d.Source != null && d.Source.ToString().Contains(currentTheme.ThemeName))
                    {
                        App.Current.Resources.MergedDictionaries.RemoveAt(i);
                        break;
                    }
                }
            }
            var path = Path.Combine(DataHelper.Themes, themeModel.ThemeName);
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(path) });
        }
    }
}
