using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

// Token: 0x02000045 RID: 69
internal class TreeViewWithTreeModel<T> : TreeView where T : TreeElement
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060001FA RID: 506 RVA: 0x0000CF80 File Offset: 0x0000B180
	// (remove) Token: 0x060001FB RID: 507 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
	public event Action treeChanged;

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060001FC RID: 508 RVA: 0x0000CFED File Offset: 0x0000B1ED
	public TreeModel<T> treeModel
	{
		get
		{
			return this.m_TreeModel;
		}
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060001FD RID: 509 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
	// (remove) Token: 0x060001FE RID: 510 RVA: 0x0000D030 File Offset: 0x0000B230
	public event Action<IList<TreeViewItem>> beforeDroppingDraggedItems;

	// Token: 0x060001FF RID: 511 RVA: 0x0000D065 File Offset: 0x0000B265
	public TreeViewWithTreeModel(TreeViewState state, TreeModel<T> model)
		: base(state)
	{
		this.Init(model);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000D082 File Offset: 0x0000B282
	public TreeViewWithTreeModel(TreeViewState state, MultiColumnHeader multiColumnHeader, TreeModel<T> model)
		: base(state, multiColumnHeader)
	{
		this.Init(model);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
	private void Init(TreeModel<T> model)
	{
		this.m_TreeModel = model;
		this.m_TreeModel.modelChanged += this.ModelChanged;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
	private void ModelChanged()
	{
		if (this.treeChanged != null)
		{
			this.treeChanged();
		}
		base.Reload();
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000D0DC File Offset: 0x0000B2DC
	protected override TreeViewItem BuildRoot()
	{
		int num = -1;
		return new TreeViewItem<T>(this.m_TreeModel.root.id, num, this.m_TreeModel.root.name, this.m_TreeModel.root);
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000D128 File Offset: 0x0000B328
	protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
	{
		if (this.m_TreeModel.root == null)
		{
			Debug.LogError("tree model root is null. did you call SetData()?");
		}
		this.m_Rows.Clear();
		if (!string.IsNullOrEmpty(base.searchString))
		{
			this.Search(this.m_TreeModel.root, base.searchString, this.m_Rows);
		}
		else if (this.m_TreeModel.root.hasChildren)
		{
			this.AddChildrenRecursive(this.m_TreeModel.root, 0, this.m_Rows);
		}
		TreeView.SetupParentsAndChildrenFromDepths(root, this.m_Rows);
		return this.m_Rows;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000D1CC File Offset: 0x0000B3CC
	private void AddChildrenRecursive(T parent, int depth, IList<TreeViewItem> newRows)
	{
		foreach (TreeElement treeElement in parent.children)
		{
			T t = (T)((object)treeElement);
			TreeViewItem<T> treeViewItem = new TreeViewItem<T>(t.id, depth, t.name, t);
			newRows.Add(treeViewItem);
			if (t.hasChildren)
			{
				if (base.IsExpanded(t.id))
				{
					this.AddChildrenRecursive(t, depth + 1, newRows);
				}
				else
				{
					treeViewItem.children = TreeView.CreateChildListForCollapsedParent();
				}
			}
		}
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000D280 File Offset: 0x0000B480
	private void Search(T searchFromThis, string search, List<TreeViewItem> result)
	{
		if (string.IsNullOrEmpty(search))
		{
			throw new ArgumentException("Invalid search: cannot be null or empty", "search");
		}
		Stack<T> stack = new Stack<T>();
		using (List<TreeElement>.Enumerator enumerator = searchFromThis.children.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TreeElement treeElement = enumerator.Current;
				stack.Push((T)((object)treeElement));
			}
			goto IL_0102;
		}
		IL_0061:
		T t = stack.Pop();
		if (t.name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
		{
			result.Add(new TreeViewItem<T>(t.id, 0, t.name, t));
		}
		if (t.children != null && t.children.Count > 0)
		{
			foreach (TreeElement treeElement2 in t.children)
			{
				stack.Push((T)((object)treeElement2));
			}
		}
		IL_0102:
		if (stack.Count <= 0)
		{
			this.SortSearchResult(result);
			return;
		}
		goto IL_0061;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
	protected virtual void SortSearchResult(List<TreeViewItem> rows)
	{
		rows.Sort((TreeViewItem x, TreeViewItem y) => EditorUtility.NaturalCompare(x.displayName, y.displayName));
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000D3E7 File Offset: 0x0000B5E7
	protected override IList<int> GetAncestors(int id)
	{
		return this.m_TreeModel.GetAncestors(id);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000D3F5 File Offset: 0x0000B5F5
	protected override IList<int> GetDescendantsThatHaveChildren(int id)
	{
		return this.m_TreeModel.GetDescendantsThatHaveChildren(id);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x000088C8 File Offset: 0x00006AC8
	protected override bool CanStartDrag(TreeView.CanStartDragArgs args)
	{
		return true;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000D404 File Offset: 0x0000B604
	protected override void SetupDragAndDrop(TreeView.SetupDragAndDropArgs args)
	{
		if (base.hasSearch)
		{
			return;
		}
		DragAndDrop.PrepareStartDrag();
		List<TreeViewItem> list = (from item in this.GetRows()
			where args.draggedItemIDs.Contains(item.id)
			select item).ToList<TreeViewItem>();
		DragAndDrop.SetGenericData("GenericDragColumnDragging", list);
		DragAndDrop.objectReferences = new Object[0];
		DragAndDrop.StartDrag((list.Count == 1) ? list[0].displayName : "< Multiple >");
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000D480 File Offset: 0x0000B680
	protected override DragAndDropVisualMode HandleDragAndDrop(TreeView.DragAndDropArgs args)
	{
		List<TreeViewItem> list = DragAndDrop.GetGenericData("GenericDragColumnDragging") as List<TreeViewItem>;
		if (list == null)
		{
			return 0;
		}
		TreeView.DragAndDropPosition dragAndDropPosition = args.dragAndDropPosition;
		if (dragAndDropPosition > 1)
		{
			if (dragAndDropPosition != 2)
			{
				Debug.LogError("Unhandled enum " + args.dragAndDropPosition.ToString());
				return 0;
			}
			if (args.performDrop)
			{
				this.OnDropDraggedElementsAtIndex(list, this.m_TreeModel.root, this.m_TreeModel.root.children.Count);
			}
			return 16;
		}
		else
		{
			bool flag = this.ValidDrag(args.parentItem, list);
			if (args.performDrop && flag)
			{
				T data = ((TreeViewItem<T>)args.parentItem).data;
				this.OnDropDraggedElementsAtIndex(list, data, (args.insertAtIndex == -1) ? 0 : args.insertAtIndex);
			}
			if (!flag)
			{
				return 0;
			}
			return 16;
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000D55C File Offset: 0x0000B75C
	public virtual void OnDropDraggedElementsAtIndex(List<TreeViewItem> draggedRows, T parent, int insertIndex)
	{
		if (this.beforeDroppingDraggedItems != null)
		{
			this.beforeDroppingDraggedItems(draggedRows);
		}
		List<TreeElement> list = new List<TreeElement>();
		foreach (TreeViewItem treeViewItem in draggedRows)
		{
			list.Add(((TreeViewItem<T>)treeViewItem).data);
		}
		int[] array = list.Select((TreeElement x) => x.id).ToArray<int>();
		this.m_TreeModel.MoveElements(parent, insertIndex, list);
		base.SetSelection(array, 2);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000D61C File Offset: 0x0000B81C
	private bool ValidDrag(TreeViewItem parent, List<TreeViewItem> draggedItems)
	{
		for (TreeViewItem treeViewItem = parent; treeViewItem != null; treeViewItem = treeViewItem.parent)
		{
			if (draggedItems.Contains(treeViewItem))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000133 RID: 307
	private TreeModel<T> m_TreeModel;

	// Token: 0x04000134 RID: 308
	private readonly List<TreeViewItem> m_Rows = new List<TreeViewItem>(100);

	// Token: 0x04000137 RID: 311
	private const string k_GenericDragID = "GenericDragColumnDragging";
}
