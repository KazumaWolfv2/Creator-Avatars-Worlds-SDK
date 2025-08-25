using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class RuleMesh : RuleBase
{
	// Token: 0x060000F2 RID: 242 RVA: 0x00008506 File Offset: 0x00006706
	public RuleMesh(RuleRatingConfig ruleRatingConfig, bool isAllowNonConvexCollider = true)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_Mesh];
		this.mAllowNonConvexCollider = isAllowNonConvexCollider;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00008528 File Offset: 0x00006728
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Mesh;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0000852C File Offset: 0x0000672C
	public override void Reset()
	{
		base.Reset();
		this.mMeshCount = 0;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000853C File Offset: 0x0000673C
	public override void Check(GameObject obj)
	{
		MeshFilter[] componentsInChildren = obj.GetComponentsInChildren<MeshFilter>(false);
		SkinnedMeshRenderer[] componentsInChildren2 = obj.GetComponentsInChildren<SkinnedMeshRenderer>(false);
		this.mMeshCount += componentsInChildren.Length + componentsInChildren2.Length;
		foreach (LODGroup lodgroup in obj.GetComponentsInChildren<LODGroup>(false))
		{
			if (lodgroup.lodCount >= 1)
			{
				this.mMeshCount += 1 - lodgroup.lodCount;
			}
		}
		if (this.NeedFixNonConvexCollider(obj))
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_NonConvexMeshCollider, new Action<GameObject>(this.FixNonConvexCollider), "");
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000085CC File Offset: 0x000067CC
	private bool NeedFixNonConvexCollider(GameObject obj)
	{
		if (this.mAllowNonConvexCollider)
		{
			return false;
		}
		MeshCollider[] componentsInChildren = obj.GetComponentsInChildren<MeshCollider>(false);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].convex)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00008606 File Offset: 0x00006806
	private int GetResultValue()
	{
		return this.mMeshCount;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000860E File Offset: 0x0000680E
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000861C File Offset: 0x0000681C
	public override void CalResult()
	{
		if (this.mMeshCount <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.mMeshCount <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.mMeshCount <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.mMeshCount <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.mMeshCount <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_MeshCount, null, "");
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000086C8 File Offset: 0x000068C8
	private void FixNonConvexCollider(GameObject go)
	{
		foreach (MeshCollider meshCollider in go.GetComponentsInChildren<MeshCollider>(false))
		{
			if (!meshCollider.convex)
			{
				meshCollider.convex = true;
			}
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00008700 File Offset: 0x00006900
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleMesh_text0"), new GUILayoutOption[]
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

	// Token: 0x040000C8 RID: 200
	private RuleRating mRuleRating;

	// Token: 0x040000C9 RID: 201
	private int mMeshCount;

	// Token: 0x040000CA RID: 202
	private bool mAllowNonConvexCollider;
}
