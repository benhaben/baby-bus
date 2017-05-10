using System;
using System.IO;

using Cirrious.CrossCore.Platform;
using Java.IO;
using Environment = System.Environment;
using File = System.IO.File;
using Console = System.Console;
using System.Threading;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Services
{

	public class EnvironmentService : IEnvironmentService
	{
		public float GetCacheSize() {
			var cachePath = Path.Combine(GetPersonalFolderPath(), "ImageData");
			float size;
			try {
				var file = new Java.IO.File(cachePath);
				size = GetCacheSize(file);
			} catch (Exception ex) {
				return 0;
			}
			return size;
		}

		private float GetCacheSize(Java.IO.File file) {
			float size = 0;
			if (file.IsDirectory) {
				var files = file.ListFiles();
				foreach (var file1 in files) {
					size = size + GetCacheSize(file1);
				}
			} else {
				size = file.Length();
			}

			return size;
		}

		public bool ClearCache() {
			var cachePath = Path.Combine(GetPersonalFolderPath(), "ImageData");
			try {
				var dir = new Java.IO.File(cachePath);
				if (dir.IsDirectory) {
					var files = dir.ListFiles();
					foreach (var file in files) {
						file.Delete();
					}
				}
			} catch (Exception ex) {
				return false;
			}
			return true;
		}

		public string GetPersonalFolderPath() {
			// Just use whatever directory SpecialFolder.Personal returns
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		public string GetDateBaseFolderPath(string sqliteFilename) {
#if __ANDROID__
			// Just use whatever directory SpecialFolder.Personal returns
			string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			;
#else
// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
// (they don't want non-user-generated data in Documents)
string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder instead
#endif
			var path = Path.Combine(libraryPath, sqliteFilename);
			return path;
		}

		public bool FileExists(string pathAndName) {
			return File.Exists(pathAndName);
		}

		public string GetTempPicturePathAndName() {
			Guid guid = Guid.NewGuid();
			//var path = Path.Combine(GetPersonalFolderPath(),"ImageData", DateTime.Now.ToString("yyyy-M-d"), guid.ToString() + Constants.TempPictureNameSuffix);
			var fileName = guid + Constants.PNGSuffix;
			return GetPicturePathAndName(fileName);
		}

		public string GetPicturePathAndName(string fileName) {
			var path = Path.Combine(GetPersonalFolderPath(), "ImageData", fileName);
			try {
				string directoryName = Path.GetDirectoryName(path);
				Directory.CreateDirectory(directoryName);
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
			}
			return path;
		}

		public void Log(string msg) {
			var path = Path.Combine("/sdcard/Log/", "ImageBug.log");
			try {
				string directoryName = Path.GetDirectoryName(path);
				if (Directory.Exists(directoryName))
					Directory.CreateDirectory(directoryName);
				var fileMode = FileExists(path) ? FileMode.Append : FileMode.Create;
				var outStream = new FileStream(path, fileMode, FileAccess.Write,
					                            FileShare.Delete);
                  
				var printStream = new PrintStream(outStream);
				printStream.Append(DateTime.Now + ":");
				printStream.Append(msg + "\n");
				printStream.Flush();
				printStream.Close();
				outStream.Flush(true);
				outStream.Close();
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
			}
		}


		public void CopyRawDataTo(string rawResourceName, string destinationPathAndName) {
			//for example :"DataAccess.Resources.Raw.babybus.db"
			using (Stream source =
				       System.Reflection.Assembly.GetExecutingAssembly()
                              .GetManifestResourceStream(rawResourceName)) {
				using (var destination = System.IO.File.Create(destinationPathAndName)) {
					if (source != null)
						source.CopyTo(destination);
				}
			}
		}

		public void CopyFile(string oldPath, string newPath) {

			//newPath = e.GetPersonalFolderPath() + "/" + DateTime.Now.ToString("yyyy-M-d") + "/" + Guid.NewGuid() + Constants.TempPictureNameSuffix;
			try {
				string directoryName = Path.GetDirectoryName(newPath);
				Directory.CreateDirectory(directoryName);
				File.Copy(oldPath, newPath, true);
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
			}
		}

		public System.Globalization.CultureInfo GetCurrentCultureInfo() {
			var androidLocale = Java.Util.Locale.Default;

			//var netLanguage = androidLocale.Language.Replace ("_", "-");
			var netLanguage = androidLocale.ToString().Replace("_", "-");

			//var netLanguage = androidLanguage.Replace ("_", "-");
			System.Console.WriteLine("android:" + androidLocale.ToString());
			Console.WriteLine("net:" + netLanguage);

			Console.WriteLine(Thread.CurrentThread.CurrentCulture);
			Console.WriteLine(Thread.CurrentThread.CurrentUICulture);

			return new System.Globalization.CultureInfo(netLanguage);
		}

		public void SetLocale() {
			var androidLocale = Java.Util.Locale.Default; // user's preferred locale
			var netLocale = androidLocale.ToString().Replace("_", "-"); 
			var ci = new System.Globalization.CultureInfo(netLocale);
//			var ci = new System.Globalization.CultureInfo ("en-US");


			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
		}

		public bool IsNetworkAvailable() {
			//ConnectivityManager con =
			return false;
		}


	}
}