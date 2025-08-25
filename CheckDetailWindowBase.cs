using System;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class CheckDetailWindowBase : EditorWindow
{
	// Token: 0x06000309 RID: 777 RVA: 0x00013FD1 File Offset: 0x000121D1
	public bool HadError()
	{
		return this.IsHaveError;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00013FDC File Offset: 0x000121DC
	private void OnEnable()
	{
		float num = 96f;
		float num2 = (float)Screen.currentResolution.height * 5f / (6f * (Screen.dpi / num));
		if (num2 > 690f)
		{
			base.minSize = new Vector2(510f, 690f);
			base.maxSize = new Vector2(510f, 690f);
			this.BottomBoxHeight = 400f;
		}
		else
		{
			this.BottomBoxHeight = num2 - 266f;
			base.minSize = new Vector2(510f, num2);
			base.maxSize = new Vector2(510f, num2 + 20f);
		}
		this.SvLp = new GUILayoutOption[] { GUILayout.Height(base.minSize.y) };
		base.titleContent.text = SdkLangManager.Get("str_sdk_resource_check");
		this.Pos = Vector2.zero;
		this.PosBottom = Vector2.zero;
		this.DataList = new List<RuleBase>();
		this.TextureWarningPath = ParaPathDefine.kEditorWarningTexturePath;
		this.TextureErrorPath = ParaPathDefine.kEditorErrorTexturePath;
		this.ErrorTexture = EditorUtil.LoadTexture(this.TextureErrorPath);
		this.WarningTexture = EditorUtil.LoadTexture(this.TextureWarningPath);
		this.DataList = new List<RuleBase>();
		this.InvalidList = new List<RuleBase>();
		this.IsHaveError = false;
		this.IsAutoFix = false;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00014138 File Offset: 0x00012338
	protected List<int> CalInfoLenght(string title, string content, bool isAuto)
	{
		GUIContent guicontent = new GUIContent();
		guicontent.text = content;
		GUIStyle guistyle = new GUIStyle(EditorStyles.wordWrappedLabel);
		float num = guistyle.CalcHeight(guicontent, 417f) + 20f;
		guicontent.text = title;
		int num2 = (isAuto ? 320 : (417 - (int)this.ErrorRect.height));
		float num3 = guistyle.CalcHeight(guicontent, (float)num2);
		num3 = ((num3 < 28f) ? 28f : num3);
		float num4 = num + num3;
		return new List<int>
		{
			(int)num3,
			(int)num,
			(int)num4 + 20
		};
	}

	// Token: 0x0600030C RID: 780 RVA: 0x000141D4 File Offset: 0x000123D4
	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		EditorGUILayout.Space(25f);
		EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		EditorGUILayout.Space(8f);
		this.GUIDrawTitle();
		EditorGUILayout.Space(5f);
		this.GUIDrawInfo();
		EditorGUILayout.Space(5f);
		this.GUIDrawDetail();
		EditorGUILayout.Space(20f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(35f);
		GUI.enabled = this.IsAutoFix;
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarPublish_allFix"), new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.ExpandWidth(false),
			GUILayout.Height(35f)
		}))
		{
			JsonData jsonData = new JsonData();
			jsonData["event"] = "unity_editor_button_click";
			jsonData["function_type"] = 3;
			jsonData["function_name"] = "resource_check";
			jsonData["button_type"] = 1;
			jsonData["button_id"] = 602;
			jsonData["tab_name"] = "";
			AppLogService.PushAppLog(jsonData);
			this.FixAll();
			GUIUtility.ExitGUI();
		}
		GUILayout.Space(20f);
		GUI.enabled = !this.IsHaveError;
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarPublish_btnUpload"), new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.ExpandWidth(false),
			GUILayout.Height(35f)
		}))
		{
			JsonData jsonData2 = new JsonData();
			jsonData2["event"] = "unity_editor_button_click";
			jsonData2["function_type"] = 3;
			jsonData2["function_name"] = "resource_check";
			jsonData2["button_type"] = 1;
			jsonData2["button_id"] = 603;
			jsonData2["tab_name"] = "";
			AppLogService.PushAppLog(jsonData2);
			this.Upload();
			GUIUtility.ExitGUI();
		}
		if (Event.current.commandName == "ValidateCommand")
		{
			this.Check();
		}
		GUI.enabled = true;
		GUILayout.Space(20f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarPublish_btnRefresh"), new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.ExpandWidth(false),
			GUILayout.Height(35f)
		}))
		{
			this.Check();
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(35f);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00014490 File Offset: 0x00012690
	protected virtual void GUIDrawTitle()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(28f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-105f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarPublish_btnReplace"), new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.ExpandWidth(false),
			GUILayout.Height(24f)
		}))
		{
			this.Replace();
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(-30f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarPublish_nameField") + this.AvatarTitleName, new GUILayoutOption[]
		{
			GUILayout.Height(28f),
			GUILayout.ExpandWidth(false)
		});
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00014584 File Offset: 0x00012784
	private void GUIDrawInfo()
	{
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(162f)
		});
		GUILayout.Space(-164f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(25f)
		});
		GUILayout.Space(-27f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarPublish_ratingField") + RuleResultDefine.GetRuleResultType(this.mRuleManager.GetResult()), new GUILayoutOption[]
		{
			GUILayout.Height(28f),
			GUILayout.ExpandWidth(false)
		});
		this.Pos = this.ScrollList<RuleBase>(this.Pos, this.DataList, new Action<int, RuleBase>(this.Render<RuleBase>), new GUILayoutOption[]
		{
			GUILayout.Width(465f),
			GUILayout.Height(130f)
		}, new GUILayoutOption[0], false, true);
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0001469C File Offset: 0x0001289C
	public void Render<T>(int index, T data)
	{
		RuleBase ruleBase = data as RuleBase;
		if (ruleBase == null)
		{
			return;
		}
		ruleBase.RenderResult();
	}

	// Token: 0x06000310 RID: 784 RVA: 0x000146C0 File Offset: 0x000128C0
	public Vector2 ScrollList<T>(Vector2 listPos, List<T> dataList, Action<int, T> renderFunc, GUILayoutOption[] svLp, GUILayoutOption[] containerLp, bool alwaysShowHorizontal = false, bool alwaysShowVertical = false)
	{
		listPos = GUILayout.BeginScrollView(listPos, alwaysShowHorizontal, alwaysShowVertical, svLp);
		GUILayout.BeginVertical(containerLp);
		for (int i = 0; i < dataList.Count; i++)
		{
			renderFunc(i, dataList[i]);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		return listPos;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0001470C File Offset: 0x0001290C
	private void GUIDrawDetail()
	{
		GUILayout.Space(5f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(this.BottomBoxHeight)
		});
		GUILayout.Space(-this.BottomBoxHeight);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		this.PosBottom = this.ScrollList<RuleBase>(this.PosBottom, this.InvalidList, new Action<int, RuleBase>(this.RenderBottom<RuleBase>), new GUILayoutOption[]
		{
			GUILayout.Width(465f),
			GUILayout.Height(this.BottomBoxHeight)
		}, new GUILayoutOption[0], false, true);
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000312 RID: 786 RVA: 0x000147CC File Offset: 0x000129CC
	public void RenderBottom<T>(int index, T data)
	{
		RuleBase ruleBase = data as RuleBase;
		if (ruleBase.FixList.Count > 0)
		{
			for (int i = 0; i < ruleBase.FixList.Count; i++)
			{
				RuleFix ruleFix = ruleBase.FixList[i];
				if (!ruleFix.IsValid)
				{
					switch (ruleFix.Type)
					{
					case RuleErrorRuleType.RuleErrorRuleType_MissingShader:
						this.GUIDrawMissingShader(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_NonOfficialShader:
						this.GUIDrawNonOfficialShader(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_ShaderCount:
						this.GUIDrawShaderCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_MaterialCount:
						this.GUIDrawMaterialCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_FragmentCountLowerLimit:
						this.GUIDrawFragmentCountLowerLimit(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_FragmentCountHightLimit:
						this.GUIDrawFragmentCountHightLimit(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_TextureSizeLowerLimit:
						this.GUIDrawTextureSizeLowerLimit(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_TextureSizeHightLimit:
						this.GUIDrawTextureSizeHightLimit(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_TextureCount:
						this.GUIDrawTextureCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_MeshCount:
						this.GUIDrawMeshCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_SkeletonCount:
						this.GUIDrawSkeletonCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_DynamicBoneCount:
						this.GUIDrawDynamicBoneCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_SkeletonColliderCount:
						this.GUIDrawSkeletonColliderCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_PostProcessCount:
						this.GUIDrawPostProcess(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_DirectionalLightCount:
						this.GUIDrawDirectionalLightCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_RealLightCount:
						this.GUIDrawRealLightCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_RealTimeShadowCount:
						this.GUIDrawRealTimeShadowCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_RealProbeCount:
						this.GUIDrawRealProbeCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_RTLayer:
						this.GUIDrawRTLayer(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_SpecialFile:
						this.GUIDrawSpecialFilePath(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_MissingAsset:
						this.GUIDrawMissingAsset(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_MissingScript:
						this.GUIDrawMissingScript(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_ParticleSystemCount:
						this.GUIDrawParticleSystemCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_ParticleSystemMesh:
						this.GUIDrawParticleSystemMesh(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_ParticleActiveCount:
						this.GUIDrawParticleActiveCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_LightmapCount:
						this.GUIDrawLightmapCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_NonConvexMeshCollider:
						this.GUIDrawNonConvexMeshCollider(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_LayerInVaild:
						this.GUIDrawLayerInvalid(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_LayerCountInVaild:
						this.GUIDrawLayerCountInvalid(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_BlendShapeCount:
						this.GUIDrawBlendShapeCount(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_EditorOnly:
						this.GUIDrawEditorOnly(ruleFix);
						break;
					case RuleErrorRuleType.RuleErrorRuleType_MissingHumanBone:
						this.GUIDrawMissingHumanBine(ruleFix);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00014A10 File Offset: 0x00012C10
	protected void GUIDrawCommonContent(string title, string content, RuleFix ruleFix)
	{
		List<int> list = this.CalInfoLenght(title, content, ruleFix.FixFunc != null);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(428f),
			GUILayout.Height((float)list[2]),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space((float)(-(float)list[2] - 2));
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(428f),
			GUILayout.Height((float)(list[0] + 12)),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space((float)(-(float)list[0] - 12));
		bool flag = false;
		if (ruleFix.FixFunc != null)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(353f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Space(6f);
			string text = SdkLangManager.Get("str_sdk_str_sdk_avatarPublish_autoFix");
			if (ruleFix.ErrorType == RuleErrorType.RuleErrorType_Warning)
			{
				text = SdkLangManager.Get("str_sdk_str_sdk_avatarPublish_shouFix");
			}
			if (GUILayout.Button(text, new GUILayoutOption[]
			{
				GUILayout.Width(65f),
				GUILayout.Height(20f)
			}))
			{
				JsonData jsonData = new JsonData();
				jsonData["event"] = "unity_editor_button_click";
				jsonData["function_type"] = 3;
				jsonData["function_name"] = "resource_check";
				jsonData["button_type"] = 1;
				jsonData["button_id"] = 601;
				jsonData["tab_name"] = "";
				AppLogService.PushAppLog(jsonData);
				foreach (GameObject gameObject in this.mRootObjects)
				{
					this.Fix(ruleFix, gameObject);
				}
				GUIUtility.ExitGUI();
			}
			flag = true;
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Space((float)(-(float)list[0] - 1));
		}
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		Texture2D texture2D = ((ruleFix.ErrorType == RuleErrorType.RuleErrorType_Error) ? this.ErrorTexture : this.WarningTexture);
		EditorUtil.DrawTexture(this.ErrorRect, texture2D, "Tu");
		int num = (flag ? 320 : (417 - (int)this.ErrorRect.width));
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(6f);
		GUILayout.Label(title.Replace(" ", "\u00a0"), EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width((float)num),
			GUILayout.Height((float)list[0])
		});
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label(content, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width(417f),
			GUILayout.Height((float)list[1]),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(8f);
		GUILayout.EndVertical();
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00014D3F File Offset: 0x00012F3F
	private void Fix(RuleFix ruleFix, GameObject go)
	{
		ruleFix.FixFunc(go);
		this.Check();
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawMissingShader(RuleFix ruleFix)
	{
	}

	// Token: 0x06000316 RID: 790 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawNonOfficialShader(RuleFix ruleFix)
	{
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawShaderCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawMaterialCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000319 RID: 793 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawFragmentCountLowerLimit(RuleFix ruleFix)
	{
	}

	// Token: 0x0600031A RID: 794 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawFragmentCountHightLimit(RuleFix ruleFix)
	{
	}

	// Token: 0x0600031B RID: 795 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawTextureSizeLowerLimit(RuleFix ruleFix)
	{
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawTextureSizeHightLimit(RuleFix ruleFix)
	{
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawTextureCount(RuleFix ruleFix)
	{
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawMeshCount(RuleFix ruleFix)
	{
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawSkeletonCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawDynamicBoneCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawSkeletonColliderCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawPostProcess(RuleFix ruleFix)
	{
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawDirectionalLightCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawRealLightCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawRealTimeShadowCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawRealProbeCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawRTLayer(RuleFix ruleFix)
	{
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawSpecialFilePath(RuleFix ruleFix)
	{
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawMissingScript(RuleFix ruleFix)
	{
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawParticleSystemCount(RuleFix ruleFix)
	{
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawParticleActiveCount(RuleFix ruleFix)
	{
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawParticleSystemMesh(RuleFix ruleFix)
	{
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawLightmapCount(RuleFix ruleFix)
	{
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawNonConvexMeshCollider(RuleFix ruleFix)
	{
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawLayerInvalid(RuleFix ruleFix)
	{
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawLayerCountInvalid(RuleFix ruleFix)
	{
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawBlendShapeCount(RuleFix ruleFix)
	{
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawMissingAsset(RuleFix ruleFix)
	{
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawEditorOnly(RuleFix ruleFix)
	{
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void GUIDrawMissingHumanBine(RuleFix ruleFix)
	{
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00014D54 File Offset: 0x00012F54
	protected virtual void Check()
	{
		this.InvalidList.Clear();
		this.mRuleManager.Check(this.mRootObjects);
		this.IsHaveError = false;
		foreach (RuleBase ruleBase in this.InvalidList)
		{
			if (ruleBase.FixList.Count > 0)
			{
				foreach (RuleFix ruleFix in ruleBase.FixList)
				{
					if (!ruleFix.IsValid)
					{
						if (ruleFix.ErrorType == RuleErrorType.RuleErrorType_Error)
						{
							this.IsHaveError = true;
						}
						if (ruleFix.FixFunc != null)
						{
							this.IsAutoFix = true;
						}
						if (this.IsAutoFix && this.IsHaveError)
						{
							return;
						}
					}
				}
			}
		}
		if (this.mRuleManager.GetResult() <= RuleResultType.RuleResultType_Bad)
		{
			this.IsHaveError = true;
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00014E60 File Offset: 0x00013060
	private void FixAll()
	{
		foreach (RuleBase ruleBase in this.InvalidList)
		{
			if (ruleBase.FixList.Count > 0)
			{
				foreach (RuleFix ruleFix in ruleBase.FixList)
				{
					if (ruleFix.FixFunc != null)
					{
						foreach (GameObject gameObject in this.mRootObjects)
						{
							ruleFix.FixFunc(gameObject);
						}
					}
				}
			}
		}
		this.Check();
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void Replace()
	{
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void Upload()
	{
	}

	// Token: 0x040001B3 RID: 435
	protected string AvatarTitleName;

	// Token: 0x040001B4 RID: 436
	protected MonoBehaviour mParaRoot;

	// Token: 0x040001B5 RID: 437
	protected GameObject[] mRootObjects;

	// Token: 0x040001B6 RID: 438
	private Vector2 Pos;

	// Token: 0x040001B7 RID: 439
	private Vector2 PosBottom;

	// Token: 0x040001B8 RID: 440
	protected List<RuleBase> DataList;

	// Token: 0x040001B9 RID: 441
	protected List<RuleBase> InvalidList;

	// Token: 0x040001BA RID: 442
	private bool IsHaveError;

	// Token: 0x040001BB RID: 443
	private bool IsAutoFix;

	// Token: 0x040001BC RID: 444
	private string TextureErrorPath;

	// Token: 0x040001BD RID: 445
	private string TextureWarningPath;

	// Token: 0x040001BE RID: 446
	private Texture2D ErrorTexture;

	// Token: 0x040001BF RID: 447
	private Texture2D WarningTexture;

	// Token: 0x040001C0 RID: 448
	protected Rect ErrorRect = new Rect(0f, 0f, 28f, 28f);

	// Token: 0x040001C1 RID: 449
	protected RuleManager mRuleManager;

	// Token: 0x040001C2 RID: 450
	protected GUILayoutOption[] SvLp;

	// Token: 0x040001C3 RID: 451
	private float BottomBoxHeight;
}
