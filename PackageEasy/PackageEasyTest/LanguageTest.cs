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
    public class LanguageTest
    {
        [TestMethod]
        public void BuildLanguage()
        {
            List<LanguageModel> languages = new List<LanguageModel>();
            languages.Add(new LanguageModel { LanguageText = "主程序", Id = 0, LanguageType = PackageEasy.Domain.Enums.LanguageType.Zh_CN });
            languages.Add(new LanguageModel { LanguageText = "Main Program", Id = 0, LanguageType = PackageEasy.Domain.Enums.LanguageType.En_SH });
            File.WriteAllText("a.json", languages.SerializeObject());
        }
    }
}
