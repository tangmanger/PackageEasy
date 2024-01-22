using PackageEasy.Controls.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PackageEasy.Controls.Controls
{
    public abstract class BaseControl : UserControl, IUserControl, INotifyPropertyChanged
    {
        public BaseControl() { }
        public abstract string Description { get; }
        public virtual bool ShowSureButton { get; } = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract void Load();
        public abstract void Save();
        public abstract void Unload();
        public void RaisePropertyChanged([CallerMemberName] string str = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
    }
}
