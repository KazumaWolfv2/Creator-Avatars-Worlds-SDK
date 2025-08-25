using System;
using System.IO;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class ABPackageSetting
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static string GetVersionStr()
	{
		string text = "version";
		if (!File.Exists(text))
		{
			return "";
		}
		return File.ReadAllText(text);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
	public static string GetBundlePath()
	{
		if (!Directory.Exists(ABPackageSetting.publishPath))
		{
			Directory.CreateDirectory(ABPackageSetting.publishPath);
		}
		string text = ABPackageSetting.publishPath + "/";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
	public static string GetAssetStreamingPath()
	{
		return Application.streamingAssetsPath + "/publish";
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020CD File Offset: 0x000002CD
	public static string GetPrefabPath()
	{
		return ABPackageSetting.prefabPath;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020D4 File Offset: 0x000002D4
	public static string GetLogPath()
	{
		if (!Directory.Exists(ABPackageSetting.logPath))
		{
			Directory.CreateDirectory(ABPackageSetting.logPath);
		}
		string text = ABPackageSetting.logPath + "/" + ABPackageSetting.GetVersionStr();
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x04000001 RID: 1
	public static string logPath = "../publish_log";

	// Token: 0x04000002 RID: 2
	public static string prefabPath = "Prefab/";

	// Token: 0x04000003 RID: 3
	public static string publishPath = "../publish";

	// Token: 0x04000004 RID: 4
	public static string abSuffixName = ".unity3d";

	// Token: 0x04000005 RID: 5
	public static string assetListFile = "filelist.txt";

	// Token: 0x04000006 RID: 6
	public static string dependceListFile = "dependences.txt";

	// Token: 0x04000007 RID: 7
	public static string JSonExt = ".json";
}
