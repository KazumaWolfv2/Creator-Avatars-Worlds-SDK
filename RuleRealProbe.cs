using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class RuleRealProbe : RuleBase
{
	// Token: 0x06000139 RID: 313 RVA: 0x0000948F File Offset: 0x0000768F
	public RuleRealProbe(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_RealProbe];
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00008A5D File Offset: 0x00006C5D
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_RealProbe;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x000094AA File Offset: 0x000076AA
	public override void Reset()
	{
		base.Reset();
		this.mProbeCount = 0;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000094BC File Offset: 0x000076BC
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

	// Token: 0x0600013D RID: 317 RVA: 0x000094F8 File Offset: 0x000076F8
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

	// Token: 0x0600013E RID: 318 RVA: 0x000095A2 File Offset: 0x000077A2
	private int GetResultValue()
	{
		return this.mProbeCount;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000095AA File Offset: 0x000077AA
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x000095B8 File Offset: 0x000077B8
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

	// Token: 0x040000DF RID: 223
	private RuleRating mRuleRating;

	// Token: 0x040000E0 RID: 224
	private int mProbeCount;
}
