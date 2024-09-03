using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common.Helpers;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain;
using PackageEasy.Domain.Models;
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
    /// TargetPathControl.xaml 的交互逻辑
    /// </summary>
    public partial class TargetPathControl : BaseControl
    {
        private List<TargetPathModel> targetPaths;

        public TargetPathControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        ProjectInfoModel _projectInfo;
        List<TargetPathModel> allFiles;
        private string searchText;

        public TargetPathControl(ProjectInfoModel projectInfo)
        {
            InitializeComponent();
            DataContext = this;
            _projectInfo = projectInfo;
        }
        public override void Load()
        {
            if (_projectInfo != null)
            {
                if (_projectInfo.TargetPaths == null)
                    _projectInfo.TargetPaths = StoreHelper.ReadLocalTargetFiles();
                allFiles = _projectInfo.TargetPaths;
            }
            else
            {
                allFiles = StoreHelper.ReadLocalTargetFiles();
            }
            Query();
        }

        private void Query()
        {
            TargetPaths = new List<TargetPathModel>(allFiles);
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                TargetPaths = TargetPaths.FindAll(p => p.DisplayName.Contains(SearchText));
            }
        }

        public override void Unload()
        {
        }
        public override void Save()
        {
            if (_projectInfo != null)
            {
                _projectInfo.TargetPaths = allFiles;
            }
            else
            {
                StoreHelper.SetLocalTargetPaths(allFiles);
            }
        }
        public override string Description => "";

        #region 属性

        /// <summary>
        /// 目标目录
        /// </summary>
        public List<TargetPathModel> TargetPaths
        {
            get => targetPaths;
            set
            {
                targetPaths = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 搜索的名称
        /// </summary>
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region 命令

        public RelayCommand<TargetPathModel> DelTargetCommand => new RelayCommand<TargetPathModel>((s) =>
        {

            allFiles.Remove(s);
            Query();

        });

        /// <summary>
        /// 查询所有
        /// </summary>
        public RelayCommand QueryCommand => new RelayCommand(() =>
        {

            Query();
        });

        public RelayCommand AddCommand => new RelayCommand(() =>
        {

        });

        #endregion
    }
}
