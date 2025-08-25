using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

// Token: 0x02000003 RID: 3
public class ABPackageTools
{
	// Token: 0x06000007 RID: 7 RVA: 0x00002173 File Offset: 0x00000373
	public static bool DisplayProgressBar(string info, int now, int total)
	{
		return EditorUtility.DisplayCancelableProgressBar(info, string.Format("{0}/{1}", now, total), 1f * (float)now / (float)total);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000219C File Offset: 0x0000039C
	public static string GetFileDir(string fullFilePath)
	{
		string[] array = fullFilePath.Split('/', StringSplitOptions.None);
		if (array.Length == 1)
		{
			return "";
		}
		return fullFilePath.Substring(0, fullFilePath.Length - array[array.Length - 1].Length - 1);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000021DC File Offset: 0x000003DC
	public static string GetFileName(string fullFilePath, bool withExtension = true)
	{
		string[] array = fullFilePath.Split('/', StringSplitOptions.None);
		if (withExtension)
		{
			return array[array.Length - 1];
		}
		array = array[array.Length - 1].Split('.', StringSplitOptions.None);
		return array[0];
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002214 File Offset: 0x00000414
	public static List<string> GetFiles(string path, string[] exceptFileTypes, bool allDirectories = true)
	{
		string[] files = Directory.GetFiles(path, "*.*", allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		List<string> list = new List<string>();
		string[] array = files;
		int i = 0;
		while (i < array.Length)
		{
			string text = array[i];
			if (exceptFileTypes == null)
			{
				goto IL_0057;
			}
			bool flag = false;
			foreach (string text2 in exceptFileTypes)
			{
				if (text.EndsWith(text2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				goto IL_0057;
			}
			IL_0070:
			i++;
			continue;
			IL_0057:
			string text3 = text.Replace('\\', '/');
			list.Add(text3.ToLower());
			goto IL_0070;
		}
		return list;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0000229C File Offset: 0x0000049C
	public static List<string> GetFiles(string[] paths, string[] exceptFileTypes)
	{
		if (paths == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < paths.Length; i++)
		{
			foreach (string text in Directory.GetFiles(paths[i], "*.*", SearchOption.AllDirectories))
			{
				bool flag = false;
				foreach (string text2 in exceptFileTypes)
				{
					if (text.EndsWith(text2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					string text3 = text.Replace('\\', '/');
					list.Add(text3.ToLower());
				}
			}
		}
		return list;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x0000233C File Offset: 0x0000053C
	public static List<string> GetPrefabs(string path, string[] exceptFileTypes, bool allDirectories = true)
	{
		string[] files = Directory.GetFiles(path, "*.prefab", allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		List<string> list = new List<string>();
		string[] array = files;
		int i = 0;
		while (i < array.Length)
		{
			string text = array[i];
			if (exceptFileTypes == null)
			{
				goto IL_0057;
			}
			bool flag = false;
			foreach (string text2 in exceptFileTypes)
			{
				if (text.EndsWith(text2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				goto IL_0057;
			}
			IL_006B:
			i++;
			continue;
			IL_0057:
			string text3 = text.Replace('\\', '/');
			list.Add(text3);
			goto IL_006B;
		}
		return list;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000023C0 File Offset: 0x000005C0
	public static void CopyFile(string srcPath, string dstPath, string[] exceptFileTypes = null)
	{
		EditorUtility.DisplayProgressBar(SdkLangManager.Get("str_sdk_copy_file"), SdkLangManager.Get("str_sdk_copying"), 0f);
		List<string> files = ABPackageTools.GetFiles(srcPath, exceptFileTypes, true);
		for (int i = 0; i < files.Count; i++)
		{
			EditorUtility.DisplayProgressBar(SdkLangManager.Get("str_sdk_copy_file"), SdkLangManager.Get("str_sdk_copying"), (float)i * 1f / (float)files.Count);
			string[] array = files[i].Split('/', StringSplitOptions.None);
			string text = array[array.Length - 1];
			File.Copy(files[i], dstPath + "/" + text);
		}
		EditorUtility.ClearProgressBar();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002464 File Offset: 0x00000664
	private static void SaveToFile(string filePath, List<string> lines)
	{
		StreamWriter streamWriter = new StreamWriter(filePath);
		foreach (string text in lines)
		{
			streamWriter.WriteLine(text);
		}
		streamWriter.Close();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000024C0 File Offset: 0x000006C0
	public static void LogOperation(string op)
	{
		StreamWriter streamWriter = new StreamWriter(ABPackageSetting.GetLogPath() + "/log_op.txt", true);
		if (op.Contains("[Menu]"))
		{
			streamWriter.WriteLine();
			streamWriter.WriteLine("---------------------------------------");
			string text = string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), op);
			streamWriter.WriteLine(text);
			ParaLog.Log(text);
		}
		else
		{
			string text2 = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), op);
			streamWriter.WriteLine(text2);
			ParaLog.Log(text2);
		}
		streamWriter.Close();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002560 File Offset: 0x00000760
	public static string ParseBundleName(string strFilePath)
	{
		string text = string.Empty;
		int num = strFilePath.LastIndexOf('/');
		int length = strFilePath.Length;
		text = strFilePath.Substring(num + 1, length - num - 1);
		num = text.IndexOf('.');
		string text2 = string.Empty;
		if (text.EndsWith(".prefab"))
		{
			text2 = text.Substring(0, num);
		}
		else
		{
			text2 = text.Replace('.', '_');
		}
		return text2;
	}
}
