using PackageEasy.Domain.Attributes;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageEasy.Common.Data;
using System.IO;
using PackageEasy.Common.Helpers;

namespace PackageEasy.PlugIns
{
    [PlugIn("进程检测", "\ue675", typeof(NProcess))]
    public class NProcess : BasePlugInModel
    {
        public string Description => "NProcess";

        public string Icon => "\ue675";

        public override string Uid => uid;
        string uid = Guid.NewGuid().ToString();
        public override Tuple<bool, string> Execute()
        {
            var filePath = Path.Combine(DataHelper.Plugin, "nsProcess.zip");
            if (!File.Exists(filePath))
            {
                return new Tuple<bool, string>(true, "文件不存在!".GetLangText());
            }
            var tempDir = DataHelper.Temp;
            var result = ZipHelper.UnZipFile(filePath, "", tempDir);
            if (!result.Item1)
            {
                return result;
            }
            var rootDir = new FileInfo(ConfigHelper.Config.NSISMakePath).Directory;
            var files = Directory.GetFiles(tempDir);
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    if (fileInfo.Name == "nsProcess.nsh")
                    {
                        var targetFilePath = Path.Combine(rootDir.FullName, "Include", fileInfo.Name);
                        File.Copy(file, targetFilePath, true);
                    }
                    else
                    {
                        var targetFilePath = Path.Combine(rootDir.FullName, "Plugins", "x86-unicode", fileInfo.Name);
                        File.Copy(file, targetFilePath, true);
                    }
                }
            }
            return new Tuple<bool, string>(true, "安装成功!".GetLangText());

        }

        public override void Loaded()
        {
        }

        public override void Unloaded()
        {
        }
    }
}
