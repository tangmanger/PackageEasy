using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain.Models;
using PackageEasy.Helpers;
using System;
using System.Collections.Generic;
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

        public SettingControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        public override string Description => "工具设置".GetLangText();
        public override void Load()
        {
            MakensisPath = ConfigHelper.Config.NSISMakePath ?? "";
            NSISHelperPath = ConfigHelper.Config.NSISHelperPath ?? "";
            ThemeList = ThemeHelper.Themes;
            CurrentTheme = ThemeList.Find(p => p.ThemeId == ConfigHelper.Config.ThemeId) ?? ThemeList.FirstOrDefault() ?? new ThemeModel();
        }
        public override void Save()
        {

        }
        public override void Unload()
        {
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

        #endregion

    }
}
