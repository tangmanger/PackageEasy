using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common;
using PackageEasy.Common.Data;
using PackageEasy.Common.Helpers;
using PackageEasy.Controls.Controls;
using PackageEasy.Domain;
using PackageEasy.Domain.Interfaces;
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
    /// TargetPathControl.xaml 的交互逻辑
    /// </summary>
    public partial class TargetPathControl : BaseControl
    {
        private List<TargetPathModel> targetPaths;

        public TargetPathControl()
        {
            InitializeComponent();
            DataContext = this;
            SaveLocalVisibility = Visibility.Collapsed;
        }
        ProjectInfoModel _projectInfo;
        List<TargetPathModel> allFiles;
        private string searchText;
        private string targetDisplayPath;
        private string targetPath;
        private Visibility addTargetPathVisibility;
        private bool isSaveToLocal;
        private Visibility saveLocalVisibility;

        public TargetPathControl(ProjectInfoModel projectInfo)
        {
            InitializeComponent();
            DataContext = this;
            _projectInfo = projectInfo;
            SaveLocalVisibility = Visibility.Visible;
        }
        public override void Load()
        {
            AddTargetPathVisibility = Visibility.Collapsed;
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
                if (IsSaveToLocal)
                {
                    StoreHelper.UpdateLocalTargetPaths(allFiles);
                    StoreHelper.SetLocalTargetPaths(allFiles);
                }
                IDataService dataService = ServiceHelper.GetService(_projectInfo.ProjectKey);
                if (dataService != null)
                    dataService.OnTargetPathChanged();
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
        /// <summary>
        /// 显示目录
        /// </summary>
        public string TargetDisplayPath
        {
            get => targetDisplayPath;
            set
            {
                targetDisplayPath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 目标目录
        /// </summary>
        public string TargetPath
        {
            get => targetPath;
            set
            {
                targetPath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 新增是否可见
        /// </summary>
        public Visibility AddTargetPathVisibility
        {
            get => addTargetPathVisibility;
            set
            {
                addTargetPathVisibility = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 保存到本地
        /// </summary>
        public bool IsSaveToLocal
        {
            get => isSaveToLocal;
            set
            {
                isSaveToLocal = value;
                RaisePropertyChanged();
            }
        }

        public Visibility SaveLocalVisibility
        {
            get => saveLocalVisibility;
            set
            {
                saveLocalVisibility = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region 命令

        public RelayCommand<TargetPathModel> DelTargetCommand => new RelayCommand<TargetPathModel>((s) =>
        {

            var c = allFiles.FirstOrDefault(c => c.DisplayName == s.DisplayName);
            if (c != null)
            {
                allFiles.Remove(c);
            }

            Query();

        });

        /// <summary>
        /// <summary>
        public RelayCommand QueryCommand => new RelayCommand(() =>
        {

            Query();
        });

        public RelayCommand AddCommand => new RelayCommand(() =>
        {
            AddTargetPathVisibility = Visibility.Visible;
        });

        public RelayCommand SureCommand => new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(TargetDisplayPath))
            {
                TMessageBox.ShowMsg(string.Format("{0}不能为空！".GetLangText(), "展示目录".GetLangText()));
                return;
            }
            if (string.IsNullOrWhiteSpace(TargetPath))
            {
                TMessageBox.ShowMsg(string.Format("{0}不能为空！".GetLangText(), "目标目录".GetLangText()));
                return;
            }
            if (allFiles != null && allFiles.Count > 0 && allFiles.Exists(c => c.DisplayName == targetDisplayPath))
            {

                TMessageBox.ShowMsg(string.Format("{0}已经存在！".GetLangText(), "展示目录".GetLangText()));
                return;
            }
            TargetPathModel targetPathModel = new TargetPathModel();
            targetPathModel.CreateTime = DateTime.Now;
            targetPathModel.UpdateTime = DateTime.Now;
            targetPathModel.TargetPath = TargetPath;
            targetPathModel.IsDefault = false;
            targetPathModel.DisplayName = TargetDisplayPath;
            targetPathModel.IsUserCreated = true;
            if (allFiles == null)
                allFiles = new List<TargetPathModel>();
            allFiles.Add(targetPathModel);
            Query();
            AddTargetPathVisibility = Visibility.Collapsed;
        });

        public RelayCommand CancelCommand => new RelayCommand(() =>
        {

            AddTargetPathVisibility = Visibility.Collapsed;
        });

        #endregion
    }
}
