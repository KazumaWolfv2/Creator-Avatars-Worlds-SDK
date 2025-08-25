using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class RuleBlendShape : RuleBase
{
	// Token: 0x060000A5 RID: 165 RVA: 0x00007169 File Offset: 0x00005369
	public RuleBlendShape(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_BlendShape];
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00007184 File Offset: 0x00005384
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_BlendShape;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00007188 File Offset: 0x00005388
	public override void Reset()
	{
		this.blendShapeCount = 0;
		this.Result = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00007198 File Offset: 0x00005398
	public override void Check(GameObject obj)
	{
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>(false))
		{
			if (skinnedMeshRenderer.sharedMesh != null)
			{
				this.blendShapeCount += skinnedMeshRenderer.sharedMesh.blendShapeCount;
			}
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000071E8 File Offset: 0x000053E8
	public override void CalResult()
	{
		if (this.blendShapeCount <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.blendShapeCount <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.blendShapeCount <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.blendShapeCount <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.blendShapeCount <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_BlendShapeCount, null, "");
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00007292 File Offset: 0x00005492
	private int GetResultValue()
	{
		return this.blendShapeCount;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0000729A File Offset: 0x0000549A
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000072A8 File Offset: 0x000054A8
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("BlendShape:", new GUILayoutOption[]
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

	// Token: 0x040000B0 RID: 176
	private RuleRating mRuleRating;

	// Token: 0x040000B1 RID: 177
	private int blendShapeCount;
}
