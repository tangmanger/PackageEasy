﻿using PackageEasy.ViewModels;
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

namespace PackageEasy.Views
{
          /// <summary>
         /// 作者：TT
    /// 时间：2023-03-12 22:49:08
    /// 描述： AssemblyView.xaml 的交互逻辑、
    /// TANGMANGER
    /// </summary>
    public partial class AssemblyView : UserControl
    {
        public AssemblyView()
        {
            InitializeComponent();
        }

        private void DataGridRow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dataGridRow = sender as DataGridRow;
            if (dataGridRow != null)
            {
                dataGridRow.IsSelected = true;
            }
        }

        private void datagrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
