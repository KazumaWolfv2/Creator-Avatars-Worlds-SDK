using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;

// Token: 0x02000006 RID: 6
public class DependencesMgr : Singleton<DependencesMgr>
{
	// Token: 0x06000019 RID: 25 RVA: 0x000028D0 File Offset: 0x00000AD0
	public void CollectDependenceMap(List<string> prefabFiles)
	{
		this.Clear();
		for (int i = 0; i < prefabFiles.Count; i++)
		{
			string text = prefabFiles[i];
			this.currentPrefab = text;
			if (i % 100 == 0)
			{
				string text2 = string.Format("({0}/{1}){2}", i + 1, prefabFiles.Count, text);
				EditorUtility.DisplayProgressBar("Calculate Resource Dependencies", text2, (float)i * 1f / (float)prefabFiles.Count);
			}
			string[] dependencies = AssetDatabase.GetDependencies(text);
			this.AddDependency(text, "");
			foreach (string text3 in dependencies)
			{
				string text4 = text3.ToLower();
				if (this.IsneedInPage(text4) && !text4.Equals(text))
				{
					this.AddDependency(text, text3);
				}
			}
		}
		EditorUtility.ClearProgressBar();
		this.totalFileCount = this.dependMap.Count;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000029B0 File Offset: 0x00000BB0
	private void AddDependency(string file, string dependFile)
	{
		if (!this.dependMap.ContainsKey(file))
		{
			this.dependMap.Add(file, new DependencesMgr.ResItem(file, DependencesMgr.fileID++));
		}
		if (string.IsNullOrEmpty(dependFile))
		{
			return;
		}
		if (!this.dependMap.ContainsKey(dependFile))
		{
			this.dependMap.Add(dependFile, new DependencesMgr.ResItem(dependFile, DependencesMgr.fileID++));
			if (this.IsFBX(dependFile))
			{
				return;
			}
			foreach (string text in AssetDatabase.GetDependencies(dependFile))
			{
				if (this.IsneedInPage(text) && !text.Equals(dependFile))
				{
					this.AddDependency(dependFile, text);
				}
			}
		}
		this.dependMap[file].depend[dependFile] = this.dependMap[dependFile];
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002A80 File Offset: 0x00000C80
	public void BuildAssetBundles()
	{
		int num = 1;
		foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in this.dependMap)
		{
			DependencesMgr.BundleItem bundleItem = new DependencesMgr.BundleItem();
			this.ParseBundleName(keyValuePair.Value.name);
			string text = this.ParsePrefabName(this.currentPrefab);
			if (this.IsFBX(keyValuePair.Value.name))
			{
				bundleItem.name = text + ABPackageSetting.abSuffixName;
			}
			if (this.IsShader(keyValuePair.Value.name))
			{
				bundleItem.name = "shaderlists" + ABPackageSetting.abSuffixName;
			}
			if (this.IsPrefab(keyValuePair.Value.name))
			{
				bundleItem.name = text + ABPackageSetting.abSuffixName;
			}
			bundleItem.id = num++;
			bundleItem.files.Add(keyValuePair.Key, keyValuePair.Value);
			try
			{
				this.bundles.Add(bundleItem);
			}
			catch (Exception ex)
			{
				ParaLog.LogError(string.Concat(new string[]
				{
					bundleItem.name,
					" ",
					keyValuePair.Key,
					" ",
					ex.ToString()
				}));
			}
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002C08 File Offset: 0x00000E08
	public long CalcMemorySize()
	{
		long num = 0L;
		foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in this.dependMap)
		{
			num += keyValuePair.Value.fileLen;
		}
		return num;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002C68 File Offset: 0x00000E68
	private void AddDependencyTemp(string file, string dependFile)
	{
		if (!this.tempdependMap.ContainsKey(file))
		{
			this.tempdependMap.Add(file, new DependencesMgr.ResItem(file, DependencesMgr.fileID2++));
		}
		if (string.IsNullOrEmpty(dependFile))
		{
			return;
		}
		if (!this.tempdependMap.ContainsKey(dependFile))
		{
			this.tempdependMap.Add(dependFile, new DependencesMgr.ResItem(dependFile, DependencesMgr.fileID2++));
			if (this.IsFBX(dependFile))
			{
				return;
			}
			foreach (string text in AssetDatabase.GetDependencies(dependFile))
			{
				if (this.IsneedInPage(text) && !text.Equals(dependFile))
				{
					this.AddDependencyTemp(dependFile, text);
				}
			}
		}
		this.tempdependMap[file].depend[dependFile] = this.tempdependMap[dependFile];
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002D38 File Offset: 0x00000F38
	public void BuildAssetBundlesForJson()
	{
		int num = 0;
		foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in this.tempdependMap)
		{
			DependencesMgr.BundleItem bundleItem = new DependencesMgr.BundleItem();
			this.ParseBundleName(keyValuePair.Value.name);
			string text = this.ParsePrefabName(this.currentPrefab);
			if (this.IsShader(keyValuePair.Value.name))
			{
				bundleItem.name = "shaderlists" + ABPackageSetting.abSuffixName;
			}
			else
			{
				bundleItem.name = text + ABPackageSetting.abSuffixName;
			}
			bundleItem.id = num++;
			bundleItem.files.Add(keyValuePair.Key, keyValuePair.Value);
			try
			{
				this.tempbundleMap.Add(bundleItem);
			}
			catch (Exception ex)
			{
				ParaLog.LogError(string.Concat(new string[]
				{
					bundleItem.name,
					" ",
					keyValuePair.Key,
					" ",
					ex.ToString()
				}));
			}
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002E70 File Offset: 0x00001070
	private string ParseBundleName(string strFilePath)
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

	// Token: 0x06000020 RID: 32 RVA: 0x00002ED8 File Offset: 0x000010D8
	private string ParsePrefabName(string prefab)
	{
		string empty = string.Empty;
		int num = prefab.LastIndexOf('/');
		int length = prefab.Length;
		string text = prefab.Substring(num + 1, length - num - 1);
		num = text.IndexOf('.');
		return text.Substring(0, num);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002F19 File Offset: 0x00001119
	private bool IsneedInPage(string file)
	{
		return !file.EndsWith(".meta") && !file.EndsWith(".manifest");
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002F38 File Offset: 0x00001138
	private bool IsPrefab(string file)
	{
		return file.EndsWith(".prefab");
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002F4A File Offset: 0x0000114A
	private bool IsFBX(string file)
	{
		return file.EndsWith(".fbx") || file.EndsWith(".FBX");
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002F69 File Offset: 0x00001169
	private bool IsShader(string file)
	{
		return file.EndsWith(".shader") || file.EndsWith(".hlsl") || file.EndsWith(".shadervariants");
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002F98 File Offset: 0x00001198
	public void SaveFileList(string path)
	{
		StreamWriter streamWriter = new StreamWriter(path + "/" + ABPackageSetting.assetListFile);
		foreach (DependencesMgr.BundleItem bundleItem in this.bundles)
		{
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in bundleItem.files)
			{
				string key = keyValuePair.Key;
				streamWriter.WriteLine(string.Format("{0}|{1}|{2}", key, keyValuePair.Value.id, bundleItem.name).ToLower());
			}
		}
		streamWriter.Close();
		streamWriter = new StreamWriter(path + "/" + ABPackageSetting.dependceListFile);
		foreach (DependencesMgr.BundleItem bundleItem2 in this.bundles)
		{
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair2 in bundleItem2.files)
			{
				string text = keyValuePair2.Value.id.ToString() + ":";
				foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair3 in keyValuePair2.Value.depend)
				{
					text = text + keyValuePair3.Value.id.ToString() + ",";
				}
				if (keyValuePair2.Value.depend.Count > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
				streamWriter.WriteLine(text);
			}
		}
		streamWriter.Close();
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000031C0 File Offset: 0x000013C0
	private void SavePrefabJSon(string path, string prefab)
	{
		foreach (DependencesMgr.BundleItem bundleItem in this.tempbundleMap)
		{
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in bundleItem.files)
			{
				string key = keyValuePair.Key;
				if (this.IsShader(key) || this.IsPrefab(key))
				{
					this.assetbundle.AddfileList(keyValuePair.Value.id, key, bundleItem.name);
				}
			}
		}
		string text = JsonMapper.ToJson(this.assetbundle);
		if (!string.IsNullOrEmpty(text))
		{
			StreamWriter streamWriter = new StreamWriter(path + "/" + prefab + ABPackageSetting.JSonExt);
			streamWriter.WriteLine(text);
			streamWriter.Close();
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000032BC File Offset: 0x000014BC
	public void Clear()
	{
		this.dependMap.Clear();
		this.bundles.Clear();
		DependencesMgr.fileID = 1;
		this.totalFileCount = 0;
	}

	// Token: 0x04000008 RID: 8
	private static int fileID = 1;

	// Token: 0x04000009 RID: 9
	public int totalFileCount;

	// Token: 0x0400000A RID: 10
	public string currentPrefab = string.Empty;

	// Token: 0x0400000B RID: 11
	public Dictionary<string, DependencesMgr.ResItem> dependMap = new Dictionary<string, DependencesMgr.ResItem>();

	// Token: 0x0400000C RID: 12
	public List<DependencesMgr.BundleItem> bundles = new List<DependencesMgr.BundleItem>();

	// Token: 0x0400000D RID: 13
	private static int fileID2 = 0;

	// Token: 0x0400000E RID: 14
	private Dictionary<string, DependencesMgr.ResItem> tempdependMap = new Dictionary<string, DependencesMgr.ResItem>();

	// Token: 0x0400000F RID: 15
	private List<DependencesMgr.BundleItem> tempbundleMap = new List<DependencesMgr.BundleItem>();

	// Token: 0x04000010 RID: 16
	public JAssetAndBundle assetbundle = new JAssetAndBundle();

	// Token: 0x02000088 RID: 136
	public class ResItem
	{
		// Token: 0x060004CC RID: 1228 RVA: 0x00023CA0 File Offset: 0x00021EA0
		public ResItem(string file, int fileID)
		{
			this.id = fileID;
			this.name = file;
			FileInfo fileInfo = new FileInfo(file);
			this.fileLen = fileInfo.Length;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00023CDF File Offset: 0x00021EDF
		public void Clear()
		{
			this.id = 0;
			this.name = "";
			this.fileLen = 0L;
			if (this.depend != null)
			{
				this.depend.Clear();
			}
		}

		// Token: 0x04000310 RID: 784
		public int id;

		// Token: 0x04000311 RID: 785
		public string name;

		// Token: 0x04000312 RID: 786
		public long fileLen;

		// Token: 0x04000313 RID: 787
		public Dictionary<string, DependencesMgr.ResItem> depend = new Dictionary<string, DependencesMgr.ResItem>();
	}

	// Token: 0x02000089 RID: 137
	public class BundleItem
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00023D10 File Offset: 0x00021F10
		public void CalcFileSize()
		{
			this.totalFileLen = 0L;
			foreach (KeyValuePair<string, DependencesMgr.ResItem> keyValuePair in this.files)
			{
				this.totalFileLen += keyValuePair.Value.fileLen;
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00023D80 File Offset: 0x00021F80
		public void Clear()
		{
			this.id = 0;
			this.name = "";
			this.totalFileLen = 0L;
			this.files.Clear();
		}

		// Token: 0x04000314 RID: 788
		public int id;

		// Token: 0x04000315 RID: 789
		public string name;

		// Token: 0x04000316 RID: 790
		public long totalFileLen;

		// Token: 0x04000317 RID: 791
		public Dictionary<string, DependencesMgr.ResItem> files = new Dictionary<string, DependencesMgr.ResItem>();
	}
}
