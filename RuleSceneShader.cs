using System;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class RuleSceneShader : RuleBase
{
	// Token: 0x06000151 RID: 337 RVA: 0x000099CA File Offset: 0x00007BCA
	public RuleSceneShader(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_Shader];
	}

	// Token: 0x06000152 RID: 338 RVA: 0x000099EF File Offset: 0x00007BEF
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Shader;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x000099F2 File Offset: 0x00007BF2
	public override void Reset()
	{
		base.Reset();
		this.ShaderNames.Clear();
		this.bReset = true;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00009A0C File Offset: 0x00007C0C
	public override void Check(GameObject obj)
	{
		if (!this.bReset)
		{
			return;
		}
		this.bReset = false;
		Renderer[] array = Object.FindObjectsOfType<Renderer>();
		for (int i = 0; i < array.Length; i++)
		{
			Material[] sharedMaterials = array[i].sharedMaterials;
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				if (!(sharedMaterials[j] == null) && !this.ShaderNames.Contains(sharedMaterials[j].shader.name))
				{
					this.ShaderNames.Add(sharedMaterials[j].shader.name);
				}
			}
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00003394 File Offset: 0x00001594
	public override void CheckNode(GameObject obj)
	{
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009A91 File Offset: 0x00007C91
	public override void CalResult()
	{
		this.Result = RuleResultType.RuleResultType_Prefect;
		if (this.HaveNonOfficialShader())
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_NonOfficialShader, new Action<GameObject>(this.OpenShaderCorrection), "");
		}
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009ABC File Offset: 0x00007CBC
	private bool HaveNonOfficialShader()
	{
		foreach (string text in this.ShaderNames)
		{
			if (this.IsBulidinShader(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00009B18 File Offset: 0x00007D18
	private bool IsBulidinShader(string shaderName)
	{
		return shaderName.Equals("Standard");
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00009B2C File Offset: 0x00007D2C
	private void OpenShaderCorrection(GameObject go)
	{
		DateTime now = DateTime.Now;
		WorldShaderPlacement window = EditorWindow.GetWindow<WorldShaderPlacement>();
		window.OpenShaderCorrectionWindow(go);
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00003394 File Offset: 0x00001594
	public override void RenderResult()
	{
	}

	// Token: 0x040000E6 RID: 230
	private RuleRating mRuleRating;

	// Token: 0x040000E7 RID: 231
	private List<string> ShaderNames = new List<string>();

	// Token: 0x040000E8 RID: 232
	private bool bReset;
}
