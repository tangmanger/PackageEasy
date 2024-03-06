using PackageEasy.Common.Helpers;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasyTest
{
    [TestClass]
    public class ThemeTest
    {
        [TestMethod]
        public void SaveTheme()
        {
            List<ThemeModel> Themes = new List<ThemeModel>();
            Themes.Add(new ThemeModel { ThemeId = "default", ThemeDescription = "默认主题", ThemeName = "DefaultColor.xaml" });
            var themeFile = Path.Combine(DataHelper.Themes, "ThemeConfig.json");
            File.WriteAllText("ThemeConfig.json", Themes.SerializeObject());
        }
    }
}
