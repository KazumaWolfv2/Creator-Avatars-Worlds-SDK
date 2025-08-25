using System;

// Token: 0x02000015 RID: 21
public static class RuleResultDefine
{
	// Token: 0x06000084 RID: 132 RVA: 0x00006B30 File Offset: 0x00004D30
	public static string GetRuleResultType(RuleResultType type)
	{
		switch (type)
		{
		case RuleResultType.RuleResultType_Nil:
			return RuleResultDefine.RuleResultType_Nil;
		case RuleResultType.RuleResultType_Bad:
			return RuleResultDefine.RuleResultType_Bad;
		case RuleResultType.RuleResultType_Poor:
			return RuleResultDefine.RuleResultType_Poor;
		case RuleResultType.RuleResultType_Normal:
			return RuleResultDefine.RuleResultType_Normal;
		case RuleResultType.RuleResultType_Good:
			return RuleResultDefine.RuleResultType_Good;
		case RuleResultType.RuleResultType_Excellent:
			return RuleResultDefine.RuleResultType_Excellent;
		case RuleResultType.RuleResultType_Prefect:
			return RuleResultDefine.RuleResultType_Prefect;
		default:
			return RuleResultDefine.RuleResultType_Nil;
		}
	}

	// Token: 0x04000097 RID: 151
	private static string RuleResultType_Nil = SdkLangManager.Get("str_sdk_checkrule_nil");

	// Token: 0x04000098 RID: 152
	private static string RuleResultType_Bad = SdkLangManager.Get("str_sdk_checkrule_bad");

	// Token: 0x04000099 RID: 153
	private static string RuleResultType_Poor = SdkLangManager.Get("str_sdk_checkrule_poor");

	// Token: 0x0400009A RID: 154
	private static string RuleResultType_Normal = SdkLangManager.Get("str_sdk_checkrule_average");

	// Token: 0x0400009B RID: 155
	private static string RuleResultType_Good = SdkLangManager.Get("str_sdk_checkrule_good");

	// Token: 0x0400009C RID: 156
	private static string RuleResultType_Excellent = SdkLangManager.Get("str_sdk_checkrule_excellent");

	// Token: 0x0400009D RID: 157
	private static string RuleResultType_Prefect = SdkLangManager.Get("str_sdk_checkrule_prefect");
}
