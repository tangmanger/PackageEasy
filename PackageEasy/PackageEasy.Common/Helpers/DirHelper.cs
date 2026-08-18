using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    public static class DirHelper
    {
        public static List<string> GetDirs(this string path, bool isDir)
        {
            var dirs = new List<string>();
            var dirPaths = path.Split('\\');
            string str = "";
            for (int i = 0; i < dirPaths.Length; i++)
            {
                if (!isDir && i == dirPaths.Length - 1)
                    break;
                str = Path.Combine(str, dirPaths[i]);
                dirs.Add(str);
            }

            return dirs;
        }
    }
}
