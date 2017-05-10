using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.Graphics;
using BabyBus.Droid.Utils.Image;
using Java.IO;

namespace BabyBus.Droid.Views.Common.Album
{
	/// <summary>
	/// Image collection for Select
	/// </summary>
	public class ImageCollection
	{
		//Image Collection Cursor
		public static int Max = 0;
		public static int Selected = 0;
		//Do not know how to use it
		public static bool IsAct = true;
		public static List<Bitmap> BmpList = new List<Bitmap>();
		public static List<string> PthList = new List<string>();

		public static Bitmap RevitionImageSize(string path) {
			return ImageRevition.RevitionImageSize(path, 120, 120);
		}

		public static List<byte[]> GetBytesList() {
			var list = new List<byte[]>();
            
			foreach (var bmp in BmpList) {
				MemoryStream ms = new MemoryStream();
				bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
				list.Add(ms.ToArray());
				ms.Close();
			}
            
			return list;
		}

		public static void ClearImage() {
			Max = 0;
			PthList.Clear();
			BmpList.Clear();
		}

		public static void DeleteImage() {
			PthList.RemoveAt(Selected);
			BmpList.RemoveAt(Selected);
			Max--;
		}
	}
}
