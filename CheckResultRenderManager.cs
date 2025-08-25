using System;
using UnityEditor;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class CheckResultRenderManager
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600007B RID: 123 RVA: 0x000066E4 File Offset: 0x000048E4
	public static CheckResultRenderManager Instance
	{
		get
		{
			if (CheckResultRenderManager._instance == null)
			{
				CheckResultRenderManager._instance = new CheckResultRenderManager();
			}
			return CheckResultRenderManager._instance;
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000066FC File Offset: 0x000048FC
	private CheckResultRenderManager()
	{
		this.StyleRed = new GUIStyle(EditorStyles.label);
		this.StyleRed.fontSize = 12;
		this.StyleRed.normal.textColor = Color.red;
		this.StyleRed.hover.textColor = Color.red;
		this.StyleGreen = new GUIStyle(EditorStyles.label);
		this.StyleGreen.fontSize = 12;
		this.StyleGreen.normal.textColor = Color.green;
		this.StyleGreen.hover.textColor = Color.green;
		this.StyleBlue = new GUIStyle(EditorStyles.label);
		this.StyleBlue.fontSize = 12;
		this.StyleBlue.normal.textColor = Color.blue;
		this.StyleBlue.hover.textColor = Color.blue;
		this.StyleYellow = new GUIStyle(EditorStyles.label);
		this.StyleYellow.fontSize = 12;
		this.StyleYellow.normal.textColor = Color.yellow;
		this.StyleYellow.hover.textColor = Color.yellow;
		this.StyleOrange = new GUIStyle(EditorStyles.label);
		this.StyleOrange.fontSize = 12;
		this.StyleOrange.normal.textColor = new Color(1f, 0.635f, 0f);
		this.StyleOrange.hover.textColor = new Color(1f, 0.635f, 0f);
		this.StyleBadOrange = new GUIStyle(EditorStyles.label);
		this.StyleBadOrange.fontSize = 12;
		this.StyleBadOrange.normal.textColor = new Color(1f, 0.369f, 0f);
		this.StyleBadOrange.hover.textColor = new Color(1f, 0.369f, 0f);
		this.StyleTitle = new GUIStyle(EditorStyles.boldLabel);
		this.StyleTitle.fontSize = 12;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00006912 File Offset: 0x00004B12
	public GUIStyle GetCheckResultStyle(RuleResultType type)
	{
		if (type == RuleResultType.RuleResultType_Prefect)
		{
			return this.StyleGreen;
		}
		if (type == RuleResultType.RuleResultType_Excellent)
		{
			return this.StyleBlue;
		}
		if (type == RuleResultType.RuleResultType_Good)
		{
			return this.StyleYellow;
		}
		if (type == RuleResultType.RuleResultType_Normal)
		{
			return this.StyleOrange;
		}
		if (type == RuleResultType.RuleResultType_Poor)
		{
			return this.StyleBadOrange;
		}
		return this.StyleRed;
	}

	// Token: 0x0400003D RID: 61
	public GUIStyle StyleRed;

	// Token: 0x0400003E RID: 62
	public GUIStyle StyleGreen;

	// Token: 0x0400003F RID: 63
	public GUIStyle StyleBlue;

	// Token: 0x04000040 RID: 64
	public GUIStyle StyleYellow;

	// Token: 0x04000041 RID: 65
	public GUIStyle StyleOrange;

	// Token: 0x04000042 RID: 66
	public GUIStyle StyleBadOrange;

	// Token: 0x04000043 RID: 67
	public GUIStyle StyleGray;

	// Token: 0x04000044 RID: 68
	public GUIStyle StyleTitle;

	// Token: 0x04000045 RID: 69
	private static CheckResultRenderManager _instance;
}
