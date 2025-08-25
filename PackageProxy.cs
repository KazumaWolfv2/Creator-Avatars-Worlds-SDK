using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x02000052 RID: 82
public static class PackageProxy
{
	// Token: 0x06000283 RID: 643 RVA: 0x0000FF28 File Offset: 0x0000E128
	public static string BuildScene(string scenePath, string exportPath, ref List<string> audios)
	{
		audios.Clear();
		double timeSinceStartup = EditorApplication.timeSinceStartup;
		int num = scenePath.LastIndexOf("/");
		string text = scenePath.Substring(num + 1) + ".unitypackage";
		string text2 = Path.Combine(exportPath, text);
		string[] dependencies = AssetDatabase.GetDependencies(new string[] { scenePath });
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (string text3 in dependencies)
		{
			if (text3.EndsWith(".shader"))
			{
				list.Add(text3);
			}
			else if (text3.EndsWith(".dds"))
			{
				string text4 = PackageProxy.HandleForDDS(text3, list);
				list2.Add(text4);
			}
			else if (!text3.EndsWith(".cs") && !text3.EndsWith(".dll"))
			{
				if (text3.EndsWith(".mp3") && AssetDatabase.GetMainAssetTypeAtPath(text3) == typeof(AudioClip))
				{
					audios.Add(text3);
				}
				list.Add(text3);
			}
		}
		ParaLog.Log(string.Format("Collection Scene Dependencies cost {0} secs.", EditorApplication.timeSinceStartup - timeSinceStartup));
		Debug.Log("[ShaderStripper] current quality level " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
		string text5 = ShaderProxy.SavePreloadShader(scenePath);
		PlayerPrefs.SetString("ScenePath", scenePath);
		list.Add(text5);
		Debug.Log("Save shader varient colletion " + text5);
		ParaLog.Log(string.Format("Collection Shader Variants Cost {0} secs.", EditorApplication.timeSinceStartup - timeSinceStartup));
		AssetDatabase.ExportPackage(list.ToArray(), text2, 2);
		PackageProxy.RemoveTmpFiles(list2);
		ParaLog.Log(string.Format("Export Package Cost {0} secs.", EditorApplication.timeSinceStartup - timeSinceStartup));
		return text;
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000100E8 File Offset: 0x0000E2E8
	public static string Build(string prefabPath, string exportPath)
	{
		int num = prefabPath.LastIndexOf("/");
		string text = prefabPath.Substring(num + 1) + ".unitypackage";
		string text2 = Path.Combine(exportPath, text);
		List<string> list = new List<string>();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		PackageProxy.CollectDependency(prefabPath, ref list, ref dictionary);
		JsonData jsonData = new JsonData();
		jsonData["AssetPath"] = prefabPath;
		jsonData["ShaderKeys"] = new JsonData();
		jsonData["ShaderValues"] = new JsonData();
		jsonData["ShaderKeys"].SetJsonType(2);
		jsonData["ShaderValues"].SetJsonType(2);
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
		{
			jsonData["ShaderKeys"].Add(keyValuePair.Key);
			jsonData["ShaderValues"].Add(keyValuePair.Value);
		}
		string kPackageShaderFilePath = ParaPathDefine.kPackageShaderFilePath;
		string text3 = kPackageShaderFilePath + ParaPathDefine.kPackageShaderFileName;
		if (!Directory.Exists(kPackageShaderFilePath))
		{
			Directory.CreateDirectory(kPackageShaderFilePath);
		}
		list.Add(text3);
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (string text4 in list)
		{
			if (text4.EndsWith(".dds"))
			{
				string text5 = PackageProxy.HandleForDDS(text4, list2);
				list3.Add(text5);
			}
			else if (!text4.EndsWith(".shader") && !text4.EndsWith(".cs") && !text4.EndsWith(".dll"))
			{
				list2.Add(text4);
			}
		}
		File.WriteAllText(text3, jsonData.ToJson());
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		AssetDatabase.ExportPackage(list2.ToArray(), text2, 2);
		Directory.Delete(ParaPathDefine.kPackageShaderFileBasePath, true);
		string text6 = ParaPathDefine.kPackageShaderFileBasePath + ".meta";
		if (File.Exists(text6))
		{
			File.Delete(text6);
		}
		PackageProxy.RemoveTmpFiles(list3);
		return text;
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00010328 File Offset: 0x0000E528
	private static bool IsNeedInPackage(string file)
	{
		return !file.EndsWith(".manifest");
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0001033C File Offset: 0x0000E53C
	private static void CollectDependency(string file, ref List<string> assetList, ref Dictionary<string, string> shaderDict)
	{
		if (assetList.Contains(file))
		{
			return;
		}
		assetList.Add(file);
		if (file.EndsWith(".mat"))
		{
			Material material = AssetDatabase.LoadAssetAtPath<Material>(file);
			if (material != null)
			{
				string assetPath = AssetDatabase.GetAssetPath(material.shader);
				if (!shaderDict.ContainsKey(file))
				{
					shaderDict.Add(file, assetPath);
				}
			}
		}
		else if (file.EndsWith(".asset"))
		{
			Material material2 = AssetDatabase.LoadAssetAtPath<Material>(file);
			if (material2 != null)
			{
				string assetPath2 = AssetDatabase.GetAssetPath(material2.shader);
				if (!shaderDict.ContainsKey(file))
				{
					shaderDict.Add(file, assetPath2);
				}
			}
		}
		foreach (string text in AssetDatabase.GetDependencies(file))
		{
			if (PackageProxy.IsNeedInPackage(text))
			{
				PackageProxy.CollectDependency(text, ref assetList, ref shaderDict);
			}
		}
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0001040C File Offset: 0x0000E60C
	private static string HandleForDDS(string path, List<string> assetList)
	{
		Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
		if (texture2D == null)
		{
			return null;
		}
		string text = path.Replace(".dds", "_cover.png");
		string text2 = Application.dataPath.Replace("Assets", text);
		EditorUtil.SaveTextureToLocal(EditorUtil.CoverCompressTexture(texture2D), text2);
		AssetDatabase.ImportAsset(text);
		assetList.Add(path);
		assetList.Add(text);
		return text2;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00010470 File Offset: 0x0000E670
	private static void RemoveTmpFiles(List<string> tmpPathList)
	{
		for (int i = 0; i < tmpPathList.Count; i++)
		{
			if (File.Exists(tmpPathList[i]))
			{
				File.Delete(tmpPathList[i]);
			}
		}
	}
}
