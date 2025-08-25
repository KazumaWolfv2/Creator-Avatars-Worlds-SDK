using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// Token: 0x0200005D RID: 93
public class AesUtil
{
	// Token: 0x060002C7 RID: 711 RVA: 0x000119D0 File Offset: 0x0000FBD0
	public static byte[] Encrypt(AesUtil.AesModel aesModel)
	{
		byte[] array = new byte[32];
		Array.Copy(Encoding.UTF8.GetBytes(aesModel.Key.PadRight(array.Length)), array, array.Length);
		byte[] array2 = new byte[16];
		Array.Copy(Encoding.UTF8.GetBytes(aesModel.IV.PadRight(array2.Length)), array2, array2.Length);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = aesModel.Mode;
		rijndaelManaged.Padding = aesModel.Padding;
		rijndaelManaged.Key = array;
		rijndaelManaged.IV = array2;
		byte[] array3 = null;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(aesModel.Data, 0, aesModel.Data.Length);
					cryptoStream.FlushFinalBlock();
					array3 = memoryStream.ToArray();
				}
			}
		}
		catch
		{
		}
		return array3;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00011AE4 File Offset: 0x0000FCE4
	public static byte[] Decrypt(AesUtil.AesModel aesModel)
	{
		byte[] array = new byte[32];
		Array.Copy(Encoding.UTF8.GetBytes(aesModel.Key.PadRight(array.Length)), array, array.Length);
		byte[] array2 = new byte[16];
		Array.Copy(Encoding.UTF8.GetBytes(aesModel.IV.PadRight(array2.Length)), array2, array2.Length);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = aesModel.Mode;
		rijndaelManaged.Padding = aesModel.Padding;
		rijndaelManaged.Key = array;
		rijndaelManaged.IV = array2;
		byte[] array3 = null;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream(aesModel.Data))
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						byte[] array4 = new byte[1048576];
						int num;
						while ((num = cryptoStream.Read(array4, 0, array4.Length)) > 0)
						{
							memoryStream2.Write(array4, 0, num);
						}
						array3 = memoryStream2.ToArray();
					}
				}
			}
		}
		catch
		{
		}
		return array3;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00011C34 File Offset: 0x0000FE34
	public static string Encrypt(string data, string key, string iv = "")
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		byte[] array = AesUtil.Encrypt(new AesUtil.AesModel
		{
			Data = bytes,
			Key = key,
			IV = iv,
			Mode = CipherMode.CBC,
			Padding = PaddingMode.PKCS7
		});
		if (array == null)
		{
			return "";
		}
		return Convert.ToBase64String(array);
	}

	// Token: 0x060002CA RID: 714 RVA: 0x00011C8C File Offset: 0x0000FE8C
	public static byte[] EncryptBytes(string data, string key, string iv = "")
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		byte[] array = AesUtil.Encrypt(new AesUtil.AesModel
		{
			Data = bytes,
			Key = key,
			IV = iv,
			Mode = CipherMode.CBC,
			Padding = PaddingMode.PKCS7
		});
		if (array == null)
		{
			return null;
		}
		return array;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00011CDC File Offset: 0x0000FEDC
	public static string Decrypt(string data, string key, string iv = "")
	{
		byte[] array = Convert.FromBase64String(data);
		byte[] array2 = AesUtil.Decrypt(new AesUtil.AesModel
		{
			Data = array,
			Key = key,
			IV = iv,
			Mode = CipherMode.CBC,
			Padding = PaddingMode.PKCS7
		});
		if (array2 == null)
		{
			return "";
		}
		return Encoding.UTF8.GetString(array2);
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00011D34 File Offset: 0x0000FF34
	public static string DecryptBytes(byte[] bytes, string key, string iv = "")
	{
		byte[] array = AesUtil.Decrypt(new AesUtil.AesModel
		{
			Data = bytes,
			Key = key,
			IV = iv,
			Mode = CipherMode.CBC,
			Padding = PaddingMode.PKCS7
		});
		if (array == null)
		{
			return "";
		}
		return Encoding.UTF8.GetString(array);
	}

	// Token: 0x020000B2 RID: 178
	public class AesModel
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00024BFF File Offset: 0x00022DFF
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x00024C07 File Offset: 0x00022E07
		public byte[] Data { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x00024C10 File Offset: 0x00022E10
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x00024C18 File Offset: 0x00022E18
		public string Key { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00024C21 File Offset: 0x00022E21
		// (set) Token: 0x0600052A RID: 1322 RVA: 0x00024C29 File Offset: 0x00022E29
		public string IV { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00024C32 File Offset: 0x00022E32
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x00024C3A File Offset: 0x00022E3A
		public CipherMode Mode { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00024C43 File Offset: 0x00022E43
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x00024C4B File Offset: 0x00022E4B
		public PaddingMode Padding { get; set; }
	}
}
