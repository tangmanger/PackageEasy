using PackageEasy.Common.Data;
using PackageEasy.Controls.Controls;
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
    /// AboutControl.xaml 的交互逻辑
    /// </summary>
    public partial class AboutControl : BaseControl
    {
        public AboutControl()
        {
            InitializeComponent();
        }

        public override string Description => "关于PackageEasy".GetLangText();

        public override bool ShowSureButton => false;
        public override void Load()
        {
        }

        public override void Save()
        {
        }

        public override void Unload()
        {
        }
    }
}
