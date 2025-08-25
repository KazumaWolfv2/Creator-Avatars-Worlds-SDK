using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class ShowResourceSizeWindow : EditorWindow
{
	// Token: 0x060001B1 RID: 433 RVA: 0x0000B736 File Offset: 0x00009936
	public static void ShowWindow()
	{
		ShowResourceSizeWindow window = EditorWindow.GetWindow<ShowResourceSizeWindow>();
		window.titleContent = new GUIContent(SdkLangManager.Get("Check Resource Size"));
		window.CollectReferenceMeshs();
		window.InitIfNeeded();
		window.Focus();
		window.Repaint();
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000B76C File Offset: 0x0000996C
	private Rect multiColumnTreeViewRect
	{
		get
		{
			return new Rect(20f, 30f, base.position.width - 40f, base.position.height - 60f);
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000B7B0 File Offset: 0x000099B0
	private Rect toolbarRect
	{
		get
		{
			return new Rect(20f, 10f, base.position.width - 40f, 20f);
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000B7E5 File Offset: 0x000099E5
	private void OnGUI()
	{
		this.InitIfNeeded();
		this.SearchBar(this.toolbarRect);
		this.DoTreeView(this.multiColumnTreeViewRect);
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000B805 File Offset: 0x00009A05
	private void SearchBar(Rect rect)
	{
		this.treeView.searchString = this.searchField.OnGUI(rect, this.treeView.searchString);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000B829 File Offset: 0x00009A29
	private void DoTreeView(Rect rect)
	{
		this.treeView.OnGUI(rect);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000B838 File Offset: 0x00009A38
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
			TreeModel<CustomTreeElement> treeModel = new TreeModel<CustomTreeElement>(this.GenerateResourceSizeTreeData());
			this.treeView = new CustomTreeView(this.treeViewState, myMultiColumnHeader, treeModel);
			this.searchField = new SearchField();
			this.searchField.downOrUpArrowKeyPressed += new SearchField.SearchFieldCallback(this.treeView.SetFocusAndEnsureSelectedItem);
			this.initialized = true;
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000B904 File Offset: 0x00009B04
	private IList<CustomTreeElement> GenerateResourceSizeTreeData()
	{
		List<CustomTreeElement> list = new List<CustomTreeElement>();
		CustomTreeElement customTreeElement = new CustomTreeElement("root", -1, 0);
		list.Add(customTreeElement);
		int num = 1;
		foreach (KeyValuePair<string, ShowResourceSizeWindow.ReferenceResource> keyValuePair in ShowResourceSizeWindow.referenceMeshs)
		{
			list.Add(new CustomTreeElement(keyValuePair.Value.resMesh.name, 0, num++)
			{
				material = null,
				refObject = null,
				text = (keyValuePair.Value.size / 1024L).ToString() + " KB",
				mesh = keyValuePair.Value.resMesh,
				Size = keyValuePair.Value.size
			});
		}
		return list;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000B9FC File Offset: 0x00009BFC
	public void CollectReferenceMeshs()
	{
		ShowResourceSizeWindow.referenceMeshs.Clear();
		foreach (Renderer renderer in Object.FindObjectsOfType<Renderer>(true))
		{
			if (renderer is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
				{
					string text = AssetDatabase.GetAssetPath(skinnedMeshRenderer.sharedMesh);
					text = text.Replace("Assets", "");
					string text2 = Application.dataPath + "/" + text;
					if (File.Exists(text2))
					{
						ShowResourceSizeWindow.ReferenceResource referenceResource = new ShowResourceSizeWindow.ReferenceResource();
						FileInfo fileInfo = new FileInfo(text2);
						referenceResource.resMesh = skinnedMeshRenderer.sharedMesh;
						referenceResource.size = fileInfo.Length;
						if (!ShowResourceSizeWindow.referenceMeshs.ContainsKey(text2))
						{
							ShowResourceSizeWindow.referenceMeshs.Add(text2, referenceResource);
						}
					}
				}
			}
			else if (renderer is MeshRenderer)
			{
				MeshRenderer meshRenderer = renderer as MeshRenderer;
				if (meshRenderer != null)
				{
					MeshFilter component = meshRenderer.gameObject.GetComponent<MeshFilter>();
					if (component != null && component.sharedMesh != null)
					{
						string text3 = AssetDatabase.GetAssetPath(component.sharedMesh);
						text3 = text3.Replace("Assets", "");
						string text4 = Application.dataPath + "/" + text3;
						if (File.Exists(text4))
						{
							ShowResourceSizeWindow.ReferenceResource referenceResource2 = new ShowResourceSizeWindow.ReferenceResource();
							FileInfo fileInfo2 = new FileInfo(text4);
							referenceResource2.resMesh = component.sharedMesh;
							referenceResource2.size = fileInfo2.Length;
							if (!ShowResourceSizeWindow.referenceMeshs.ContainsKey(text4))
							{
								ShowResourceSizeWindow.referenceMeshs.Add(text4, referenceResource2);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x04000116 RID: 278
	[NonSerialized]
	private bool initialized;

	// Token: 0x04000117 RID: 279
	[SerializeField]
	private TreeViewState treeViewState;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	private MultiColumnHeaderState multiColumnHeaderState;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private CustomTreeView treeView;

	// Token: 0x0400011A RID: 282
	private SearchField searchField;

	// Token: 0x0400011B RID: 283
	private string[] menuTilte = new string[] { "Asset", "Mesh", "Size", "GameObject" };

	// Token: 0x0400011C RID: 284
	public static Dictionary<string, ShowResourceSizeWindow.ReferenceResource> referenceMeshs = new Dictionary<string, ShowResourceSizeWindow.ReferenceResource>();

	// Token: 0x02000096 RID: 150
	public enum CheckResType
	{
		// Token: 0x0400033A RID: 826
		Mesh,
		// Token: 0x0400033B RID: 827
		Texture
	}

	// Token: 0x02000097 RID: 151
	public class ReferenceResource
	{
		// Token: 0x0400033C RID: 828
		public ShowResourceSizeWindow.CheckResType resType;

		// Token: 0x0400033D RID: 829
		public Mesh resMesh;

		// Token: 0x0400033E RID: 830
		public long size;
	}
}
