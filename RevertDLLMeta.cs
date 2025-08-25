using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class RevertDLLMeta
{
	// Token: 0x060002E0 RID: 736 RVA: 0x00012570 File Offset: 0x00010770
	public static string GetAesKey()
	{
		return RevertDLLMeta.kAesKey;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00012577 File Offset: 0x00010777
	public static string GetDllAssetPath(bool isJson = false)
	{
		if (isJson)
		{
			return Application.dataPath.Replace("Assets", "Packages/com.para.common/Config/DllAsset.json");
		}
		return Application.dataPath.Replace("Assets", "Packages/com.para.common/Config/DllAsset.bin");
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000125A8 File Offset: 0x000107A8
	public static void Refresh(bool isJson = false)
	{
		DllAsset dllAsset = RevertDLLMeta.LoadDllAsset(RevertDLLMeta.GetDllAssetPath(isJson), isJson);
		if (dllAsset == null)
		{
			ParaLog.LogError("Can't find DllAsset.json");
			return;
		}
		RevertDLLMeta.RefreshMeta(dllAsset);
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000125D6 File Offset: 0x000107D6
	public static DllAsset LoadDllAsset(bool isJson = false)
	{
		return RevertDLLMeta.LoadDllAsset(RevertDLLMeta.GetDllAssetPath(isJson), isJson);
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x000125E4 File Offset: 0x000107E4
	private static DllAsset LoadDllAsset(string path, bool isJson)
	{
		if (!File.Exists(path))
		{
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new DllAsset
			{
				InfoList = new List<DllInfo>()
			};
		}
		if (isJson)
		{
			return JsonUtility.FromJson<DllAsset>(File.ReadAllText(path));
		}
		return JsonUtility.FromJson<DllAsset>(AesUtil.DecryptBytes(File.ReadAllBytes(path), RevertDLLMeta.kAesKey, ""));
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0001264C File Offset: 0x0001084C
	private static void RefreshMeta(DllAsset dllAsset)
	{
		foreach (DllInfo dllInfo in dllAsset.InfoList)
		{
			string text2;
			if (dllInfo.Path.StartsWith("Packages"))
			{
				string text = Application.dataPath.Replace("Assets", "Packages");
				text2 = dllInfo.Path.Replace("Packages", text);
			}
			else
			{
				text2 = dllInfo.Path.Replace("Assets", Application.dataPath);
			}
			if (string.IsNullOrEmpty(text2))
			{
				Debug.LogWarning("The file path is wrong: " + text2);
			}
			else if (!File.Exists(text2))
			{
				ParaLog.LogWarning(SdkLangManager.Get("str_sdk_bottomMessage_text37") + text2);
			}
			else
			{
				string text3 = AssetDatabase.AssetPathToGUID(dllInfo.Path);
				if (text3.Equals(dllInfo.Guid))
				{
					ParaLog.Log(string.Concat(new string[] { "No need to refresh meta: ", dllInfo.Path, " guid: ", dllInfo.Guid, " resPath: ", text2 }));
				}
				else if (string.IsNullOrEmpty(text3))
				{
					Debug.LogWarning("The file`s guid  is empty: " + dllInfo.Path);
				}
				else if (string.IsNullOrEmpty(dllInfo.Guid))
				{
					Debug.LogWarning("The info guid  is empty: " + dllInfo.Path);
				}
				else
				{
					string text4 = text2 + ".meta";
					if (!File.Exists(text4))
					{
						Debug.LogWarning("The meta file is not exists: " + text4);
					}
					else
					{
						string text5 = File.ReadAllText(text4);
						if (string.IsNullOrEmpty(text5))
						{
							Debug.LogWarning("The meta file content is empty: " + text4);
						}
						else
						{
							text5 = text5.Replace(text3, dllInfo.Guid);
							File.WriteAllText(text4, text5);
						}
					}
				}
			}
		}
		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
	}

	// Token: 0x04000185 RID: 389
	private static string kAesKey = "_para_sdk_aes_key_2022_";
}
