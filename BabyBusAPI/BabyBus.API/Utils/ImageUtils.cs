using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.API.Utils
{
    public class ImageUtils
    {

        /// <summary>
        /// Save Image On Server
        /// </summary>
        /// <param name="base64Image">base64 Encode Image</param>
        /// <returns>File Name</returns>
        public static string SaveIamge(string base64Image)
        {
            var guid = Guid.NewGuid();
            var imagePath = Config.ImagePath;
            var fileName = guid + Config.PhotoSuffix;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var ms = new MemoryStream(Convert.FromBase64String(base64Image));
            Image img = Image.FromStream(ms);
            img.Save(Path.Combine(filePath,fileName),ImageFormat.Png);
            ms.Close();
            return fileName;
        }
    }
}
