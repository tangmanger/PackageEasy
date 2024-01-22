using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

namespace PackageEasy.Common.Helpers
{
    /// <summary>
    /// author:TT
    /// time:2023-03-15 23:46:42
    /// desc:ZipHelper
    /// </summary>
    public static class ZipHelper
    {
        #region 压缩文件夹,支持递归
        public delegate void UpdateProcessHandler();
        /// <summary>
        /// 进度更新
        /// </summary>
        public static UpdateProcessHandler UpdateProcessCallBack;
        public static bool Compress(string dir, string targetFileName, string pwd = "",
            INotifyProressChanged proressChanged = null)
        {
            return Compress(dir, targetFileName, 5, pwd, proressChanged);
        }


        /// <summary>
        ///　压缩文件夹
        /// </summary>
        /// <param name="dir">待压缩的文件夹</param>
        /// <param name="targetFileName">压缩后文件路径（包括文件名）</param>
        /// <param name="level">压缩级别</param>
        /// <param name="pwd">压缩密码</param>
        /// <param name="proressChanged">通知进度变更</param>
        /// <returns></returns>
        public static bool Compress(string dir, string targetFileName, int level, string pwd,
            INotifyProressChanged proressChanged)
        {
            try
            {
                //如果已经存在目标文件，询问用户是否覆盖
                if (File.Exists(targetFileName))
                {
                    File.Delete(targetFileName);
                }

                string innerDir = dir;
                if (!File.Exists(dir) && Directory.Exists(dir))
                    innerDir = dir + @"\";
                else if (!File.Exists(dir) && !Directory.Exists(dir))
                {
                    return false;
                }

                if (dir != string.Empty)
                {
                    FileStream zipFile = File.Create(targetFileName);
                    ZipOutputStream zipStream = new ZipOutputStream(zipFile);
                    zipStream.Password = pwd;
                    zipStream.SetLevel(level);
                    try
                    {
                        _CompressFolder(dir, zipStream, innerDir, proressChanged);
                        zipStream.Finish();
                        zipStream.Close();
                    }
                    catch (Exception)
                    {
                        zipStream.Finish();
                        zipStream.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 压缩某个子文件夹
        /// </summary>
        /// <param name="basePath">被压缩的文件路径</param>
        /// <param name="zips"></param>
        /// <param name="zipfolername">zip文件中路径</param>
        /// <param name="proressChanged">zip文件中路径</param>
        private static void _CompressFolder(string basePath, ZipOutputStream zips, string zipfolername,
            INotifyProressChanged proressChanged)
        {
            if (File.Exists(basePath))
            {
                _CreateZipFile(basePath, zips, zipfolername, proressChanged);
                return;
            }
            string[] names = Directory.GetFiles(basePath);
            foreach (string fileName in names)
            {
                _CreateZipFile(fileName.Replace("\\", "/"), zips, zipfolername, proressChanged);
            }
            names = Directory.GetDirectories(basePath);
            foreach (string folderName in names)
            {
                _CompressFolder(folderName, zips, zipfolername, proressChanged);
            }
        }

        /// <summary>
        /// 压缩单独文件
        /// </summary>
        /// <param name="fileToZip">被压缩的文件路径</param>
        /// <param name="zips">被压缩的文件</param>
        /// <param name="zipfolername">写入路径</param>
        /// <param name="proressChanged">写入路径</param>
        private static void _CreateZipFile(string fileToZip, ZipOutputStream zips, string zipfolername,
            INotifyProressChanged proressChanged)
        {
            if (!File.Exists(fileToZip))
                return;
            try
            {
                FileStream streamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read, FileShare.Read);
                string temp = fileToZip;
                string temp1 = zipfolername;
                if (temp1.Length > 0)
                {
                    int i = temp1.LastIndexOf('\\') + 1;
                    int j = temp.Length - i;
                    temp = temp.Substring(i, j);
                }
                ZipEntry zipEn = new ZipEntry(temp);
                zipEn.IsUnicodeText = true;

                zips.PutNextEntry(zipEn);
                byte[] buffer = new byte[1024 * 10];
                while (true)
                {
                    int readSize = streamToZip.Read(buffer, 0, buffer.Length);
                    if (readSize > 0)
                    {
                        zips.Write(buffer, 0, readSize);
                        proressChanged?.ProgressChanged(readSize);
                    }
                    else
                        break;
                }
                streamToZip.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 递归解压缩zip文件

        /// <summary>
        /// 功能：递归解压文件
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="pwd"></param>
        /// <param name="unZipDir"></param>
        /// <returns></returns>
        public static Tuple<bool, string> UnZipFile(string zipFilePath, string pwd, string unZipDir = "")
        {
            string msg = string.Empty;
            return UnZipFile(zipFilePath, unZipDir, pwd, out msg);
        }

        /// <summary>  
        /// 功能：递归解压文件
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>  
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <param name="pwd">密码</param> 
        /// <param name="err">出错信息</param>  
        /// <returns>解压是否成功</returns>  
        public static Tuple<bool, string> UnZipFile(string zipFilePath, string unZipDir, string pwd, out string err)
        {
            err = "";
            if (zipFilePath == string.Empty)
            {
                err = "压缩文件不能为空！";
                return new Tuple<bool, string>(false, err);
            }
            if (!File.Exists(zipFilePath))
            {
                err = "压缩文件不存在！";
                return new Tuple<bool, string>(false, err);
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath),
                    Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("\\"))
                unZipDir += "\\";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    s.Password = pwd;
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (fileName != string.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {
                                byte[] data = new byte[16384];

                                while (true)
                                {
                                    int size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                            }
                            if (UpdateProcessCallBack != null)
                                UpdateProcessCallBack();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return new Tuple<bool, string>(false, err);
            }
            return new Tuple<bool, string>(true, err);
        } //解压结束

        /// <summary>
        /// 功能：读取压缩文件的全部目录信息
        /// </summary>  
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="pwd">压缩文件路径</param>  
        /// <returns>目录信息</returns>
        public static List<string> GetZipAllDirectories(string zipFilePath, string pwd = "")
        {
            List<string> dirs = new List<string>();
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    s.Password = pwd;
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                        dirs.Add(theEntry.Name);
                }
            }
            catch (Exception)
            {
                return new List<string>();
            }
            return dirs;
        }

        /// <summary>
        /// 功能：读取压缩文件的子目录信息
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="pwd"></param>
        /// <param name="innerPath"></param>
        /// <returns></returns>
        public static List<string> GetZipDirectories(string zipFilePath, string pwd = "", string innerPath = "")
        {
            List<string> dirs = new List<string>();
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    s.Password = pwd;
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                        if (theEntry.Name.StartsWith(innerPath))
                        {
                            string name = theEntry.Name.Substring(innerPath.Length);
                            string[] strs = name.Split('/');
                            string str = strs.Length == 1 ? strs[0] : strs[0] + "/";
                            str = innerPath + str;
                            if (!dirs.Contains(str))
                                dirs.Add(str);
                        }
                }
            }
            catch (Exception)
            {
                return new List<string>();
            }
            return dirs;
        }

        /// <summary>
        /// 从压缩文件中抽取文本内容，默认为UTF8编码
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="innerFilePath"></param>
        /// <returns></returns>
        public static string GetZipTextFile(string zipFilePath, string innerFilePath)
        {
            return GetZipTextFile(zipFilePath, innerFilePath, Encoding.UTF8);
        }

        /// <summary>
        /// 从压缩文件中抽取文本内容，需要指定内容编码格式
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="innerFilePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetZipTextFile(string zipFilePath, string innerFilePath, Encoding encoding)
        {
            string str = "";
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                        if (theEntry.Name == innerFilePath && theEntry.Name != String.Empty)
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                int size = 2048;
                                byte[] data = new byte[size];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        stream.Write(data, 0, size);
                                    else
                                        break;
                                }
                                str = encoding.GetString(stream.ToArray());
                            }
                            break;
                        }
                }
            }
            catch
            {
            }
            return str;
        }



        #endregion

        #region 分包 合包







        #endregion

        #region 数据压缩

        /// <summary>
        /// 将字节数组压缩，返回字节数组
        /// </summary>
        /// <param name="bytesToCompress"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytesToCompress)
        {
            MemoryStream ms = new MemoryStream();
            GZipOutputStream s = new GZipOutputStream(ms);
            s.Write(bytesToCompress, 0, bytesToCompress.Length);
            s.Close();
            byte[] rebyte = ms.ToArray();
            ms.Close();
            return rebyte;
        }

        /// <summary>
        /// 将字符串压缩返回字符串
        /// </summary>
        /// <param name="stringToCompress"></param>
        /// <returns></returns>
        public static string Compress(string stringToCompress)
        {
            byte[] compressedData = CompressToByte(stringToCompress);
            return Convert.ToBase64String(compressedData);
        }

        public static byte[] CompressToByte(string stringToCompress)
        {
            byte[] bytData = Encoding.Default.GetBytes(stringToCompress);
            return Compress(bytData);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DeCompress(byte[] data)
        {
            int orginalLen = data.Length;
            int maxDecompressLen = 20 * orginalLen;

            if (maxDecompressLen < 100000) //缓冲区最小100K,最大8M,原始数据如果大于25KB，则解压缓冲为20倍原始数据大小
            {
                maxDecompressLen = 100000;
            }
            if (maxDecompressLen > 8000000)
            {
                maxDecompressLen = 8000000;
            }
            byte[] decompressByteArr = new byte[maxDecompressLen];
            try
            {
                GZipInputStream s2 = new GZipInputStream(new MemoryStream(data));
                MemoryStream outStream = new MemoryStream();
                int read = s2.Read(decompressByteArr, 0, decompressByteArr.Length);
                while (read > 0)
                {
                    outStream.Write(decompressByteArr, 0, read);
                    read = s2.Read(decompressByteArr, 0, decompressByteArr.Length);
                }
                s2.Close();
                return outStream.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"字节数组解压缩异常，异常信息：{ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 获取数据压缩后的字节码 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static byte[] Compress(DataSet ds)
        {
            try
            {
                // 声明MemoryStream   
                MemoryStream ms = new MemoryStream();
                // 写入DataSet中的数据到ms中   
                //ds.WriteXml(ms, XmlWriteMode.WriteSchema);
                ds.WriteXml(ms);
                // ms转换为数组序列   
                byte[] bsrc = ms.ToArray();
                //关闭ms并释放资源   
                ms.Close();
                ms.Dispose();

                ms = new MemoryStream();
                ms.Position = 0;
                // 压缩数组序列中的数据   
                DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Compress);
                zipStream.Write(bsrc, 0, bsrc.Length);
                zipStream.Close();
                zipStream.Dispose();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对象压缩后的字节码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] NewCompress(string str)
        {
            try
            {
                // ms读取传入的str并转存如数组序列中   
                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms);
                sw.Write(str);
                sw.Close();
                byte[] bsrc = ms.ToArray();
                ms.Close();
                ms.Dispose();
                ms = new MemoryStream();
                ms.Position = 0;
                // 压缩数组序列并返回压缩后的数组   
                DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Compress);
                zipStream.Write(bsrc, 0, bsrc.Length);
                zipStream.Close();
                zipStream.Dispose();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取解压缩后的数据集
        /// </summary>
        /// <param name="arrbts"></param>
        /// <returns></returns>
        public static DataSet NewDeCompress(byte[] arrbts)
        {
            try
            {
                //    
                MemoryStream ms = new MemoryStream();
                ms.Write(arrbts, 0, arrbts.Length);
                ms.Position = 0;
                //   
                DeflateStream ZipStream = new DeflateStream(ms, CompressionMode.Decompress);
                MemoryStream UnzipStream = new MemoryStream();
                byte[] sDecompressed = new byte[128];
                while (true)
                {
                    int bytesRead = ZipStream.Read(sDecompressed, 0, 128);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    UnzipStream.Write(sDecompressed, 0, bytesRead);
                }
                ZipStream.Close();
                ms.Close();
                UnzipStream.Position = 0;
                DataSet ds = new DataSet();
                // 读取解压后xml数据   
                ds.ReadXml(UnzipStream);

                ds.AcceptChanges();     //更新所有行的状态为初始状态   
                return ds;
            }
            catch
            { return null; }
        }

        /// <summary>
        /// 获取解压缩后的字符串
        /// </summary>
        /// <param name="arrbts"></param>
        /// <returns></returns>
        public static string DeCompressString(byte[] arrbts)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(arrbts, 0, arrbts.Length);
                ms.Position = 0;
                DeflateStream ZipStream = new DeflateStream(ms, CompressionMode.Decompress);
                MemoryStream UnzipStream = new MemoryStream();
                byte[] sDecompressed = new byte[128];
                while (true)
                {
                    int bytesRead = ZipStream.Read(sDecompressed, 0, 128);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    UnzipStream.Write(sDecompressed, 0, bytesRead);
                }
                ZipStream.Close();
                ms.Close();
                UnzipStream.Position = 0;// 解压起始位置设置为头   
                StreamReader sr = new StreamReader(UnzipStream);
                return sr.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
    public interface INotifyProressChanged
    {
        void ProgressChanged(int changedValue);

        void ProgressChangedByWeight(long changedValue, long totalValue, int weight);

        bool AskContinue();
    }
}
