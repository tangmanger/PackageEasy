using PackageEasy.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Helpers
{
    public class NSISHelper
    {
        public static void Init()
        {
            string nisiPath = @"C:\Program Files (x86)\NSIS\makensis.exe";
            string helper = @"C:\Program Files (x86)\NSIS\NSIS.chm";

            if (File.Exists(nisiPath) && string.IsNullOrWhiteSpace(ConfigHelper.Config.NSISMakePath))
            {
                ConfigHelper.Config.NSISMakePath = nisiPath;
                ConfigHelper.Save(true);
            }
            if (File.Exists(helper) && string.IsNullOrWhiteSpace(ConfigHelper.Config.NSISHelperPath))
            {
                ConfigHelper.Config.NSISHelperPath = helper;
                ConfigHelper.Save(true);
            }
        }
    }
}
