using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class RuleTriangles : RuleBase
{
	// Token: 0x06000190 RID: 400 RVA: 0x0000AC59 File Offset: 0x00008E59
	public RuleTriangles(RuleRatingConfig ruleRatingConfig, RuleResultType warningStartResult = RuleResultType.RuleResultType_Good)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_Triangles];
		this.mWarningStartResult = warningStartResult;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000AC7A File Offset: 0x00008E7A
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Triangles;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000AC7D File Offset: 0x00008E7D
	public override void Reset()
	{
		base.Reset();
		this.mTrianglesCount = 0;
		this.ResultJudgment = this.mRuleRating.Poor;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000ACA0 File Offset: 0x00008EA0
	public override void Check(GameObject obj)
	{
		foreach (MeshFilter meshFilter in obj.GetComponentsInChildren<MeshFilter>(false))
		{
			if (!(meshFilter == null) && !(meshFilter.sharedMesh == null))
			{
				int num = meshFilter.sharedMesh.triangles.Length / 3;
				this.mTrianglesCount += num;
			}
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>(false))
		{
			if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
			{
				int num2 = skinnedMeshRenderer.sharedMesh.triangles.Length / 3;
				this.mTrianglesCount += num2;
			}
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000AD58 File Offset: 0x00008F58
	private int GetResultValue()
	{
		return this.mTrianglesCount;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000AD60 File Offset: 0x00008F60
	private int GetResultJudgment()
	{
		return this.ResultJudgment;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000AD68 File Offset: 0x00008F68
	public override void CalResult()
	{
		int num = this.mTrianglesCount;
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
			if (this.Result <= this.mWarningStartResult)
			{
				base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_FragmentCountLowerLimit, null, "");
				return;
			}
		}
		else if (num <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			if (this.Result <= this.mWarningStartResult)
			{
				base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_FragmentCountLowerLimit, null, "");
				return;
			}
		}
		else if (num <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			if (this.Result <= this.mWarningStartResult)
			{
				base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_FragmentCountLowerLimit, null, "");
				return;
			}
		}
		else
		{
			this.Result = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_FragmentCountHightLimit, null, "");
		}
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000AE58 File Offset: 0x00009058
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Triangles:", new GUILayoutOption[]
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

	// Token: 0x040000FC RID: 252
	private RuleRating mRuleRating;

	// Token: 0x040000FD RID: 253
	private RuleResultType mWarningStartResult;

	// Token: 0x040000FE RID: 254
	private int ResultJudgment;

	// Token: 0x040000FF RID: 255
	private int mTrianglesCount;
}
