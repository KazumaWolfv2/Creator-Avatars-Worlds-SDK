using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class AssetBundlePackage
{
	// Token: 0x06000012 RID: 18 RVA: 0x000025D0 File Offset: 0x000007D0
	public static void ClearAllBundleNames()
	{
		AssetDatabase.RemoveUnusedAssetBundleNames();
		string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
		for (int i = 0; i < allAssetBundleNames.Length; i++)
		{
			string[] assetPathsFromAssetBundle = AssetDatabase.GetAssetPathsFromAssetBundle(allAssetBundleNames[i]);
			if (ABPackageTools.DisplayProgressBar(SdkLangManager.Get("str_sdk_clear_ab"), i, allAssetBundleNames.Length))
			{
				break;
			}
			foreach (string text in assetPathsFromAssetBundle)
			{
				if (!Singleton<DependencesMgr>.instance.dependMap.ContainsKey(text))
				{
					AssetImporter atPath = AssetImporter.GetAtPath(text);
					if (atPath)
					{
						atPath.assetBundleName = "";
					}
				}
			}
		}
		EditorUtility.ClearProgressBar();
		AssetDatabase.RemoveUnusedAssetBundleNames();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000266C File Offset: 0x0000086C
	public static void SetBundleNames()
	{
		int num = 0;
		int totalFileCount = Singleton<DependencesMgr>.instance.totalFileCount;
		StreamWriter streamWriter = new StreamWriter(ABPackageSetting.GetLogPath() + "/SetAssetBundleNames.txt");
		foreach (DependencesMgr.BundleItem bundleItem in Singleton<DependencesMgr>.instance.bundles)
		{
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in bundleItem.files)
			{
				string key = keyValuePair.Key;
				if (ABPackageTools.DisplayProgressBar("Clear AssetBundleName", num++, totalFileCount))
				{
					goto IL_0119;
				}
				AssetImporter atPath = AssetImporter.GetAtPath(key);
				streamWriter.WriteLine("set " + key);
				if (atPath && atPath.assetBundleName != bundleItem.name)
				{
					atPath.assetBundleName = bundleItem.name;
					streamWriter.WriteLine("\t" + key + " > " + bundleItem.name);
				}
			}
		}
		IL_0119:
		EditorUtility.ClearProgressBar();
		streamWriter.Close();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000027BC File Offset: 0x000009BC
	public static bool CreateBundlesByPlatfrom(bool clearUselessFiles = false, bool CollectStreamingAsset = true)
	{
		string assetStreamingPath = ABPackageSetting.GetAssetStreamingPath();
		Debug.Log("[Build] CreateBundlesByPlatfrom Start");
		try
		{
			BuildPipeline.BuildAssetBundles(assetStreamingPath, 2, EditorUserBuildSettings.activeBuildTarget);
		}
		catch (Exception ex)
		{
			Debug.LogError("[Build] CreateBundlesByPlatfrom " + ex.ToString());
			return false;
		}
		Debug.Log("[Build] CreateBundlesByPlatfrom End");
		return true;
	}
}
