using CommunityToolkit.Mvvm.Input;
using PackageEasy.Common.Helpers;
using PackageEasy.Domain;
using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using PackageEasy.Enums;
using PackageEasy.Models;
using PackageEasy.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 22:22:54
    /// desc:ProjectViewModel
    /// </summary>
    public class ProjectViewModel : BaseProjectViewModel
    {
        Dictionary<ViewType, ViewCaheModel> ViewCaches = new Dictionary<ViewType, ViewCaheModel>();
        public ProjectViewModel()
        {
            _workView = new FrameworkElement();
            InitView();
            GoTo(ViewType.BaseInfoView);
        }

        #region 属性

        private FrameworkElement _workView;
        //// <summary>
        /// 界面
        /// </summary>
        public FrameworkElement WorkView
        {
            get
            {
                return _workView;
            }
            set
            {
                _workView = value;
                OnPropertyChanged();
            }
        }

        private string _projectName = "";
        /// <summary>
        /// 用户名称
        /// </summary>
        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region 命令

        /// <summary>
        /// 查看
        /// </summary>
        public RelayCommand<string> ViewCommand => new RelayCommand<string>((s) =>
        {
            int a;
            int.TryParse(s, out a);
            ViewType viewType = (ViewType)a;
            GoTo(viewType);
        });

        #endregion

        #region 方法

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitView()
        {
            ViewCaches.Clear();
            BaseInfoView baseInfoView = new BaseInfoView();
            BaseInfoViewModel baseInfoViewModel = new BaseInfoViewModel(ViewType.BaseInfoView, Key);
            baseInfoViewModel.ProjectInfo = ProjectInfo;
            baseInfoView.DataContext = baseInfoViewModel;
            ViewCaches.Add(ViewType.BaseInfoView, new ViewCaheModel() { BaseProjectViewModel = baseInfoViewModel, ProjectView = baseInfoView });


            AssemblyView assemblyView = new AssemblyView();
            AssemblyViewModel assemblyViewModel = new AssemblyViewModel(ViewType.AssemblyView, Key);
            assemblyView.DataContext = assemblyViewModel;
            assemblyViewModel.ProjectInfo = ProjectInfo;
            ViewCaches.Add(ViewType.AssemblyView, new ViewCaheModel() { BaseProjectViewModel = assemblyViewModel, ProjectView = assemblyView });
            AppIconInfoView appIconInfoView = new AppIconInfoView();
            AppIconInfoViewModel appIconInfoViewModel = new AppIconInfoViewModel(ViewType.AppIconInfoView, Key);
            appIconInfoView.DataContext = appIconInfoViewModel;
            appIconInfoViewModel.ProjectInfo = ProjectInfo;
            ViewCaches.Add(ViewType.AppIconInfoView, new ViewCaheModel() { BaseProjectViewModel = appIconInfoViewModel, ProjectView = appIconInfoView });

            EndView endView = new EndView();
            EndViewModel endViewModel = new EndViewModel(ViewType.EndView, Key);
            endView.DataContext = endViewModel;
            endViewModel.ProjectInfo = ProjectInfo;
            ViewCaches.Add(ViewType.EndView, new ViewCaheModel() { BaseProjectViewModel = endViewModel, ProjectView = endView });

            RegistryView registryView = new RegistryView();
            RegistryViewModel registryViewModel = new RegistryViewModel(ViewType.RegistryView, Key);
            registryView.DataContext = registryViewModel;
            registryViewModel.ProjectInfo = ProjectInfo;
            ViewCaches.Add(ViewType.RegistryView, new ViewCaheModel() { BaseProjectViewModel = registryViewModel, ProjectView = registryView });

            LanguageView languageView = new LanguageView();
            LanguageViewModel languageViewModel = new LanguageViewModel(ViewType.LanguageView, Key);
            languageView.DataContext = languageViewModel;
            languageViewModel.ProjectInfo = ProjectInfo;
            ViewCaches.Add(ViewType.LanguageView, new ViewCaheModel() { BaseProjectViewModel = languageViewModel, ProjectView = languageView });
            //暂时放到注册表界面去
            //SecurityView securityView = new SecurityView();
            //SecurityViewModel securityViewModel = new SecurityViewModel(ViewType.SecurityView, Key);
            //securityView.DataContext = securityViewModel;
            //securityViewModel.ProjectInfo = ProjectInfo;
            //ViewCaches.Add(ViewType.SecurityView, new ViewCaheModel() { BaseProjectViewModel = securityViewModel, ProjectView = securityView });
        }
        public override void RefreshData()
        {

            foreach (var item in ViewCaches)
            {
                if (item.Value.BaseProjectViewModel.ProjectInfo == null)
                    item.Value.BaseProjectViewModel.ProjectInfo = ProjectInfo;
                item.Value.BaseProjectViewModel.RefreshData();
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            foreach (var item in ViewCaches)
            {
                item.Value.BaseProjectViewModel.Dispose();
            }
        }
        public override void Save()
        {
            var baseInfoPath = Path.Combine(SavePath, "ExtraInfo.json");
            if (ProjectInfo == null)
                ProjectInfo = new ProjectInfoModel();
            File.WriteAllText(baseInfoPath, ProjectInfo.ExtraInfo.SerializeObject());
            foreach (var item in ViewCaches)
            {
                item.Value.BaseProjectViewModel.Save();
            }
        }
        /// <summary>
        /// 导航到
        /// </summary>
        /// <param name="viewType"></param>
        void GoTo(ViewType viewType)
        {
            if (ViewCaches.ContainsKey(viewType))
            {
                if (WorkView != null && WorkView.DataContext != null)
                {
                    INavigateOut? navigateOut = WorkView.DataContext as INavigateOut;
                    if (navigateOut != null)
                        navigateOut?.NavigateOut();
                }
                var vm = ViewCaches[viewType].BaseProjectViewModel;
                INavigateIn? navigateIn = vm as INavigateIn;
                if (navigateIn != null)
                    navigateIn?.NavigateIn();
                WorkView = ViewCaches[viewType].ProjectView;

            }
        }

        public override void Compile()
        {
            foreach (var item in ViewCaches)
            {
                item.Value.BaseProjectViewModel.Compile();
            }
        }
        public override bool ValidateData()
        {
            foreach (var item in ViewCaches)
            {
                if (!item.Value.BaseProjectViewModel.ValidateData()) return false;
            }
            return true;
        }

        #endregion

    }
}
