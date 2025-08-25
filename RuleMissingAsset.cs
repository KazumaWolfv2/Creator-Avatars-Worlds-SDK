using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class RuleMissingAsset : RuleBase
{
	// Token: 0x060000FC RID: 252 RVA: 0x000087A2 File Offset: 0x000069A2
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_MissingAsset;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000087A5 File Offset: 0x000069A5
	public override void Reset()
	{
		base.Reset();
		this.Result = RuleResultType.RuleResultType_Prefect;
		this.mInvalidGameobject.Clear();
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00003394 File Offset: 0x00001594
	public override void Check(GameObject obj)
	{
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00003394 File Offset: 0x00001594
	public void Fix(GameObject obj)
	{
	}

	// Token: 0x06000100 RID: 256 RVA: 0x000087C0 File Offset: 0x000069C0
	private void FindMissing(GameObject obj)
	{
		if (obj.name.Contains("Missing Prefab"))
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_MissingAsset, null, obj.name);
		}
		if (PrefabUtility.IsPrefabAssetMissing(obj))
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_MissingAsset, null, obj.name);
			this.mInvalidGameobject.Add(obj);
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00008813 File Offset: 0x00006A13
	public override void CheckNode(GameObject obj)
	{
		this.FindMissing(obj);
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00003394 File Offset: 0x00001594
	public override void CalResult()
	{
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000881C File Offset: 0x00006A1C
	public override void RenderResult()
	{
		RuleResultType result = this.GetResult();
		if (result == RuleResultType.RuleResultType_Prefect)
		{
			return;
		}
		GUIStyle styleRed = CheckResultRenderManager.Instance.StyleRed;
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleMissingAsset_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleMissingAsset_title1", RuleResultDefine.GetRuleResultType(result)), styleRed, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000CB RID: 203
	private List<GameObject> mInvalidGameobject = new List<GameObject>();
}
