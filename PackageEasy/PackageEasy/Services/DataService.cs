using PackageEasy.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void OnCreateProject(string name, string key)
        {
            CreateProject?.Invoke(name, key);
        }

        public void OnPreCompile()
        {
            PreCompile?.Invoke();
        }
    }
}
