using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200002F RID: 47
public class RuleSceneMemory : RuleBase
{
	// Token: 0x06000149 RID: 329 RVA: 0x00009831 File Offset: 0x00007A31
	public RuleSceneMemory(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleConfig = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_PackageTotalSize];
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000984C File Offset: 0x00007A4C
	public override void Reset()
	{
		this.UseMemorySize = 0f;
		this.Result = RuleResultType.RuleResultType_Prefect;
		this.IsReset = true;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009867 File Offset: 0x00007A67
	public override void Check(GameObject go)
	{
		if (this.IsReset)
		{
			this.UseMemorySize = this.CalcObjectUseMemorySize(go);
			this.IsReset = false;
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00009885 File Offset: 0x00007A85
	public override void CalResult()
	{
		if (this.UseMemorySize <= (float)this.mRuleConfig.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000098AC File Offset: 0x00007AAC
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleSceneMemory_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType result = this.GetResult();
		GUIStyle checkResultStyle = CheckResultRenderManager.Instance.GetCheckResultStyle(result);
		GUILayout.Label(string.Format("{0}/{1}(MB)({2})", this.GetResultValue(), this.GetResultJudgment(), RuleResultDefine.GetRuleResultType(result)), checkResultStyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00009949 File Offset: 0x00007B49
	private string GetResultValue()
	{
		return string.Format("{0:0.00}", this.UseMemorySize);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00009960 File Offset: 0x00007B60
	private float GetResultJudgment()
	{
		return (float)this.mRuleConfig.Poor;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00009970 File Offset: 0x00007B70
	private float CalcObjectUseMemorySize(GameObject go)
	{
		long num = 0L;
		if (go != null)
		{
			string path = SceneManager.GetActiveScene().path;
			if (!string.IsNullOrEmpty(path))
			{
				List<string> list = new List<string>();
				list.Add(path);
				Singleton<DependencesMgr>.instance.CollectDependenceMap(list);
				num = Singleton<DependencesMgr>.instance.CalcMemorySize();
			}
		}
		return (float)num / 1048576f;
	}

	// Token: 0x040000E3 RID: 227
	private float UseMemorySize;

	// Token: 0x040000E4 RID: 228
	private RuleRating mRuleConfig;

	// Token: 0x040000E5 RID: 229
	private bool IsReset;
}
