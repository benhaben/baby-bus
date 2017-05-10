using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using BabyBus.Core.Helper;

namespace BabyBus.API.Controllers.Common
{
    public class ImageController : ApiController
    {
        private const string PhotoSuffix = ".png";
        private const string ImagePath = "ImageUpload";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ImageController)); 
        public ApiResponser Post()
        {
            

            var response = new ApiResponser();
            try
            {
                var guid = Guid.NewGuid();
                var fileName = guid + PhotoSuffix;
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagePath);
                if (!Directory.Exists(ImagePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                WriteImage(Path.Combine(filePath,fileName));

                response.Status = true;
                response.Attach = fileName;
                return response;
            }
            catch (Exception ex)
            {
                log.Debug(ex.ToString());
                response.Status = false;
                response.Message = ex.Message;
                return response;
            }
        }

        private async void WriteImage(string filePath)
        {
            var content = Request.Content;
            var str = await content.ReadAsStringAsync();
            //log.Debug(String.Format("Bytes Length: {0}",bytes.Length));
            var ms = new MemoryStream(Convert.FromBase64String(str));
            Image img = Image.FromStream(ms);
            
            img.Save(filePath,ImageFormat.Png);
            //流用完要及时关闭  
            ms.Close(); 
        }
    }
}
