using PackageEasy.Common.Data;
using PackageEasy.Controls.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using System.Windows.Xps.Packaging;

namespace PackageEasy.Views.Tools
{
    /// <summary>
    /// ScriptControl.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptControl : BaseControl
    {
        public ScriptControl(string path)
        {
            InitializeComponent();
            doc.Text = File.ReadAllText(path);// TxtToFlowDocument(File.ReadAllText(path));
        }
        public static FlowDocument TxtToFlowDocument(string text)
        {
            var document = new FlowDocument();
            using (var stream = new MemoryStream((new UTF8Encoding()).GetBytes(text)))
            {
                var txt = new TextRange(document.ContentStart, document.ContentEnd);
                txt.Load(stream, DataFormats.Text);
            }
            return document;
        }
        public override string Description => "脚本预览".GetLangText();

        public override void Load()
        {
        }

        public override void Save()
        {
        }

        public override void Unload()
        {
        }

        private void doc_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                doc.ScrollToHorizontalOffset(doc.HorizontalOffset - e.Delta);
            }
            else
            {
                doc.ScrollToVerticalOffset(doc.VerticalOffset - e.Delta);
            }
        }
    }
}
