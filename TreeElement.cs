using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
[Serializable]
public class TreeElement
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000C3BB File Offset: 0x0000A5BB
	// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000C3C3 File Offset: 0x0000A5C3
	public int depth
	{
		get
		{
			return this.m_Depth;
		}
		set
		{
			this.m_Depth = value;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000C3CC File Offset: 0x0000A5CC
	// (set) Token: 0x060001D3 RID: 467 RVA: 0x0000C3D4 File Offset: 0x0000A5D4
	public TreeElement parent
	{
		get
		{
			return this.m_Parent;
		}
		set
		{
			this.m_Parent = value;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000C3DD File Offset: 0x0000A5DD
	// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000C3E5 File Offset: 0x0000A5E5
	public List<TreeElement> children
	{
		get
		{
			return this.m_Children;
		}
		set
		{
			this.m_Children = value;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000C3EE File Offset: 0x0000A5EE
	public bool hasChildren
	{
		get
		{
			return this.children != null && this.children.Count > 0;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000C408 File Offset: 0x0000A608
	// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000C410 File Offset: 0x0000A610
	public string name
	{
		get
		{
			return this.m_Name;
		}
		set
		{
			this.m_Name = value;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000C419 File Offset: 0x0000A619
	// (set) Token: 0x060001DA RID: 474 RVA: 0x0000C421 File Offset: 0x0000A621
	public int id
	{
		get
		{
			return this.m_ID;
		}
		set
		{
			this.m_ID = value;
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000025C5 File Offset: 0x000007C5
	public TreeElement()
	{
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000C42A File Offset: 0x0000A62A
	public TreeElement(string name, int depth, int id)
	{
		this.m_Name = name;
		this.m_ID = id;
		this.m_Depth = depth;
	}

	// Token: 0x04000129 RID: 297
	[SerializeField]
	private int m_ID;

	// Token: 0x0400012A RID: 298
	[SerializeField]
	private string m_Name;

	// Token: 0x0400012B RID: 299
	[SerializeField]
	private int m_Depth;

	// Token: 0x0400012C RID: 300
	[NonSerialized]
	private TreeElement m_Parent;

	// Token: 0x0400012D RID: 301
	[NonSerialized]
	private List<TreeElement> m_Children;
}
