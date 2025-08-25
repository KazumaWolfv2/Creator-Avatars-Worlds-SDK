using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class RuleRealTimeShadow : RuleBase
{
	// Token: 0x06000141 RID: 321 RVA: 0x00009655 File Offset: 0x00007855
	public RuleRealTimeShadow(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_RealTimeShadow];
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00009670 File Offset: 0x00007870
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_RealTimeShadow;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00009674 File Offset: 0x00007874
	public override void Reset()
	{
		base.Reset();
		this.mRealTimeShadowCount = 0;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00009684 File Offset: 0x00007884
	public override void Check(GameObject obj)
	{
		foreach (Light light in obj.GetComponentsInChildren<Light>(false))
		{
			if (light.type != 1 && light.lightmapBakeType != 2 && light.shadows != null)
			{
				this.mRealTimeShadowCount++;
			}
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x000096D4 File Offset: 0x000078D4
	public override void CalResult()
	{
		if (this.mRealTimeShadowCount <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.mRealTimeShadowCount <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.mRealTimeShadowCount <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.mRealTimeShadowCount <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.mRealTimeShadowCount <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_RealTimeShadowCount, null, "");
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000977E File Offset: 0x0000797E
	private int GetResultValue()
	{
		return this.mRealTimeShadowCount;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00009786 File Offset: 0x00007986
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00009794 File Offset: 0x00007994
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("RealShadowLight:", new GUILayoutOption[]
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

	// Token: 0x040000E1 RID: 225
	private RuleRating mRuleRating;

	// Token: 0x040000E2 RID: 226
	private int mRealTimeShadowCount;
}
