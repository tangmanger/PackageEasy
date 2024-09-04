using PackageEasy.Domain.Enums;
using PackageEasy.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    public class StoreHelper
    {
        const string TARGETFILENAME = "TargetFiles.json";


        static string TargetFilePath = Path.Combine(DataHelper.Store, TARGETFILENAME);

        /// <summary>
        /// 本地目标目录
        /// </summary>
        static List<TargetPathModel> LocalTargetPaths = new List<TargetPathModel>();
        /// <summary>
        /// 读取目录
        /// </summary>
        public static List<TargetPathModel> ReadLocalTargetFiles()
        {
            if (File.Exists(TargetFilePath))
            {
                LocalTargetPaths = File.ReadAllText(TargetFilePath).DeserializeObject<List<TargetPathModel>>();
            }
            else
            {
                foreach (var target in Enum.GetValues(typeof(TargetDirType)))
                {
                    TargetDirType targetDirType = (TargetDirType)target;
                    LocalTargetPaths.Add(new TargetPathModel()
                    {
                        CreateTime = DateTime.Now,
                        DisplayName = $"${targetDirType.ToString()}",
                        IsDefault = true,
                        IsUserCreated = false,
                        TargetPath = $"${targetDirType.ToString()}",
                        UpdateTime = DateTime.Now,
                    });
                }
                var data = LocalTargetPaths.SerializeObject();
                File.WriteAllText(TargetFilePath, data);
            }
            return LocalTargetPaths;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="localTargetPaths"></param>
        public static void SetLocalTargetPaths(List<TargetPathModel> localTargetPaths)
        {
            if (localTargetPaths == null || localTargetPaths.Count == 0) return;
            LocalTargetPaths = localTargetPaths;
            var data = LocalTargetPaths.SerializeObject();
            File.WriteAllText(TargetFilePath, data);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="localTargetPaths"></param>
        public static void UpdateLocalTargetPaths(List<TargetPathModel> localTargetPaths)
        {
            if (localTargetPaths == null || localTargetPaths.Count == 0) return;
            var allNewData = localTargetPaths.FindAll(p => !LocalTargetPaths.Exists(c => c.DisplayName == p.DisplayName));
            if (allNewData == null || allNewData.Count == 0) return;
            LocalTargetPaths.AddRange(allNewData);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="targetPath"></param>
        public static void DelLocalTargetPath(TargetPathModel targetPath)
        {
            if (targetPath == null) return;
            var data = LocalTargetPaths.Find(c => c.DisplayName == targetPath.DisplayName);
            if (data != null)
            {
                LocalTargetPaths.Remove(data);
            }
            var dataStr = LocalTargetPaths.SerializeObject();
            File.WriteAllText(TargetFilePath, dataStr);
        }
    }
}
