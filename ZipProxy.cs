using System;
using System.IO;
using System.Text;
using Unity.VisualScripting.IonicZip;

// Token: 0x02000054 RID: 84
public static class ZipProxy
{
	// Token: 0x0600028C RID: 652 RVA: 0x00010518 File Offset: 0x0000E718
	public static string BuildZipWithPath(string prefabPath, string exportPath, ref string fileName, string encryptPassword = null)
	{
		int num = prefabPath.LastIndexOf("/");
		fileName = prefabPath.Substring(num + 1) + ".zip";
		string text = Path.Combine(exportPath, fileName);
		try
		{
			using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
			{
				if (!string.IsNullOrEmpty(encryptPassword))
				{
					zipFile.Password = encryptPassword;
				}
				zipFile.AddFile(prefabPath, "");
				zipFile.Save(text);
			}
		}
		catch (Exception ex)
		{
			ParaLog.LogError(ex);
		}
		return text;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x000105B0 File Offset: 0x0000E7B0
	public static void DeCompressFile(string fileName, string deCompressedFilePath, string password = "")
	{
		if (!File.Exists(fileName))
		{
			throw new FileNotFoundException("No Find Compressed file！", fileName);
		}
		try
		{
			using (ZipFile zipFile = new ZipFile(fileName, Encoding.UTF8))
			{
				if (!string.IsNullOrEmpty(password))
				{
					zipFile.Password = password;
				}
				if (!Directory.Exists(deCompressedFilePath))
				{
					Directory.CreateDirectory(deCompressedFilePath);
				}
				zipFile.ExtractAll(deCompressedFilePath, 1);
			}
		}
		catch (Exception ex)
		{
			ParaLog.LogError(ex);
		}
	}
}
