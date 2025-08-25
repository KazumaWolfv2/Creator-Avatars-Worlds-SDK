using System;
using System.Collections.Generic;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class RuleLayer : RuleBase
{
	// Token: 0x060000D1 RID: 209 RVA: 0x00007CA5 File Offset: 0x00005EA5
	public RuleLayer(RuleRatingConfig ruleRatingConfig)
	{
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00007CD2 File Offset: 0x00005ED2
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Layer;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00007CD6 File Offset: 0x00005ED6
	public override void Reset()
	{
		base.Reset();
		this.ErrorList.Clear();
		this.mLayerResult = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00007CF0 File Offset: 0x00005EF0
	public override void Check(GameObject rootObj)
	{
		RuleLayer.IsValidLayer(rootObj, this.ErrorList, this.mBeginIndex, this.mEndIndex);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00007D0C File Offset: 0x00005F0C
	private static void IsValidLayer(GameObject go, List<int> errorList, int beginIndex, int endIndex)
	{
		int layer = go.layer;
		if (layer > beginIndex && layer < endIndex && !errorList.Contains(layer))
		{
			errorList.Add(layer);
		}
		foreach (object obj in go.transform)
		{
			RuleLayer.IsValidLayer(((Transform)obj).gameObject, errorList, beginIndex, endIndex);
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00007D8C File Offset: 0x00005F8C
	public override void CalResult()
	{
		this.ErrorList.Sort();
		if (this.ErrorList.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int num in this.ErrorList)
			{
				stringBuilder.Append(num);
				stringBuilder.Append(",");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			this.mContent = stringBuilder.ToString();
			this.mLayerResult = RuleResultType.RuleResultType_Bad;
		}
		if (this.mLayerResult == RuleResultType.RuleResultType_Bad)
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_LayerInVaild, null, this.mContent);
		}
		if (InternalEditorUtility.tags != null && InternalEditorUtility.tags.Length > 32)
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_LayerCountInVaild, null, this.mContent);
		}
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00007E6C File Offset: 0x0000606C
	public override RuleResultType GetResult()
	{
		return this.mLayerResult;
	}

	// Token: 0x040000BC RID: 188
	private int mBeginIndex = 5;

	// Token: 0x040000BD RID: 189
	private int mEndIndex = 16;

	// Token: 0x040000BE RID: 190
	private RuleResultType mLayerResult;

	// Token: 0x040000BF RID: 191
	private string mContent = "";

	// Token: 0x040000C0 RID: 192
	private List<int> ErrorList = new List<int>();
}
