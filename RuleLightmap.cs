using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class RuleLightmap : RuleBase
{
	// Token: 0x060000D8 RID: 216 RVA: 0x00007E74 File Offset: 0x00006074
	public RuleLightmap(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_LightmapCount];
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00007E8F File Offset: 0x0000608F
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_LightmapCount;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00007E93 File Offset: 0x00006093
	public override void Reset()
	{
		base.Reset();
		this.mLightmapCount = 0;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00007EA4 File Offset: 0x000060A4
	public override void Check(GameObject obj)
	{
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		this.mLightmapCount = lightmaps.Length;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00007EC0 File Offset: 0x000060C0
	public override void CalResult()
	{
		if (this.mLightmapCount <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.mLightmapCount <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.mLightmapCount <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.mLightmapCount <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.mLightmapCount <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_LightmapCount, null, "");
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00007F6A File Offset: 0x0000616A
	private int GetResultValue()
	{
		return this.mLightmapCount;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00007F72 File Offset: 0x00006172
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00007F80 File Offset: 0x00006180
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleLightmap_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType result = this.GetResult();
		GUIStyle checkResultStyle = CheckResultRenderManager.Instance.GetCheckResultStyle(result);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetResultValue(), this.GetResultJudgment(), RuleResultDefine.GetRuleResultType(result)), checkResultStyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000C1 RID: 193
	private RuleRating mRuleRating;

	// Token: 0x040000C2 RID: 194
	private int mLightmapCount;
}
