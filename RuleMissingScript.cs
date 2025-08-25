using System;
using UnityEditor;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class RuleMissingScript : RuleBase
{
	// Token: 0x06000106 RID: 262 RVA: 0x000088C8 File Offset: 0x00006AC8
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_MissingScript;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000088CB File Offset: 0x00006ACB
	public override void Reset()
	{
		base.Reset();
		this.Count = 0;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000088DC File Offset: 0x00006ADC
	public override void Check(GameObject rootObj)
	{
		this.Count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(rootObj);
		foreach (Transform transform in rootObj.GetComponentsInChildren<Transform>(true))
		{
			this.Count += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject);
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00008928 File Offset: 0x00006B28
	private void Fix(GameObject rootObj)
	{
		GameObjectUtility.RemoveMonoBehavioursWithMissingScript(rootObj);
		foreach (Transform transform in rootObj.GetComponentsInChildren<Transform>(true))
		{
			if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject) > 0)
			{
				Undo.RegisterCompleteObjectUndo(transform, SdkLangManager.Get("str_sdk_ruleMissingScript_text0"));
				GameObjectUtility.RemoveMonoBehavioursWithMissingScript(transform.gameObject);
			}
		}
		base.DelRuleFix(RuleErrorRuleType.RuleErrorRuleType_MissingScript);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00008988 File Offset: 0x00006B88
	public override void CalResult()
	{
		if (this.Count > 0)
		{
			this.Result = RuleResultType.RuleResultType_Bad;
			Action<GameObject> action = delegate(GameObject go)
			{
				this.Fix(go);
			};
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_MissingScript, action, "");
			return;
		}
		this.Result = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x000089CC File Offset: 0x00006BCC
	public override void RenderResult()
	{
		RuleResultType result = this.GetResult();
		if (result == RuleResultType.RuleResultType_Prefect)
		{
			return;
		}
		GUIStyle styleRed = CheckResultRenderManager.Instance.StyleRed;
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleMissingScript_text1", RuleResultDefine.GetRuleResultType(result)), styleRed, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000CC RID: 204
	private int Count;
}
