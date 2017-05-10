using System;
using Java.Security;

namespace BabyBus.Droid
{
	public class MD5
	{
		public static string GetMessageDigest(byte[] buffer) {
			char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
			try {
				MessageDigest mdTemp = MessageDigest.GetInstance("MD5");
				mdTemp.Update(buffer);
				byte[] md = mdTemp.Digest();
				int j = md.Length;
				char[] str = new char[j * 2];
				int k = 0;
				for (int i = 0; i < j; i++) {
					byte byte0 = md[i];
					str[k++] = hexDigits[(int)((uint)byte0 >> 4) & 0xf];
					str[k++] = hexDigits[byte0 & 0xf];
				}
				return new string(str);
			} catch (Exception ex) {
				return null;
			}
		}
	}
}

