using PackageEasy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PackageEasy.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 22:35:09
    /// desc:ViewCaheModel
    /// </summary>
    public class ViewCaheModel
    {
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectKey { get; set; }
        /// <summary>
        /// View
        /// </summary>
        public UserControl ProjectView { get; set; }
        /// <summary>
        /// model
        /// </summary>
        public BaseProjectViewModel BaseProjectViewModel { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}
