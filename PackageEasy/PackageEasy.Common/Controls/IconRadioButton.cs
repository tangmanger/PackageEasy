using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PackageEasy.Common.Controls
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:24:58
    /// desc:IconRadioButton
    /// </summary>
    public class IconRadioButton : RadioButton
    {


        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconRadioButton));


        public bool IsShowClose
        {
            get { return (bool)GetValue(IsShowCloseProperty); }
            set { SetValue(IsShowCloseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowClose.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowCloseProperty =
            DependencyProperty.Register("IsShowClose", typeof(bool), typeof(IconRadioButton), new PropertyMetadata(true));






        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CloseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(IconRadioButton));



    }
}
