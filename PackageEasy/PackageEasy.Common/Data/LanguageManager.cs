using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;

namespace PackageEasy.Common.Data
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:36:32
    /// desc:LanguageManager
    /// </summary>
    public class LanguageManager : DependencyObject
    {
        public static List<WeakReference> LanguageControlList = new List<WeakReference>();
        public static string GetLangText(DependencyObject obj)
        {
            return (string)obj.GetValue(LangTextProperty);
        }

        public static void SetLangText(DependencyObject obj, string value)
        {
            obj.SetValue(LangTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for LangText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LangTextProperty =
            DependencyProperty.RegisterAttached("LangText", typeof(string), typeof(LanguageHelper), new PropertyMetadata(null, OnLangChanged));

        public static List<LanguageModel> languageModels = new List<LanguageModel>();
        private static object _obj = new object();
        static int index = 0;
        private static void OnLangChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj == null)
                return;
            var element = obj;

            if (e.NewValue == null)
                return;

            lock (_obj)
            {
                LanguageControlList.RemoveAll(c => !c.IsAlive || ReferenceEquals(c.Target, obj));
                WeakReference reference = new WeakReference(obj);
                LanguageControlList.Add(reference);
            }

            //if (e.NewValue != null)
            //    CheckLanguage(e.NewValue.ToString());

            string text = e.NewValue?.ToString().GetLangText();
            if (text == null)
                return;
            //if (!LanguageHelper.LanguageDic[LanguageType.en_SH].Exists(p => p.LanguageText == text) && !languageModels.Exists(p => p.LanguageText == text))
            //{
            //    var lang = LanguageHelper.LanguageDic[LanguageType.zh_CN].Find(p => p.LanguageText == text);
            //    if (lang != null)
            //    {
            //        var en = LanguageHelper.LanguageDic[LanguageType.en_SH].Exists(p => p.Id == lang.Id);
            //        if (!en)
            //        {
            //            index = LanguageHelper.LanguageDic[LanguageType.zh_CN].FindAll(p => p.LanguageType == LanguageType.zh_CN).Max(p => p.Id);
            //            if (languageModels.Count > 0)
            //                index = languageModels.Max(p => p.Id);
            //            index++;
            //            languageModels.Add(new LanguageModel() { Id = index, LanguageText = text, LanguageType = LanguageType.zh_CN });

            //            var languageFile = Path.Combine(DataHelper.Laguage, "language111.json");
            //            File.WriteAllText(languageFile, languageModels.SerializeObject());
            //        }
            //    }
            //    else
            //    {
            //        index = LanguageHelper.LanguageDic[LanguageType.zh_CN].FindAll(p => p.LanguageType == LanguageType.zh_CN).Max(p => p.Id);
            //        if (languageModels.Count > 0)
            //            index = languageModels.Max(p => p.Id);
            //        index++;
            //        languageModels.Add(new LanguageModel() { Id = index, LanguageText = text, LanguageType = LanguageType.zh_CN });

            //        var languageFile = Path.Combine(DataHelper.Laguage, "language111.json");
            //        File.WriteAllText(languageFile, languageModels.SerializeObject());
            //    }
            //}
            SetLanguage(element, text);
        }
        private static void SetLanguage(DependencyObject element, string text)
        {
            if (element is Window)
            {
                Window window = element as Window;
                window.Title = text;
            }
            else if (element is TextBlock)
            {
                TextBlock tb = element as TextBlock;
                tb.Text = text;
            }
            else if (element is Run)
            {
                Run run = element as Run;
                run.Text = text;
            }
            else if (element is GridViewColumn)
            {
                GridViewColumn gridViewColumn = element as GridViewColumn;
                gridViewColumn.Header = text;
            }
            else if (element is DataGridColumn)
            {
                DataGridColumn dataGridColumn = element as DataGridColumn;
                dataGridColumn.Header = text;
            }
            else if (element is HeaderedContentControl)
            {
                HeaderedContentControl header = element as HeaderedContentControl;
                header.Header = text;
            }
            else if (element is MenuItem)
            {
                MenuItem menu = element as MenuItem;
                menu.Header = text;
            }
            else if (element is TextBox)
            {
                TextBox infoTextBox = element as TextBox;
                infoTextBox.Text = text;
            }
            else if (element is DatePicker)
            {
                DatePicker datePicker = element as DatePicker;

                if (LanguageHelper.CurrentLanguageType == LanguageType.Zh_CN)
                {
                    datePicker.Language = System.Windows.Markup.XmlLanguage.GetLanguage("zh-CN");
                }
                else if (LanguageHelper.CurrentLanguageType == LanguageType.En_SH)
                {
                    datePicker.Language = System.Windows.Markup.XmlLanguage.GetLanguage("En");
                }

            }
            else if (element is ContentControl)
            {
                ContentControl cc = element as ContentControl;
                cc.Content = text;
            }
        }



        public static EventHandler<LanguageSourceArgs> LanguageSourceChanged;

        public static void RefreshLanguage(LanguageSourceArgs sourceArgs)
        {
            lock (_obj)
            {
                var list = LanguageControlList.ToList();
                foreach (var item in list)
                {
                    try
                    {
                        if (!item.IsAlive)
                        {
                            LanguageControlList.Remove(item);
                            continue;
                        }
                        var control = item.Target as DependencyObject;
                        if (control == null)
                            continue;
                        var langText = GetLangText(control);
                        var text = langText.GetLangText();
                        SetLanguage(control, text);
                        LanguageSourceChanged?.Invoke(null, sourceArgs);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
