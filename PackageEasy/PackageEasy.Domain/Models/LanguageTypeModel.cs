using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    public class LanguageTypeModel
    {

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FilePath { get; set; }
    }
}
