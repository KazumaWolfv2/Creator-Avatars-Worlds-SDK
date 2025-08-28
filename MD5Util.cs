using System;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000007 RID: 7
public static class MD5Util
{
	// Token: 0x06000022 RID: 34 RVA: 0x00002DF0 File Offset: 0x00000FF0
	public static string GetMD5(string source)
	{
		HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(source);
		byte[] array = hashAlgorithm.ComputeHash(bytes);
		string text = "";
		for (int i = 0; i < array.Length; i++)
		{
			text += array[i].ToString("x").PadLeft(2, '0');
		}
		return text;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002E4C File Offset: 0x0000104C
	public static string GetMD5(byte[] data)
	{
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(data);
		string text = "";
		for (int i = 0; i < array.Length; i++)
		{
			text += array[i].ToString("x").PadLeft(2, '0');
		}
		return text;
	}
}
