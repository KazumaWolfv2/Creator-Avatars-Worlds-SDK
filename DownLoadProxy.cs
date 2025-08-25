using System;
using System.IO;
using System.Net;

// Token: 0x02000048 RID: 72
public static class DownLoadProxy
{
	// Token: 0x06000221 RID: 545 RVA: 0x0000E1F8 File Offset: 0x0000C3F8
	public static bool DownloadPng(string url, string exportPath, Action<int> func)
	{
		if (string.IsNullOrEmpty(url))
		{
			if (func != null)
			{
				func(-1);
			}
			return false;
		}
		string text = exportPath.Substring(0, exportPath.Length - 4);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = text + "/tmp.png";
		if (File.Exists(text2))
		{
			File.Delete(text2);
		}
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Method = "GET";
		httpWebRequest.ContentType = "image/png";
		WebResponse response = httpWebRequest.GetResponse();
		Stream responseStream = response.GetResponseStream();
		byte[] array = new byte[response.ContentLength];
		int num = 0;
		int num2;
		do
		{
			num2 = responseStream.Read(array, num, array.Length - num);
			num += num2;
		}
		while (num2 > 0);
		byte[] array2 = new MemoryStream(array).ToArray();
		File.WriteAllBytes(text2, array2);
		if (File.Exists(exportPath))
		{
			File.Delete(exportPath);
		}
		if (File.Exists(text2))
		{
			File.Move(text2, exportPath);
		}
		if (Directory.Exists(text))
		{
			new DirectoryInfo(text).Delete(true);
		}
		if (func != null)
		{
			func(0);
		}
		return true;
	}
}
