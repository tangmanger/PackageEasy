using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-23 23:35:41
    /// desc:AssemblyModel
    /// </summary>
    public class AssemblyModel : BaseModel
    {
        private string assemblyName;
        private string assemblyDescription;
        private bool isSelected;
        private List<AssemblyFileModel> fileList;

        /// <summary>
        /// 选择
        /// </summary>
        [JsonIgnore]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 选择路径
        /// </summary>
        public string SelectDir { get; set; } = "";
        /// <summary>
        /// 组件Id
        /// </summary>
        public int AssemblyId { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        public string AssemblyName
        {
            get => assemblyName;
            set
            {
                assemblyName = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 组件描述
        /// </summary>
        public string AssemblyDescription
        {
            get => assemblyDescription;
            set
            {
                assemblyDescription = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<AssemblyFileModel> FileList
        {

            get => fileList;
            set
            {
                fileList = value;
                RaisePropertyChanged();
            }
        }
        public string SectionName { get; set; }
    }
}
