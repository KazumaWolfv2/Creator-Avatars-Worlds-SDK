using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class ShowReferenceShadersWindow : EditorWindow
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000B1E8 File Offset: 0x000093E8
	private Rect multiColumnTreeViewRect
	{
		get
		{
			return new Rect(20f, 30f, base.position.width - 40f, base.position.height - 60f);
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000B22C File Offset: 0x0000942C
	private Rect toolbarRect
	{
		get
		{
			return new Rect(20f, 10f, base.position.width - 40f, 20f);
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000B264 File Offset: 0x00009464
	private Rect bottomToolbarRect
	{
		get
		{
			return new Rect(20f, base.position.height - 18f, base.position.width - 40f, 16f);
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000B2A8 File Offset: 0x000094A8
	public static void ShowWindow()
	{
		ShowReferenceShadersWindow window = EditorWindow.GetWindow<ShowReferenceShadersWindow>();
		window.CollectReferenceShaders();
		window.InitIfNeeded();
		window.titleContent = new GUIContent("Reference Shaders");
		window.Focus();
		window.Repaint();
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000B2D8 File Offset: 0x000094D8
	private void InitIfNeeded()
	{
		if (!this.initialized)
		{
			if (this.treeViewState == null)
			{
				this.treeViewState = new TreeViewState();
			}
			bool flag = this.multiColumnHeaderState == null;
			MultiColumnHeaderState multiColumnHeaderState = CustomTreeView.CreateDefaultMultiColumnHeaderState(this.multiColumnTreeViewRect.width, this.menuTilte);
			if (MultiColumnHeaderState.CanOverwriteSerializedFields(this.multiColumnHeaderState, multiColumnHeaderState))
			{
				MultiColumnHeaderState.OverwriteSerializedFields(this.multiColumnHeaderState, multiColumnHeaderState);
			}
			this.multiColumnHeaderState = multiColumnHeaderState;
			MyMultiColumnHeader myMultiColumnHeader = new MyMultiColumnHeader(multiColumnHeaderState);
			if (flag)
			{
				myMultiColumnHeader.ResizeToFit();
			}
			TreeModel<CustomTreeElement> treeModel = new TreeModel<CustomTreeElement>(this.GenerateReferenceShaderTreeData());
			this.treeView = new CustomTreeView(this.treeViewState, myMultiColumnHeader, treeModel);
			this.searchField = new SearchField();
			this.searchField.downOrUpArrowKeyPressed += new SearchField.SearchFieldCallback(this.treeView.SetFocusAndEnsureSelectedItem);
			this.initialized = true;
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000B3A4 File Offset: 0x000095A4
	private IList<CustomTreeElement> GenerateReferenceShaderTreeData()
	{
		int num = 1;
		List<CustomTreeElement> list = new List<CustomTreeElement>();
		CustomTreeElement customTreeElement = new CustomTreeElement("root", -1, 0);
		list.Add(customTreeElement);
		foreach (KeyValuePair<Shader, ShowReferenceShadersWindow.ReferenceShader> keyValuePair in ShowReferenceShadersWindow.activeShaders)
		{
			list.Add(new CustomTreeElement(keyValuePair.Key.name, 0, num++)
			{
				material = null,
				refObject = null
			});
			List<ShowReferenceShadersWindow.ReferenceShaderGameObject> list2 = null;
			if (ShowReferenceShadersWindow.referenceShaderObjects.TryGetValue(keyValuePair.Value, out list2))
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list.Add(new CustomTreeElement(list2[i].material.name, 1, num++)
					{
						material = list2[i].material,
						refObject = list2[i].refObject
					});
				}
			}
		}
		return list;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00003394 File Offset: 0x00001594
	private void OnSelectionChange()
	{
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000B4C4 File Offset: 0x000096C4
	private void OnGUI()
	{
		this.InitIfNeeded();
		this.SearchBar(this.toolbarRect);
		this.DoTreeView(this.multiColumnTreeViewRect);
		this.BottomToolBar(this.bottomToolbarRect);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000B4F0 File Offset: 0x000096F0
	private void SearchBar(Rect rect)
	{
		this.treeView.searchString = this.searchField.OnGUI(rect, this.treeView.searchString);
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000B514 File Offset: 0x00009714
	private void DoTreeView(Rect rect)
	{
		this.treeView.OnGUI(rect);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000B524 File Offset: 0x00009724
	public void CollectReferenceShaders()
	{
		ShowReferenceShadersWindow.activeShaders.Clear();
		ShowReferenceShadersWindow.referenceShaderObjects.Clear();
		foreach (Renderer renderer in Object.FindObjectsOfType<Renderer>(true))
		{
			if (!(renderer == null))
			{
				GameObject gameObject = renderer.gameObject;
				foreach (Material material in renderer.sharedMaterials)
				{
					if (!(material == null) && !(material.shader == null))
					{
						ShowReferenceShadersWindow.ReferenceShader referenceShader;
						List<ShowReferenceShadersWindow.ReferenceShaderGameObject> list2;
						if (!ShowReferenceShadersWindow.activeShaders.TryGetValue(material.shader, out referenceShader))
						{
							ShowReferenceShadersWindow.ReferenceShaderGameObject referenceShaderGameObject = new ShowReferenceShadersWindow.ReferenceShaderGameObject();
							referenceShaderGameObject.material = material;
							referenceShaderGameObject.refObject = gameObject;
							referenceShader = new ShowReferenceShadersWindow.ReferenceShader();
							List<ShowReferenceShadersWindow.ReferenceShaderGameObject> list = new List<ShowReferenceShadersWindow.ReferenceShaderGameObject>();
							list.Add(referenceShaderGameObject);
							ShowReferenceShadersWindow.referenceShaderObjects.Add(referenceShader, list);
							ShowReferenceShadersWindow.activeShaders.Add(material.shader, referenceShader);
						}
						else if (ShowReferenceShadersWindow.referenceShaderObjects.TryGetValue(referenceShader, out list2))
						{
							ShowReferenceShadersWindow.ReferenceShaderGameObject referenceShaderGameObject2 = new ShowReferenceShadersWindow.ReferenceShaderGameObject();
							referenceShaderGameObject2.material = material;
							referenceShaderGameObject2.refObject = gameObject;
							list2.Add(referenceShaderGameObject2);
						}
					}
				}
			}
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000B16C File Offset: 0x0000936C
	private void SelectObject(Object selectedObject)
	{
		Selection.activeObject = selectedObject;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000B658 File Offset: 0x00009858
	private void BottomToolBar(Rect rect)
	{
		GUILayout.BeginArea(rect);
		using (new EditorGUILayout.HorizontalScope(Array.Empty<GUILayoutOption>()))
		{
			string text = "miniButton";
			if (GUILayout.Button("Expand All", text, Array.Empty<GUILayoutOption>()))
			{
				this.treeView.ExpandAll();
			}
			if (GUILayout.Button("Collapse All", text, Array.Empty<GUILayoutOption>()))
			{
				this.treeView.CollapseAll();
			}
			GUILayout.Space(10f);
		}
		GUILayout.EndArea();
	}

	// Token: 0x0400010E RID: 270
	public static Dictionary<Shader, ShowReferenceShadersWindow.ReferenceShader> activeShaders = new Dictionary<Shader, ShowReferenceShadersWindow.ReferenceShader>();

	// Token: 0x0400010F RID: 271
	public static Dictionary<ShowReferenceShadersWindow.ReferenceShader, List<ShowReferenceShadersWindow.ReferenceShaderGameObject>> referenceShaderObjects = new Dictionary<ShowReferenceShadersWindow.ReferenceShader, List<ShowReferenceShadersWindow.ReferenceShaderGameObject>>();

	// Token: 0x04000110 RID: 272
	[NonSerialized]
	private bool initialized;

	// Token: 0x04000111 RID: 273
	[SerializeField]
	private TreeViewState treeViewState;

	// Token: 0x04000112 RID: 274
	[SerializeField]
	private MultiColumnHeaderState multiColumnHeaderState;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private CustomTreeView treeView;

	// Token: 0x04000114 RID: 276
	private SearchField searchField;

	// Token: 0x04000115 RID: 277
	private string[] menuTilte = new string[] { "Asset", "Name", "Material", "GameObject" };

	// Token: 0x02000094 RID: 148
	public class ReferenceShader
	{
		// Token: 0x04000335 RID: 821
		public Shader shader;

		// Token: 0x04000336 RID: 822
		public bool isExpand;
	}

	// Token: 0x02000095 RID: 149
	public class ReferenceShaderGameObject
	{
		// Token: 0x04000337 RID: 823
		public GameObject refObject;

		// Token: 0x04000338 RID: 824
		public Material material;
	}
}
