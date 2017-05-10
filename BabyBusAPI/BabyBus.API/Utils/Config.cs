using System.Configuration;

namespace BabyBus.API.Utils
{
    public static class Config
    {
        public static string ImagePath {
            get { return ConfigurationManager.AppSettings["ImageUpload"] ?? string.Empty; }
        }

        public static string ApkPath {
            get { return ConfigurationManager.AppSettings["ApkPath"] ?? string.Empty; }
        }

        public static string ApkBaseUrl {
            get { return ConfigurationManager.AppSettings["ApkDownloadUrl"] ?? string.Empty; }
        }

        public static string VersionList {
            get { return ConfigurationManager.AppSettings["VersionList"] ?? string.Empty; }
        }

        public static string PhotoSuffix {
            get { return ConfigurationManager.AppSettings["PhotoSuffix"] ?? string.Empty; }
        }

        public static string Parent_AppKey {
            get { return ConfigurationManager.AppSettings["Parent_AppKey"] ?? string.Empty; }
        }

        public static string Parent_ApiMaster {
            get { return ConfigurationManager.AppSettings["Parent_ApiMaster"] ?? string.Empty; }
        }

        public static string Teacher_AppKey {
            get { return ConfigurationManager.AppSettings["Teacher_AppKey"] ?? string.Empty; }
        }

        public static string Teacher_ApiMaster {
            get { return ConfigurationManager.AppSettings["Teacher_ApiMaster"] ?? string.Empty; }
        }

        public static string Master_ApiMaster {
            get { return ConfigurationManager.AppSettings["Master_ApiMaster"] ?? string.Empty; }
        }

        public static string Master_AppKey {
            get { return ConfigurationManager.AppSettings["Master_AppKey"] ?? string.Empty; }
        }
    }
}