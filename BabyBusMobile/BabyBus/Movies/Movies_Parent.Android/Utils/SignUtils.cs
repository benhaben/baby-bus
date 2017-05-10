using System;
using Java.Security.Spec;
using Android.Util;
using Java.Security;

namespace BabyBus.Logic.Shared
{
	public  class SignUtils
	{

		private const  String ALGORITHM = "RSA";

		private const  String SIGN_ALGORITHMS = "SHA1WithRSA";

		private const  String DEFAULT_CHARSET = "UTF-8";

		public static String sign (String content, String privateKey)
		{
			try {
				var ss = Base64.decode (privateKey);
				PKCS8EncodedKeySpec priPKCS8 = new PKCS8EncodedKeySpec (
					                               Base64.decode (privateKey));
				KeyFactory keyf = KeyFactory.GetInstance (ALGORITHM);
				var priKey = keyf.GeneratePrivate (priPKCS8);

				Signature signature = Signature.GetInstance (SIGN_ALGORITHMS);
				signature.InitSign (priKey);
				var d = System.Text.Encoding.UTF8.GetBytes (content);
				signature.Update (d);

				var signed = signature.Sign ();

				return Base64.encode (signed);
			} catch (Exception e) {
				e.GetBaseException ();
			}

			return null;
		}
				
	}
}

