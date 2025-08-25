using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000043 RID: 67
public class TreeModel<T> where T : TreeElement
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000C8FA File Offset: 0x0000AAFA
	// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000C902 File Offset: 0x0000AB02
	public T root
	{
		get
		{
			return this.m_Root;
		}
		set
		{
			this.m_Root = value;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060001E5 RID: 485 RVA: 0x0000C90C File Offset: 0x0000AB0C
	// (remove) Token: 0x060001E6 RID: 486 RVA: 0x0000C944 File Offset: 0x0000AB44
	public event Action modelChanged;

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000C979 File Offset: 0x0000AB79
	public int numberOfDataElements
	{
		get
		{
			return this.m_Data.Count;
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000C986 File Offset: 0x0000AB86
	public TreeModel(IList<T> data)
	{
		this.SetData(data);
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000C998 File Offset: 0x0000AB98
	public T Find(int id)
	{
		return this.m_Data.FirstOrDefault((T element) => element.id == id);
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000C9C9 File Offset: 0x0000ABC9
	public void SetData(IList<T> data)
	{
		this.Init(data);
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
	private void Init(IList<T> data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data", "Input data is null. Ensure input is a non-null list.");
		}
		this.m_Data = data;
		if (this.m_Data.Count > 0)
		{
			this.m_Root = TreeElementUtility.ListToTree<T>(data);
		}
		this.m_MaxID = this.m_Data.Max((T e) => e.id);
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000CA48 File Offset: 0x0000AC48
	public int GenerateUniqueID()
	{
		int num = this.m_MaxID + 1;
		this.m_MaxID = num;
		return num;
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000CA68 File Offset: 0x0000AC68
	public IList<int> GetAncestors(int id)
	{
		List<int> list = new List<int>();
		TreeElement treeElement = this.Find(id);
		if (treeElement != null)
		{
			while (treeElement.parent != null)
			{
				list.Add(treeElement.parent.id);
				treeElement = treeElement.parent;
			}
		}
		return list;
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
	public IList<int> GetDescendantsThatHaveChildren(int id)
	{
		T t = this.Find(id);
		if (t != null)
		{
			return this.GetParentsBelowStackBased(t);
		}
		return new List<int>();
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
	private IList<int> GetParentsBelowStackBased(TreeElement searchFromThis)
	{
		Stack<TreeElement> stack = new Stack<TreeElement>();
		stack.Push(searchFromThis);
		List<int> list = new List<int>();
		while (stack.Count > 0)
		{
			TreeElement treeElement = stack.Pop();
			if (treeElement.hasChildren)
			{
				list.Add(treeElement.id);
				foreach (TreeElement treeElement2 in treeElement.children)
				{
					stack.Push(treeElement2);
				}
			}
		}
		return list;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000CB70 File Offset: 0x0000AD70
	public void RemoveElements(IList<int> elementIDs)
	{
		IList<T> list = this.m_Data.Where((T element) => elementIDs.Contains(element.id)).ToArray<T>();
		this.RemoveElements(list);
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
	public void RemoveElements(IList<T> elements)
	{
		using (IEnumerator<T> enumerator = elements.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == this.m_Root)
				{
					throw new ArgumentException("It is not allowed to remove the root element");
				}
			}
		}
		foreach (T t in TreeElementUtility.FindCommonAncestorsWithinList<T>(elements))
		{
			t.parent.children.Remove(t);
			t.parent = null;
		}
		TreeElementUtility.TreeToList<T>(this.m_Root, this.m_Data);
		this.Changed();
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000CC84 File Offset: 0x0000AE84
	public void AddElements(IList<T> elements, TreeElement parent, int insertPosition)
	{
		if (elements == null)
		{
			throw new ArgumentNullException("elements", "elements is null");
		}
		if (elements.Count == 0)
		{
			throw new ArgumentNullException("elements", "elements Count is 0: nothing to add");
		}
		if (parent == null)
		{
			throw new ArgumentNullException("parent", "parent is null");
		}
		if (parent.children == null)
		{
			parent.children = new List<TreeElement>();
		}
		parent.children.InsertRange(insertPosition, elements.Cast<TreeElement>());
		foreach (T t in elements)
		{
			t.parent = parent;
			t.depth = parent.depth + 1;
			TreeElementUtility.UpdateDepthValues<T>(t);
		}
		TreeElementUtility.TreeToList<T>(this.m_Root, this.m_Data);
		this.Changed();
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000CD64 File Offset: 0x0000AF64
	public void AddRoot(T root)
	{
		if (root == null)
		{
			throw new ArgumentNullException("root", "root is null");
		}
		if (this.m_Data == null)
		{
			throw new InvalidOperationException("Internal Error: data list is null");
		}
		if (this.m_Data.Count != 0)
		{
			throw new InvalidOperationException("AddRoot is only allowed on empty data list");
		}
		root.id = this.GenerateUniqueID();
		root.depth = -1;
		this.m_Data.Add(root);
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000CDE0 File Offset: 0x0000AFE0
	public void AddElement(T element, TreeElement parent, int insertPosition)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element", "element is null");
		}
		if (parent == null)
		{
			throw new ArgumentNullException("parent", "parent is null");
		}
		if (parent.children == null)
		{
			parent.children = new List<TreeElement>();
		}
		parent.children.Insert(insertPosition, element);
		element.parent = parent;
		TreeElementUtility.UpdateDepthValues<TreeElement>(parent);
		TreeElementUtility.TreeToList<T>(this.m_Root, this.m_Data);
		this.Changed();
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000CE68 File Offset: 0x0000B068
	public void MoveElements(TreeElement parentElement, int insertionIndex, List<TreeElement> elements)
	{
		if (insertionIndex < 0)
		{
			throw new ArgumentException("Invalid input: insertionIndex is -1, client needs to decide what index elements should be reparented at");
		}
		if (parentElement == null)
		{
			return;
		}
		if (insertionIndex > 0)
		{
			insertionIndex -= parentElement.children.GetRange(0, insertionIndex).Count(new Func<TreeElement, bool>(elements.Contains));
		}
		foreach (TreeElement treeElement in elements)
		{
			treeElement.parent.children.Remove(treeElement);
			treeElement.parent = parentElement;
		}
		if (parentElement.children == null)
		{
			parentElement.children = new List<TreeElement>();
		}
		parentElement.children.InsertRange(insertionIndex, elements);
		TreeElementUtility.UpdateDepthValues<T>(this.root);
		TreeElementUtility.TreeToList<T>(this.m_Root, this.m_Data);
		this.Changed();
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000CF44 File Offset: 0x0000B144
	private void Changed()
	{
		if (this.modelChanged != null)
		{
			this.modelChanged();
		}
	}

	// Token: 0x0400012E RID: 302
	private IList<T> m_Data;

	// Token: 0x0400012F RID: 303
	private T m_Root;

	// Token: 0x04000130 RID: 304
	private int m_MaxID;
}
