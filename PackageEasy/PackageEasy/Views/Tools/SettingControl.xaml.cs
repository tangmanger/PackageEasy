using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain.Common;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;
using PackageEasy.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LanguageHelper = PackageEasy.Helpers.LanguageHelper;

namespace PackageEasy.Views.Tools
{
    /// <summary>
    /// SettingControl.xaml 的交互逻辑
    /// </summary>
    public partial class SettingControl : BaseControl
    {
        private string makensisPath;
        private string nSISHelperPath;
        private List<ThemeModel> themeList;
        private ThemeModel currentTheme;
        private List<LanguageTypeModel> languageTypes;
        private LanguageTypeModel selectedLanguageType;

        public SettingControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        public override string Description => CommonSettings.ToolSettingControl.GetLangText();
        public override void Load()
        {
            LanguageHelper.LanguageChanged += LanguageHelper_LanguageChanged;
            MakensisPath = ConfigHelper.Config.NSISMakePath ?? "";
            NSISHelperPath = ConfigHelper.Config.NSISHelperPath ?? "";
            LanguageHelper_LanguageChanged();
        }

        private void LanguageHelper_LanguageChanged()
        {
            ThemeList = new List<ThemeModel>();
            foreach (var item in ThemeHelper.Themes)
            {
                ThemeList.Add(new ThemeModel()
                {
                    ThemeDescription = item.ThemeDescription.GetLangText(),
                    ThemeId = item.ThemeId,
                    ThemeName = item.ThemeName,
                });
            }
            ThemeList = new List<ThemeModel>(ThemeList);
            CurrentTheme = ThemeList.Find(p => p.ThemeId == ConfigHelper.Config.ThemeId) ?? ThemeList.FirstOrDefault() ?? new ThemeModel();
            LanguageTypes = new List<LanguageTypeModel>();
            foreach (var item in LanguageHelper.LanguageTypes)
            {
                LanguageTypes.Add(new LanguageTypeModel()
                {
                    DisplayName = item.DisplayName.GetLangText(),
                    FilePath = item.FilePath,
                    Id = item.Id,
                });
            }
            LanguageTypes = new List<LanguageTypeModel>(LanguageTypes);
            SelectedLanguageType = LanguageTypes.Find(p => p.Id == ConfigHelper.Config.Lang);
        }


        public override void Save()
        {

        }
        public override void Unload()
        {
            LanguageHelper.LanguageChanged -= LanguageHelper_LanguageChanged;
        }

        #region 属性

        /// <summary>
        /// 编译路径
        /// </summary>
        public string MakensisPath
        {
            get => makensisPath;
            set
            {
                makensisPath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 帮助目录
        /// </summary>
        public string NSISHelperPath
        {
            get => nSISHelperPath;
            set
            {
                nSISHelperPath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 主题列表
        /// </summary>
        public List<ThemeModel> ThemeList
        {
            get => themeList;
            set
            {
                themeList = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 当前主题
        /// </summary>
        public ThemeModel CurrentTheme
        {
            get => currentTheme;
            set
            {
                currentTheme = value;
                if (value != null && ConfigHelper.Config.ThemeId != value.ThemeId)
                {
                    ConfigHelper.Config.ThemeId = value.ThemeId;
                    ConfigHelper.Save(true);
                    ThemeHelper.UpdateTheme(value);
                }
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 多语言列表
        /// </summary>

        public List<LanguageTypeModel> LanguageTypes
        {
            get => languageTypes;
            set
            {
                languageTypes = value;
                RaisePropertyChanged();

            }
        }

        /// <summary>
        /// 选择类型
        /// </summary>
        public LanguageTypeModel SelectedLanguageType
        {
            get => selectedLanguageType;
            set
            {
                selectedLanguageType = value;
                if (value != null && ConfigHelper.Config.Lang != value.Id)
                {
                    LanguageHelper.SetLangType(value);
                }
                RaisePropertyChanged();
            }
        }

        #endregion


        #region 命令

        /// <summary>
        /// 设置路径
        /// </summary>
        public RelayCommand SetNisiCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var filePath = openFileDialog.FileName;
                if (File.Exists(filePath))
                {
                    ConfigHelper.Config.NSISMakePath = filePath;
                    ConfigHelper.Save(true);
                    MakensisPath = ConfigHelper.Config.NSISMakePath ?? "";
                }
            }
        });

        /// <summary>
        /// 编译路径
        /// </summary>
        public RelayCommand HelperCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var filePath = openFileDialog.FileName;
                if (File.Exists(filePath))
                {
                    ConfigHelper.Config.NSISHelperPath = filePath;
                    ConfigHelper.Save(true);
                    NSISHelperPath = ConfigHelper.Config.NSISHelperPath ?? "";
                }
            }
        });


        #endregion

    }
}
