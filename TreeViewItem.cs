using System;
using UnityEditor.IMGUI.Controls;

// Token: 0x02000044 RID: 68
internal class TreeViewItem<T> : TreeViewItem where T : TreeElement
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000CF59 File Offset: 0x0000B159
	// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000CF61 File Offset: 0x0000B161
	public T data { get; set; }

	// Token: 0x060001F9 RID: 505 RVA: 0x0000CF6A File Offset: 0x0000B16A
	public TreeViewItem(int id, int depth, string displayName, T data)
		: base(id, depth, displayName)
	{
		this.data = data;
	}
}
