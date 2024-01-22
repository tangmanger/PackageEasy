using PackageEasy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Domain.Models
{
    /// <summary>
    /// author:TT
    /// time:2023-03-10 23:50:25
    /// desc:LanguageSourceArgs
    /// </summary>
    public class LanguageSourceArgs : EventArgs
    {
        public LanguageSourceArgs(LanguageType oldValue, LanguageType newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
        public LanguageSourceArgs(string projectKey, LanguageType oldValue, LanguageType newValue)
        {
            ProjectKey = projectKey;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string ProjectKey { get; set; }

        public LanguageType OldValue { get; set; }
        public LanguageType NewValue { get; set; }
    }
}
