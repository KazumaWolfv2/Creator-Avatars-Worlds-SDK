using System;
using System.Security.Cryptography;
using System.Text;

// Token: 0x0200005A RID: 90
public static class FileIDUtil
{
	// Token: 0x060002AE RID: 686 RVA: 0x000110B4 File Offset: 0x0000F2B4
	public static int Compute(Type t)
	{
		string text = "s\0\0\0" + t.Namespace + t.Name;
		int num2;
		using (HashAlgorithm hashAlgorithm = new MD4())
		{
			byte[] array = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
			int num = 0;
			for (int i = 3; i >= 0; i--)
			{
				num <<= 8;
				num |= (int)array[i];
			}
			num2 = num;
		}
		return num2;
	}
}
