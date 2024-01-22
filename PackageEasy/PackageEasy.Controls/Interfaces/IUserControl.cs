using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Controls.Interfaces
{
    public interface IUserControl
    {
        /// <summary>
        /// 加载和
        /// </summary>
        void Load();
        /// <summary>
        /// 卸载
        /// </summary>
        void Unload();
        /// <summary>
        /// 保存
        /// </summary>
        void Save();
        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }
    }
}
