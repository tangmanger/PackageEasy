using PackageEasy.Domain.Interfaces;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Services
{
    /// <summary>
    /// author:TT
    /// time:2023-03-11 23:19:30
    /// desc:DataService
    /// </summary>
    public class DataService : IDataService
    {
        public string ProjectKey { get; set; }

        public event Action<string, string> CreateProject;
        public event Action PreCompile;
        public event Action LanguageChanged;
        public event Action TargetPathChanged;
        public event Action<AssemblyFileModel, string> AssemblyItemChanged;

        public void OnCreateProject(string name, string key)
        {
            CreateProject?.Invoke(name, key);
        }

        public void OnLanguageChanged()
        {
            LanguageChanged?.Invoke();
        }

        public void OnPreCompile()
        {
            PreCompile?.Invoke();
        }

        public void OnSelectedAssemblyItemChanged(AssemblyFileModel assemblyFile, string key)
        {
            AssemblyItemChanged?.Invoke(assemblyFile, key);
        }

        public void OnTargetPathChanged()
        {
            TargetPathChanged?.Invoke();
        }
    }
}
