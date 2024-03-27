using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.ViewModels
{
    public class SecurityViewModel : BaseProjectViewModel
    {
        public SecurityViewModel() { }
        public SecurityViewModel(ViewType viewType, string key) : base(viewType, key)
        {
        }
    }
}
