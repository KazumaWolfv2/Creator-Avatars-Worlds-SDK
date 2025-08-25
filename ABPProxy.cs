using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000008 RID: 8
public class ABPProxy
{
	// Token: 0x0600002F RID: 47 RVA: 0x000033C0 File Offset: 0x000015C0
	public static void BuildAssetBundle_BaseRes()
	{
		AssetBundlePackage.ClearAllBundleNames();
		List<string> prefabs = ABPackageTools.GetPrefabs("Assets/Res/" + ABPackageSetting.GetPrefabPath(), new string[] { "meta", "tpsheet" }, true);
		Singleton<DependencesMgr>.instance.CollectDependenceMap(prefabs);
		Singleton<DependencesMgr>.instance.BuildAssetBundles();
		AssetBundlePackage.SetBundleNames();
		EditorUtility.ClearProgressBar();
		AssetBundlePackage.CreateBundlesByPlatfrom(false, true);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003428 File Offset: 0x00001628
	public static void BuildAssetBundles()
	{
		AssetBundlePackage.ClearAllBundleNames();
		Object activeObject = Selection.activeObject;
		if (activeObject != null)
		{
			string text = AssetDatabase.GetAssetPath(activeObject.GetInstanceID());
			if (string.IsNullOrEmpty(text))
			{
				text = AssetDatabase.GetAssetPath(activeObject);
			}
			if (activeObject is SceneAsset)
			{
				if (SceneManager.GetActiveScene().path != text)
				{
					EditorUtility.DisplayDialog(SdkLangManager.Get("str_sdk_bottomMessage_text46"), SdkLangManager.Get("str_sdk_bottomMessage_text47"), SdkLangManager.Get("str_sdk_ok"));
					return;
				}
				HashSet<string> hashSet = new HashSet<string>();
				foreach (ParaNetworkGuid paraNetworkGuid in Object.FindObjectsOfType<ParaNetworkGuid>())
				{
					if (!hashSet.Add(paraNetworkGuid.GetGuidStr()))
					{
						EditorUtility.DisplayDialog(SdkLangManager.Get("str_sdk_bottomMessage_text46"), SdkLangManager.Get("str_sdk_bottomMessage_text50", paraNetworkGuid.name, paraNetworkGuid.GetGuidStr()), SdkLangManager.Get("str_sdk_ok"));
						return;
					}
				}
			}
			ABPProxy.BuildAssetBundlesWithPath(text);
		}
		AssetBundlePackage.CreateBundlesByPlatfrom(false, true);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003523 File Offset: 0x00001723
	public static void BuildShaderAssetBundles()
	{
		ParaLog.Log("[Build Resource] Finished processing Shader Assets");
		AssetBundlePackage.ClearAllBundleNames();
		AssetShaderPackage.HandleShaderListAssetBundle();
		AssetBundlePackage.CreateBundlesByPlatfrom(false, true);
		AssetDatabase.Refresh();
		ParaLog.Log("[Build Resource] Started processing Shader Assets");
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003550 File Offset: 0x00001750
	public static void BuildAssetBundlesWithPath(string prefabPath)
	{
		if (!string.IsNullOrEmpty(prefabPath))
		{
			string text = ABPackageTools.ParseBundleName(prefabPath);
			AssetImporter atPath = AssetImporter.GetAtPath(prefabPath);
			if (atPath && atPath.assetBundleName != text)
			{
				atPath.assetBundleName = text + ABPackageSetting.abSuffixName;
			}
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0000359C File Offset: 0x0000179C
	public static void BuildRTStage()
	{
		string text = "Assets/OriginalResources/AvatarRT/AvatarRTStage.prefab";
		AssetBundlePackage.ClearAllBundleNames();
		string text2 = ABPackageTools.ParseBundleName(text);
		AssetImporter atPath = AssetImporter.GetAtPath(text);
		if (atPath && atPath.assetBundleName != text2)
		{
			atPath.assetBundleName = text2 + ABPackageSetting.abSuffixName;
		}
		AssetBundlePackage.CreateBundlesByPlatfrom(false, true);
		AssetBundlePackage.ClearAllBundleNames();
		AssetDatabase.Refresh();
	}

	// Token: 0x06000034 RID: 52 RVA: 0x000035FC File Offset: 0x000017FC
	public static void BuildGiftEffect()
	{
		Debug.Log("ABPMenus.BuildGiftEffect");
		string text = Path.Combine(Application.streamingAssetsPath, "GiftEffect");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		List<string> list = new List<string>();
		foreach (string text2 in Directory.GetDirectories("Assets/OriginalResources/GiftEffect", "*.*", SearchOption.TopDirectoryOnly))
		{
			string fileName = Path.GetFileName(text2);
			Debug.Log("ABPMenus.BuildGiftEffect " + text2 + " - " + fileName);
			Singleton<DependencesMgr>.instance.Clear();
			AssetBundlePackage.ClearAllBundleNames();
			string[] files = Directory.GetFiles(text2, "*.*", SearchOption.AllDirectories);
			list.Clear();
			foreach (string text3 in files)
			{
				if (!text3.EndsWith(".meta"))
				{
					list.Add(text3);
				}
			}
			Singleton<DependencesMgr>.instance.CollectDependenceMap(list);
			Singleton<DependencesMgr>.instance.BuildAssetBundles();
			foreach (DependencesMgr.BundleItem bundleItem in Singleton<DependencesMgr>.instance.bundles)
			{
				foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in bundleItem.files)
				{
					if (!keyValuePair.Value.name.EndsWith(".cs"))
					{
						AssetImporter atPath = AssetImporter.GetAtPath(keyValuePair.Key);
						if (atPath)
						{
							atPath.assetBundleName = fileName + ABPackageSetting.abSuffixName;
						}
					}
				}
			}
			BuildPipeline.BuildAssetBundles(text, 128, EditorUserBuildSettings.activeBuildTarget);
			Singleton<DependencesMgr>.instance.Clear();
			AssetBundlePackage.ClearAllBundleNames();
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000037D0 File Offset: 0x000019D0
	public static void BuildAnimation()
	{
		Debug.Log("[Build] BuildAnimation");
		AssetBundlePackage.ClearAllBundleNames();
		string text = Path.Combine(Application.streamingAssetsPath, "publish", "animation.unity3d");
		if (File.Exists(text))
		{
			Debug.Log("[Build] BuildAnimation Delete File");
			File.Delete(text);
		}
		string text2 = Path.Combine(Application.streamingAssetsPath, "publish", "animation.unity3d.manifest");
		if (File.Exists(text2))
		{
			Debug.Log("[Build] BuildAnimation Delete Manifest");
			File.Delete(text2);
		}
		IEnumerable<string> files = Directory.GetFiles("Assets/OriginalResources/AnimationClips", "*.anim", SearchOption.AllDirectories);
		string[] files2 = Directory.GetFiles("Assets/OriginalResources/AnimationController", "*.controller", SearchOption.AllDirectories);
		List<string> list = files.ToList<string>();
		list.AddRange(files2.ToList<string>());
		Singleton<DependencesMgr>.instance.CollectDependenceMap(list);
		Singleton<DependencesMgr>.instance.BuildAssetBundles();
		int num = 0;
		int totalFileCount = Singleton<DependencesMgr>.instance.totalFileCount;
		foreach (DependencesMgr.BundleItem bundleItem in Singleton<DependencesMgr>.instance.bundles)
		{
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in bundleItem.files)
			{
				if (!keyValuePair.Value.name.EndsWith(".cs"))
				{
					ABPackageTools.DisplayProgressBar(SdkLangManager.Get("str_sdk_bundleSettings_windowTitle0"), num++, totalFileCount);
					AssetImporter atPath = AssetImporter.GetAtPath(keyValuePair.Key);
					if (atPath && atPath.assetBundleName != bundleItem.name)
					{
						atPath.assetBundleName = SdkLangManager.Get("str_sdk_bundleSettings_windowtext0") + ABPackageSetting.abSuffixName;
					}
				}
			}
		}
		EditorUtility.ClearProgressBar();
		Debug.Log("[Build] BuildAnimation animation.unity_3d");
		AssetBundlePackage.CreateBundlesByPlatfrom(false, true);
		AssetBundlePackage.ClearAllBundleNames();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x000039C0 File Offset: 0x00001BC0
	public static void BuildActivity()
	{
		AssetBundlePackage.ClearAllBundleNames();
		List<string> list = new List<string>();
		foreach (string text in Directory.GetFiles("Assets/OriginalResources/Activity", "*.*", SearchOption.AllDirectories))
		{
			if (!text.EndsWith(".meta"))
			{
				list.Add(text);
			}
		}
		Singleton<DependencesMgr>.instance.CollectDependenceMap(list);
		Singleton<DependencesMgr>.instance.BuildAssetBundles();
		foreach (DependencesMgr.BundleItem bundleItem in Singleton<DependencesMgr>.instance.bundles)
		{
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in bundleItem.files)
			{
				if (!keyValuePair.Value.name.EndsWith(".cs"))
				{
					AssetImporter atPath = AssetImporter.GetAtPath(keyValuePair.Key);
					if (atPath && atPath.assetBundleName != bundleItem.name)
					{
						atPath.assetBundleName = "activity" + ABPackageSetting.abSuffixName;
					}
				}
			}
		}
		EditorUtility.ClearProgressBar();
		AssetBundlePackage.CreateBundlesByPlatfrom(false, true);
		AssetBundlePackage.ClearAllBundleNames();
	}
}
