using System;

// Token: 0x02000078 RID: 120
[Serializable]
public class SdkUrlData
{
	// Token: 0x0600043C RID: 1084 RVA: 0x0001D15F File Offset: 0x0001B35F
	public SdkUrlData(string key, string url)
	{
		this.Url = url;
		this.Key = key;
	}

	// Token: 0x04000257 RID: 599
	public string Key;

	// Token: 0x04000258 RID: 600
	public string Url;
}
