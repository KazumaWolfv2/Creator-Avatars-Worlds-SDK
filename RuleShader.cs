using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class RuleShader : RuleBase
{
	// Token: 0x0600015B RID: 347 RVA: 0x00009BBC File Offset: 0x00007DBC
	public RuleShader(RuleRatingConfig ruleRatingConfig, bool needCheckNonOffical = true, bool hasAutoFix = true, bool hasAvatar = true)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_Shader];
		this.mNeedCheckNonOfficalShader = needCheckNonOffical;
		this.mHasAutoFixFunction = hasAutoFix;
		if (!hasAvatar)
		{
			this.mHasAutoShowFunction = true;
		}
		this.isAvatar = hasAvatar;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x000099EF File Offset: 0x00007BEF
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Shader;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00009C23 File Offset: 0x00007E23
	public override void Reset()
	{
		base.Reset();
		this.ResultJudgment = this.mRuleRating.Poor;
		this.ShaderNames.Clear();
		this.mShaders.Clear();
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00009C54 File Offset: 0x00007E54
	public override void Check(GameObject obj)
	{
		List<Material> materials = EditorUtil.GetMaterials(obj, false);
		this.materialSet.Clear();
		foreach (Material material in materials)
		{
			if (!(material == null) && !this.ShaderNames.Contains(material.shader.name))
			{
				this.materialSet.Add(material);
				this.ShaderNames.Add(material.shader.name);
				this.mShaders.Add(material.shader);
			}
		}
		if (this.isAvatar)
		{
			Object[] array = new GameObject[] { obj };
			array = EditorUtility.CollectDependencies(array);
			for (int i = 0; i < array.Length; i++)
			{
				Material material2 = array[i] as Material;
				if (material2 != null && !this.materialSet.Contains(material2) && !this.ShaderNames.Contains(material2.shader.name))
				{
					this.ShaderNames.Add(material2.shader.name);
					this.mShaders.Add(material2.shader);
				}
			}
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00009D88 File Offset: 0x00007F88
	public override void CalResult()
	{
		int count = this.ShaderNames.Count;
		if (count <= this.mRuleRating.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
		}
		else if (count <= this.mRuleRating.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
		}
		else if (count <= this.mRuleRating.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
		}
		else if (count <= this.mRuleRating.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
		}
		else if (count <= this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
		}
		else
		{
			this.Result = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_ShaderCount, null, "");
		}
		if (this.HaveNonOfficialShader())
		{
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_NonOfficialShader, this.mHasAutoFixFunction ? new Action<GameObject>(this.OpenShaderCorrection) : null, "");
		}
		if (this.HaveCompiledErrorShader())
		{
			if (this.isAvatar)
			{
				base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_MissingShader, this.mHasAutoFixFunction ? new Action<GameObject>(this.OpenShaderCorrection) : null, "");
				return;
			}
			base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_MissingShader, this.mHasAutoShowFunction ? new Action<GameObject>(this.OpenMateraiErrorWindow) : null, "");
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00009EB0 File Offset: 0x000080B0
	private bool HaveNonOfficialShader()
	{
		if (!this.mNeedCheckNonOfficalShader)
		{
			return false;
		}
		foreach (string text in this.ShaderNames)
		{
			if (!ConvertMaterialTools.IsLibriyShader(text) && text != "Hidden/InternalErrorShader")
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00009F24 File Offset: 0x00008124
	private bool ShaderHasError(Shader shader)
	{
		if (shader.name == "Hidden/InternalErrorShader")
		{
			return true;
		}
		return ShaderUtil.GetShaderMessages(shader).Any((ShaderMessage x) => x.severity == 0);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00009F64 File Offset: 0x00008164
	private bool HaveCompiledErrorShader()
	{
		foreach (Shader shader in this.mShaders)
		{
			if (this.ShaderHasError(shader))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00009FC0 File Offset: 0x000081C0
	private void OpenShaderCorrection(GameObject go)
	{
		DateTime now = DateTime.Now;
		AvatarShaderPlacement window = EditorWindow.GetWindow<AvatarShaderPlacement>();
		window.titleContent = new GUIContent(SdkLangManager.Get("str_sdk_shaderPlacement_title8"));
		window.OpenShaderCorrectionWindow(go);
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000A068 File Offset: 0x00008268
	private void OpenMateraiErrorWindow(GameObject go)
	{
		DateTime now = DateTime.Now;
		ShowMaterialErrorWindow window = EditorWindow.GetWindow<ShowMaterialErrorWindow>();
		window.titleContent = new GUIContent(SdkLangManager.Get("str_sdk_material_error_title8"));
		window.CorrectionErrorMaterials();
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000A10C File Offset: 0x0000830C
	private int GetResultValue()
	{
		return this.ShaderNames.Count;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000A119 File Offset: 0x00008319
	private int GetResultJudgment()
	{
		return this.ResultJudgment;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000A124 File Offset: 0x00008324
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleShader_text0"), new GUILayoutOption[]
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

	// Token: 0x040000E9 RID: 233
	private RuleRating mRuleRating;

	// Token: 0x040000EA RID: 234
	private List<string> ShaderNames = new List<string>();

	// Token: 0x040000EB RID: 235
	private List<Shader> mShaders = new List<Shader>();

	// Token: 0x040000EC RID: 236
	private int ResultJudgment;

	// Token: 0x040000ED RID: 237
	private bool mNeedCheckNonOfficalShader;

	// Token: 0x040000EE RID: 238
	private bool mHasAutoFixFunction;

	// Token: 0x040000EF RID: 239
	private bool mHasAutoShowFunction;

	// Token: 0x040000F0 RID: 240
	private bool isAvatar;

	// Token: 0x040000F1 RID: 241
	private HashSet<Material> materialSet = new HashSet<Material>();
}
