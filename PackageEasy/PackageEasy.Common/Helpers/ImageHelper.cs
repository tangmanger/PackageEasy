using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PackageEasy.Common.Helpers
{
    public class ImageHelper
    {
        /// <summary>
        /// 转图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string? IcoToPng(string path)
        {
            if (!File.Exists(path)) return null;
            var filePath = Path.Combine(DataHelper.Temp, Guid.NewGuid().ToString() + ".png");
            // 加载ICO文件  
            using (Image icoImage = Image.FromFile(path))
            {
                using (Bitmap pngImage = new Bitmap(icoImage.Width, icoImage.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(pngImage))
                    {
                        graphics.DrawImage(icoImage, new Rectangle(0, 0, icoImage.Width, icoImage.Height));
                        pngImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            if (File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                return null;
            }
        
        }
        /// <summary>
        /// 转换到base64
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string? ImageToBase64(string path)
        {
            if (!File.Exists(path)) return null;
            using (Image image = Image.FromFile(path))
            {



                // 创建MemoryStream  
                using (MemoryStream ms = new MemoryStream())
                {
                    // 将图片保存到MemoryStream中  
                    image.Save(ms, image.RawFormat);

                    // 将MemoryStream中的数据转换为字节数组  
                    byte[] imageBytes = ms.ToArray();

                    // 将字节数组转换为Base64字符串  
                    string base64String = Convert.ToBase64String(imageBytes);

                    return base64String;
                }
            }
        }

    }
}
