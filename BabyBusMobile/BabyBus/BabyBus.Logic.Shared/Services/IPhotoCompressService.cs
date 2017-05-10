using System;
using System.IO;

using System.Threading.Tasks;

namespace BabyBus.Logic.Shared
{
	public interface IPictureService
	{
		string GetMd5String(byte[] bytes);

		Task<Byte[]> GetImageBytesFromFile(string fileName);


		string SaveImageInFile(string fileName, Byte[] bytes);

		MemoryStream GetStreamFromFile(string filePath);

		void LoadIamgeFromSource(string fileName, Action<Stream> imageAvailable, string path = Constants.ImageServerPath);
	}

	public interface IDroidPictureService:IPictureService
	{
		void LoadIamgeFromSource(string fileName, Action<Stream> imageAvailable, Action<string> loadfailure, string path = Constants.ImageServerPath);
	}

    
}
