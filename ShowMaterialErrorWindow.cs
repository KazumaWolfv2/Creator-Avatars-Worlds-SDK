using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Token: 0x0200003A RID: 58
[ExecuteAlways]
public class ShowMaterialErrorWindow : EditorWindow
{
	// Token: 0x0600019B RID: 411 RVA: 0x0000AF08 File Offset: 0x00009108
	private void OnGUI()
	{
		GUILayout.BeginHorizontal(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
		GUILayout.Label("Materials : " + this.ActiveMaterials.Count.ToString(), Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		if (this.ActiveInspectType == ShowMaterialErrorWindow.InspectType.MissingMaterial)
		{
			this.ShowMissingMaterials();
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000AF60 File Offset: 0x00009160
	public void CorrectionErrorMaterials()
	{
		this.ActiveMaterials.Clear();
		foreach (Renderer renderer in Object.FindObjectsOfType<Renderer>())
		{
			if (!(renderer == null))
			{
				GameObject gameObject = renderer.gameObject;
				foreach (Material material in renderer.sharedMaterials)
				{
					if (!(material == null) && this.FindMissingMaterial(material) == null && this.ShaderHasError(material.shader))
					{
						MissingMaterial missingMaterial = new MissingMaterial();
						missingMaterial.mat = material;
						missingMaterial.refObject = gameObject;
						this.ActiveMaterials.Add(missingMaterial);
					}
				}
			}
		}
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000B010 File Offset: 0x00009210
	private MissingMaterial FindMissingMaterial(Material tMaterial)
	{
		foreach (MissingMaterial missingMaterial in this.ActiveMaterials)
		{
			if (missingMaterial.mat == tMaterial)
			{
				return missingMaterial;
			}
		}
		return null;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000B074 File Offset: 0x00009274
	private void ShowMissingMaterials()
	{
		this.missingMaterialListScrollPos = EditorGUILayout.BeginScrollView(this.missingMaterialListScrollPos, Array.Empty<GUILayoutOption>());
		foreach (MissingMaterial missingMaterial in this.ActiveMaterials)
		{
			if (missingMaterial != null)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox, Array.Empty<GUILayoutOption>());
				GUILayout.Box(AssetPreview.GetAssetPreview(missingMaterial.mat), new GUILayoutOption[]
				{
					GUILayout.Width(this.ThumbnailWidth),
					GUILayout.Height(this.ThumbnailHeight)
				});
				if (GUILayout.Button(missingMaterial.mat.name, new GUILayoutOption[]
				{
					GUILayout.Width(250f),
					GUILayout.Height(this.ThumbnailWidth)
				}))
				{
					this.SelectObject(missingMaterial.mat);
				}
				GUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndScrollView();
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000B16C File Offset: 0x0000936C
	private void SelectObject(Object selectedObject)
	{
		Selection.activeObject = selectedObject;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000B174 File Offset: 0x00009374
	private bool ShaderHasError(Shader shader)
	{
		if (shader.name == "Hidden/InternalErrorShader")
		{
			return true;
		}
		return ShaderUtil.GetShaderMessages(shader).Any((ShaderMessage x) => x.severity == 0);
	}

	// Token: 0x04000109 RID: 265
	private ShowMaterialErrorWindow.InspectType ActiveInspectType;

	// Token: 0x0400010A RID: 266
	private List<MissingMaterial> ActiveMaterials = new List<MissingMaterial>();

	// Token: 0x0400010B RID: 267
	private Vector2 missingMaterialListScrollPos = Vector2.zero;

	// Token: 0x0400010C RID: 268
	private float ThumbnailWidth = 40f;

	// Token: 0x0400010D RID: 269
	private float ThumbnailHeight = 40f;

	// Token: 0x02000092 RID: 146
	private enum InspectType
	{
		// Token: 0x04000330 RID: 816
		MissingMaterial,
		// Token: 0x04000331 RID: 817
		MissingGraphic,
		// Token: 0x04000332 RID: 818
		MissingPerfab
	}
}
