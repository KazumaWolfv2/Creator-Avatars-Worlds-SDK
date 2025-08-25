using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000086 RID: 134
	[CustomEditor(typeof(ParaImpostor))]
	public class ParaImpostorInspector : Editor
	{
		// Token: 0x060004B4 RID: 1204 RVA: 0x000205AC File Offset: 0x0001E7AC
		private void UpdateDynamicSizes()
		{
			if (this.m_currentData.LockedSizes)
			{
				this.m_tableSizes = this.m_tableSizesLocked;
			}
			else
			{
				this.m_tableSizes = this.m_tableSizesUnlocked;
			}
			for (int i = 0; i < this.m_sizesScaleStr.Length; i++)
			{
				if (this.m_currentData.LockedSizes)
				{
					this.m_sizesScaleStr[i] = new GUIContent((this.m_currentData.TexSize.x / (float)ParaImpostorBakePresetEditor.TexScaleOpt[i]).ToString());
				}
				else
				{
					this.m_sizesScaleStr[i] = new GUIContent((this.m_currentData.TexSize.x / (float)ParaImpostorBakePresetEditor.TexScaleOpt[i]).ToString() + "x" + (this.m_currentData.TexSize.y / (float)ParaImpostorBakePresetEditor.TexScaleOpt[i]).ToString());
				}
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00020690 File Offset: 0x0001E890
		public void OnEnable()
		{
			this.m_instance = base.target as ParaImpostor;
			if (this.m_instance.Data == null)
			{
				this.m_currentData = ScriptableObject.CreateInstance<ParaImpostorAsset>();
				this.m_currentData.ImpostorType = (ImpostorType)Enum.Parse(typeof(ImpostorType), EditorPrefs.GetString(ImpostorBakingTools.PrefDataImpType, this.m_currentData.ImpostorType.ToString()));
				this.m_currentData.SelectedSize = EditorPrefs.GetInt(ImpostorBakingTools.PrefDataTexSizeSelected, this.m_currentData.SelectedSize);
				this.m_currentData.LockedSizes = EditorPrefs.GetBool(ImpostorBakingTools.PrefDataTexSizeLocked, this.m_currentData.LockedSizes);
				this.m_currentData.TexSize.x = EditorPrefs.GetFloat(ImpostorBakingTools.PrefDataTexSizeX, this.m_currentData.TexSize.x);
				this.m_currentData.TexSize.y = EditorPrefs.GetFloat(ImpostorBakingTools.PrefDataTexSizeY, this.m_currentData.TexSize.y);
				this.m_currentData.DecoupleAxisFrames = EditorPrefs.GetBool(ImpostorBakingTools.PrefDataDecoupledFrames, this.m_currentData.DecoupleAxisFrames);
				this.m_currentData.HorizontalFrames = EditorPrefs.GetInt(ImpostorBakingTools.PrefDataXFrames, this.m_currentData.HorizontalFrames);
				this.m_currentData.VerticalFrames = EditorPrefs.GetInt(ImpostorBakingTools.PrefDataYFrames, this.m_currentData.VerticalFrames);
				this.m_currentData.PixelPadding = EditorPrefs.GetInt(ImpostorBakingTools.PrefDataPixelBleeding, this.m_currentData.PixelPadding);
				this.m_currentData.Tolerance = EditorPrefs.GetFloat(ImpostorBakingTools.PrefDataTolerance, this.m_currentData.Tolerance);
				this.m_currentData.NormalScale = EditorPrefs.GetFloat(ImpostorBakingTools.PrefDataNormalScale, this.m_currentData.NormalScale);
				this.m_currentData.MaxVertices = EditorPrefs.GetInt(ImpostorBakingTools.PrefDataMaxVertices, this.m_currentData.MaxVertices);
			}
			else
			{
				this.m_currentData = this.m_instance.Data;
			}
			ImpostorBakingTools.LoadDefaults();
			Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("31bd3cd74692f384a916d9d7ea87710d"));
			this.m_alphaMaterial = new Material(shader);
			if (this.m_instance.m_cutMode == null)
			{
				this.m_recalculateMesh = true;
			}
			if (this.m_instance.RootTransform == null)
			{
				this.m_instance.RootTransform = this.m_instance.transform;
			}
			if ((this.m_instance.Renderers == null || this.m_instance.Renderers.Length == 0) && this.m_instance.RootTransform != null)
			{
				this.m_instance.Renderers = this.m_instance.RootTransform.GetComponentsInChildren<Renderer>();
			}
			this.m_renderers = base.serializedObject.FindProperty("m_renderers");
			if (this.m_instance.Renderers == null)
			{
				this.m_instance.Renderers = new Renderer[0];
			}
			this.UpdateDynamicSizes();
			if (this.m_currentData.Preset == null)
			{
				this.m_instance.DetectRenderPipeline();
				this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("0403878495ffa3c4e9d4bcb3eac9b559"));
				if (this.m_currentData.Version < 8500)
				{
					for (int i = 0; i < 4; i++)
					{
						TextureOutput textureOutput = this.m_currentData.Preset.Output[i].Clone();
						textureOutput.Index = i;
						textureOutput.OverrideMask = 32;
						textureOutput.ImageFormat = 0;
						this.m_currentData.OverrideOutput.Add(textureOutput);
					}
					this.m_currentData.OverrideOutput.Sort((TextureOutput x, TextureOutput y) => x.Index.CompareTo(y.Index));
				}
			}
			this.AddList();
			this.FetchShaderTag();
			this.m_instance.CheckSRPVerionAndApply();
			this.m_instance.CheckHDRPMaterial();
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00020A64 File Offset: 0x0001EC64
		public void ReCheckRenderers()
		{
			LOD[] lods = this.m_instance.LodGroup.GetLODs();
			int num = lods.Length - 1;
			switch (this.m_instance.m_lodReplacement)
			{
			default:
				this.m_flash = Color.white;
				return;
			case 1:
				this.m_instance.m_insertIndex = num;
				break;
			case 2:
				num = lods.Length - 2;
				this.m_instance.m_insertIndex = num + 1;
				num = Mathf.Max(num, 0);
				break;
			case 3:
				num = 0;
				this.m_instance.m_insertIndex = num;
				break;
			case 4:
				num = this.m_instance.m_insertIndex - 1;
				this.m_instance.m_insertIndex = num + 1;
				num = Mathf.Max(num, 0);
				break;
			case 5:
			case 6:
				num = this.m_instance.m_insertIndex;
				this.m_instance.m_insertIndex = num;
				break;
			}
			for (int i = num; i >= 0; i--)
			{
				if (lods[i].renderers != null && lods[i].renderers.Length != 0)
				{
					this.m_instance.Renderers = lods[i].renderers;
					break;
				}
			}
			this.m_flash = this.m_flashColor;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00020B8C File Offset: 0x0001ED8C
		private void OnDisable()
		{
			Object.DestroyImmediate(this.m_alphaMaterial);
			this.m_alphaMaterial = null;
			if (this.m_instance.Data == null && this.m_currentData != null && !AssetDatabase.IsMainAsset(this.m_currentData))
			{
				Object.DestroyImmediate(this.m_currentData);
			}
			this.RemoveList();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00020BEC File Offset: 0x0001EDEC
		private void RemoveList()
		{
			if (this.m_texturesOutput != null)
			{
				ReorderableList texturesOutput = this.m_texturesOutput;
				texturesOutput.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate)Delegate.Remove(texturesOutput.drawHeaderCallback, new ReorderableList.HeaderCallbackDelegate(this.DrawHeader));
				ReorderableList texturesOutput2 = this.m_texturesOutput;
				texturesOutput2.drawElementCallback = (ReorderableList.ElementCallbackDelegate)Delegate.Remove(texturesOutput2.drawElementCallback, new ReorderableList.ElementCallbackDelegate(this.DrawElement));
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00020C50 File Offset: 0x0001EE50
		private void AddList()
		{
			this.m_usingStandard = false;
			if (this.m_currentData.Preset.BakeShader == null)
			{
				this.m_usingStandard = true;
			}
			this.m_texturesOutput = new ReorderableList(this.m_currentData.Preset.Output, typeof(TextureOutput), false, true, false, false)
			{
				footerHeight = 0f
			};
			ReorderableList texturesOutput = this.m_texturesOutput;
			texturesOutput.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate)Delegate.Combine(texturesOutput.drawHeaderCallback, new ReorderableList.HeaderCallbackDelegate(this.DrawHeader));
			ReorderableList texturesOutput2 = this.m_texturesOutput;
			texturesOutput2.drawElementCallback = (ReorderableList.ElementCallbackDelegate)Delegate.Combine(texturesOutput2.drawElementCallback, new ReorderableList.ElementCallbackDelegate(this.DrawElement));
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00020D05 File Offset: 0x0001EF05
		private void RefreshList()
		{
			this.RemoveList();
			this.AddList();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00020D14 File Offset: 0x0001EF14
		private void FetchShaderTag()
		{
			if (this.m_currentData.Preset.RuntimeShader != null)
			{
				Material material = new Material(this.m_currentData.Preset.RuntimeShader);
				this.m_shaderTag = material.GetTag("ImpostorType", true);
				Object.DestroyImmediate(material);
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00020D68 File Offset: 0x0001EF68
		private void DrawHeader(Rect rect)
		{
			rect.xMax -= 20f;
			Rect rect2 = rect;
			rect2.width = 20f;
			rect2.x = rect.xMax;
			rect2.height = 24f;
			rect.xMax -= 35f;
			Rect rect3 = rect;
			rect3.width = 32f;
			EditorGUI.LabelField(rect3, ParaImpostorBakePresetEditor.TargetsStr);
			rect3 = rect;
			rect3.xMin += 40f;
			rect3.width = EditorGUIUtility.labelWidth - rect3.xMin + 18f;
			EditorGUI.LabelField(rect3, ParaImpostorBakePresetEditor.SuffixStr);
			Rect rect4 = rect;
			rect4.xMin = EditorGUIUtility.labelWidth + 18f;
			float width = rect4.width;
			rect4.width = width * this.m_tableSizes[0];
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.TexScaleStr);
			rect4.x += rect4.width;
			rect4.width = 35f;
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.ColorSpaceStr);
			rect4.x += rect4.width;
			rect4.width = width * this.m_tableSizes[1];
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.CompressionStr);
			rect4.x += rect4.width;
			rect4.width = width * this.m_tableSizes[2];
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.FormatStr);
			EditorGUI.LabelField(rect2, ParaImpostorBakePresetEditor.OverrideStr);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00020EE8 File Offset: 0x0001F0E8
		private void DrawElement(Rect rect, int index, bool active, bool focused)
		{
			rect.y += 2f;
			Rect rect2 = rect;
			rect2.width = 20f;
			rect2.height = EditorGUIUtility.singleLineHeight;
			rect2.x = rect.xMax - rect2.width;
			rect.xMax -= rect2.width;
			Rect rect3 = rect;
			rect3.width = 16f;
			rect3.height = EditorGUIUtility.singleLineHeight;
			TextureOutput textureOutput = this.m_currentData.OverrideOutput.Find((TextureOutput x) => x.Index == index);
			bool flag = textureOutput != null;
			rect3.x += 10f;
			EditorGUI.LabelField(rect3, new GUIContent(index.ToString()));
			Rect rect4 = rect;
			rect4.x = rect3.xMax;
			rect4.width = 16f;
			rect4.y -= 1f;
			if (flag && (textureOutput.OverrideMask & 1) == 1)
			{
				GUI.color = this.m_overColor;
				textureOutput.Active = EditorGUI.Toggle(rect4, textureOutput.Active);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				this.m_currentData.Preset.Output[index].Active = EditorGUI.Toggle(rect4, this.m_currentData.Preset.Output[index].Active);
				EditorGUI.EndDisabledGroup();
			}
			if (flag)
			{
				EditorGUI.BeginDisabledGroup(!textureOutput.Active);
			}
			Rect rect5 = rect;
			rect5.height = EditorGUIUtility.singleLineHeight;
			rect5.width = EditorGUIUtility.labelWidth - 5f;
			rect5.xMin = rect4.xMax;
			if (flag && (textureOutput.OverrideMask & 2) == 2)
			{
				GUI.color = this.m_overColor;
				textureOutput.Name = EditorGUI.TextField(rect5, textureOutput.Name);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				this.m_currentData.Preset.Output[index].Name = EditorGUI.TextField(rect5, this.m_currentData.Preset.Output[index].Name);
				EditorGUI.EndDisabledGroup();
			}
			float num = rect2.xMin - rect5.xMax - 35f;
			Rect rect6 = rect;
			rect6.height = EditorGUIUtility.singleLineHeight;
			rect6.x = rect5.xMax;
			rect6.width = num * this.m_tableSizes[0];
			if (flag && (textureOutput.OverrideMask & 4) == 4)
			{
				GUI.color = this.m_overColor;
				textureOutput.Scale = EditorGUI.IntPopup(rect6, textureOutput.Scale, this.m_sizesScaleStr, ParaImpostorBakePresetEditor.TexScaleOpt);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				this.m_currentData.Preset.Output[index].Scale = EditorGUI.IntPopup(rect6, this.m_currentData.Preset.Output[index].Scale, this.m_sizesScaleStr, ParaImpostorBakePresetEditor.TexScaleOpt);
				EditorGUI.EndDisabledGroup();
			}
			rect6.x += rect6.width + 10f;
			rect6.width = 35f;
			rect6.y -= 1f;
			EditorGUI.BeginDisabledGroup(this.m_usingStandard);
			if (flag && (textureOutput.OverrideMask & 8) == 8)
			{
				GUI.color = this.m_overColor;
				textureOutput.SRGB = EditorGUI.Toggle(rect6, textureOutput.SRGB);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				this.m_currentData.Preset.Output[index].SRGB = EditorGUI.Toggle(rect6, this.m_currentData.Preset.Output[index].SRGB);
				EditorGUI.EndDisabledGroup();
			}
			EditorGUI.EndDisabledGroup();
			rect6.y += 1f;
			rect6.x += rect6.width - 10f;
			rect6.width = num * this.m_tableSizes[1];
			if (flag && (textureOutput.OverrideMask & 16) == 16)
			{
				GUI.color = this.m_overColor;
				textureOutput.Compression = EditorGUI.IntPopup(rect6, textureOutput.Compression, ParaImpostorBakePresetEditor.CompressionListStr, ParaImpostorBakePresetEditor.CompressionOpt);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				this.m_currentData.Preset.Output[index].Compression = EditorGUI.IntPopup(rect6, this.m_currentData.Preset.Output[index].Compression, ParaImpostorBakePresetEditor.CompressionListStr, ParaImpostorBakePresetEditor.CompressionOpt);
				EditorGUI.EndDisabledGroup();
			}
			rect6.x += rect6.width;
			rect6.width = num * this.m_tableSizes[2];
			if (flag && (textureOutput.OverrideMask & 32) == 32)
			{
				GUI.color = this.m_overColor;
				textureOutput.ImageFormat = (ImageFormat)EditorGUI.EnumPopup(rect6, textureOutput.ImageFormat);
				GUI.color = Color.white;
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				this.m_currentData.Preset.Output[index].ImageFormat = (ImageFormat)EditorGUI.EnumPopup(rect6, this.m_currentData.Preset.Output[index].ImageFormat);
				EditorGUI.EndDisabledGroup();
			}
			if (flag)
			{
				EditorGUI.EndDisabledGroup();
			}
			rect2.x += 1f;
			GUI.color = Color.white;
			EditorGUI.BeginChangeCheck();
			OverrideMask overrideMask;
			if (flag)
			{
				overrideMask = (OverrideMask)EditorGUI.EnumFlagsField(rect2, textureOutput.OverrideMask);
			}
			else
			{
				overrideMask = (OverrideMask)EditorGUI.EnumFlagsField(rect2, this.m_currentData.Preset.Output[index].OverrideMask);
			}
			if (EditorGUI.EndChangeCheck())
			{
				if (overrideMask != null && textureOutput == null)
				{
					TextureOutput textureOutput2 = this.m_currentData.Preset.Output[index].Clone();
					textureOutput2.Index = index;
					textureOutput2.OverrideMask = overrideMask;
					this.m_currentData.OverrideOutput.Add(textureOutput2);
					this.m_currentData.OverrideOutput.Sort((TextureOutput x, TextureOutput y) => x.Index.CompareTo(y.Index));
					return;
				}
				if (flag && overrideMask == null)
				{
					this.m_currentData.OverrideOutput.Remove(this.m_currentData.OverrideOutput.Find((TextureOutput x) => x.Index == index));
					return;
				}
				if (flag)
				{
					TextureOutput textureOutput3 = this.m_currentData.Preset.Output[index].Clone();
					if ((textureOutput.OverrideMask & 2) != 2 && (overrideMask & 2) == 2)
					{
						textureOutput.Name = textureOutput3.Name;
					}
					if ((textureOutput.OverrideMask & 4) != 4 && (overrideMask & 4) == 4)
					{
						textureOutput.Scale = textureOutput3.Scale;
					}
					if ((textureOutput.OverrideMask & 8) != 8 && (overrideMask & 8) == 8)
					{
						textureOutput.SRGB = textureOutput3.SRGB;
					}
					if ((textureOutput.OverrideMask & 16) != 16 && (overrideMask & 16) == 16)
					{
						textureOutput.Compression = textureOutput3.Compression;
					}
					if ((textureOutput.OverrideMask & 32) != 32 && (overrideMask & 32) == 32)
					{
						textureOutput.ImageFormat = textureOutput3.ImageFormat;
					}
					textureOutput.OverrideMask = overrideMask;
				}
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000216C0 File Offset: 0x0001F8C0
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			if (this.m_foldout == null)
			{
				this.m_foldout = "foldout";
			}
			if (ParaImpostorInspector.LockIconOpen == null)
			{
				ParaImpostorInspector.LockIconOpen = new GUIContent(EditorGUIUtility.IconContent("LockIcon-On"));
			}
			if (ParaImpostorInspector.LockIconClosed == null)
			{
				ParaImpostorInspector.LockIconClosed = new GUIContent(EditorGUIUtility.IconContent("LockIcon"));
			}
			if (ParaImpostorInspector.TextureIcon == null)
			{
				ParaImpostorInspector.TextureIcon = new GUIContent(EditorGUIUtility.IconContent("Texture Icon"))
				{
					text = " Bake Impostor"
				};
			}
			if (ParaImpostorInspector.CreateIcon == null)
			{
				ParaImpostorInspector.CreateIcon = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus"))
				{
					text = ""
				};
			}
			if (ParaImpostorInspector.SettingsIcon == null)
			{
				ParaImpostorInspector.SettingsIcon = new GUIContent(EditorGUIUtility.IconContent("icons/d_TerrainInspector.TerrainToolSettings.png"));
			}
			this.m_instance = base.target as ParaImpostor;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (this.m_instance.LodGroup != null && this.m_instance.m_lastImpostor == null)
			{
				double num = (double)Time.realtimeSinceStartup - this.m_lastTime;
				this.m_lastTime = (double)Time.realtimeSinceStartup;
				this.m_isFlashing = true;
				this.m_flash = Color.Lerp(this.m_flash, Color.white, (float)num * 3f);
			}
			else
			{
				this.m_isFlashing = false;
				this.m_flash = Color.white;
			}
			EditorGUI.BeginChangeCheck();
			this.m_instance.Data = EditorGUILayout.ObjectField(ParaImpostorInspector.AssetFieldStr, this.m_instance.Data, typeof(ParaImpostorAsset), false, Array.Empty<GUILayoutOption>()) as ParaImpostorAsset;
			if (this.m_instance.Data != null)
			{
				this.m_currentData = this.m_instance.Data;
			}
			this.m_instance.LodGroup = EditorGUILayout.ObjectField(ParaImpostorInspector.LODGroupStr, this.m_instance.LodGroup, typeof(LODGroup), true, Array.Empty<GUILayoutOption>()) as LODGroup;
			Color color = GUI.color;
			GUI.color = this.m_flash;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.m_renderers, ParaImpostorInspector.RenderersStr, false, Array.Empty<GUILayoutOption>());
			this.DrawRenderersInfo(EditorGUIUtility.currentViewWidth);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_recalculatePreviewTexture = true;
			}
			GUI.color = color;
			GUILayout.Space(9f);
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (this.m_instance.Data != null)
			{
				EditorGUI.BeginChangeCheck();
				ImpostorBakingTools.GlobalBakingOptions = GUILayout.Toggle(ImpostorBakingTools.GlobalBakingOptions, ParaImpostorInspector.SettingsIcon, "buttonleft", new GUILayoutOption[]
				{
					GUILayout.Width(32f),
					GUILayout.Height(24f)
				});
				if (EditorGUI.EndChangeCheck())
				{
					EditorPrefs.SetBool(ImpostorBakingTools.PrefGlobalBakingOptions, ImpostorBakingTools.GlobalBakingOptions);
				}
			}
			else if (GUILayout.Button(ParaImpostorInspector.CreateIcon, "buttonleft", new GUILayoutOption[]
			{
				GUILayout.Width(32f),
				GUILayout.Height(24f)
			}))
			{
				this.m_instance.CreateAssetFile(this.m_currentData);
			}
			if (GUILayout.Button(ParaImpostorInspector.TextureIcon, "buttonright", new GUILayoutOption[] { GUILayout.Height(24f) }))
			{
				this.m_outdatedTexture = true;
				this.m_recalculatePreviewTexture = true;
				flag3 = true;
			}
			EditorGUILayout.EndHorizontal();
			Vector3 lossyScale = this.m_instance.transform.lossyScale;
			if (lossyScale.x - lossyScale.y >= 0.0001f || lossyScale.y - lossyScale.z >= 0.0001f)
			{
				EditorGUILayout.HelpBox("Impostors can't render non-uniform scales correctly. Please consider scaling the object and it's parents uniformly or generate it as a child of one.", 2);
			}
			if (ImpostorBakingTools.GlobalBakingOptions && this.m_instance.Data != null)
			{
				EditorGUILayout.BeginVertical("helpbox", Array.Empty<GUILayoutOption>());
				EditorGUI.BeginChangeCheck();
				this.m_currentData.ImpostorType = (ImpostorType)EditorGUILayout.EnumPopup(ParaImpostorInspector.BakeTypeStr, this.m_currentData.ImpostorType, Array.Empty<GUILayoutOption>());
				if (EditorGUI.EndChangeCheck())
				{
					this.m_recalculatePreviewTexture = true;
					this.FetchShaderTag();
				}
				if (this.m_currentData.Preset != null && this.m_currentData.Preset.RuntimeShader != null && !this.m_shaderTag.Equals(this.m_currentData.ImpostorType.ToString()) && !string.IsNullOrEmpty(this.m_shaderTag))
				{
					EditorGUILayout.HelpBox("Bake type differs from shader impostor type, make sure to compile your shader with the correct impostor type.", 2);
				}
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				EditorGUI.BeginChangeCheck();
				if (this.m_currentData.LockedSizes)
				{
					this.m_currentData.SelectedSize = EditorGUILayout.IntPopup(ParaImpostorInspector.TextureSizeStr, this.m_currentData.SelectedSize, this.m_sizesStr, this.m_sizes, Array.Empty<GUILayoutOption>());
					this.m_currentData.LockedSizes = GUILayout.Toggle(this.m_currentData.LockedSizes, ParaImpostorInspector.LockIconOpen, "minibutton", new GUILayoutOption[] { GUILayout.Width(22f) });
					this.m_currentData.TexSize.Set((float)this.m_currentData.SelectedSize, (float)this.m_currentData.SelectedSize);
				}
				else
				{
					EditorGUILayout.LabelField(ParaImpostorInspector.TextureSizeStr, new GUILayoutOption[] { GUILayout.Width(EditorGUIUtility.labelWidth - 11f) });
					float labelWidth = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 12f;
					this.m_currentData.TexSize.x = (float)EditorGUILayout.IntPopup(new GUIContent("X"), (int)this.m_currentData.TexSize.x, this.m_sizesStr, this.m_sizes, Array.Empty<GUILayoutOption>());
					this.m_currentData.TexSize.y = (float)EditorGUILayout.IntPopup(new GUIContent("Y"), (int)this.m_currentData.TexSize.y, this.m_sizesStr, this.m_sizes, Array.Empty<GUILayoutOption>());
					EditorGUIUtility.labelWidth = labelWidth;
					this.m_currentData.LockedSizes = GUILayout.Toggle(this.m_currentData.LockedSizes, ParaImpostorInspector.LockIconClosed, "minibutton", new GUILayoutOption[] { GUILayout.Width(22f) });
				}
				if (EditorGUI.EndChangeCheck())
				{
					this.UpdateDynamicSizes();
					this.m_recalculatePreviewTexture = true;
				}
				EditorGUILayout.EndHorizontal();
				if (!this.m_currentData.DecoupleAxisFrames || this.m_currentData.ImpostorType != null)
				{
					EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					EditorGUI.BeginChangeCheck();
					this.m_currentData.HorizontalFrames = EditorGUILayout.IntSlider(ParaImpostorInspector.AxisFramesStr, this.m_currentData.HorizontalFrames, 1, 32, Array.Empty<GUILayoutOption>());
					if (EditorGUI.EndChangeCheck())
					{
						this.m_recalculatePreviewTexture = true;
					}
					this.m_currentData.VerticalFrames = this.m_currentData.HorizontalFrames;
					if (this.m_currentData.ImpostorType == null)
					{
						this.m_currentData.DecoupleAxisFrames = !GUILayout.Toggle(!this.m_currentData.DecoupleAxisFrames, ParaImpostorInspector.LockIconOpen, "minibutton", new GUILayoutOption[] { GUILayout.Width(22f) });
					}
					EditorGUILayout.EndHorizontal();
				}
				else
				{
					EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					EditorGUILayout.LabelField(ParaImpostorInspector.AxisFramesStr, Array.Empty<GUILayoutOption>());
					this.m_currentData.DecoupleAxisFrames = !GUILayout.Toggle(!this.m_currentData.DecoupleAxisFrames, ParaImpostorInspector.LockIconClosed, "minibutton", new GUILayoutOption[] { GUILayout.Width(22f) });
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel++;
					EditorGUI.BeginChangeCheck();
					this.m_currentData.HorizontalFrames = EditorGUILayout.IntSlider("X", this.m_currentData.HorizontalFrames, 1, 32, Array.Empty<GUILayoutOption>());
					this.m_currentData.VerticalFrames = EditorGUILayout.IntSlider("Y", this.m_currentData.VerticalFrames, 1, 32, Array.Empty<GUILayoutOption>());
					if (EditorGUI.EndChangeCheck())
					{
						this.m_recalculatePreviewTexture = true;
					}
					EditorGUI.indentLevel--;
				}
				this.m_currentData.PixelPadding = EditorGUILayout.IntSlider(ParaImpostorInspector.PixelPaddingStr, this.m_currentData.PixelPadding, 0, 64, Array.Empty<GUILayoutOption>());
				EditorGUI.BeginDisabledGroup(this.m_instance.m_lastImpostor != null || this.m_instance.LodGroup == null);
				EditorGUI.BeginChangeCheck();
				this.m_instance.m_lodReplacement = (LODReplacement)EditorGUILayout.EnumPopup(ParaImpostorInspector.LODModeStr, this.m_instance.m_lodReplacement, Array.Empty<GUILayoutOption>());
				EditorGUI.BeginDisabledGroup(this.m_instance.m_lodReplacement < 4 || this.m_instance.LodGroup == null);
				int num2 = 0;
				if (this.m_instance.LodGroup != null)
				{
					num2 = this.m_instance.LodGroup.lodCount - 1;
				}
				this.m_instance.m_insertIndex = EditorGUILayout.IntSlider(ParaImpostorInspector.LODTargetIndexStr, this.m_instance.m_insertIndex, 0, num2, Array.Empty<GUILayoutOption>());
				EditorGUI.EndDisabledGroup();
				if (EditorGUI.EndChangeCheck())
				{
					this.ReCheckRenderers();
				}
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel++;
				this.m_billboardMesh = GUILayout.Toggle(this.m_billboardMesh, "Billboard Mesh", "foldout", Array.Empty<GUILayoutOption>());
				EditorGUI.indentLevel--;
				if (this.m_recalculatePreviewTexture && this.m_instance.m_alphaTex != null)
				{
					this.m_outdatedTexture = true;
				}
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (this.m_billboardMesh)
				{
					this.DrawBillboardMesh(ref flag, ref flag2, 160);
					EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
					EditorGUI.BeginChangeCheck();
					this.m_instance.m_cutMode = GUILayout.Toolbar(this.m_instance.m_cutMode, new string[] { "Automatic", "Manual" }, Array.Empty<GUILayoutOption>());
					if (EditorGUI.EndChangeCheck() && this.m_instance.m_cutMode == null)
					{
						this.m_recalculateMesh = true;
					}
					float labelWidth2 = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 120f;
					CutMode cutMode = this.m_instance.m_cutMode;
					if (cutMode == null || cutMode != 1)
					{
						EditorGUI.BeginChangeCheck();
						this.m_currentData.MaxVertices = EditorGUILayout.IntSlider(ParaImpostorInspector.MaxVerticesStr, this.m_currentData.MaxVertices, 4, 16, Array.Empty<GUILayoutOption>());
						this.m_currentData.Tolerance = EditorGUILayout.Slider(ParaImpostorInspector.OutlineToleranceStr, this.m_currentData.Tolerance * 5f, 0f, 1f, Array.Empty<GUILayoutOption>()) * 0.2f;
						this.m_currentData.NormalScale = EditorGUILayout.Slider(ParaImpostorInspector.NormalScaleStr, this.m_currentData.NormalScale, 0f, 1f, Array.Empty<GUILayoutOption>());
						if (EditorGUI.EndChangeCheck())
						{
							this.m_recalculateMesh = true;
						}
					}
					else
					{
						this.m_currentData.MaxVertices = EditorGUILayout.IntSlider(ParaImpostorInspector.MaxVerticesStr, this.m_currentData.MaxVertices, 4, 16, Array.Empty<GUILayoutOption>());
						this.m_currentData.Tolerance = EditorGUILayout.Slider(ParaImpostorInspector.OutlineToleranceStr, this.m_currentData.Tolerance * 5f, 0f, 1f, Array.Empty<GUILayoutOption>()) * 0.2f;
						this.m_currentData.NormalScale = EditorGUILayout.Slider(ParaImpostorInspector.NormalScaleStr, this.m_currentData.NormalScale, 0f, 1f, Array.Empty<GUILayoutOption>());
						if (GUILayout.Button("Update", Array.Empty<GUILayoutOption>()))
						{
							this.m_recalculateMesh = true;
						}
						this.m_lastPointSelected = Mathf.Clamp(this.m_lastPointSelected, 0, this.m_currentData.ShapePoints.Length - 1);
						EditorGUILayout.Space();
						if (this.m_currentData.ShapePoints.Length != 0)
						{
							this.m_currentData.ShapePoints[this.m_lastPointSelected] = EditorGUILayout.Vector2Field("", this.m_currentData.ShapePoints[this.m_lastPointSelected], Array.Empty<GUILayoutOption>());
							this.m_currentData.ShapePoints[this.m_lastPointSelected].x = Mathf.Clamp01(this.m_currentData.ShapePoints[this.m_lastPointSelected].x);
							this.m_currentData.ShapePoints[this.m_lastPointSelected].y = Mathf.Clamp01(this.m_currentData.ShapePoints[this.m_lastPointSelected].y);
						}
					}
					EditorGUIUtility.labelWidth = labelWidth2;
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.m_presetOptions = GUILayout.Toggle(this.m_presetOptions, ParaImpostorInspector.BakingPresetStr, "foldout", new GUILayoutOption[] { GUILayout.Width(EditorGUIUtility.labelWidth - 11f) });
				this.m_currentData.Preset = EditorGUILayout.ObjectField(this.m_currentData.Preset, typeof(ParaImpostorBakePreset), false, Array.Empty<GUILayoutOption>()) as ParaImpostorBakePreset;
				if (GUILayout.Button("New", "minibutton", new GUILayoutOption[] { GUILayout.Width(50f) }))
				{
					string text = EditorUtility.SaveFilePanelInProject("Create new Bake Preset", "New Preset", "asset", "", AssetDatabase.GetAssetPath(this.m_currentData));
					if (!string.IsNullOrEmpty(text))
					{
						AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ParaImpostorBakePreset>(), text);
						this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(text);
						this.m_currentData.Preset.Output = new List<TextureOutput>
						{
							new TextureOutput(true, ImpostorBakingTools.GlobalAlbedoAlpha, 1, true, 0, 1, 1),
							new TextureOutput(true, ImpostorBakingTools.GlobalSpecularSmoothness, 1, true, 0, 1, 1),
							new TextureOutput(true, ImpostorBakingTools.GlobalNormalDepth, 1, false, 0, 1, 1),
							new TextureOutput(true, ImpostorBakingTools.GlobalEmissionOcclusion, 1, false, 0, 1, 1)
						};
						EditorUtility.SetDirty(this.m_currentData.Preset);
					}
				}
				EditorGUILayout.EndHorizontal();
				if (this.m_currentData.Preset == null)
				{
					this.m_instance.DetectRenderPipeline();
					if (this.m_instance.m_renderPipelineInUse == 1)
					{
						this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("47b6b3dcefe0eaf4997acf89caf8c75e"));
					}
					else if (this.m_instance.m_renderPipelineInUse == 2)
					{
						this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("0403878495ffa3c4e9d4bcb3eac9b559"));
					}
					else
					{
						this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("e4786beb7716da54dbb02a632681cc37"));
					}
				}
				if (this.m_presetOptions)
				{
					EditorGUI.BeginDisabledGroup(true);
					this.m_currentData.Preset.BakeShader = EditorGUILayout.ObjectField(ParaImpostorBakePresetEditor.BakeShaderStr, this.m_currentData.Preset.BakeShader, typeof(Shader), false, Array.Empty<GUILayoutOption>()) as Shader;
					this.m_currentData.Preset.RuntimeShader = EditorGUILayout.ObjectField(ParaImpostorBakePresetEditor.RuntimeShaderStr, this.m_currentData.Preset.RuntimeShader, typeof(Shader), false, Array.Empty<GUILayoutOption>()) as Shader;
					EditorGUI.EndDisabledGroup();
					if (EditorGUI.EndChangeCheck() || this.m_texturesOutput.count != this.m_currentData.Preset.Output.Count)
					{
						this.RefreshList();
						base.Repaint();
					}
					this.m_texturesOutput.DoLayoutList();
				}
				EditorGUILayout.EndVertical();
			}
			if (this.m_currentData.Preset == null)
			{
				this.m_instance.DetectRenderPipeline();
				if (this.m_instance.m_renderPipelineInUse == 1)
				{
					this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("47b6b3dcefe0eaf4997acf89caf8c75e"));
				}
				else if (this.m_instance.m_renderPipelineInUse == 2)
				{
					this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("0403878495ffa3c4e9d4bcb3eac9b559"));
				}
				else
				{
					this.m_currentData.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("e4786beb7716da54dbb02a632681cc37"));
				}
			}
			if (((this.m_billboardMesh || this.m_recalculatePreviewTexture) && this.m_instance.m_alphaTex == null) || (flag3 && this.m_recalculatePreviewTexture))
			{
				try
				{
					this.m_instance.RenderCombinedAlpha(this.m_currentData);
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[ParaImpostors] Something went wrong with the mesh preview process, please contact support@amplify.pt with this log message.\n" + ex.Message + ex.StackTrace);
				}
				if (this.m_instance.m_cutMode == null)
				{
					this.m_recalculateMesh = true;
				}
				this.m_recalculatePreviewTexture = false;
			}
			if (this.m_recalculateMesh && this.m_instance.m_alphaTex != null)
			{
				this.m_recalculateMesh = false;
				this.m_instance.GenerateAutomaticMesh(this.m_currentData);
				flag = true;
				EditorUtility.SetDirty(this.m_instance);
			}
			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(this.m_currentData);
				EditorUtility.SetDirty(this.m_instance);
			}
			if (flag)
			{
				this.m_previewMesh = this.GeneratePreviewMesh(this.m_currentData.ShapePoints, true);
			}
			if (flag2)
			{
				flag2 = false;
				this.m_instance.m_cutMode = 1;
				Event.current.Use();
			}
			if (flag3)
			{
				flag3 = false;
				EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.delayCall, new EditorApplication.CallbackFunction(this.DelayedBake));
			}
			base.serializedObject.ApplyModifiedProperties();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000227D8 File Offset: 0x000209D8
		private void DelayedBake()
		{
			try
			{
				this.m_instance.RenderAllDeferredGroups(this.m_currentData);
			}
			catch (Exception ex)
			{
				EditorUtility.ClearProgressBar();
				Debug.LogWarning("[ParaImpostors] Something went wrong with the baking process, please contact support@amplify.pt with this log message.\n" + ex.Message + ex.StackTrace);
			}
			EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.delayCall, new EditorApplication.CallbackFunction(this.DelayedBake));
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0002284C File Offset: 0x00020A4C
		private void OnInspectorUpdate()
		{
			base.Repaint();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00022854 File Offset: 0x00020A54
		public override bool RequiresConstantRepaint()
		{
			return this.m_isFlashing || (this.m_billboardMesh && ImpostorBakingTools.GlobalBakingOptions);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00022870 File Offset: 0x00020A70
		public Mesh GeneratePreviewMesh(Vector2[] points, bool invertY = true)
		{
			Triangulator triangulator = new Triangulator(points, invertY);
			int[] array = triangulator.Triangulate();
			Vector3[] array2 = new Vector3[triangulator.Points.Count];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = new Vector3(triangulator.Points[i].x, triangulator.Points[i].y, 0f);
			}
			return new Mesh
			{
				vertices = array2,
				uv = points,
				triangles = array
			};
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000228F8 File Offset: 0x00020AF8
		public static ParaImpostorInspector.GUIStyles Styles
		{
			get
			{
				if (ParaImpostorInspector.s_Styles == null)
				{
					ParaImpostorInspector.s_Styles = new ParaImpostorInspector.GUIStyles();
				}
				return ParaImpostorInspector.s_Styles;
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00022910 File Offset: 0x00020B10
		private void DrawRenderersInfo(float availableWidth)
		{
			int num = Mathf.FloorToInt(availableWidth / 60f);
			SerializedProperty renderers = this.m_renderers;
			int num2 = renderers.arraySize + 1;
			int num3 = Mathf.CeilToInt((float)num2 / (float)num);
			Rect rect = GUILayoutUtility.GetRect(0f, (float)(num3 * 60), new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
			Rect rect2 = rect;
			GUI.Box(rect, GUIContent.none);
			rect2.width -= 6f;
			rect2.x += 3f;
			float num4 = rect2.width / (float)num;
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < num3; i++)
			{
				int num5 = 0;
				while (num5 < num && i * num + num5 < renderers.arraySize)
				{
					Rect rect3;
					rect3..ctor(2f + rect2.x + (float)num5 * num4, 2f + rect2.y + (float)(i * 60), num4 - 4f, 56f);
					list.Add(rect3);
					this.DrawRendererButton(rect3, i * num + num5);
					num5++;
				}
			}
			int num6 = (num2 - 1) % num;
			int num7 = num3 - 1;
			this.HandleAddRenderer(new Rect(2f + rect2.x + (float)num6 * num4, 2f + rect2.y + (float)(num7 * 60), num4 - 4f, 56f), list, rect);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00022A84 File Offset: 0x00020C84
		private void DrawBillboardMesh(ref bool triangulateMesh, ref bool autoChangeToManual, int cutPreviewSize = 160)
		{
			Rect rect = GUILayoutUtility.GetRect((float)(cutPreviewSize + 10), (float)(cutPreviewSize + 10), (float)(cutPreviewSize + 10), (float)(cutPreviewSize + 10));
			int controlID = GUIUtility.GetControlID("miniShapeEditorControl".GetHashCode(), 2, rect);
			Rect rect2;
			rect2..ctor(5f, 5f, (float)cutPreviewSize, (float)cutPreviewSize);
			Rect rect3;
			rect3..ctor(0f, 0f, (float)(cutPreviewSize + 10), (float)(cutPreviewSize + 10));
			Event current = Event.current;
			GUI.BeginClip(rect);
			if (current.type == 7)
			{
				if (this.m_instance.m_alphaTex != null)
				{
					Graphics.DrawTexture(rect2, this.m_instance.m_alphaTex, this.m_alphaMaterial, 3);
				}
				else
				{
					Graphics.DrawTexture(rect2, Texture2D.blackTexture, this.m_alphaMaterial, 3);
				}
			}
			if (this.m_outdatedTexture)
			{
				Color color = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, 0.2f);
				EditorGUI.DrawPreviewTexture(rect2, Texture2D.whiteTexture);
				GUI.color = color;
				Rect rect4 = rect2;
				rect4.xMin += 50f;
				rect4.xMax -= 50f;
				rect4.yMin += 70f;
				rect4.yMax -= 60f;
				if (GUI.Button(rect4, "UPDATE", "AssetLabel"))
				{
					this.m_instance.m_alphaTex = null;
					this.m_outdatedTexture = false;
					if (this.m_instance.m_cutMode == null)
					{
						this.m_recalculateMesh = true;
					}
				}
				GUI.color = color;
			}
			EventType typeForControl = current.GetTypeForControl(controlID);
			switch (typeForControl)
			{
			case 0:
				if (rect3.Contains(current.mousePosition))
				{
					for (int i = 0; i < this.m_currentData.ShapePoints.Length; i++)
					{
						Rect rect5;
						rect5..ctor(this.m_currentData.ShapePoints[i].x * (float)cutPreviewSize, this.m_currentData.ShapePoints[i].y * (float)cutPreviewSize, 10f, 10f);
						if (current.type == null && rect5.Contains(current.mousePosition))
						{
							EditorGUI.FocusTextInControl(null);
							this.m_activeHandle = i;
							this.m_lastPointSelected = i;
							this.m_lastMousePos = current.mousePosition;
							this.m_originalPos = this.m_currentData.ShapePoints[i];
						}
					}
					GUIUtility.hotControl = controlID;
					goto IL_040A;
				}
				goto IL_040A;
			case 1:
				break;
			case 2:
				goto IL_040A;
			case 3:
				if (GUIUtility.hotControl == controlID && this.m_activeHandle > -1)
				{
					this.m_currentData.ShapePoints[this.m_activeHandle] = this.m_originalPos + (current.mousePosition - this.m_lastMousePos) / (float)(cutPreviewSize + 10);
					if (current.modifiers != 2)
					{
						this.m_currentData.ShapePoints[this.m_activeHandle].x = (float)Math.Round((double)this.m_currentData.ShapePoints[this.m_activeHandle].x, 2);
						this.m_currentData.ShapePoints[this.m_activeHandle].y = (float)Math.Round((double)this.m_currentData.ShapePoints[this.m_activeHandle].y, 2);
					}
					this.m_currentData.ShapePoints[this.m_activeHandle].x = Mathf.Clamp01(this.m_currentData.ShapePoints[this.m_activeHandle].x);
					this.m_currentData.ShapePoints[this.m_activeHandle].y = Mathf.Clamp01(this.m_currentData.ShapePoints[this.m_activeHandle].y);
					autoChangeToManual = true;
					goto IL_040A;
				}
				goto IL_040A;
			default:
				if (typeForControl != 11)
				{
					goto IL_040A;
				}
				break;
			}
			if (GUIUtility.hotControl == controlID)
			{
				this.m_activeHandle = -1;
				triangulateMesh = true;
				GUIUtility.hotControl = 0;
				GUI.changed = true;
			}
			IL_040A:
			if (current.type == 7)
			{
				Vector3[] array = new Vector3[this.m_currentData.ShapePoints.Length + 1];
				for (int j = 0; j < this.m_currentData.ShapePoints.Length; j++)
				{
					array[j] = new Vector3(this.m_currentData.ShapePoints[j].x * (float)cutPreviewSize + 5f, this.m_currentData.ShapePoints[j].y * (float)cutPreviewSize + 5f, 0f);
				}
				array[this.m_currentData.ShapePoints.Length] = array[0];
				Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
				for (int k = 0; k < this.m_currentData.ShapePoints.Length; k++)
				{
					if (k == this.m_currentData.ShapePoints.Length - 1)
					{
						dictionary.Add("0" + k.ToString(), true);
					}
					else
					{
						dictionary.Add(k.ToString() + (k + 1).ToString(), true);
					}
				}
				if (this.m_previewMesh != null && this.m_instance.m_cutMode == 1)
				{
					Color color2 = Handles.color;
					Handles.color = new Color(1f, 1f, 1f, 0.5f);
					for (int l = 0; l < this.m_previewMesh.triangles.Length - 1; l += 3)
					{
						int num = this.m_previewMesh.triangles[l];
						int num2 = this.m_previewMesh.triangles[l + 1];
						int num3 = this.m_previewMesh.triangles[l + 2];
						string text = ((num < num2) ? (num.ToString() + num2.ToString()) : (num2.ToString() + num.ToString()));
						string text2 = ((num2 < num3) ? (num2.ToString() + num3.ToString()) : (num3.ToString() + num2.ToString()));
						string text3 = ((num < num3) ? (num.ToString() + num3.ToString()) : (num3.ToString() + num.ToString()));
						Vector3 vector;
						vector..ctor(this.m_currentData.ShapePoints[num].x * (float)cutPreviewSize + 5f, this.m_currentData.ShapePoints[num].y * (float)cutPreviewSize + 5f, 0f);
						Vector3 vector2;
						vector2..ctor(this.m_currentData.ShapePoints[num2].x * (float)cutPreviewSize + 5f, this.m_currentData.ShapePoints[num2].y * (float)cutPreviewSize + 5f, 0f);
						Vector3 vector3;
						vector3..ctor(this.m_currentData.ShapePoints[num3].x * (float)cutPreviewSize + 5f, this.m_currentData.ShapePoints[num3].y * (float)cutPreviewSize + 5f, 0f);
						if (!dictionary.ContainsKey(text))
						{
							Handles.DrawAAPolyLine(new Vector3[] { vector, vector2 });
							dictionary.Add(text, true);
						}
						if (!dictionary.ContainsKey(text2))
						{
							Handles.DrawAAPolyLine(new Vector3[] { vector2, vector3 });
							dictionary.Add(text2, true);
						}
						if (!dictionary.ContainsKey(text3))
						{
							Handles.DrawAAPolyLine(new Vector3[] { vector, vector3 });
							dictionary.Add(text3, true);
						}
					}
					Handles.color = color2;
				}
				Handles.DrawAAPolyLine(array);
				if (this.m_instance.m_cutMode == 1)
				{
					for (int m = 0; m < this.m_currentData.ShapePoints.Length; m++)
					{
						Handles.DrawSolidRectangleWithOutline(new Rect(this.m_currentData.ShapePoints[m].x * (float)cutPreviewSize + 1f, this.m_currentData.ShapePoints[m].y * (float)cutPreviewSize + 1f, 8f, 8f), (this.m_activeHandle == m) ? Color.cyan : Color.clear, (this.m_lastPointSelected == m && this.m_instance.m_cutMode == 1) ? Color.cyan : Color.white);
					}
				}
				else
				{
					for (int n = 0; n < this.m_currentData.ShapePoints.Length; n++)
					{
						Handles.DrawSolidRectangleWithOutline(new Rect(this.m_currentData.ShapePoints[n].x * (float)cutPreviewSize + 3f, this.m_currentData.ShapePoints[n].y * (float)cutPreviewSize + 3f, 4f, 4f), Color.white, Color.white);
					}
				}
			}
			GUI.EndClip();
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000233B4 File Offset: 0x000215B4
		private void DrawRendererButton(Rect position, int rendererIndex)
		{
			Renderer renderer = this.m_renderers.GetArrayElementAtIndex(rendererIndex).objectReferenceValue as Renderer;
			Rect rect;
			rect..ctor(position.xMax - 20f, position.yMax - 20f, 20f, 20f);
			Event current = Event.current;
			EventType type = current.type;
			if (type != null)
			{
				if (type == 7)
				{
					if (renderer != null)
					{
						MeshFilter component = renderer.GetComponent<MeshFilter>();
						GUIContent guicontent;
						if (component != null && component.sharedMesh != null)
						{
							guicontent = new GUIContent(AssetPreview.GetAssetPreview(component.sharedMesh), renderer.gameObject.name);
						}
						else if (renderer is SkinnedMeshRenderer)
						{
							guicontent = new GUIContent(AssetPreview.GetAssetPreview((renderer as SkinnedMeshRenderer).sharedMesh), renderer.gameObject.name);
						}
						else
						{
							guicontent = new GUIContent(ObjectNames.NicifyVariableName(renderer.GetType().Name), renderer.gameObject.name);
						}
						ParaImpostorInspector.Styles.m_LODBlackBox.Draw(position, GUIContent.none, false, false, false, false);
						ParaImpostorInspector.Styles.m_LODRendererButton.Draw(new Rect(position.x + 2f, position.y + 2f, position.width - 4f, position.height - 4f), guicontent, false, false, false, false);
					}
					else
					{
						ParaImpostorInspector.Styles.m_LODBlackBox.Draw(position, GUIContent.none, false, false, false, false);
						ParaImpostorInspector.Styles.m_LODRendererButton.Draw(position, "<Empty>", false, false, false, false);
					}
					ParaImpostorInspector.Styles.m_LODBlackBox.Draw(rect, GUIContent.none, false, false, false, false);
					ParaImpostorInspector.Styles.m_LODRendererRemove.Draw(rect, ParaImpostorInspector.Styles.m_IconRendererMinus, false, false, false, false);
					return;
				}
			}
			else
			{
				if (rect.Contains(current.mousePosition))
				{
					this.m_instance.Renderers = Array.FindAll<Renderer>(this.m_instance.Renderers, (Renderer x) => Array.IndexOf<Renderer>(this.m_instance.Renderers, x) != rendererIndex);
					GUI.changed = true;
					current.Use();
					base.serializedObject.ApplyModifiedProperties();
					return;
				}
				if (position.Contains(current.mousePosition))
				{
					EditorGUIUtility.PingObject(renderer);
					current.Use();
				}
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00023614 File Offset: 0x00021814
		private void HandleAddRenderer(Rect position, IEnumerable<Rect> alreadyDrawn, Rect drawArea)
		{
			Event evt = Event.current;
			EventType type = evt.type;
			if (type <= 7)
			{
				if (type != null)
				{
					if (type != 7)
					{
						return;
					}
					ParaImpostorInspector.Styles.m_LODStandardButton.Draw(position, GUIContent.none, false, false, false, false);
					ParaImpostorInspector.Styles.m_LODRendererAddButton.Draw(new Rect(position.x - 2f, position.y, position.width, position.height), "Add", false, false, false, false);
					return;
				}
				else if (position.Contains(evt.mousePosition))
				{
					evt.Use();
					int hashCode = "ImpostorsSelector".GetHashCode();
					EditorGUIUtility.ShowObjectPicker<Renderer>(null, true, null, hashCode);
					GUIUtility.ExitGUI();
					return;
				}
			}
			else if (type - 9 > 1)
			{
				if (type != 14)
				{
					return;
				}
				if (evt.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == "ImpostorsSelector".GetHashCode())
				{
					GameObject gameObject = EditorGUIUtility.GetObjectPickerObject() as GameObject;
					if (gameObject != null)
					{
						this.AddGameObjectRenderers(this.GetRenderers(new List<GameObject> { gameObject }, true), true);
					}
					evt.Use();
					GUIUtility.ExitGUI();
				}
			}
			else
			{
				bool flag = false;
				if (drawArea.Contains(evt.mousePosition) && alreadyDrawn.All((Rect x) => !x.Contains(evt.mousePosition)))
				{
					flag = true;
				}
				if (flag)
				{
					if (DragAndDrop.objectReferences.Count<Object>() > 0)
					{
						DragAndDrop.visualMode = 1;
						if (evt.type == 10)
						{
							IEnumerable<GameObject> enumerable = from go in DragAndDrop.objectReferences
								where go as GameObject != null
								select go as GameObject;
							IEnumerable<Renderer> renderers = this.GetRenderers(enumerable, true);
							this.AddGameObjectRenderers(renderers, true);
							DragAndDrop.AcceptDrag();
							evt.Use();
							return;
						}
					}
					evt.Use();
					return;
				}
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00023834 File Offset: 0x00021A34
		private IEnumerable<Renderer> GetRenderers(IEnumerable<GameObject> selectedGameObjects, bool searchChildren)
		{
			bool flag = EditorUtility.IsPersistent(this.m_instance);
			List<Renderer> list = new List<Renderer>();
			foreach (GameObject gameObject in selectedGameObjects)
			{
				if (!flag || EditorUtility.IsPersistent(gameObject))
				{
					if (searchChildren)
					{
						list.AddRange(gameObject.GetComponentsInChildren<Renderer>());
					}
					else
					{
						list.Add(gameObject.GetComponent<Renderer>());
					}
				}
			}
			IEnumerable<Renderer> enumerable = from go in DragAndDrop.objectReferences
				where go as Renderer != null
				select go as Renderer;
			list.AddRange(enumerable);
			return list;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0002390C File Offset: 0x00021B0C
		private void AddGameObjectRenderers(IEnumerable<Renderer> toAdd, bool add)
		{
			SerializedProperty renderers = this.m_renderers;
			if (!add)
			{
				renderers.ClearArray();
			}
			List<Renderer> list = new List<Renderer>();
			for (int i = 0; i < renderers.arraySize; i++)
			{
				Renderer renderer = renderers.GetArrayElementAtIndex(i).objectReferenceValue as Renderer;
				if (!(renderer == null))
				{
					list.Add(renderer);
				}
			}
			foreach (Renderer renderer2 in toAdd)
			{
				if (!list.Contains(renderer2))
				{
					renderers.arraySize++;
					renderers.GetArrayElementAtIndex(renderers.arraySize - 1).objectReferenceValue = renderer2;
					list.Add(renderer2);
				}
			}
			base.serializedObject.ApplyModifiedProperties();
			GUI.changed = true;
		}

		// Token: 0x040002D4 RID: 724
		private bool m_billboardMesh;

		// Token: 0x040002D5 RID: 725
		private bool m_presetOptions;

		// Token: 0x040002D6 RID: 726
		private GUIStyle m_foldout;

		// Token: 0x040002D7 RID: 727
		private ParaImpostor m_instance;

		// Token: 0x040002D8 RID: 728
		private SerializedProperty m_renderers;

		// Token: 0x040002D9 RID: 729
		private static GUIContent LockIconOpen = null;

		// Token: 0x040002DA RID: 730
		private static GUIContent LockIconClosed = null;

		// Token: 0x040002DB RID: 731
		private static GUIContent TextureIcon = null;

		// Token: 0x040002DC RID: 732
		private static GUIContent CreateIcon = null;

		// Token: 0x040002DD RID: 733
		private static GUIContent SettingsIcon = null;

		// Token: 0x040002DE RID: 734
		private float[] m_tableSizes = new float[] { 0.33f, 0.33f, 0.33f };

		// Token: 0x040002DF RID: 735
		private readonly float[] m_tableSizesLocked = new float[] { 0.33f, 0.33f, 0.33f };

		// Token: 0x040002E0 RID: 736
		private readonly float[] m_tableSizesUnlocked = new float[] { 0.4f, 0.3f, 0.3f };

		// Token: 0x040002E1 RID: 737
		private GUIContent[] m_sizesScaleStr = new GUIContent[]
		{
			new GUIContent("2048"),
			new GUIContent("1024"),
			new GUIContent("512"),
			new GUIContent("256")
		};

		// Token: 0x040002E2 RID: 738
		private readonly int[] m_sizes = new int[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };

		// Token: 0x040002E3 RID: 739
		private readonly GUIContent[] m_sizesStr = new GUIContent[]
		{
			new GUIContent("32"),
			new GUIContent("64"),
			new GUIContent("128"),
			new GUIContent("256"),
			new GUIContent("512"),
			new GUIContent("1024"),
			new GUIContent("2048"),
			new GUIContent("4096"),
			new GUIContent("8192")
		};

		// Token: 0x040002E4 RID: 740
		private Mesh m_previewMesh;

		// Token: 0x040002E5 RID: 741
		private int m_activeHandle = -1;

		// Token: 0x040002E6 RID: 742
		private Vector2 m_lastMousePos;

		// Token: 0x040002E7 RID: 743
		private Vector2 m_originalPos;

		// Token: 0x040002E8 RID: 744
		private int m_lastPointSelected = -1;

		// Token: 0x040002E9 RID: 745
		private bool m_recalculateMesh;

		// Token: 0x040002EA RID: 746
		private bool m_recalculatePreviewTexture;

		// Token: 0x040002EB RID: 747
		private bool m_outdatedTexture;

		// Token: 0x040002EC RID: 748
		private string m_shaderTag = "";

		// Token: 0x040002ED RID: 749
		private Material m_alphaMaterial;

		// Token: 0x040002EE RID: 750
		private static readonly GUIContent AssetFieldStr = new GUIContent("Impostor Asset", "Asset that will hold most of the impostor data");

		// Token: 0x040002EF RID: 751
		private static readonly GUIContent LODGroupStr = new GUIContent("LOD Group", "If it exists this allows to automatically setup the impostor in the given LOD Group");

		// Token: 0x040002F0 RID: 752
		private static readonly GUIContent RenderersStr = new GUIContent("References", "References to the renderers that will be used to bake the impostor");

		// Token: 0x040002F1 RID: 753
		private static readonly GUIContent BakeTypeStr = new GUIContent("Bake Type", "Technique used for both baking and rendering the impostor");

		// Token: 0x040002F2 RID: 754
		private static readonly GUIContent TextureSizeStr = new GUIContent("Texture Size", "The texture size in pixels for the final baked images. Higher resolution images provides better results at closer ranges, but are heavier in both storage and runtime.");

		// Token: 0x040002F3 RID: 755
		private static readonly GUIContent AxisFramesStr = new GUIContent("Axis Frames", "The amount frames per axis");

		// Token: 0x040002F4 RID: 756
		private static readonly GUIContent PixelPaddingStr = new GUIContent("Pixel Padding", "Padding size in pixels. Padding expands the edge pixels of the individual shots to avoid rendering artifacts caused by mipmapping.");

		// Token: 0x040002F5 RID: 757
		private static readonly GUIContent BakingPresetStr = new GUIContent("Bake Preset", "Preset object that contains the baking configuration. When empty it will use the standard preset.");

		// Token: 0x040002F6 RID: 758
		private static readonly GUIContent LODModeStr = new GUIContent("LOD Insert Mode", "A rule of how the impostor will be automatically included in the LOD Group");

		// Token: 0x040002F7 RID: 759
		private static readonly GUIContent LODTargetIndexStr = new GUIContent("LOD Target Index", "Target index for the current insert mode");

		// Token: 0x040002F8 RID: 760
		private static readonly GUIContent MaxVerticesStr = new GUIContent("Max Vertices", "Maximum number of vertices that ensures the final created amount does not exceed it");

		// Token: 0x040002F9 RID: 761
		private static readonly GUIContent OutlineToleranceStr = new GUIContent("Outline Tolerance", "Allows the final shape to more tightly fit the object by increasing its number or vertices");

		// Token: 0x040002FA RID: 762
		private static readonly GUIContent NormalScaleStr = new GUIContent("Normal Scale", "Scales the vertices out according to the shape normals");

		// Token: 0x040002FB RID: 763
		private Color m_overColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x040002FC RID: 764
		private Color m_flashColor = new Color(0.35f, 0.55f, 1f);

		// Token: 0x040002FD RID: 765
		private Color m_flash = Color.white;

		// Token: 0x040002FE RID: 766
		private ParaImpostorAsset m_currentData;

		// Token: 0x040002FF RID: 767
		private double m_lastTime;

		// Token: 0x04000300 RID: 768
		private bool m_isFlashing;

		// Token: 0x04000301 RID: 769
		private ReorderableList m_texturesOutput;

		// Token: 0x04000302 RID: 770
		private bool m_usingStandard;

		// Token: 0x04000303 RID: 771
		public const int kRenderersButtonHeight = 60;

		// Token: 0x04000304 RID: 772
		public const int kButtonPadding = 2;

		// Token: 0x04000305 RID: 773
		public const int kDeleteButtonSize = 20;

		// Token: 0x04000306 RID: 774
		public const int kRenderAreaForegroundPadding = 3;

		// Token: 0x04000307 RID: 775
		private static ParaImpostorInspector.GUIStyles s_Styles;

		// Token: 0x020000DF RID: 223
		public class GUIStyles
		{
			// Token: 0x0400041C RID: 1052
			public readonly GUIStyle m_LODStandardButton = "Button";

			// Token: 0x0400041D RID: 1053
			public readonly GUIStyle m_LODRendererButton = "LODRendererButton";

			// Token: 0x0400041E RID: 1054
			public readonly GUIStyle m_LODRendererAddButton = "LODRendererAddButton";

			// Token: 0x0400041F RID: 1055
			public readonly GUIStyle m_LODRendererRemove = "LODRendererRemove";

			// Token: 0x04000420 RID: 1056
			public readonly GUIStyle m_LODBlackBox = "LODBlackBox";

			// Token: 0x04000421 RID: 1057
			public readonly GUIContent m_IconRendererMinus = EditorGUIUtility.IconContent("Toolbar Minus", "|Remove Renderer");
		}
	}
}
