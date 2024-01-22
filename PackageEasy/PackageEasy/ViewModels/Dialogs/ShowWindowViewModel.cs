using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackageEasy.Controls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PackageEasy.ViewModels.Dialogs
{
    internal class ShowWindowViewModel : ObservableObject
    {
        public ShowWindowViewModel(BaseControl baseControl)
        {
            if (baseControl != null)
            {
                WorkView = baseControl;
                ShowSureVisibility = baseControl.ShowSureButton ? Visibility.Visible : Visibility.Collapsed;
                Title = baseControl.Description;
                baseControl?.Load();
            }
        }

        #region 属性

        private string title;
        private BaseControl workView;
        private Visibility showSureVisibility;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public BaseControl WorkView
        {
            get => workView;
            set
            {
                workView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 展示确定按钮
        /// </summary>
        public Visibility ShowSureVisibility
        {
            get => showSureVisibility;
            set
            {
                showSureVisibility = value;
                OnPropertyChanged();
            }
        }


        #endregion


        #region 命令

        /// <summary>
        /// 保存
        /// </summary>
        public RelayCommand SaveCommand => new RelayCommand(() =>
        {
            WorkView?.Save();

        });

        /// <summary>
        /// 卸载
        /// </summary>

        public RelayCommand UnloadCommand => new RelayCommand(() =>
        {
            WorkView?.Unload();
        });

        #endregion
    }
}
