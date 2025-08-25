using System;
using System.Collections.Generic;

// Token: 0x02000079 RID: 121
[Serializable]
public class SdkUrlPair
{
	// Token: 0x0600043D RID: 1085 RVA: 0x0001D178 File Offset: 0x0001B378
	public string GetUrl(string key)
	{
		foreach (SdkUrlData sdkUrlData in this.UrlList)
		{
			if (sdkUrlData.Key.Equals(key))
			{
				return sdkUrlData.Url;
			}
		}
		return null;
	}

	// Token: 0x04000259 RID: 601
	public List<SdkUrlData> UrlList;
}
