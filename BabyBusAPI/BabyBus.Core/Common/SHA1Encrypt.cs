using System;
using System.Text;
using System.Security.Cryptography;

namespace BabyBus.Core.Common
{
    public class Sha1Encrypt
    {
        public static string Sha1EncryptPassword(string str)
        {
            var sha1Cng = new SHA1Cng();
            var str1 = Encoding.UTF8.GetBytes(str);
            var str2 = sha1Cng.ComputeHash(str1);
            sha1Cng.Clear();
            (sha1Cng as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }
    }
}
