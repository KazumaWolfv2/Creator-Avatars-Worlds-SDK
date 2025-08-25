using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

// Token: 0x0200003E RID: 62
internal class CustomTreeView : TreeViewWithTreeModel<CustomTreeElement>
{
	// Token: 0x060001BD RID: 445 RVA: 0x0000BC20 File Offset: 0x00009E20
	public static void TreeToList(TreeViewItem root, IList<TreeViewItem> result)
	{
		if (root == null)
		{
			throw new NullReferenceException("root");
		}
		if (result == null)
		{
			throw new NullReferenceException("result");
		}
		result.Clear();
		if (root.children == null)
		{
			return;
		}
		Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
		for (int i = root.children.Count - 1; i >= 0; i--)
		{
			stack.Push(root.children[i]);
		}
		while (stack.Count > 0)
		{
			TreeViewItem treeViewItem = stack.Pop();
			result.Add(treeViewItem);
			if (treeViewItem.hasChildren && treeViewItem.children[0] != null)
			{
				for (int j = treeViewItem.children.Count - 1; j >= 0; j--)
				{
					stack.Push(treeViewItem.children[j]);
				}
			}
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000BCE4 File Offset: 0x00009EE4
	public CustomTreeView(TreeViewState treeViewState, MultiColumnHeader multicolumnHeader, TreeModel<CustomTreeElement> model)
		: base(treeViewState, multicolumnHeader, model)
	{
		base.rowHeight = 20f;
		base.columnIndexForTreeFoldouts = 1;
		base.showAlternatingRowBackgrounds = true;
		base.showBorder = true;
		base.customFoldoutYOffset = (20f - EditorGUIUtility.singleLineHeight) * 0.5f;
		base.extraSpaceBeforeIconAndLabel = 18f;
		multicolumnHeader.sortingChanged += new MultiColumnHeader.HeaderCallback(this.OnSortingChanged);
		base.Reload();
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000BD5C File Offset: 0x00009F5C
	protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
	{
		IList<TreeViewItem> list = base.BuildRows(root);
		this.SortIfNeeded(root, list);
		return list;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000BD7A File Offset: 0x00009F7A
	private int GetIcon1Index(TreeViewItem<CustomTreeElement> item)
	{
		return Mathf.Min(item.data.text.Length, CustomTreeView.s_TestIcons.Length - 1);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000BD9C File Offset: 0x00009F9C
	protected override void RowGUI(TreeView.RowGUIArgs args)
	{
		TreeViewItem<CustomTreeElement> treeViewItem = (TreeViewItem<CustomTreeElement>)args.item;
		for (int i = 0; i < args.GetNumVisibleColumns(); i++)
		{
			this.CellGUI(args.GetCellRect(i), treeViewItem, (CustomTreeView.MyColumns)args.GetColumn(i), ref args);
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000BDE0 File Offset: 0x00009FE0
	private void CellGUI(Rect cellRect, TreeViewItem<CustomTreeElement> item, CustomTreeView.MyColumns column, ref TreeView.RowGUIArgs args)
	{
		base.CenterRectUsingSingleLineHeight(ref cellRect);
		switch (column)
		{
		case CustomTreeView.MyColumns.Name:
		{
			Rect rect = cellRect;
			rect.x += base.GetContentIndent(item);
			rect.width = 18f;
			if (rect.xMax < cellRect.xMax)
			{
				item.data.enabled = EditorGUI.Toggle(rect, item.data.enabled);
			}
			args.rowRect = cellRect;
			base.RowGUI(args);
			return;
		}
		case CustomTreeView.MyColumns.Material:
			if (item.data.material != null)
			{
				EditorGUI.ObjectField(cellRect, GUIContent.none, item.data.material, typeof(Material), false);
				if (GUI.Button(cellRect, "Find"))
				{
					this.SelectObject(item.data.material);
				}
			}
			if (!string.IsNullOrEmpty(item.data.text))
			{
				GUI.enabled = false;
				EditorGUI.TextField(cellRect, item.data.text);
				GUI.enabled = true;
				return;
			}
			break;
		case CustomTreeView.MyColumns.GameObject:
			if (item.data.refObject != null && GUI.Button(cellRect, "Find In Scene"))
			{
				this.SelectObject(item.data.refObject);
			}
			if (item.data.mesh != null && GUI.Button(cellRect, "Find"))
			{
				this.SelectObject(item.data.mesh);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000B16C File Offset: 0x0000936C
	private void SelectObject(Object selectedObject)
	{
		Selection.activeObject = selectedObject;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000BF58 File Offset: 0x0000A158
	private void OnSortingChanged(MultiColumnHeader multiColumnHeader)
	{
		this.SortIfNeeded(base.rootItem, this.GetRows());
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000BF6C File Offset: 0x0000A16C
	private void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
	{
		if (rows.Count <= 1)
		{
			return;
		}
		if (base.multiColumnHeader.sortedColumnIndex == -1)
		{
			return;
		}
		this.SortByMultipleColumns();
		CustomTreeView.TreeToList(root, rows);
		base.Repaint();
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000BF9C File Offset: 0x0000A19C
	private void SortByMultipleColumns()
	{
		int[] sortedColumns = base.multiColumnHeader.state.sortedColumns;
		if (sortedColumns.Length == 0)
		{
			return;
		}
		IEnumerable<TreeViewItem<CustomTreeElement>> enumerable = base.rootItem.children.Cast<TreeViewItem<CustomTreeElement>>();
		IOrderedEnumerable<TreeViewItem<CustomTreeElement>> orderedEnumerable = this.InitialOrder(enumerable, sortedColumns);
		base.rootItem.children = orderedEnumerable.Cast<TreeViewItem>().ToList<TreeViewItem>();
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
	private IOrderedEnumerable<TreeViewItem<CustomTreeElement>> InitialOrder(IEnumerable<TreeViewItem<CustomTreeElement>> myTypes, int[] history)
	{
		CustomTreeView.SortOption sortOption = CustomTreeView.SortOption.Name;
		if (base.multiColumnHeader.sortedColumnIndex == 2)
		{
			sortOption = CustomTreeView.SortOption.Size;
		}
		bool flag = base.multiColumnHeader.IsSortedAscending(history[0]);
		if (sortOption == CustomTreeView.SortOption.Name)
		{
			return myTypes.Order((TreeViewItem<CustomTreeElement> l) => l.data.name, flag);
		}
		if (sortOption != CustomTreeView.SortOption.Size)
		{
			Assert.IsTrue(false, "Unhandled enum");
			return myTypes.Order((TreeViewItem<CustomTreeElement> l) => l.data.Size, flag);
		}
		return myTypes.Order((TreeViewItem<CustomTreeElement> l) => l.data.Size, flag);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
	public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth, string[] menuName)
	{
		MultiColumnHeaderState.Column[] array = new MultiColumnHeaderState.Column[]
		{
			new MultiColumnHeaderState.Column
			{
				headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByLabel"), ""),
				contextMenuText = menuName[0],
				headerTextAlignment = 1,
				sortedAscending = true,
				sortingArrowAlignment = 2,
				width = 16f,
				minWidth = 16f,
				maxWidth = 32f,
				autoResize = false,
				allowToggleVisibility = true
			},
			new MultiColumnHeaderState.Column
			{
				headerContent = new GUIContent(menuName[1]),
				headerTextAlignment = 0,
				sortedAscending = true,
				sortingArrowAlignment = 1,
				width = 250f,
				minWidth = 60f,
				autoResize = false,
				allowToggleVisibility = false
			},
			new MultiColumnHeaderState.Column
			{
				headerContent = new GUIContent(menuName[2]),
				headerTextAlignment = 0,
				sortedAscending = true,
				sortingArrowAlignment = 0,
				width = 125f,
				minWidth = 60f,
				autoResize = true,
				allowToggleVisibility = true
			},
			new MultiColumnHeaderState.Column
			{
				headerContent = new GUIContent(menuName[3]),
				headerTextAlignment = 0,
				sortedAscending = true,
				sortingArrowAlignment = 0,
				width = 125f,
				minWidth = 60f,
				autoResize = true,
				allowToggleVisibility = true
			}
		};
		Assert.AreEqual(array.Length, Enum.GetValues(typeof(CustomTreeView.MyColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");
		return new MultiColumnHeaderState(array);
	}

	// Token: 0x04000124 RID: 292
	private const float kRowHeights = 20f;

	// Token: 0x04000125 RID: 293
	private const float kToggleWidth = 18f;

	// Token: 0x04000126 RID: 294
	public bool showControls = true;

	// Token: 0x04000127 RID: 295
	private static Texture2D[] s_TestIcons = new Texture2D[]
	{
		EditorGUIUtility.FindTexture("Folder Icon"),
		EditorGUIUtility.FindTexture("AudioSource Icon"),
		EditorGUIUtility.FindTexture("Camera Icon"),
		EditorGUIUtility.FindTexture("Windzone Icon"),
		EditorGUIUtility.FindTexture("GameObject Icon")
	};

	// Token: 0x02000098 RID: 152
	private enum MyColumns
	{
		// Token: 0x04000340 RID: 832
		Icon1,
		// Token: 0x04000341 RID: 833
		Name,
		// Token: 0x04000342 RID: 834
		Material,
		// Token: 0x04000343 RID: 835
		GameObject
	}

	// Token: 0x02000099 RID: 153
	public enum SortOption
	{
		// Token: 0x04000345 RID: 837
		Name,
		// Token: 0x04000346 RID: 838
		Size
	}
}
