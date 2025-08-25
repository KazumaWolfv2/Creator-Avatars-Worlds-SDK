using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000085 RID: 133
	[CustomEditor(typeof(ParaImpostorBakePreset))]
	public class ParaImpostorBakePresetEditor : Editor
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x0001FB4C File Offset: 0x0001DD4C
		public void OnEnable()
		{
			this.instance = (ParaImpostorBakePreset)base.target;
			ImpostorBakingTools.LoadDefaults();
			this.AlphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
			this.AlphaIcon.tooltip = "Alpha output selection";
			this.AddList();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001FB8A File Offset: 0x0001DD8A
		private void OnDisable()
		{
			this.RemoveList();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001FB94 File Offset: 0x0001DD94
		private void RemoveList()
		{
			ReorderableList reorderableOutput = this.m_reorderableOutput;
			reorderableOutput.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate)Delegate.Remove(reorderableOutput.drawHeaderCallback, new ReorderableList.HeaderCallbackDelegate(this.DrawHeader));
			ReorderableList reorderableOutput2 = this.m_reorderableOutput;
			reorderableOutput2.drawElementCallback = (ReorderableList.ElementCallbackDelegate)Delegate.Remove(reorderableOutput2.drawElementCallback, new ReorderableList.ElementCallbackDelegate(this.DrawElement));
			ReorderableList reorderableOutput3 = this.m_reorderableOutput;
			reorderableOutput3.onAddCallback = (ReorderableList.AddCallbackDelegate)Delegate.Remove(reorderableOutput3.onAddCallback, new ReorderableList.AddCallbackDelegate(this.AddItem));
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001FC18 File Offset: 0x0001DE18
		private void AddList()
		{
			this.m_usingStandard = false;
			if (this.instance.BakeShader == null)
			{
				this.m_usingStandard = true;
			}
			this.m_reorderableOutput = new ReorderableList(this.instance.Output, typeof(TextureOutput), !this.m_usingStandard, true, !this.m_usingStandard, !this.m_usingStandard);
			ReorderableList reorderableOutput = this.m_reorderableOutput;
			reorderableOutput.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate)Delegate.Combine(reorderableOutput.drawHeaderCallback, new ReorderableList.HeaderCallbackDelegate(this.DrawHeader));
			ReorderableList reorderableOutput2 = this.m_reorderableOutput;
			reorderableOutput2.drawElementCallback = (ReorderableList.ElementCallbackDelegate)Delegate.Combine(reorderableOutput2.drawElementCallback, new ReorderableList.ElementCallbackDelegate(this.DrawElement));
			ReorderableList reorderableOutput3 = this.m_reorderableOutput;
			reorderableOutput3.onAddCallback = (ReorderableList.AddCallbackDelegate)Delegate.Combine(reorderableOutput3.onAddCallback, new ReorderableList.AddCallbackDelegate(this.AddItem));
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001FCF7 File Offset: 0x0001DEF7
		private void RefreshList()
		{
			this.RemoveList();
			this.AddList();
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001FD08 File Offset: 0x0001DF08
		private void DrawHeader(Rect rect)
		{
			rect.xMax -= 20f;
			Rect rect2 = rect;
			rect2.width = 24f;
			rect2.x = rect.xMax;
			rect2.height = 24f;
			rect.xMax -= 35f;
			Rect rect3 = rect;
			rect3.width = 32f;
			EditorGUI.LabelField(rect3, ParaImpostorBakePresetEditor.TargetsStr);
			rect3 = rect;
			rect3.xMin += (float)(32 + (this.m_usingStandard ? 0 : 13));
			rect3.width = EditorGUIUtility.labelWidth - rect3.xMin + 13f;
			EditorGUI.LabelField(rect3, ParaImpostorBakePresetEditor.SuffixStr);
			Rect rect4 = rect;
			rect4.xMin = EditorGUIUtility.labelWidth + 13f;
			float width = rect4.width;
			rect4.width = width * 0.25f;
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.TexScaleStr);
			rect4.x += rect4.width;
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.ChannelsStr);
			rect4.x += rect4.width;
			rect4.width = 35f;
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.ColorSpaceStr);
			rect4.x += rect4.width;
			rect4.width = width * 0.25f;
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.CompressionStr);
			rect4.x += rect4.width;
			EditorGUI.LabelField(rect4, ParaImpostorBakePresetEditor.FormatStr);
			EditorGUI.LabelField(rect2, this.AlphaIcon);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
		private void DrawElement(Rect rect, int index, bool active, bool focused)
		{
			rect.y += 1f;
			Rect rect2 = rect;
			rect2.height = EditorGUIUtility.singleLineHeight;
			rect2.width = 20f;
			rect2.x = rect.xMax - rect2.width;
			rect.xMax -= rect2.width + 35f;
			Rect rect3 = rect;
			rect3.width = 16f;
			rect3.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(rect3, new GUIContent(index.ToString()));
			rect.height = EditorGUIUtility.singleLineHeight;
			Rect rect4 = rect;
			rect4.x = rect3.xMax;
			rect4.width = 16f;
			this.instance.Output[index].Active = EditorGUI.Toggle(rect4, this.instance.Output[index].Active);
			rect.y += 1f;
			EditorGUI.BeginDisabledGroup(!this.instance.Output[index].Active);
			Rect rect5 = rect;
			rect5.x = rect4.xMax;
			rect5.width = EditorGUIUtility.labelWidth - 32f - (float)(this.m_usingStandard ? 5 : 19);
			this.instance.Output[index].Name = EditorGUI.TextField(rect5, this.instance.Output[index].Name);
			Rect rect6 = rect;
			rect6.xMin = rect5.xMax;
			float width = rect6.width;
			rect6.width = width * 0.25f;
			this.instance.Output[index].Scale = EditorGUI.IntPopup(rect6, this.instance.Output[index].Scale, ParaImpostorBakePresetEditor.TexScaleListStr, ParaImpostorBakePresetEditor.TexScaleOpt);
			rect6.x += rect6.width;
			this.instance.Output[index].Channels = (TextureChannels)EditorGUI.EnumPopup(rect6, this.instance.Output[index].Channels);
			rect6.x += rect6.width + 10f;
			rect6.width = 35f;
			rect6.y -= 1f;
			this.instance.Output[index].SRGB = EditorGUI.Toggle(rect6, this.instance.Output[index].SRGB);
			rect6.y += 1f;
			rect6.x += rect6.width - 10f;
			rect6.width = width * 0.25f;
			this.instance.Output[index].Compression = EditorGUI.IntPopup(rect6, this.instance.Output[index].Compression, ParaImpostorBakePresetEditor.CompressionListStr, ParaImpostorBakePresetEditor.CompressionOpt);
			rect6.x += rect6.width;
			this.instance.Output[index].ImageFormat = (ImageFormat)EditorGUI.EnumPopup(rect6, this.instance.Output[index].ImageFormat);
			EditorGUI.EndDisabledGroup();
			rect2.xMin += 4f;
			this.instance.AlphaIndex = (EditorGUI.Toggle(rect2, this.instance.AlphaIndex == index, "radio") ? index : this.instance.AlphaIndex);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00020267 File Offset: 0x0001E467
		private void AddItem(ReorderableList reordableList)
		{
			reordableList.list.Add(new TextureOutput());
			EditorUtility.SetDirty(base.target);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00020288 File Offset: 0x0001E488
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			this.instance.BakeShader = EditorGUILayout.ObjectField(ParaImpostorBakePresetEditor.BakeShaderStr, this.instance.BakeShader, typeof(Shader), false, Array.Empty<GUILayoutOption>()) as Shader;
			this.instance.RuntimeShader = EditorGUILayout.ObjectField(ParaImpostorBakePresetEditor.RuntimeShaderStr, this.instance.RuntimeShader, typeof(Shader), false, Array.Empty<GUILayoutOption>()) as Shader;
			this.m_usingStandard = this.instance.BakeShader == null;
			bool flag = false;
			if (EditorGUI.EndChangeCheck())
			{
				flag = true;
			}
			if (flag || (this.m_usingStandard && this.instance.Output.Count == 0))
			{
				if (this.m_usingStandard)
				{
					this.instance.Output.Clear();
					this.instance.Output = new List<TextureOutput>
					{
						new TextureOutput(true, ImpostorBakingTools.GlobalAlbedoAlpha, 1, true, 0, 1, 1),
						new TextureOutput(true, ImpostorBakingTools.GlobalSpecularSmoothness, 1, true, 0, 1, 1),
						new TextureOutput(true, ImpostorBakingTools.GlobalNormalDepth, 1, false, 0, 1, 1),
						new TextureOutput(true, ImpostorBakingTools.GlobalEmissionOcclusion, 1, false, 0, 1, 1)
					};
				}
				this.RefreshList();
				base.Repaint();
			}
			this.m_reorderableOutput.DoLayoutList();
			if (GUI.changed)
			{
				EditorUtility.SetDirty(this.instance);
			}
		}

		// Token: 0x040002C0 RID: 704
		private ParaImpostorBakePreset instance;

		// Token: 0x040002C1 RID: 705
		private ReorderableList m_reorderableOutput;

		// Token: 0x040002C2 RID: 706
		private bool m_usingStandard;

		// Token: 0x040002C3 RID: 707
		public static readonly GUIContent BakeShaderStr = new GUIContent("Bake Shader", "Shader used to bake the different outputs");

		// Token: 0x040002C4 RID: 708
		public static readonly GUIContent RuntimeShaderStr = new GUIContent("Runtime Shader", "Custom impostor shader to assign the outputs to");

		// Token: 0x040002C5 RID: 709
		public static readonly GUIContent PipelineStr = new GUIContent("Pipeline", "Defines the default preset for the selected pipeline");

		// Token: 0x040002C6 RID: 710
		public static readonly GUIContent TargetsStr = new GUIContent("RT#", "Render Target number");

		// Token: 0x040002C7 RID: 711
		public static readonly GUIContent SuffixStr = new GUIContent("Suffix", "Name suffix for file saving and for material assignment");

		// Token: 0x040002C8 RID: 712
		public static readonly int[] TexScaleOpt = new int[] { 1, 2, 4, 8 };

		// Token: 0x040002C9 RID: 713
		public static readonly GUIContent[] TexScaleListStr = new GUIContent[]
		{
			new GUIContent("1"),
			new GUIContent("1⁄2"),
			new GUIContent("1⁄4"),
			new GUIContent("1⁄8")
		};

		// Token: 0x040002CA RID: 714
		public static readonly GUIContent TexScaleStr = new GUIContent("Scale", "Texture Scaling");

		// Token: 0x040002CB RID: 715
		public static readonly GUIContent ColorSpaceStr = new GUIContent("sRGB", "Texture color space");

		// Token: 0x040002CC RID: 716
		public static readonly GUIContent[] ColorSpaceListStr = new GUIContent[]
		{
			new GUIContent("sRGB"),
			new GUIContent("Linear")
		};

		// Token: 0x040002CD RID: 717
		public static readonly int[] CompressionOpt = new int[] { 0, 3, 1, 2 };

		// Token: 0x040002CE RID: 718
		public static readonly GUIContent[] CompressionListStr = new GUIContent[]
		{
			new GUIContent("None"),
			new GUIContent("Low"),
			new GUIContent("Normal"),
			new GUIContent("High")
		};

		// Token: 0x040002CF RID: 719
		public static readonly GUIContent CompressionStr = new GUIContent("Compression", "Compression quality");

		// Token: 0x040002D0 RID: 720
		public static readonly GUIContent FormatStr = new GUIContent("Format", "File save format");

		// Token: 0x040002D1 RID: 721
		public static readonly GUIContent ChannelsStr = new GUIContent("Channels", "Channels being used");

		// Token: 0x040002D2 RID: 722
		public GUIContent AlphaIcon;

		// Token: 0x040002D3 RID: 723
		public static readonly GUIContent OverrideStr = new GUIContent("Override", "Override");
	}
}
