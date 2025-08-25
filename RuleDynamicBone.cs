using System;
using System.Collections.Generic;
using Oasis.Unity;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class RuleDynamicBone : RuleBase
{
	// Token: 0x060000AF RID: 175 RVA: 0x000073BF File Offset: 0x000055BF
	public RuleDynamicBone(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_DynamicBone];
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000073E5 File Offset: 0x000055E5
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_DynamicBone;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000073E9 File Offset: 0x000055E9
	public override void Reset()
	{
		base.Reset();
		this.ObjList.Clear();
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000073FC File Offset: 0x000055FC
	public override void Check(GameObject obj)
	{
		Type type = AssemblyHandler.GetType("DynamicBone", "Assembly-CSharp");
		if (type != null)
		{
			Component[] componentsInChildren = obj.GetComponentsInChildren(type, false);
			this.ObjList.AddRange(componentsInChildren);
		}
		SpringAnimator[] componentsInChildren2 = obj.GetComponentsInChildren<SpringAnimator>(false);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			SpringBone[] springBones = componentsInChildren2[i].SpringBones;
			this.ObjList.AddRange(springBones);
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00007464 File Offset: 0x00005664
	private int GetResultValue()
	{
		return this.ObjList.Count;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00007471 File Offset: 0x00005671
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00007480 File Offset: 0x00005680
	public override void CalResult()
	{
		int count = this.ObjList.Count;
		if (count <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (count <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (count <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (count <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (count <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_DynamicBoneCount, null, "");
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00007520 File Offset: 0x00005720
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("DynamicBone:", new GUILayoutOption[]
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

	// Token: 0x040000B2 RID: 178
	private RuleRating mRuleRating;

	// Token: 0x040000B3 RID: 179
	private List<object> ObjList = new List<object>();
}
