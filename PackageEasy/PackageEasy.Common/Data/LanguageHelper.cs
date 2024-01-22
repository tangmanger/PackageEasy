using PackageEasy.Common.Helpers;
using PackageEasy.Common.Logs;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Data
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:36:00
    /// desc:LanguageHelper
    /// </summary>
    public static class LanguageHelper
    {

        /// <summary>
        /// 语言dic
        /// </summary>
        public static Dictionary<LanguageType, List<Domain.Models.LanguageModel>> LanguageDic = new Dictionary<LanguageType, List<LanguageModel>>();
        /// <summary>
        /// 语言类型
        /// </summary>
        public static LanguageType CurrentLanguageType { get; set; } = LanguageType.Zh_CN;

        /// <summary>
        /// 初始化lang
        /// </summary>
        public static void InitLanguage(LanguageType languageType = LanguageType.En_SH)
        {
            //var languageFilse = Path.Combine(DataHelper.Laguage, "language111.json");
            //var languageModel = File.ReadAllText(languageFilse).DeserializeObject<List<LanguageModel>>();
            //languageModel=languageModel.Distinct(new hhh()).ToList();
            //File.WriteAllText(languageFilse, languageModel.SerializeObject());

            CurrentLanguageType = languageType;
            var languageFile = Path.Combine(DataHelper.Language, "language.json");
            if (!File.Exists(languageFile))
            {
                Log.Write($"语言文件{languageFile}不存在，多语言切换无效!");
                return;
            }
            var data = File.ReadAllText(languageFile);
            var langList = data.DeserializeObject<List<LanguageModel>>();
            if (langList == null)
            {
                Log.Write($"多语言文件已经损坏！");
                return;
            }
            //var list = AddIinit(langList);
            //File.WriteAllText(languageFile + "1111", list.SerializeObject());
            //foreach (var lang in langList)
            //{
            //    if (lang.LanguageType == LanguageType.en_SH)
            //    {
            //        var datad = lang.LanguageText.Split(' ');
            //        string str = "";
            //        foreach (string str2 in datad)
            //        {
            //            var cc = str2.ToArray();
            //            for (int i = 0; i < cc.Length; i++)
            //            {
            //                if (i == 0)
            //                    str += cc[i].ToString().ToUpper();
            //                else
            //                    str += cc[i].ToString();
            //            }
            //            if (datad.Length > 1)
            //                str += " ";
            //        }
            //        lang.LanguageText = str;
            //    }
            //}
            //File.WriteAllText(languageFile, langList.SerializeObject());
            LanguageDic = langList.GroupBy(p => p.LanguageType).ToDictionary(s => s.Key, m => m.ToList());
        }
        //static List<LanguageModel> AddIinit(List<LanguageModel> languageModels)
        //{
        //    string str = @"C:\Users\Tangmanger\Documents\WeChat Files\tangmiger\FileStorage\File\2022-08\当前文件可能已经被移动或删除!.ini";
        //    var data = File.ReadAllLines(str);
        //    var ing = languageModels.FindAll(p => p.LanguageType == LanguageType.en_SH);
        //    var ch = languageModels.FindAll(p => p.LanguageType == LanguageType.zh_CN);
        //    var maId = ch.Max(p => p.Id);
        //    foreach (var item in data)
        //    {
        //        var dd = item.Split(':');
        //        maId = maId + 1;
        //        languageModels.Add(new LanguageModel() { Id = maId, LanguageType = LanguageType.zh_CN, LanguageText = dd[0] });
        //        languageModels.Add(new LanguageModel() { Id = maId, LanguageType = LanguageType.en_SH, LanguageText = dd[1] });
        //    }

        //    return languageModels;
        //}
        /// <summary>
        /// /获取语言
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="oldLangType">切换前语言</param>
        /// <returns></returns>
        public static string GetLangText(this string key, LanguageType oldLangType)
        {
            //切换前么有语言 则返回当前译文
            if (oldLangType == LanguageType.None)
            {
                return GetLangText(key);
            }
            if (!LanguageDic.ContainsKey(CurrentLanguageType)) return key;
            var langList = LanguageDic[CurrentLanguageType];
            if (langList == null || langList.Count == 0) return key;
            var old = LanguageDic[oldLangType].Find(p => p.LanguageText == key);
            if (old == null) return key;
            var other = langList.Find(p => p.Id == old.Id);
            if (other == null)
            {
                //SSSS();
                return key;
            }
            return other.LanguageText;
        }
        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLangText(this string key)
        {
            if (CurrentLanguageType == LanguageType.Zh_CN) return key;
            if (!LanguageDic.ContainsKey(CurrentLanguageType)) return key;
            var langList = LanguageDic[CurrentLanguageType];
            if (langList == null || langList.Count == 0) return key;
            if (!LanguageDic.ContainsKey(LanguageType.Zh_CN)) return key;

            var chinese = LanguageDic[LanguageType.Zh_CN].Find(p => p.LanguageText == key);
            if (chinese == null)
            {
                //SSSS();
                return key;
            }
            var other = langList.Find(p => p.Id == chinese.Id);
            if (other == null)
            {
                //SSSS();
                return key;
            }
            return other.LanguageText;

            
        }



    }
}
