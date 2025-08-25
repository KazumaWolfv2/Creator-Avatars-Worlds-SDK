using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class RulePackageSize : RuleBase
{
	// Token: 0x0600010E RID: 270 RVA: 0x00008A42 File Offset: 0x00006C42
	public RulePackageSize(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_RealProbe];
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00008A5D File Offset: 0x00006C5D
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_RealProbe;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00008A61 File Offset: 0x00006C61
	public override void Reset()
	{
		base.Reset();
		this.mProbeCount = 0;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00008A70 File Offset: 0x00006C70
	public override void Check(GameObject obj)
	{
		ReflectionProbe[] componentsInChildren = obj.GetComponentsInChildren<ReflectionProbe>(false);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].mode == 1)
			{
				this.mProbeCount++;
			}
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00008AAC File Offset: 0x00006CAC
	public override void CalResult()
	{
		if (this.mProbeCount <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.mProbeCount <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.mProbeCount <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.mProbeCount <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.mProbeCount <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_RealProbeCount, null, "");
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00008B56 File Offset: 0x00006D56
	private int GetResultValue()
	{
		return this.mProbeCount;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00008B5E File Offset: 0x00006D5E
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00008B6C File Offset: 0x00006D6C
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("RealProbe:", new GUILayoutOption[]
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

	// Token: 0x040000CD RID: 205
	private RuleRating mRuleRating;

	// Token: 0x040000CE RID: 206
	private int mProbeCount;
}
