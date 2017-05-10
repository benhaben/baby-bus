using System;
using BabyBus.Logic.Shared;
using System.IO;

using Cirrious.CrossCore.Platform;
using Environment = System.Environment;
using File = System.IO.File;
using Foundation;
using System.Threading;

namespace BabyBus.iOS
{
    public class EnvironmentService : IEnvironmentService
    {
        public EnvironmentService()
        {
        }

        void IEnvironmentService.Log(string msg)
        {
            return;
        }

        public float GetCacheSize()
        {
            return 0f;
//            throw new NotImplementedException();
        }

        public bool ClearCache()
        {
            return true;
//            throw new NotImplementedException();
        }

        public string GetPersonalFolderPath()
        {
            // Just use whatever directory SpecialFolder.Personal returns
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            ;
        }

        public string GetDateBaseFolderPath(string sqliteFilename)
        {
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
//			var documents = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0];

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
            var path = Path.Combine(libraryPath, sqliteFilename);
            return path;
        }

        public bool FileExists(string pathAndName)
        {
            return File.Exists(pathAndName);
        }

        public string GetTempPicturePathAndName()
        {
            Guid guid = Guid.NewGuid();
            //var path = Path.Combine(GetPersonalFolderPath(),"ImageData", DateTime.Now.ToString("yyyy-M-d"), guid.ToString() + Constants.TempPictureNameSuffix);
            var fileName = guid + Constants.PNGSuffix;
            return GetPicturePathAndName(fileName);
        }

        public string GetPicturePathAndName(string fileName)
        {
            var path = Path.Combine(GetPersonalFolderPath(), "ImageData", fileName);
            try
            {
                string directoryName = Path.GetDirectoryName(path);
                Directory.CreateDirectory(directoryName);
            }
            catch (Exception e)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
            }
            return path;
        }


        public void CopyRawDataTo(string rawResourceName, string destinationPathAndName)
        {
            //for example :"DataAccess.Resources.Raw.babybus.db"
            using (Stream source =
                       System.Reflection.Assembly.GetExecutingAssembly()
				.GetManifestResourceStream(rawResourceName))
            {
                using (var destination = System.IO.File.Create(destinationPathAndName))
                {
                    if (source != null)
                        source.CopyTo(destination);
                }
            }
        }

        public void CopyFile(string oldPath, string newPath)
        {

            //newPath = e.GetPersonalFolderPath() + "/" + DateTime.Now.ToString("yyyy-M-d") + "/" + Guid.NewGuid() + Constants.TempPictureNameSuffix;
            try
            {
                string directoryName = Path.GetDirectoryName(newPath);
                Directory.CreateDirectory(directoryName);
                File.Copy(oldPath, newPath, true);
            }
            catch (Exception e)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
            }
        }

        public void SetLocale()
        {
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var netLocale = iosLocaleAuto.Replace("_", "-");
            System.Globalization.CultureInfo ci;
            try
            {
                ci = new System.Globalization.CultureInfo(netLocale);
            }
            catch
            {
                ci = GetCurrentCultureInfo();
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine("SetLocale: " + ci.Name);
        }

        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            #region not sure why this isn't working for me (in simulator at least)
            var iosLocaleAuto = NSLocale.AutoUpdatingCurrentLocale.LocaleIdentifier;
            var iosLanguageAuto = NSLocale.AutoUpdatingCurrentLocale.LanguageCode;
            Console.WriteLine("nslocaleid:" + iosLocaleAuto);
            Console.WriteLine("nslanguage:" + iosLanguageAuto);

            var iosLocale = NSLocale.CurrentLocale.LocaleIdentifier;
            var iosLanguage = NSLocale.CurrentLocale.LanguageCode;

            var netLocale = iosLocale.Replace("_", "-");
            var netLanguage = iosLanguage.Replace("_", "-");

            Console.WriteLine("ios:" + iosLanguage + " " + iosLocale);
            Console.WriteLine("net:" + netLanguage + " " + netLocale);

            // doesn't seem to affect anything (well, i didn't expect it to affect UIKit controls)
            //			var ci = new System.Globalization.CultureInfo ("JA-jp");
            //			System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            #endregion

            // HACK: not sure why NSLocale isn't ever returning correct data
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                netLanguage = pref.Replace("_", "-");
                Console.WriteLine("preferred:" + netLanguage);
            }
            return new System.Globalization.CultureInfo(netLanguage);
        }
    }
}

