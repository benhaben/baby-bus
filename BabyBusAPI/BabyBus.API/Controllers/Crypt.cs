using System;

namespace BabyBus.API.Controllers
{
    public class Crypt
    {
         public static string Base64Encrypt(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        } 

        //Crypto crypto = new Crypto();
        //public static string Encrypt(string str)
        //{
        //    public abstract class Rijndael : SymmetricAlgorithm

        //    return null;
        //}

    }
}