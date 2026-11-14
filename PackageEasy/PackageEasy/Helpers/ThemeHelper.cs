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
            //移除已加载的旧主题字典（ThemeId 与已加载字典不一致时也能正确移除）
            for (int i = App.Current.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                var d = App.Current.Resources.MergedDictionaries[i];
                if (d.Source != null && Themes.Exists(p => d.Source.ToString().Contains(p.ThemeName)))
                {
                    App.Current.Resources.MergedDictionaries.RemoveAt(i);
                }
            }
            var path = Path.Combine(DataHelper.Themes, themeModel.ThemeName);
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(path) });
        }
    }
}
