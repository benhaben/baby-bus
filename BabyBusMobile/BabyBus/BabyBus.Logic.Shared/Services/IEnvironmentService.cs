using System;
using System.Globalization;

namespace BabyBus.Logic.Shared
{
    public interface IEnvironmentService
    {
        float GetCacheSize();

        bool ClearCache();

        string GetPersonalFolderPath();

        string GetDateBaseFolderPath(string sqliteFilename);

        bool FileExists(string pathAndName);

        string GetTempPicturePathAndName();

        string GetPicturePathAndName(string fileName);

        void CopyRawDataTo(string rawResourceName, string destinationPathAndName);

        void CopyFile(string oldPath, String newPath);

        CultureInfo GetCurrentCultureInfo();

        void SetLocale();

        void Log(string msg);
    }
}
