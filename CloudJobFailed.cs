using System;

// Token: 0x02000068 RID: 104
public static class CloudJobFailed
{
	// Token: 0x0600033A RID: 826 RVA: 0x00014F60 File Offset: 0x00013160
	public static string GetAvatarJobFailed(int code)
	{
		switch (code)
		{
		case -1:
			return SdkLangManager.Get("str_sdk_cloudFailed_text4");
		case 0:
			return SdkLangManager.Get("str_sdk_cloudFailed_text3");
		case 1:
			return SdkLangManager.Get("str_sdk_cloudFailed_text2");
		default:
			return SdkLangManager.Get("str_sdk_cloudFailed_title2");
		}
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00014FB0 File Offset: 0x000131B0
	public static string GetWorldJobFailed(int code)
	{
		switch (code)
		{
		case -1:
			return SdkLangManager.Get("str_sdk_cloudFailed_text4");
		case 0:
			return SdkLangManager.Get("str_sdk_cloudFailed_text3");
		case 1:
			return SdkLangManager.Get("str_sdk_cloudFailed_text2");
		default:
			return SdkLangManager.Get("str_sdk_cloudFailed_title2");
		}
	}
}
