using System;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000046 RID: 70
[ExecuteAlways]
public class WorldShaderPlacement : EditorWindow
{
	// Token: 0x0600020F RID: 527 RVA: 0x0000D644 File Offset: 0x0000B844
	public static void ShowWindow()
	{
		if (WorldShaderPlacement.IsShowedWindows)
		{
			return;
		}
		DateTime now = DateTime.Now;
		WorldShaderPlacement window = EditorWindow.GetWindow<WorldShaderPlacement>();
		window.LoadConvertRuleConfig();
		window.titleContent = new GUIContent(SdkLangManager.Get("str_sdk_shaderPlacement_title0"));
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
	private void OnEnable()
	{
		WorldShaderPlacement.IsShowedWindows = true;
		base.minSize = new Vector2(700f, 480f);
		base.maxSize = new Vector2(700f, 480f);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000D722 File Offset: 0x0000B922
	private void OnDisable()
	{
		WorldShaderPlacement.IsShowedWindows = false;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000D72A File Offset: 0x0000B92A
	public void OpenShaderCorrectionWindow(GameObject avater)
	{
		this.LoadConvertRuleConfig();
		this.ConvertPropertysGameObject();
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000D738 File Offset: 0x0000B938
	private void OnGUI()
	{
		EditorGUILayout.BeginVertical(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_shaderPlacement_title2"), EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
		EditorGUILayout.EndVertical();
		EditorGUILayout.Space(20f);
		this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[]
		{
			GUILayout.Width(700f),
			GUILayout.Height(280f)
		});
		EditorGUILayout.Space(8f);
		foreach (WorldShaderPlacement.ConvertUnityForShader convertUnityForShader in this.paddings)
		{
			if (convertUnityForShader.src != null && this.IsPBRShader(convertUnityForShader.src.shader.name))
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title3"), Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(convertUnityForShader.srcShaderName, Array.Empty<GUILayoutOption>());
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_shaderPlacement_title4"), Array.Empty<GUILayoutOption>());
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.Space(50f);
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_shaderPlacement_btn0"), Array.Empty<GUILayoutOption>()))
		{
			this.ReplaceMaterialPropertys();
			this.SaveModify();
			this.SendEventEditorWidnow();
		}
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_shaderPlacement_btn1"), Array.Empty<GUILayoutOption>()))
		{
			this.RevertLocalOpreation();
			this.SaveModify();
			this.SendEventEditorWidnow();
		}
		EditorGUILayout.EndHorizontal();
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00005558 File Offset: 0x00003758
	public static EditorWindow[] GetAllOpenEditorWindows()
	{
		return Resources.FindObjectsOfTypeAll<EditorWindow>();
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000D904 File Offset: 0x0000BB04
	private void SendEventEditorWidnow()
	{
		EditorWindow[] allOpenEditorWindows = WorldShaderPlacement.GetAllOpenEditorWindows();
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

	// Token: 0x06000216 RID: 534 RVA: 0x0000D956 File Offset: 0x0000BB56
	private bool IsPBRShader(string shaderName)
	{
		return shaderName.Equals(SdkLangManager.Get("str_sdk_shaderPlacement_title5"));
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000D96D File Offset: 0x0000BB6D
	private void LoadConvertRuleConfig()
	{
		if (this.convertTools == null)
		{
			this.convertTools = new ConvertMaterialTools();
			this.convertTools.Readconfig();
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000D990 File Offset: 0x0000BB90
	public void ConvertPropertysGameObject()
	{
		this.paddings.Clear();
		this.shaders.Clear();
		Renderer[] array = Object.FindObjectsOfType<Renderer>();
		if (array != null)
		{
			foreach (Renderer renderer in array)
			{
				if (!(renderer == null))
				{
					foreach (Material material in renderer.sharedMaterials)
					{
						if (!(material == null))
						{
							WorldShaderPlacement.ConvertUnityForShader convertUnityForShader = new WorldShaderPlacement.ConvertUnityForShader();
							convertUnityForShader.src = material;
							convertUnityForShader.skin = renderer;
							convertUnityForShader.instance = new Material(material);
							convertUnityForShader.srcShaderName = material.shader.name;
							this.paddings.Add(convertUnityForShader);
							if (!this.shaders.ContainsKey(convertUnityForShader.srcShaderName))
							{
								this.shaders.Add(convertUnityForShader.srcShaderName, 1);
							}
						}
					}
				}
			}
		}
		if (this.paddings.Count <= 0)
		{
			ParaLog.LogError("\ufffd\ufffd\ufffd\ufffd\u05bb\u05a7\ufffd\ufffd\ufffd\ufffdƤ\ufffd\ufffd\ufffd\ufffd!!!");
			return;
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
	private void ReplaceMaterialPropertys()
	{
		if (this.convertTools == null)
		{
			ParaLog.LogError("\ufffd\ufffdʼ\ufffd\ufffd\ufffd\ufffd\ufffd\u0337\ufffd\ufffd\ufffd\ufffd\u02f4\ufffd\ufffd\ufffd!!!");
			return;
		}
		if (this.paddings.Count <= 0)
		{
			return;
		}
		Dictionary<Renderer, List<WorldShaderPlacement.ConvertUnityForShader>> dictionary = new Dictionary<Renderer, List<WorldShaderPlacement.ConvertUnityForShader>>();
		foreach (WorldShaderPlacement.ConvertUnityForShader convertUnityForShader in this.paddings)
		{
			if (dictionary.ContainsKey(convertUnityForShader.skin))
			{
				List<WorldShaderPlacement.ConvertUnityForShader> list = null;
				if (dictionary.TryGetValue(convertUnityForShader.skin, out list))
				{
					list.Add(convertUnityForShader);
				}
				else
				{
					list = new List<WorldShaderPlacement.ConvertUnityForShader>();
					list.Add(convertUnityForShader);
					dictionary.Add(convertUnityForShader.skin, list);
				}
			}
			else
			{
				List<WorldShaderPlacement.ConvertUnityForShader> list2 = new List<WorldShaderPlacement.ConvertUnityForShader>();
				list2.Add(convertUnityForShader);
				dictionary.Add(convertUnityForShader.skin, list2);
			}
		}
		int count = dictionary.Count;
		int num = 0;
		try
		{
			foreach (KeyValuePair<Renderer, List<WorldShaderPlacement.ConvertUnityForShader>> keyValuePair in dictionary)
			{
				num++;
				List<WorldShaderPlacement.ConvertUnityForShader> value = keyValuePair.Value;
				Renderer key = keyValuePair.Key;
				if (!(key == null) && value != null)
				{
					List<Material> list3 = new List<Material>();
					foreach (WorldShaderPlacement.ConvertUnityForShader convertUnityForShader2 in value)
					{
						Material src = convertUnityForShader2.src;
						if (src != null)
						{
							if (!this.IsPBRShader(src.shader.name))
							{
								list3.Add(src);
							}
							else
							{
								this.convertTools.Remapshader(src, true, false);
								Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
								if (material != null)
								{
									this.convertTools.ConverPropertys(src, material);
									Material material2 = this.convertTools.SaveNewMaterial(src);
									list3.Add(material2);
								}
							}
						}
					}
					float num2 = (float)((double)num / (1.0 * (double)count));
					EditorUtility.DisplayCancelableProgressBar(SdkLangManager.Get("str_sdk_shaderPlacement_title6"), "", num2);
					key.sharedMaterials = list3.ToArray();
				}
			}
		}
		catch (Exception)
		{
			EditorUtility.ClearProgressBar();
		}
		dictionary.Clear();
		EditorUtility.ClearProgressBar();
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000DD44 File Offset: 0x0000BF44
	private void RevertLocalOpreation()
	{
		if (this.paddings.Count <= 0)
		{
			return;
		}
		Dictionary<Renderer, List<WorldShaderPlacement.ConvertUnityForShader>> dictionary = new Dictionary<Renderer, List<WorldShaderPlacement.ConvertUnityForShader>>();
		foreach (WorldShaderPlacement.ConvertUnityForShader convertUnityForShader in this.paddings)
		{
			if (dictionary.ContainsKey(convertUnityForShader.skin))
			{
				List<WorldShaderPlacement.ConvertUnityForShader> list = null;
				if (dictionary.TryGetValue(convertUnityForShader.skin, out list))
				{
					list.Add(convertUnityForShader);
				}
				else
				{
					list = new List<WorldShaderPlacement.ConvertUnityForShader>();
					list.Add(convertUnityForShader);
					dictionary.Add(convertUnityForShader.skin, list);
				}
			}
			else
			{
				List<WorldShaderPlacement.ConvertUnityForShader> list2 = new List<WorldShaderPlacement.ConvertUnityForShader>();
				list2.Add(convertUnityForShader);
				dictionary.Add(convertUnityForShader.skin, list2);
			}
		}
		int count = dictionary.Count;
		int num = 0;
		try
		{
			foreach (KeyValuePair<Renderer, List<WorldShaderPlacement.ConvertUnityForShader>> keyValuePair in dictionary)
			{
				num++;
				List<WorldShaderPlacement.ConvertUnityForShader> value = keyValuePair.Value;
				Renderer key = keyValuePair.Key;
				if (!(key == null) && value != null)
				{
					List<Material> list3 = new List<Material>();
					foreach (WorldShaderPlacement.ConvertUnityForShader convertUnityForShader2 in value)
					{
						Material instance = convertUnityForShader2.instance;
						if (instance != null)
						{
							string assetPath = AssetDatabase.GetAssetPath(convertUnityForShader2.src);
							if (!string.IsNullOrEmpty(assetPath) && !assetPath.EndsWith(".fbx"))
							{
								if (AssetDatabase.DeleteAsset(assetPath))
								{
									AssetDatabase.CreateAsset(instance, assetPath);
								}
								Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
								list3.Add(material);
							}
							else
							{
								list3.Add(instance);
							}
						}
					}
					float num2 = (float)((double)num / (1.0 * (double)count));
					EditorUtility.DisplayCancelableProgressBar(SdkLangManager.Get("str_sdk_shaderPlacement_title6"), "", num2);
					if (list3.Count > 0)
					{
						key.sharedMaterials = list3.ToArray();
					}
				}
			}
		}
		catch (Exception)
		{
			EditorUtility.ClearProgressBar();
		}
		dictionary.Clear();
		EditorUtility.ClearProgressBar();
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
	private void SaveModify()
	{
		string path = SceneManager.GetActiveScene().path;
		EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path);
		AssetDatabase.SaveAssets();
	}

	// Token: 0x04000138 RID: 312
	private static bool IsShowedWindows;

	// Token: 0x04000139 RID: 313
	private Vector2 scrollPos;

	// Token: 0x0400013A RID: 314
	private List<WorldShaderPlacement.ConvertUnityForShader> paddings = new List<WorldShaderPlacement.ConvertUnityForShader>();

	// Token: 0x0400013B RID: 315
	private Dictionary<string, int> shaders = new Dictionary<string, int>();

	// Token: 0x0400013C RID: 316
	private ConvertMaterialTools convertTools;

	// Token: 0x020000A2 RID: 162
	public class ConvertUnityForShader
	{
		// Token: 0x04000358 RID: 856
		public Material src;

		// Token: 0x04000359 RID: 857
		public Renderer skin;

		// Token: 0x0400035A RID: 858
		public Material instance;

		// Token: 0x0400035B RID: 859
		public string srcShaderName = string.Empty;
	}
}
