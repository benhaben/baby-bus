using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Services {
    public interface IPictureService {
        //        Task<MemoryStream> ConvertImageSourceToMemoryStream(ImageSource imageSource);
        //        MemoryStream GetSmallStreamFromFile(string filePath, int reqWidth, int reqHeight, ref string newFilePaht);
        string GetMd5String(byte[] bytes);

        string SaveImageInFile(string fileName, Byte[] bytes);

        MemoryStream GetStreamFromFile(string filePath);

        /// <summary>
        /// Load Image From Local/Remote Source
        /// </summary>
        void LoadIamgeFromSource(string fileName, Action<Stream> iamgeAvailable, string path = Constants.ImageServerPath);
    }
}
