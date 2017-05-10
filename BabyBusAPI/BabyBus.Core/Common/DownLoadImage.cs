using System;
using System.Drawing;
using System.Net;
using System.IO;

namespace BabyBus.Core.Common
{
    public class DownLoadImage 
    {
        /// <summary>
        /// 上传图片到服务器
        /// </summary>
        /// 
        /// <returns></returns>
        public static string GetImagePath(int managerId,string kindergartenName, string className, string url)
        {
            var file = managerId + "-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            HttpWebResponse response = null;

            var path = System.Web.HttpContext.Current.Server.MapPath("~/Image/") + kindergartenName + "-" + className + "\\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            try
            {
                var httpUrl = new Uri(url);
                var request = (HttpWebRequest)(WebRequest.Create(httpUrl));
                request.Timeout = 180000; //设置超时值10秒
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                request.Method = "GET";
                response = (HttpWebResponse)(request.GetResponse());
                var image = new Bitmap(response.GetResponseStream());
                image.Save(path + file + ".jpg");

                //string src = path + file + ".jpg";
                //string dest = path + file + "_thumb.jpg";
                //SaveThumb(src, dest);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (response != null) response.Close();
            }
            return "Image/" + kindergartenName + "-" + className + "/" + file + ".jpg";
        }
        public static void SaveThumb(string src,string dest)
        {
            const int thumbWidth = 90; //要生成的缩略图的宽度

            var image = Image.FromFile(src); //利用Image对象装载源图像

            //接着创建一个System.Drawing.Bitmap对象，并设置你希望的缩略图的宽度和高度。

            var srcWidth = image.Width;
            var srcHeight = image.Height;
            var thumbHeight = (srcHeight / srcWidth) * thumbWidth;
            var bmp = new Bitmap(thumbWidth, thumbHeight);

            //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。

            var gr = Graphics.FromImage(bmp);

            //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            //下面这个也设成高质量

            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

            //下面这个设成High

            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

            //把原始图像绘制成上面所设置宽高的缩小图

            var rectDestination = new Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

            //保存图像，大功告成！

            bmp.Save(dest);

            //最后别忘了释放资源
            bmp.Dispose();
            image.Dispose();
        }
    }
}
