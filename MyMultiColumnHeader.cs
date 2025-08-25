using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

// Token: 0x02000040 RID: 64
internal class MyMultiColumnHeader : MultiColumnHeader
{
	// Token: 0x060001CC RID: 460 RVA: 0x0000C2C1 File Offset: 0x0000A4C1
	public MyMultiColumnHeader(MultiColumnHeaderState state)
		: base(state)
	{
		this.mode = MyMultiColumnHeader.Mode.DefaultHeader;
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060001CD RID: 461 RVA: 0x0000C2D1 File Offset: 0x0000A4D1
	// (set) Token: 0x060001CE RID: 462 RVA: 0x0000C2DC File Offset: 0x0000A4DC
	public MyMultiColumnHeader.Mode mode
	{
		get
		{
			return this.m_Mode;
		}
		set
		{
			this.m_Mode = value;
			switch (this.m_Mode)
			{
			case MyMultiColumnHeader.Mode.LargeHeader:
				base.canSort = true;
				base.height = 37f;
				return;
			case MyMultiColumnHeader.Mode.DefaultHeader:
				base.canSort = true;
				base.height = MultiColumnHeader.DefaultGUI.defaultHeight;
				return;
			case MyMultiColumnHeader.Mode.MinimumHeaderWithoutSorting:
				base.canSort = false;
				base.height = MultiColumnHeader.DefaultGUI.minimumHeight;
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000C344 File Offset: 0x0000A544
	protected override void ColumnHeaderGUI(MultiColumnHeaderState.Column column, Rect headerRect, int columnIndex)
	{
		base.ColumnHeaderGUI(column, headerRect, columnIndex);
		if (this.mode == MyMultiColumnHeader.Mode.LargeHeader && columnIndex > 2)
		{
			headerRect.xMax -= 3f;
			TextAnchor alignment = EditorStyles.largeLabel.alignment;
			EditorStyles.largeLabel.alignment = 2;
			GUI.Label(headerRect, (36 + columnIndex).ToString() + "%", EditorStyles.largeLabel);
			EditorStyles.largeLabel.alignment = alignment;
		}
	}

	// Token: 0x04000128 RID: 296
	private MyMultiColumnHeader.Mode m_Mode;

	// Token: 0x0200009B RID: 155
	public enum Mode
	{
		// Token: 0x0400034C RID: 844
		LargeHeader,
		// Token: 0x0400034D RID: 845
		DefaultHeader,
		// Token: 0x0400034E RID: 846
		MinimumHeaderWithoutSorting
	}
}
