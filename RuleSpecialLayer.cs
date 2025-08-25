using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class RuleSpecialLayer : RuleBase
{
	// Token: 0x0600017A RID: 378 RVA: 0x0000A5D0 File Offset: 0x000087D0
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_SpecialLayer;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000A1E5 File Offset: 0x000083E5
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00003394 File Offset: 0x00001594
	public override void Check(GameObject obj)
	{
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00003394 File Offset: 0x00001594
	public void Fix(GameObject obj)
	{
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000A5D3 File Offset: 0x000087D3
	public override void CheckNode(GameObject obj)
	{
		if (obj.layer == LayerMask.NameToLayer("RT"))
		{
			this.Result = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_RTLayer, null, "");
			return;
		}
		this.Result = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00003394 File Offset: 0x00001594
	public override void CalResult()
	{
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000A608 File Offset: 0x00008808
	public override void RenderResult()
	{
		RuleResultType result = this.GetResult();
		if (result == RuleResultType.RuleResultType_Prefect)
		{
			return;
		}
		GUIStyle styleRed = CheckResultRenderManager.Instance.StyleRed;
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleSpecialLayer_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleSpecialLayer_text1", RuleResultDefine.GetRuleResultType(result)), styleRed, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}
}
