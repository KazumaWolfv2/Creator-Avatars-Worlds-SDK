using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class RuleMaterial : RuleBase
{
	// Token: 0x060000E0 RID: 224 RVA: 0x00008022 File Offset: 0x00006222
	public RuleMaterial(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_Material];
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00008047 File Offset: 0x00006247
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Material;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000804A File Offset: 0x0000624A
	public override void Reset()
	{
		base.Reset();
		this.ResultJudgment = this.mRuleRating.Poor;
		this.MatNames.Clear();
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000806E File Offset: 0x0000626E
	public override void Check(GameObject obj)
	{
		this.CalMaterials(obj, false);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00008078 File Offset: 0x00006278
	public void CalMaterials(GameObject obj, bool includeDisabled)
	{
		MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>(includeDisabled);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren[i].sharedMaterials.Length; j++)
			{
				if (componentsInChildren[i].sharedMaterials[j] != null)
				{
					string assetPath = AssetDatabase.GetAssetPath(componentsInChildren[i].sharedMaterials[j]);
					if (!this.MatNames.Contains(assetPath))
					{
						this.MatNames.Add(assetPath);
					}
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = obj.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			for (int l = 0; l < componentsInChildren2[k].sharedMaterials.Length; l++)
			{
				if (componentsInChildren2[k].sharedMaterials[l] != null)
				{
					string assetPath2 = AssetDatabase.GetAssetPath(componentsInChildren2[k].sharedMaterials[l]);
					if (!this.MatNames.Contains(assetPath2))
					{
						this.MatNames.Add(assetPath2);
					}
				}
			}
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00008168 File Offset: 0x00006368
	public override void CalResult()
	{
		int count = this.MatNames.Count;
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
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_MaterialCount, null, "");
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00008204 File Offset: 0x00006404
	public int GetResultValue()
	{
		return this.MatNames.Count;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00008211 File Offset: 0x00006411
	public int GetResultJudgment()
	{
		return this.ResultJudgment;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000821C File Offset: 0x0000641C
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Material:", new GUILayoutOption[]
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

	// Token: 0x040000C3 RID: 195
	private RuleRating mRuleRating;

	// Token: 0x040000C4 RID: 196
	private List<string> MatNames = new List<string>();

	// Token: 0x040000C5 RID: 197
	private int ResultJudgment;
}
