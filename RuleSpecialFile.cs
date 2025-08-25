using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class RuleSpecialFile : RuleBase
{
	// Token: 0x06000170 RID: 368 RVA: 0x0000A3A5 File Offset: 0x000085A5
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_SpecialFile;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000A3A8 File Offset: 0x000085A8
	public override void Reset()
	{
		base.Reset();
		this.PathList.Clear();
		this.PathList.Add(Application.dataPath + "/HamletSDK/HamletPackageJson/hamlet_package.json");
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000A3D8 File Offset: 0x000085D8
	private void AddJsonFile(GameObject obj)
	{
		GameObject correspondingObjectFromSource = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(obj);
		if (correspondingObjectFromSource == null)
		{
			return;
		}
		string assetPath = AssetDatabase.GetAssetPath(correspondingObjectFromSource);
		int num = assetPath.LastIndexOf(".");
		int num2 = assetPath.IndexOf("/");
		string text = assetPath.Substring(num2, num - num2) + "/HamletPackageJson/hamlet_package.asset";
		this.PathList.Add(text);
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000A434 File Offset: 0x00008634
	public override void Check(GameObject obj)
	{
		this.AddJsonFile(obj);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00003394 File Offset: 0x00001594
	public override void CheckNode(GameObject obj)
	{
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000A440 File Offset: 0x00008640
	public void Fix(GameObject obj)
	{
		foreach (string text in this.PathList)
		{
			if (File.Exists(text))
			{
				File.Delete(text);
			}
		}
		base.DelRuleFix(RuleErrorRuleType.RuleErrorRuleType_SpecialFile);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000A4A4 File Offset: 0x000086A4
	public override void CalResult()
	{
		this.Result = RuleResultType.RuleResultType_Prefect;
		using (List<string>.Enumerator enumerator = this.PathList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (File.Exists(enumerator.Current))
				{
					this.Result = RuleResultType.RuleResultType_Bad;
					Action<GameObject> action = delegate(GameObject go)
					{
						this.Fix(go);
					};
					base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_SpecialFile, action, "");
					break;
				}
			}
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000A524 File Offset: 0x00008724
	public override void RenderResult()
	{
		RuleResultType result = this.GetResult();
		if (result == RuleResultType.RuleResultType_Prefect)
		{
			return;
		}
		GUIStyle styleRed = CheckResultRenderManager.Instance.StyleRed;
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleSpecialFile_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleSpecialFile_text1", RuleResultDefine.GetRuleResultType(result)), styleRed, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000F4 RID: 244
	private List<string> PathList = new List<string>();
}
