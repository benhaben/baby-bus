using System;

using System.Text;
using System.Security.Cryptography;
using PCLCrypto.WinRTCrypto;
using PCLCrypto.NetFxCrypto;
using PCLCrypto;

namespace EncryptTest1 {
    class MainClass {
        public static void Main(string[] args) {

            var sss = Get("ssss");
            var sss1 = Get("ssss");

            Console.WriteLine("Hello World! {0}", sss);
            Console.WriteLine("Hello World! {0}", sss1);
            {
                YinEncrypt("shenyin");
                YinEncrypt("shenyin");
                YinEncrypt("shenyin1");

            }

        }

        static string YinEncrypt(string password) {
            // WinRT-like API
            int keyLengthInBytes = 15;
            //                byte[] cryptoRandomBuffer = WinRTCrypto.CryptographicBuffer.GenerateRandom(keyLengthInBytes);

            // .NET Framework-like API
            // best initialized to a unique value for each user, and stored with the user record
            byte[] salt = new byte[keyLengthInBytes];
            NetFxCrypto.RandomNumberGenerator.GetBytes(salt);


            int iterations = 5000; // higher makes brute force attacks more expensive
            byte[] data = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            var hashMd5 = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(PCLCrypto.HashAlgorithm.Md5);

            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(PCLCrypto.HashAlgorithm.Sha1);
            byte[] hash = hasher.HashData(data);
            string hashBase64 = Convert.ToBase64String(hash);
            Console.WriteLine("Hello World! {0}", hashBase64);
            return hashBase64;
        }

        public static string Base64Encrypt(string str) {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        public static string Base64Decrypt(string str) {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        //        public string Get(string password) {
        //            var test = Sha1Encrypt.Sha1EncryptPassword(password);
        //            return test;
        //        }

        static public string Get(string password) {
            var enc = new UTF8Encoding();
            var data = enc.GetBytes(password);
            SHA1 sha = new SHA1CryptoServiceProvider();
            password = BitConverter.ToString(sha.ComputeHash(data)).Replace("-", "");
            return password;
        }
    }
}
