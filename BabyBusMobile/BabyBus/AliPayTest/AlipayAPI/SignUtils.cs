using AlipayApp.AlipayAPI;
using Java.Security;
using Java.Security.Spec;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlipayApp.AlipayAPI
{
    public class SignUtils
    {
        private const string ALGORITHM = "RSA";
        private const string SIGN_ALGORITHMS = "SHA1WithRSA";
        private const string DEFAULT_CHARSET = "UTF-8";
        public static string sign(string content, string privateKey)
        {
            try
            {
                PKCS8EncodedKeySpec priPKCS8 = new PKCS8EncodedKeySpec(new Base64Decoder().GetDecoded(privateKey));
                KeyFactory keyf = KeyFactory.GetInstance(ALGORITHM);
                IPrivateKey priKey = keyf.GeneratePrivate(priPKCS8);
                Java.Security.Signature signature = Java.Security.Signature.GetInstance(SIGN_ALGORITHMS);
                signature.InitSign(priKey);
                byte[] getByteTxt = System.Text.Encoding.UTF8.GetBytes(content);
                signature.Update(getByteTxt);
                byte[] signed = signature.Sign();
                return new Base64Encoder().GetEncoded(signed);
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
            }
            return null;
        }
    }
}
