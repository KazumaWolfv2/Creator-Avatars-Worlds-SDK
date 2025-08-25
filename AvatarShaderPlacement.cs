using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200000E RID: 14
[ExecuteAlways]
public class AvatarShaderPlacement : EditorWindow
{
	// Token: 0x06000058 RID: 88 RVA: 0x00004850 File Offset: 0x00002A50
	public static void ShowWindow()
	{
		if (AvatarShaderPlacement.IsShowedWindows)
		{
			return;
		}
		DateTime now = DateTime.Now;
		AvatarShaderPlacement window = EditorWindow.GetWindow<AvatarShaderPlacement>();
		window.LoadConvertRuleConfig();
		window.titleContent = new GUIContent(SdkLangManager.Get("str_sdk_shaderPlacement_title8"));
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000048FC File Offset: 0x00002AFC
	private void InitAvailableCategories()
	{
		this.m_shaderCategorysLabels = new GUIContent[17];
		this.m_shaderCategorys = new string[17];
		this.m_shaderCategorys[0] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_HD");
		this.m_shaderCategorys[1] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_HD_Outline");
		this.m_shaderCategorys[2] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_HD_Translucent");
		this.m_shaderCategorys[3] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_Preset_Hair");
		this.m_shaderCategorys[4] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_Preset_Skin");
		this.m_shaderCategorys[5] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_Preset_Skin_Outline");
		this.m_shaderCategorys[6] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_Simple");
		this.m_shaderCategorys[7] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_Universe");
		this.m_shaderCategorys[8] = new string("ParaSpace/Avatar/Cartoon/AvatarNPR_HD_AlphaTest");
		this.m_shaderCategorys[9] = new string("ParaSpace/Avatar/PBR/AvatarPBR");
		this.m_shaderCategorys[10] = new string("ParaSpace/Avatar/PBR/AvatarPBR_Optimatize");
		this.m_shaderCategorys[11] = new string("ParaSpace/Avatar/PBR/AvatarPBR_Preset_Glass");
		this.m_shaderCategorys[12] = new string("ParaSpace/Avatar/PBR/AvatarPBR_Translucent");
		this.m_shaderCategorys[13] = new string("ParaSpace/Avatar/PBR/AvatarPBR_AlphaTest");
		this.m_shaderCategorys[14] = new string("ParaSpace/Avatar/Effect/AvatarVFX");
		this.m_shaderCategorys[15] = new string("ParaSpace/Avatar/Effect/AvatarVFX_ParticlesUnlit");
		this.m_shaderCategorys[16] = new string("ParaSpace/Avatar/Effect/AvatarVFX_Rotation");
		this.m_shaderCategorysLabels[0] = new GUIContent("Cartoon/AvatarNPR_HD");
		this.m_shaderCategorysLabels[1] = new GUIContent("Cartoon/AvatarNPR_HD_Outline");
		this.m_shaderCategorysLabels[2] = new GUIContent("Cartoon/AvatarNPR_HD_Translucent");
		this.m_shaderCategorysLabels[3] = new GUIContent("Cartoon/AvatarNPR_Preset_Hair");
		this.m_shaderCategorysLabels[4] = new GUIContent("Cartoon/AvatarNPR_Preset_Skin");
		this.m_shaderCategorysLabels[5] = new GUIContent("Cartoon/AvatarNPR_Preset_Skin_Outline");
		this.m_shaderCategorysLabels[6] = new GUIContent("Cartoon/AvatarNPR_Simple");
		this.m_shaderCategorysLabels[7] = new GUIContent("Cartoon/AvatarNPR_Universe");
		this.m_shaderCategorysLabels[8] = new GUIContent("Cartoon/AvatarNPR_HD_AlphaTest");
		this.m_shaderCategorysLabels[9] = new GUIContent("PBR/AvatarPBR");
		this.m_shaderCategorysLabels[10] = new GUIContent("PBR/AvatarPBR_Optimatize");
		this.m_shaderCategorysLabels[11] = new GUIContent("PBR/AvatarPBR_Preset_Glass");
		this.m_shaderCategorysLabels[12] = new GUIContent("PBR/AvatarPBR_Translucent");
		this.m_shaderCategorysLabels[13] = new GUIContent("PBR/AvatarPBR_AlphaTest");
		this.m_shaderCategorysLabels[14] = new GUIContent("Effect/AvatarVFX");
		this.m_shaderCategorysLabels[15] = new GUIContent("Effect/AvatarVFX_ParticlesUnlit");
		this.m_shaderCategorysLabels[16] = new GUIContent("Effect/AvatarVFX_Rotation");
		this.errorIcon = EditorUtil.LoadTexture("Packages/com.para.common/Config/Texture/errorIcon.png");
		this.rightIcon = EditorUtil.LoadTexture("Packages/com.para.common/Config/Texture/rightIcon.png");
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00004C0C File Offset: 0x00002E0C
	protected void DrawCurrentShaderType()
	{
		if (this.m_shaderCategorysLabels == null)
		{
			this.InitAvailableCategories();
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00004C1C File Offset: 0x00002E1C
	private void OnEnable()
	{
		AvatarShaderPlacement.IsShowedWindows = true;
		base.minSize = new Vector2(700f, 480f);
		base.maxSize = new Vector2(700f, 480f);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00004C4E File Offset: 0x00002E4E
	private void OnDisable()
	{
		AvatarShaderPlacement.IsShowedWindows = false;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003394 File Offset: 0x00001594
	private void OnHierarchyChange()
	{
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004C56 File Offset: 0x00002E56
	public void OpenShaderCorrectionWindow(GameObject avater)
	{
		this.selectObj = avater;
		this.lastObj = this.selectObj;
		this.LoadConvertRuleConfig();
		this.ConvertPropertysGameObject(this.selectObj as GameObject);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00004C84 File Offset: 0x00002E84
	public static bool IsPrefabInstance(GameObject obj)
	{
		bool prefabAssetType = PrefabUtility.GetPrefabAssetType(obj) != null;
		PrefabInstanceStatus prefabInstanceStatus = PrefabUtility.GetPrefabInstanceStatus(obj);
		return prefabAssetType && prefabInstanceStatus != null;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004CA8 File Offset: 0x00002EA8
	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title10"), EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
		this.selectObj = EditorGUILayout.ObjectField(this.selectObj, typeof(GameObject), Array.Empty<GUILayoutOption>());
		if (this.selectObj != this.lastObj)
		{
			this.lastObj = this.selectObj;
			if (this.selectObj != null)
			{
				this.ConvertPropertysGameObject(this.selectObj as GameObject);
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space(8f);
		EditorGUILayout.BeginVertical(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_shaderPlacement_title11"), EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ContentManager_text32"), EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
		this.selectObject = GUILayout.Toggle(this.selectObject, "                           ", Array.Empty<GUILayoutOption>());
		if (this.selectObject != this.lastSelect)
		{
			PlayerPrefs.SetInt("LocalErrorHandle", this.selectObject ? 1 : 0);
			this.lastSelect = this.selectObject;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.Space(8f);
		EditorGUILayout.BeginVertical(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_shaderPlacement_title12"), EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
		EditorGUILayout.EndVertical();
		this.DrawCurrentShaderType();
		this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[]
		{
			GUILayout.Width(700f),
			GUILayout.Height(280f)
		});
		EditorGUILayout.Space(8f);
		foreach (KeyValuePair<Material, AvatarShaderPlacement.ConvertUnityForShader> keyValuePair in this.displayList)
		{
			AvatarShaderPlacement.ConvertUnityForShader value = keyValuePair.Value;
			if (value != null && value.src != null)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title13"), new GUILayoutOption[] { GUILayout.MaxWidth(150f) });
				EditorGUILayout.LabelField(value.src.name + "( " + value.src.shader.name + " )", Array.Empty<GUILayoutOption>());
				TextAnchor alignment = GUI.skin.label.alignment;
				GUI.skin.label.alignment = 5;
				if (!value.IsLibriy)
				{
					GUILayout.Label(this.errorIcon, new GUILayoutOption[]
					{
						GUILayout.Width(32f),
						GUILayout.Height(16f),
						GUILayout.ExpandWidth(false),
						GUILayout.ExpandHeight(false)
					});
				}
				else
				{
					GUILayout.Label(this.rightIcon, new GUILayoutOption[]
					{
						GUILayout.Width(32f),
						GUILayout.Height(16f),
						GUILayout.ExpandWidth(false),
						GUILayout.ExpandHeight(false)
					});
				}
				GUI.skin.label.alignment = alignment;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title14"), new GUILayoutOption[] { GUILayout.MaxWidth(150f) });
				this.PopupShaderWindow(value);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}
		foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader in this.attachmaterials)
		{
			if (convertUnityForShader.src != null)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title13"), new GUILayoutOption[] { GUILayout.MaxWidth(150f) });
				EditorGUILayout.LabelField(convertUnityForShader.src.name + "( " + convertUnityForShader.src.shader.name + " )", Array.Empty<GUILayoutOption>());
				TextAnchor alignment2 = GUI.skin.label.alignment;
				GUI.skin.label.alignment = 5;
				ConvertMaterialTools.IsLibriyShader(convertUnityForShader.src.shader.name);
				if (!convertUnityForShader.IsLibriy)
				{
					GUILayout.Label(this.errorIcon, new GUILayoutOption[]
					{
						GUILayout.Width(32f),
						GUILayout.Height(16f),
						GUILayout.ExpandWidth(false),
						GUILayout.ExpandHeight(false)
					});
				}
				else
				{
					GUILayout.Label(this.rightIcon, new GUILayoutOption[]
					{
						GUILayout.Width(32f),
						GUILayout.Height(16f),
						GUILayout.ExpandWidth(false),
						GUILayout.ExpandHeight(false)
					});
				}
				GUI.skin.label.alignment = alignment2;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title14"), new GUILayoutOption[] { GUILayout.MaxWidth(150f) });
				this.PopupShaderWindow(convertUnityForShader);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.Space(50f);
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		string text = this.shaders.Count.ToString() + "/5";
		GUILayout.Label(SdkLangManager.Get("str_sdk_shaderPlacement_text1") + text, EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_shaderPlacement_btn3"), Array.Empty<GUILayoutOption>()))
		{
			JsonData jsonData = new JsonData();
			jsonData["event"] = "unity_editor_button_click";
			jsonData["function_type"] = 3;
			jsonData["function_name"] = "shader_configurator";
			jsonData["button_type"] = 1;
			jsonData["button_id"] = 604;
			jsonData["tab_name"] = "";
			AppLogService.PushAppLog(jsonData);
			this.ClearMissingScripte();
			try
			{
				this.ReplaceMaterialPropertys();
			}
			catch (Exception)
			{
				EditorUtility.DisplayDialog(SdkLangManager.Get("str_sdk_shaderPlacement_title8"), SdkLangManager.Get("str_sdk_shaderPlacement_text3"), SdkLangManager.Get("str_sdk_ok"));
			}
			this.SaveModify();
			this.SendEventEditorWidnow();
		}
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_shaderPlacement_btn5"), Array.Empty<GUILayoutOption>()))
		{
			JsonData jsonData2 = new JsonData();
			jsonData2["event"] = "unity_editor_button_click";
			jsonData2["function_type"] = 3;
			jsonData2["function_name"] = "shader_configurator";
			jsonData2["button_type"] = 1;
			jsonData2["button_id"] = 605;
			jsonData2["tab_name"] = "";
			AppLogService.PushAppLog(jsonData2);
			this.RevertLocalOpreation();
			this.SaveModify();
			this.SendEventEditorWidnow();
		}
		EditorGUILayout.EndHorizontal();
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00005420 File Offset: 0x00003620
	private void PopupShaderWindow(AvatarShaderPlacement.ConvertUnityForShader useData)
	{
		Rect controlRect = EditorGUILayout.GetControlRect(Array.Empty<GUILayoutOption>());
		if (EditorGUI.DropdownButton(controlRect, this.m_shaderCategorysLabels[useData.dstShadingtype], 1, EditorStyles.popup))
		{
			GenericMenu genericMenu = new GenericMenu();
			genericMenu.allowDuplicateNames = true;
			for (int i = 0; i < this.m_shaderCategorysLabels.Length; i++)
			{
				AvatarShaderPlacement.MenuSession menuSession = new AvatarShaderPlacement.MenuSession
				{
					shaderkey = i,
					shader = useData
				};
				genericMenu.AddItem(this.m_shaderCategorysLabels[i], useData.dstShadingtype == i, new GenericMenu.MenuFunction2(this.OnShaderSelected), menuSession);
			}
			genericMenu.DropDown(controlRect);
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000054B4 File Offset: 0x000036B4
	private void OnShaderSelected(object selectionData)
	{
		AvatarShaderPlacement.MenuSession menuSession = (AvatarShaderPlacement.MenuSession)selectionData;
		if (menuSession != null)
		{
			menuSession.shader.dstShadingtype = menuSession.shaderkey;
			this.ModifyMaterialsShader(menuSession.shader);
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000054E8 File Offset: 0x000036E8
	private void ModifyMaterialsShader(AvatarShaderPlacement.ConvertUnityForShader selectShader)
	{
		if (selectShader != null)
		{
			foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader in this.paddings)
			{
				if (convertUnityForShader.src == selectShader.src)
				{
					convertUnityForShader.dstShadingtype = selectShader.dstShadingtype;
				}
			}
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00005558 File Offset: 0x00003758
	public static EditorWindow[] GetAllOpenEditorWindows()
	{
		return Resources.FindObjectsOfTypeAll<EditorWindow>();
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00005560 File Offset: 0x00003760
	private void SendEventEditorWidnow()
	{
		EditorWindow[] allOpenEditorWindows = AvatarShaderPlacement.GetAllOpenEditorWindows();
		if (allOpenEditorWindows != null)
		{
			foreach (EditorWindow editorWindow in allOpenEditorWindows)
			{
				if (editorWindow.title == SdkLangManager.Get("str_sdk_resource_check"))
				{
					editorWindow.SendEvent(EditorGUIUtility.CommandEvent("ValidateCommand"));
				}
			}
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x000055B2 File Offset: 0x000037B2
	private bool IsStandardShader(Material from, string shaderName)
	{
		return shaderName.Equals("Universal Render Pipeline/Lit") || shaderName.Equals("Standard");
	}

	// Token: 0x06000067 RID: 103 RVA: 0x000055D1 File Offset: 0x000037D1
	private bool IsVRMAlphaTestShader(Material vrm)
	{
		return vrm != null && vrm.shader.name == "VRM/MToon" && (int)vrm.GetFloat("_BlendMode") == 1;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00005604 File Offset: 0x00003804
	private bool IsPBRShader(Material from, string shaderName)
	{
		if (from != null)
		{
			AvatarShaderPlacement.<>c__DisplayClass37_0 CS$<>8__locals1;
			CS$<>8__locals1.srcTexs = MaterialExtensions.CollectTextures(from);
			string[] array = new string[] { "_MetallicGlossMap" };
			for (int i = 0; i < array.Length; i++)
			{
				MaterialExtensions.CustomPropertys customPropertys = AvatarShaderPlacement.<IsPBRShader>g__FindProperty|37_0(array[i], ref CS$<>8__locals1);
				if (customPropertys != null && customPropertys.property != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00005664 File Offset: 0x00003864
	private void LoadConvertRuleConfig()
	{
		this.paddings.Clear();
		this.attachmaterials.Clear();
		if (PlayerPrefs.GetInt("LocalErrorHandle") == 0)
		{
			this.selectObject = false;
		}
		else
		{
			this.selectObject = true;
		}
		this.lastSelect = this.selectObject;
		if (this.convertTools == null)
		{
			this.convertTools = new ConvertMaterialTools();
			this.convertTools.Readconfig();
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000056D0 File Offset: 0x000038D0
	public void ConvertPropertysGameObject(GameObject go)
	{
		this.paddings.Clear();
		this.shaders.Clear();
		this.attachmaterials.Clear();
		this.displayList.Clear();
		HashSet<Material> hashSet = new HashSet<Material>();
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(true);
		if (componentsInChildren != null)
		{
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer == null))
				{
					foreach (Material material in renderer.sharedMaterials)
					{
						if (!(material == null))
						{
							hashSet.Add(material);
							bool flag = this.IsVRMAlphaTestShader(material);
							bool flag2 = material.renderQueue >= 3000;
							bool flag3 = this.IsPBRShader(material, material.shader.name);
							AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader = new AvatarShaderPlacement.ConvertUnityForShader();
							convertUnityForShader.src = material;
							convertUnityForShader.srcInstance = new Material(material);
							convertUnityForShader.skin = renderer;
							convertUnityForShader.Instance = null;
							string text = material.name + "( " + material.shader.name + " )";
							convertUnityForShader.bModify = false;
							convertUnityForShader.IsLibriy = ConvertMaterialTools.IsLibriyShader(material.shader.name);
							if (flag3)
							{
								convertUnityForShader.dstShadingtype = 9;
								if (flag2)
								{
									convertUnityForShader.dstShadingtype = 12;
								}
								if (flag)
								{
									convertUnityForShader.dstShadingtype = 13;
								}
							}
							else
							{
								convertUnityForShader.dstShadingtype = 0;
								if (flag2)
								{
									convertUnityForShader.dstShadingtype = 2;
								}
								if (flag)
								{
									convertUnityForShader.dstShadingtype = 8;
								}
							}
							this.paddings.Add(convertUnityForShader);
							if (!this.shaders.ContainsKey(text))
							{
								this.shaders.Add(text, 1);
							}
							if (!this.displayList.ContainsKey(convertUnityForShader.src))
							{
								this.displayList.Add(convertUnityForShader.src, convertUnityForShader);
							}
						}
					}
				}
			}
		}
		Object[] array2 = new GameObject[] { go };
		array2 = EditorUtility.CollectDependencies(array2);
		for (int i = 0; i < array2.Length; i++)
		{
			Material material2 = array2[i] as Material;
			if (material2 != null && !hashSet.Contains(material2))
			{
				bool flag4 = material2.renderQueue >= 3000;
				bool flag5 = this.IsPBRShader(material2, material2.shader.name);
				AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader2 = new AvatarShaderPlacement.ConvertUnityForShader();
				convertUnityForShader2.src = material2;
				convertUnityForShader2.srcInstance = new Material(material2);
				convertUnityForShader2.skin = null;
				convertUnityForShader2.Instance = null;
				string text2 = material2.name + "( " + material2.shader.name + " )";
				convertUnityForShader2.bModify = false;
				convertUnityForShader2.IsLibriy = ConvertMaterialTools.IsLibriyShader(material2.shader.name);
				if (flag5)
				{
					convertUnityForShader2.dstShadingtype = 8;
					if (flag4)
					{
						convertUnityForShader2.dstShadingtype = 11;
					}
				}
				else
				{
					convertUnityForShader2.dstShadingtype = 0;
					if (flag4)
					{
						convertUnityForShader2.dstShadingtype = 2;
					}
				}
				this.attachmaterials.Add(convertUnityForShader2);
				if (!this.shaders.ContainsKey(text2))
				{
					this.shaders.Add(text2, 1);
				}
			}
		}
		if (this.paddings.Count <= 0)
		{
			int count = this.attachmaterials.Count;
			return;
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00005A1C File Offset: 0x00003C1C
	private void ReplaceMaterialPropertys()
	{
		if (this.convertTools == null)
		{
			return;
		}
		if (this.paddings.Count <= 0 && this.attachmaterials.Count <= 0)
		{
			return;
		}
		this.bModifyAllShader = true;
		Dictionary<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>> dictionary = new Dictionary<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>>();
		foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader in this.paddings)
		{
			if (dictionary.ContainsKey(convertUnityForShader.skin))
			{
				List<AvatarShaderPlacement.ConvertUnityForShader> list = null;
				if (dictionary.TryGetValue(convertUnityForShader.skin, out list))
				{
					list.Add(convertUnityForShader);
				}
				else
				{
					list = new List<AvatarShaderPlacement.ConvertUnityForShader>();
					list.Add(convertUnityForShader);
					dictionary.Add(convertUnityForShader.skin, list);
				}
			}
			else
			{
				List<AvatarShaderPlacement.ConvertUnityForShader> list2 = new List<AvatarShaderPlacement.ConvertUnityForShader>();
				list2.Add(convertUnityForShader);
				dictionary.Add(convertUnityForShader.skin, list2);
			}
		}
		int num = ((dictionary.Count == 0) ? 1 : dictionary.Count);
		int num2 = 0;
		foreach (KeyValuePair<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>> keyValuePair in dictionary)
		{
			num2++;
			List<AvatarShaderPlacement.ConvertUnityForShader> value = keyValuePair.Value;
			Renderer key = keyValuePair.Key;
			if (!(key == null) && value != null)
			{
				bool flag = false;
				List<Material> list3 = new List<Material>();
				foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader2 in value)
				{
					convertUnityForShader2.bModify = true;
					convertUnityForShader2.IsLibriy = true;
					Material src = convertUnityForShader2.src;
					if (src != null)
					{
						convertUnityForShader2.backPathName = this.convertTools.BackupMaterial(src);
						Material material = new Material(convertUnityForShader2.srcInstance);
						string text = this.m_shaderCategorys[convertUnityForShader2.dstShadingtype];
						src.shader = Shader.Find(text);
						this.convertTools.ConverPropertys(material, src);
						Material material2 = this.convertTools.SaveNewMaterial(src);
						convertUnityForShader2.Instance = material2;
						convertUnityForShader2.src = material2;
						list3.Add(material2);
						flag = true;
					}
				}
				float num3 = (float)((double)num2 / (1.0 * (double)num));
				EditorUtility.DisplayCancelableProgressBar(SdkLangManager.Get("str_sdk_shaderPlacement_title17"), "", num3);
				if (flag && list3.Count > 0)
				{
					key.sharedMaterials = list3.ToArray();
				}
			}
		}
		EditorUtility.ClearProgressBar();
		dictionary.Clear();
		this.HandleAttachMaterials();
		EditorUtility.ClearProgressBar();
		EditorUtility.DisplayDialog(SdkLangManager.Get("str_sdk_shaderPlacement_title8"), SdkLangManager.Get("str_sdk_shaderPlacement_text39"), SdkLangManager.Get("str_sdk_ok"));
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00005D10 File Offset: 0x00003F10
	private void HandleAttachMaterials()
	{
		foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader in this.attachmaterials)
		{
			if (!convertUnityForShader.bModify)
			{
				Material src = convertUnityForShader.src;
				convertUnityForShader.bModify = true;
				convertUnityForShader.IsLibriy = true;
				if (src != null)
				{
					convertUnityForShader.backPathName = this.convertTools.BackupMaterial(src);
					Material material = new Material(convertUnityForShader.srcInstance);
					string text = this.m_shaderCategorys[convertUnityForShader.dstShadingtype];
					src.shader = Shader.Find(text);
					this.convertTools.ConverPropertys(material, src);
					Material material2 = this.convertTools.SaveNewMaterial(src);
					convertUnityForShader.Instance = material2;
				}
			}
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00005DE8 File Offset: 0x00003FE8
	private void RevertAttackMaterials()
	{
		foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader in this.attachmaterials)
		{
			convertUnityForShader.bModify = false;
			convertUnityForShader.IsLibriy = false;
			if (convertUnityForShader.src != null)
			{
				this.RevertBackPathMaterial(convertUnityForShader.backPathName);
				string assetPath = AssetDatabase.GetAssetPath(convertUnityForShader.Instance);
				string text = assetPath.ToLower();
				if (!string.IsNullOrEmpty(assetPath) && !text.EndsWith(".fbx"))
				{
					if (AssetDatabase.DeleteAsset(assetPath))
					{
						AssetDatabase.CreateAsset(convertUnityForShader.srcInstance, assetPath);
					}
					Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
					if (material != null)
					{
						convertUnityForShader.src = material;
						convertUnityForShader.srcInstance = new Material(material);
						convertUnityForShader.IsLibriy = ConvertMaterialTools.IsLibriyShader(material.shader.name);
					}
				}
			}
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00005EE0 File Offset: 0x000040E0
	private void RevertBackPathMaterial(string backpath)
	{
		if (!string.IsNullOrEmpty(backpath))
		{
			AssetDatabase.DeleteAsset(backpath);
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00005EF4 File Offset: 0x000040F4
	private void RevertLocalOpreation()
	{
		if (this.paddings.Count <= 0)
		{
			return;
		}
		if (!this.bModifyAllShader)
		{
			return;
		}
		Dictionary<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>> dictionary = new Dictionary<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>>();
		foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader in this.paddings)
		{
			if (dictionary.ContainsKey(convertUnityForShader.skin))
			{
				List<AvatarShaderPlacement.ConvertUnityForShader> list = null;
				if (dictionary.TryGetValue(convertUnityForShader.skin, out list))
				{
					list.Add(convertUnityForShader);
				}
				else
				{
					list = new List<AvatarShaderPlacement.ConvertUnityForShader>();
					list.Add(convertUnityForShader);
					dictionary.Add(convertUnityForShader.skin, list);
				}
			}
			else
			{
				List<AvatarShaderPlacement.ConvertUnityForShader> list2 = new List<AvatarShaderPlacement.ConvertUnityForShader>();
				list2.Add(convertUnityForShader);
				dictionary.Add(convertUnityForShader.skin, list2);
			}
		}
		Dictionary<Material, string> dictionary2 = new Dictionary<Material, string>();
		foreach (KeyValuePair<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>> keyValuePair in dictionary)
		{
			List<AvatarShaderPlacement.ConvertUnityForShader> value = keyValuePair.Value;
			if (!(keyValuePair.Key == null) && value != null)
			{
				foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader2 in value)
				{
					if (convertUnityForShader2.Instance)
					{
						string assetPath = AssetDatabase.GetAssetPath(convertUnityForShader2.Instance);
						string text = assetPath.ToLower();
						if (!string.IsNullOrEmpty(assetPath) && !text.EndsWith(".fbx") && !dictionary2.ContainsKey(convertUnityForShader2.srcInstance))
						{
							dictionary2.Add(convertUnityForShader2.srcInstance, assetPath);
						}
					}
				}
			}
		}
		Dictionary<string, Material> dictionary3 = new Dictionary<string, Material>();
		foreach (KeyValuePair<Material, string> keyValuePair2 in dictionary2)
		{
			if (!dictionary3.ContainsKey(keyValuePair2.Value))
			{
				dictionary3.Add(keyValuePair2.Value, keyValuePair2.Key);
			}
		}
		Dictionary<string, Material> dictionary4 = new Dictionary<string, Material>();
		foreach (KeyValuePair<string, Material> keyValuePair3 in dictionary3)
		{
			string key = keyValuePair3.Key;
			if (AssetDatabase.DeleteAsset(key))
			{
				AssetDatabase.CreateAsset(keyValuePair3.Value, key);
			}
			Material material = AssetDatabase.LoadAssetAtPath<Material>(key);
			if (material != null && !dictionary4.ContainsKey(key))
			{
				dictionary4.Add(key, material);
			}
		}
		dictionary3.Clear();
		Dictionary<Material, Material> dictionary5 = new Dictionary<Material, Material>();
		foreach (KeyValuePair<Material, string> keyValuePair4 in dictionary2)
		{
			Material material2 = null;
			if (dictionary4.TryGetValue(keyValuePair4.Value, out material2) && material2 != null && !dictionary5.ContainsKey(keyValuePair4.Key))
			{
				dictionary5.Add(keyValuePair4.Key, material2);
			}
		}
		dictionary4.Clear();
		dictionary2.Clear();
		this.displayList.Clear();
		int num = ((dictionary.Count == 0) ? 1 : dictionary.Count);
		int num2 = 0;
		try
		{
			foreach (KeyValuePair<Renderer, List<AvatarShaderPlacement.ConvertUnityForShader>> keyValuePair5 in dictionary)
			{
				num2++;
				List<AvatarShaderPlacement.ConvertUnityForShader> value2 = keyValuePair5.Value;
				Renderer key2 = keyValuePair5.Key;
				if (!(key2 == null) && value2 != null)
				{
					List<Material> list3 = new List<Material>();
					foreach (AvatarShaderPlacement.ConvertUnityForShader convertUnityForShader3 in value2)
					{
						if (convertUnityForShader3.bModify)
						{
							convertUnityForShader3.bModify = false;
							convertUnityForShader3.IsLibriy = false;
							this.RevertBackPathMaterial(convertUnityForShader3.backPathName);
							Material material3 = null;
							if (dictionary5.TryGetValue(convertUnityForShader3.srcInstance, out material3))
							{
								list3.Add(material3);
								convertUnityForShader3.src = material3;
								convertUnityForShader3.srcInstance = new Material(material3);
								convertUnityForShader3.IsLibriy = ConvertMaterialTools.IsLibriyShader(material3.shader.name);
								if (!this.displayList.ContainsKey(material3))
								{
									this.displayList.Add(material3, convertUnityForShader3);
								}
							}
							else
							{
								list3.Add(convertUnityForShader3.srcInstance);
								convertUnityForShader3.src = convertUnityForShader3.srcInstance;
							}
						}
					}
					float num3 = (float)((double)num2 / (1.0 * (double)num));
					EditorUtility.DisplayCancelableProgressBar(SdkLangManager.Get("str_sdk_shaderPlacement_title17"), "", num3);
					if (list3.Count > 0)
					{
						key2.sharedMaterials = list3.ToArray();
					}
				}
			}
		}
		catch (Exception)
		{
			EditorUtility.ClearProgressBar();
		}
		dictionary.Clear();
		this.RevertAttackMaterials();
		EditorUtility.ClearProgressBar();
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00006498 File Offset: 0x00004698
	private void ClearMissingScripte()
	{
		if (this.selectObj == null)
		{
			return;
		}
		GameObject gameObject = this.selectObj as GameObject;
		if (AvatarShaderPlacement.IsPrefabInstance(gameObject) && AvatarShaderPlacement.IsHaveMissBehaviour(gameObject))
		{
			this.RemoveRecursively(gameObject);
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000064D8 File Offset: 0x000046D8
	private void RemoveRecursively(GameObject go)
	{
		GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
		int childCount = go.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = go.transform.GetChild(i);
			if (child != null)
			{
				this.RemoveRecursively(child.gameObject);
			}
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00006528 File Offset: 0x00004728
	private static bool IsHaveMissBehaviour(GameObject obj)
	{
		if (obj.hideFlags == null)
		{
			Component[] components = obj.GetComponents<Component>();
			new SerializedObject(obj).FindProperty("m_Component");
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] == null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00006574 File Offset: 0x00004774
	private void SaveModify()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		if (activeScene.name == "")
		{
			string text = Application.dataPath + "/AutoSave_Temp.unity";
			EditorSceneManager.SaveScene(activeScene, text);
		}
		else
		{
			string path = SceneManager.GetActiveScene().path;
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path);
		}
		AssetDatabase.SaveAssets();
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000065D3 File Offset: 0x000047D3
	public int EditorGUILayoutPopup(string label, int selectedIndex, string[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, style, options);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x000065E1 File Offset: 0x000047E1
	public int EditorGUILayoutPopup(GUIContent label, int selectedIndex, GUIContent[] displayedOptions, params GUILayoutOption[] options)
	{
		return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, options);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000065ED File Offset: 0x000047ED
	public int EditorGUILayoutPopup(GUIContent label, int selectedIndex, GUIContent[] displayedOptions, GUIStyle style, params GUILayoutOption[] options)
	{
		return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, style, options);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x000065FB File Offset: 0x000047FB
	public int EditorGUILayoutPopup(int selectedIndex, string[] displayedOptions, params GUILayoutOption[] options)
	{
		return EditorGUILayout.Popup(selectedIndex, displayedOptions, options);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00006605 File Offset: 0x00004805
	public int EditorGUILayoutPopup(string label, int selectedIndex, string[] displayedOptions, params GUILayoutOption[] options)
	{
		return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, options);
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00006680 File Offset: 0x00004880
	[CompilerGenerated]
	internal static MaterialExtensions.CustomPropertys <IsPBRShader>g__FindProperty|37_0(string key, ref AvatarShaderPlacement.<>c__DisplayClass37_0 A_1)
	{
		foreach (MaterialExtensions.CustomPropertys customPropertys in A_1.srcTexs)
		{
			if (customPropertys.propertyName.Equals(key))
			{
				return customPropertys;
			}
		}
		return null;
	}

	// Token: 0x0400002A RID: 42
	private static bool IsShowedWindows;

	// Token: 0x0400002B RID: 43
	private Object selectObj;

	// Token: 0x0400002C RID: 44
	private Object lastObj;

	// Token: 0x0400002D RID: 45
	private Vector2 scrollPos;

	// Token: 0x0400002E RID: 46
	private bool selectObject = true;

	// Token: 0x0400002F RID: 47
	private bool lastSelect = true;

	// Token: 0x04000030 RID: 48
	private Dictionary<Material, AvatarShaderPlacement.ConvertUnityForShader> displayList = new Dictionary<Material, AvatarShaderPlacement.ConvertUnityForShader>();

	// Token: 0x04000031 RID: 49
	private List<AvatarShaderPlacement.ConvertUnityForShader> paddings = new List<AvatarShaderPlacement.ConvertUnityForShader>();

	// Token: 0x04000032 RID: 50
	private List<AvatarShaderPlacement.ConvertUnityForShader> attachmaterials = new List<AvatarShaderPlacement.ConvertUnityForShader>();

	// Token: 0x04000033 RID: 51
	private Dictionary<string, int> shaders = new Dictionary<string, int>();

	// Token: 0x04000034 RID: 52
	private ConvertMaterialTools convertTools;

	// Token: 0x04000035 RID: 53
	private Texture2D errorIcon;

	// Token: 0x04000036 RID: 54
	private Texture2D rightIcon;

	// Token: 0x04000037 RID: 55
	protected GUIContent m_categoryLabel = new GUIContent(SdkLangManager.Get("str_sdk_shaderPlacement_title9"), SdkLangManager.Get("str_sdk_shaderPlacement_text0"));

	// Token: 0x04000038 RID: 56
	protected GUIContent[] m_shaderCategorysLabels;

	// Token: 0x04000039 RID: 57
	protected string[] m_shaderCategorys;

	// Token: 0x0400003A RID: 58
	protected int m_masterNodeCategory;

	// Token: 0x0400003B RID: 59
	private MaterialProperty _blendMode;

	// Token: 0x0400003C RID: 60
	private bool bModifyAllShader;

	// Token: 0x0200008C RID: 140
	public class ConvertUnityForShader
	{
		// Token: 0x0400031C RID: 796
		public Material src;

		// Token: 0x0400031D RID: 797
		public Material Instance;

		// Token: 0x0400031E RID: 798
		public Material srcInstance;

		// Token: 0x0400031F RID: 799
		public Renderer skin;

		// Token: 0x04000320 RID: 800
		public bool bModify;

		// Token: 0x04000321 RID: 801
		public int dstShadingtype;

		// Token: 0x04000322 RID: 802
		public bool IsLibriy;

		// Token: 0x04000323 RID: 803
		public string backPathName = string.Empty;
	}

	// Token: 0x0200008D RID: 141
	public class MenuSession
	{
		// Token: 0x04000324 RID: 804
		public int shaderkey;

		// Token: 0x04000325 RID: 805
		public AvatarShaderPlacement.ConvertUnityForShader shader;
	}

	// Token: 0x0200008E RID: 142
	public enum RenderMode
	{
		// Token: 0x04000327 RID: 807
		Opaque,
		// Token: 0x04000328 RID: 808
		Cutout,
		// Token: 0x04000329 RID: 809
		Transparent,
		// Token: 0x0400032A RID: 810
		TransparentWithZWrite
	}
}
