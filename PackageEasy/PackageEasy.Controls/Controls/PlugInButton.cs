using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PackageEasy.Controls.Controls
{
    public class PlugInButton:Button
    {




        public PlugInState PlugInState
        {
            get { return (PlugInState)GetValue(PlugInStateProperty); }
            set { SetValue(PlugInStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlugInState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlugInStateProperty =
            DependencyProperty.Register("PlugInState", typeof(PlugInState), typeof(PlugInButton));




    }
}
