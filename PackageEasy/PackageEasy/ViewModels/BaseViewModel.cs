using CommunityToolkit.Mvvm.ComponentModel;
using PackageEasy.Common.Data;
using PackageEasy.Helpers;
using PackageEasy.Models;
using PackageEasy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels
{
    /// <summary>
    /// author:TT
    /// time:2023-03-15 23:12:29
    /// desc:BaseViewModel
    /// </summary>
    public class BaseViewModel: ObservableObject
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        public ProjectViewModel CreateProject()
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            ProjectView projectView = new ProjectView();
            projectView.DataContext = projectViewModel;
            projectViewModel.ProjectName = "新工程*".GetLangText();
            ViewCaheModel viewCaheModel = new ViewCaheModel();
            viewCaheModel.ProjectKey = projectViewModel.Key;
            viewCaheModel.BaseProjectViewModel = projectViewModel;
            viewCaheModel.ProjectView = projectView;

            CacheDataHelper.ProjectCahes.Add(projectViewModel.Key, viewCaheModel);
            return projectViewModel;
           
        }
    }
}
