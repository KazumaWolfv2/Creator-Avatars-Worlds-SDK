using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class RefreshGuid
{
	// Token: 0x060002D4 RID: 724 RVA: 0x00011ECC File Offset: 0x000100CC
	public static void Refresh()
	{
		AssetDatabase.Refresh();
		DllAsset dllAsset = RefreshGuid.ReadMetaFile();
		List<string> list = RefreshGuid.CalPathList(dllAsset);
		List<string> list2 = RefreshGuid.CalGuidList(dllAsset);
		Dictionary<string, List<string>> dictionary = RefreshGuid.BuildDependencies();
		RefreshGuid.RefreshMetaFileGuid(RefreshGuid.FindAllMetaFile(list), list2, dictionary, list);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00011F0E File Offset: 0x0001010E
	private static DllAsset ReadMetaFile()
	{
		DllAsset dllAsset = RevertDLLMeta.LoadDllAsset(false);
		if (dllAsset == null)
		{
			ParaLog.Log("assets is null");
		}
		return dllAsset;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00011F24 File Offset: 0x00010124
	private static Dictionary<string, List<string>> BuildDependencies()
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (string text in Directory.GetFiles(Application.dataPath, "*.meta", SearchOption.AllDirectories))
		{
			string text2 = text.Substring(0, text.Length - 5);
			if ((File.Exists(text2) || Directory.Exists(text2)) && (!File.Exists(text2) || new FileInfo(text2).Length != 0L))
			{
				text2 = text2.Replace(Application.dataPath, "Assets");
				foreach (string text3 in AssetDatabase.GetDependencies(text2))
				{
					if (!dictionary.ContainsKey(text3))
					{
						dictionary.Add(text3, new List<string>());
					}
					dictionary[text3].Add(text2);
				}
			}
		}
		foreach (string text4 in Directory.GetFiles(Application.dataPath.Replace("Assets", "Packages"), "*.meta", SearchOption.AllDirectories))
		{
			string text5 = text4.Substring(0, text4.Length - 5);
			if ((File.Exists(text5) || Directory.Exists(text5)) && (!File.Exists(text5) || new FileInfo(text5).Length != 0L))
			{
				text5 = text5.Replace(Application.dataPath.Replace("Assets", "Packages"), "Packages");
				foreach (string text6 in AssetDatabase.GetDependencies(text5))
				{
					if (!dictionary.ContainsKey(text6))
					{
						dictionary.Add(text6, new List<string>());
					}
					dictionary[text6].Add(text5);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x000120DC File Offset: 0x000102DC
	private static List<string> CalPathList(DllAsset assets)
	{
		List<string> list = new List<string>();
		foreach (DllInfo dllInfo in assets.InfoList)
		{
			list.Add(dllInfo.Path);
		}
		return list;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0001213C File Offset: 0x0001033C
	private static List<string> CalGuidList(DllAsset assets)
	{
		List<string> list = new List<string>();
		foreach (DllInfo dllInfo in assets.InfoList)
		{
			list.Add(dllInfo.Guid);
		}
		return list;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0001219C File Offset: 0x0001039C
	private static List<AssetFileInfo> FindAllMetaFile(List<string> sdkPathList)
	{
		List<AssetFileInfo> list = new List<AssetFileInfo>();
		foreach (string text in Directory.GetFiles(Application.dataPath, "*.meta", SearchOption.AllDirectories))
		{
			string text2 = text.Replace(Application.dataPath, "Assets");
			text2 = text2.Substring(0, text2.Length - 5);
			if (!sdkPathList.Contains(text2))
			{
				string text3 = AssetDatabase.AssetPathToGUID(text2);
				list.Add(new AssetFileInfo(text2, text.Substring(0, text.Length - 5), text3));
			}
		}
		foreach (string text4 in Directory.GetFiles(Application.dataPath.Replace("Assets", "Packages"), "*.meta", SearchOption.AllDirectories))
		{
			string text5 = text4.Replace(Application.dataPath.Replace("Assets", "Packages"), "Packages");
			text5 = text5.Substring(0, text5.Length - 5);
			if (!sdkPathList.Contains(text5))
			{
				string text6 = AssetDatabase.AssetPathToGUID(text5);
				list.Add(new AssetFileInfo(text5, text4.Substring(0, text4.Length - 5), text6));
			}
		}
		return list;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000122C4 File Offset: 0x000104C4
	private static void RefreshMetaFileGuid(List<AssetFileInfo> pathList, List<string> guidList, Dictionary<string, List<string>> dependenciesDict, List<string> sdkPathList)
	{
		foreach (AssetFileInfo assetFileInfo in pathList)
		{
			if (guidList.Contains(assetFileInfo.Guid))
			{
				RefreshGuid.ReleaceGuid(assetFileInfo, dependenciesDict, sdkPathList);
			}
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00012324 File Offset: 0x00010524
	private static void ReleaceGuid(AssetFileInfo info, Dictionary<string, List<string>> dependenciesDict, List<string> sdkPathList)
	{
		string text = Guid.NewGuid().ToString("N");
		string realPath = info.RealPath;
		string guid = info.Guid;
		if (Directory.Exists(realPath))
		{
			RefreshGuid.RewriteMeta(realPath, text, guid);
			return;
		}
		if (!File.Exists(realPath))
		{
			ParaLog.Log("ReleaceGuid the  file  is not  exists:" + realPath);
			return;
		}
		List<string> dependencies = RefreshGuid.GetDependencies(info.Path, dependenciesDict);
		Debug.Log(string.Concat(new string[] { "=xx==============>>>>1>path>:", realPath, "   guid:", text, "  oldGuid:", guid }));
		RefreshGuid.RewriteMeta(realPath + ".meta", text, guid);
		foreach (string text2 in dependencies)
		{
			if (!string.Equals(text2, info.Path))
			{
				if (sdkPathList.Contains(text2))
				{
					Debug.Log("=xx==============>>>>2xx>path>:" + text2);
				}
				else
				{
					Debug.Log(string.Concat(new string[] { "=xx==============>>>>2>path>:", text2, "   guid:", text, "  oldGuid:", guid }));
					RefreshGuid.ReplaceMeta(text2, text, guid);
				}
			}
		}
		AssetDatabase.Refresh();
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0001247C File Offset: 0x0001067C
	private static void RewriteMeta(string path, string guid, string originGuid)
	{
		if (!File.Exists(path))
		{
			ParaLog.LogWarning(SdkLangManager.Get("str_sdk_bottomMessage_text38") + path + ".meta");
			return;
		}
		string text = File.ReadAllText(path);
		text = text.Replace(originGuid, guid);
		File.WriteAllText(path, text);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x000124C4 File Offset: 0x000106C4
	private static List<string> GetDependencies(string path, Dictionary<string, List<string>> dependenciesDict)
	{
		List<string> list = new List<string>();
		if (dependenciesDict.ContainsKey(path))
		{
			return dependenciesDict[path];
		}
		return list;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x000124EC File Offset: 0x000106EC
	private static void ReplaceMeta(string path, string guid, string originGuid)
	{
		string text;
		if (path.StartsWith("Assets"))
		{
			text = path.Replace("Assets", Application.dataPath);
		}
		else
		{
			text = path.Replace("Packages", Application.dataPath.Replace("Assets", "Packages"));
		}
		if (!File.Exists(text))
		{
			ParaLog.LogWarning(SdkLangManager.Get("str_sdk_bottomMessage_text39") + text);
			return;
		}
		string text2 = File.ReadAllText(text);
		text2 = text2.Replace(originGuid, guid);
		File.WriteAllText(text, text2);
	}
}
