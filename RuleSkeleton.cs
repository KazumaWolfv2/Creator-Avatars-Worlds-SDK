using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class RuleSkeleton : RuleBase
{
	// Token: 0x06000168 RID: 360 RVA: 0x0000A1C6 File Offset: 0x000083C6
	public RuleSkeleton(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_Skeleton];
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000A1E1 File Offset: 0x000083E1
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Skeleton;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000A1E5 File Offset: 0x000083E5
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000A1F0 File Offset: 0x000083F0
	public override void Check(GameObject obj)
	{
		HashSet<Transform> hashSet = new HashSet<Transform>();
		SkinnedMeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			foreach (Transform transform in componentsInChildren[i].bones)
			{
				if (!(transform == null))
				{
					hashSet.Add(transform);
				}
			}
		}
		this.BoneCount = hashSet.Count;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000A258 File Offset: 0x00008458
	public override void CalResult()
	{
		int boneCount = this.BoneCount;
		if (boneCount <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (boneCount <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (boneCount <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (boneCount <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (boneCount <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_SkeletonCount, null, "");
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000A2F0 File Offset: 0x000084F0
	private int GetResultValue()
	{
		return this.BoneCount;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000A2F8 File Offset: 0x000084F8
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000A308 File Offset: 0x00008508
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Skeleton:", new GUILayoutOption[]
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

	// Token: 0x040000F2 RID: 242
	private RuleRating mRuleRating;

	// Token: 0x040000F3 RID: 243
	private int BoneCount;
}
