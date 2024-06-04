using PackageEasy.Common.Helpers;
using PackageEasy.Domain.Common;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasyTest
{
    [TestClass]
    public class LanguageTest
    {
        [TestMethod]
        public void BuildLanguage()
        {
            //List<LanguageModel> languages = new List<LanguageModel>();
            //languages.Add(new LanguageModel { LanguageText = "主程序", Id = 0, LanguageType = PackageEasy.Domain.Enums.LanguageType.Zh_CN });
            //languages.Add(new LanguageModel { LanguageText = "Main Program", Id = 0, LanguageType = PackageEasy.Domain.Enums.LanguageType.En_SH });
            //File.WriteAllText("a.json", languages.SerializeObject());

           List<LanguageTypeModel> languageTypeModels = new List<LanguageTypeModel>();
            languageTypeModels.Add(new LanguageTypeModel { Id = 0, DisplayName = CommonSettings.SampleChinese, FilePath = "Zh-CN.xaml" });
            languageTypeModels.Add(new LanguageTypeModel { Id = 1, DisplayName = CommonSettings.English, FilePath = "En-US.xaml" });
            File.WriteAllText("language.json", languageTypeModels.SerializeObject());
        }
    }
}
