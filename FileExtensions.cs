using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// Token: 0x02000058 RID: 88
public class FileExtensions
{
	// Token: 0x06000299 RID: 665 RVA: 0x00010ADC File Offset: 0x0000ECDC
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
				num = (num << 8) | (int)array[i];
			}
			num2 = num;
		}
		return num2;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00010B58 File Offset: 0x0000ED58
	public static string getFileNameFromPath(string path)
	{
		string fileName = Path.GetFileName(path);
		if (fileName.Length > FileExtensions.kSufFix)
		{
			return fileName.Substring(0, fileName.Length - FileExtensions.kSufFix);
		}
		return null;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00010B90 File Offset: 0x0000ED90
	public static List<string> FindAllFileWithSuffixs(string path, string[] suffixs)
	{
		List<string> list = new List<string>();
		FileExtensions.FindAllFileWithSuffixs(path, suffixs, ref list);
		return list;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00010BB0 File Offset: 0x0000EDB0
	public static void FindAllFileWithSuffixs(string path, string[] suffixs, ref List<string> resultList)
	{
		if (File.Exists(path))
		{
			resultList.Add(path);
			return;
		}
		if (!string.IsNullOrEmpty(path))
		{
			foreach (string text in Directory.GetFiles(path))
			{
				foreach (string text2 in suffixs)
				{
					if (text.EndsWith(text2))
					{
						resultList.Add(text);
						break;
					}
				}
			}
			string[] directories = Directory.GetDirectories(path);
			for (int k = 0; k < directories.Length; k++)
			{
				FileExtensions.FindAllFileWithSuffixs(directories[k], suffixs, ref resultList);
			}
		}
	}

	// Token: 0x04000175 RID: 373
	private static int kSufFix = 8;
}
