using System;
using Oasis.Unity;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class RuleDynamicBoneCollider : RuleBase
{
	// Token: 0x060000B7 RID: 183 RVA: 0x000075BD File Offset: 0x000057BD
	public RuleDynamicBoneCollider(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_DynamicCollider];
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000075D8 File Offset: 0x000057D8
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_DynamicCollider;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000075DC File Offset: 0x000057DC
	public override void Reset()
	{
		base.Reset();
		this.mColliderCount = 0;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000075EC File Offset: 0x000057EC
	public override void Check(GameObject obj)
	{
		Type type = AssemblyHandler.GetType("DynamicBoneCollider", "Assembly-CSharp");
		if (type != null)
		{
			Component[] componentsInChildren = obj.GetComponentsInChildren(type, false);
			this.mColliderCount += componentsInChildren.Length;
		}
		SpringAnimator[] componentsInChildren2 = obj.GetComponentsInChildren<SpringAnimator>(false);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			foreach (SpringBone springBone in componentsInChildren2[i].SpringBones)
			{
				this.mColliderCount += springBone.sphereColliders.Length;
				this.mColliderCount += springBone.capsuleColliders.Length;
			}
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00007694 File Offset: 0x00005894
	public override void CalResult()
	{
		int num = this.mColliderCount;
		if (num <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (num <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (num <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (num <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (num <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_SkeletonColliderCount, null, "");
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000772C File Offset: 0x0000592C
	private int GetResultValue()
	{
		return this.mColliderCount;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00007734 File Offset: 0x00005934
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00007744 File Offset: 0x00005944
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("DynamicBoneCollider:", new GUILayoutOption[]
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

	// Token: 0x040000B4 RID: 180
	private RuleRating mRuleRating;

	// Token: 0x040000B5 RID: 181
	private int mColliderCount;
}
