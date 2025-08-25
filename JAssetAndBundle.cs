using System;
using System.Collections.Generic;

// Token: 0x02000007 RID: 7
[Serializable]
public class JAssetAndBundle
{
	// Token: 0x0600002A RID: 42 RVA: 0x00003347 File Offset: 0x00001547
	public void Clear()
	{
		this.PrefabAssetGUID = "";
		this.JsonVersion = "1.0.0.0";
		this.fileList.Clear();
	}

	// Token: 0x0600002B RID: 43 RVA: 0x0000336A File Offset: 0x0000156A
	public void SetGUID(string guid)
	{
		this.PrefabAssetGUID = guid;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003373 File Offset: 0x00001573
	public void AddfileList(int id, string _assetName, string _bundleName)
	{
		this.fileList.Add(id, new JAssetAndBundle.TagKeyValue
		{
			assetname = _assetName,
			bundleid = _bundleName
		});
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00003394 File Offset: 0x00001594
	public void AddDependenceUnit(int id, string depID)
	{
	}

	// Token: 0x04000011 RID: 17
	public string PrefabAssetGUID = string.Empty;

	// Token: 0x04000012 RID: 18
	public string JsonVersion = string.Empty;

	// Token: 0x04000013 RID: 19
	public Dictionary<int, JAssetAndBundle.TagKeyValue> fileList = new Dictionary<int, JAssetAndBundle.TagKeyValue>();

	// Token: 0x0200008A RID: 138
	[Serializable]
	public class TagKeyValue
	{
		// Token: 0x04000318 RID: 792
		public string assetname;

		// Token: 0x04000319 RID: 793
		public string bundleid;
	}
}
