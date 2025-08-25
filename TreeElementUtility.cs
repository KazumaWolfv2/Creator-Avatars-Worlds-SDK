using System;
using System.Collections.Generic;

// Token: 0x02000042 RID: 66
public static class TreeElementUtility
{
	// Token: 0x060001DD RID: 477 RVA: 0x0000C448 File Offset: 0x0000A648
	public static void TreeToList<T>(T root, IList<T> result) where T : TreeElement
	{
		if (result == null)
		{
			throw new NullReferenceException("The input 'IList<T> result' list is null");
		}
		result.Clear();
		Stack<T> stack = new Stack<T>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			T t = stack.Pop();
			result.Add(t);
			if (t.children != null && t.children.Count > 0)
			{
				for (int i = t.children.Count - 1; i >= 0; i--)
				{
					stack.Push((T)((object)t.children[i]));
				}
			}
		}
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
	public static T ListToTree<T>(IList<T> list) where T : TreeElement
	{
		TreeElementUtility.ValidateDepthValues<T>(list);
		foreach (T t in list)
		{
			t.parent = null;
			t.children = null;
		}
		for (int i = 0; i < list.Count; i++)
		{
			T t2 = list[i];
			if (t2.children == null)
			{
				int depth = t2.depth;
				int num = 0;
				for (int j = i + 1; j < list.Count; j++)
				{
					if (list[j].depth == depth + 1)
					{
						num++;
					}
					if (list[j].depth <= depth)
					{
						break;
					}
				}
				List<TreeElement> list2 = null;
				if (num != 0)
				{
					list2 = new List<TreeElement>(num);
					num = 0;
					for (int k = i + 1; k < list.Count; k++)
					{
						if (list[k].depth == depth + 1)
						{
							list[k].parent = t2;
							list2.Add(list[k]);
							num++;
						}
						if (list[k].depth <= depth)
						{
							break;
						}
					}
				}
				t2.children = list2;
			}
		}
		return list[0];
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000C670 File Offset: 0x0000A870
	public static void ValidateDepthValues<T>(IList<T> list) where T : TreeElement
	{
		if (list.Count == 0)
		{
			throw new ArgumentException("list should have items, count is 0, check before calling ValidateDepthValues", "list");
		}
		if (list[0].depth != -1)
		{
			throw new ArgumentException("list item at index 0 should have a depth of -1 (since this should be the hidden root of the tree). Depth is: " + list[0].depth.ToString(), "list");
		}
		for (int i = 0; i < list.Count - 1; i++)
		{
			int depth = list[i].depth;
			int depth2 = list[i + 1].depth;
			if (depth2 > depth && depth2 - depth > 1)
			{
				throw new ArgumentException(string.Format("Invalid depth info in input list. Depth cannot increase more than 1 per row. Index {0} has depth {1} while index {2} has depth {3}", new object[]
				{
					i,
					depth,
					i + 1,
					depth2
				}));
			}
		}
		for (int j = 1; j < list.Count; j++)
		{
			if (list[j].depth < 0)
			{
				throw new ArgumentException("Invalid depth value for item at index " + j.ToString() + ". Only the first item (the root) should have depth below 0.");
			}
		}
		if (list.Count > 1 && list[1].depth != 0)
		{
			throw new ArgumentException("Input list item at index 1 is assumed to have a depth of 0", "list");
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
	public static void UpdateDepthValues<T>(T root) where T : TreeElement
	{
		if (root == null)
		{
			throw new ArgumentNullException("root", "The root is null");
		}
		if (!root.hasChildren)
		{
			return;
		}
		Stack<TreeElement> stack = new Stack<TreeElement>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			TreeElement treeElement = stack.Pop();
			if (treeElement.children != null)
			{
				foreach (TreeElement treeElement2 in treeElement.children)
				{
					treeElement2.depth = treeElement.depth + 1;
					stack.Push(treeElement2);
				}
			}
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000C87C File Offset: 0x0000AA7C
	private static bool IsChildOf<T>(T child, IList<T> elements) where T : TreeElement
	{
		while (child != null)
		{
			child = (T)((object)child.parent);
			if (elements.Contains(child))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
	public static IList<T> FindCommonAncestorsWithinList<T>(IList<T> elements) where T : TreeElement
	{
		if (elements.Count == 1)
		{
			return new List<T>(elements);
		}
		List<T> list = new List<T>(elements);
		list.RemoveAll((T g) => TreeElementUtility.IsChildOf<T>(g, elements));
		return list;
	}
}
