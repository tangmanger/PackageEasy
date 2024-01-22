using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 22:54:42
    /// desc:TableModel
    /// </summary>
    public class TableModel : ObservableObject
    {
        private string projectName;

        /// <summary>
        /// 实验key
        /// </summary>
        public string ProjectKey { get; set; }
        /// <summary>
        /// 实验名称
        /// </summary>
        public string ProjectName
        {
            get => projectName;
            set
            {
                projectName = value;
                OnPropertyChanged(nameof(ProjectName));

            }
        }
        private bool _isActive;

        /// <summary>
        /// 是否是激活
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }
    }
}
