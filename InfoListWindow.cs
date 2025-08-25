using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class InfoListWindow : EditorWindow
{
	// Token: 0x0600003C RID: 60 RVA: 0x00003C4C File Offset: 0x00001E4C
	private void OnEnable()
	{
		this.Pos = Vector2.zero;
		base.minSize = new Vector2(400f, 650f);
		base.maxSize = new Vector2(400f, 650f);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003C83 File Offset: 0x00001E83
	public void SetInfo(string baseTitle, string title, Action func = null)
	{
		this.BaseTitle = baseTitle;
		this.Title = title;
		this.Func = func;
		base.titleContent.text = this.BaseTitle;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003CAB File Offset: 0x00001EAB
	public void SetErrorInfo(string error, string content)
	{
		this.ContentList.Add(new InfoListWindow.ErrorContent
		{
			Title = error,
			Content = content
		});
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003CCC File Offset: 0x00001ECC
	public void DeleteErrorInfo(string error, string content)
	{
		int i = 0;
		while (i < this.ContentList.Count)
		{
			if (this.ContentList[i].Title == error && this.ContentList[i].Content == content)
			{
				this.ContentList.RemoveAt(i);
				int num = 0;
				if (this.ContentList.Count == 0)
				{
					this.OnClose();
					return;
				}
				base.minSize = new Vector2(400f, (float)(120 + this.ContentList.Count * 60 - num));
				base.maxSize = new Vector2(400f, (float)(120 + this.ContentList.Count * 60 - num));
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003D98 File Offset: 0x00001F98
	private void OnGUI()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		if (this.ContentList.Count > 0)
		{
			GUILayout.Label(this.Title, EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Height(15f) });
			GUILayout.Space(5f);
		}
		for (int i = 0; i < this.ContentList.Count; i++)
		{
			this.ContentSize.text = this.ContentList[i].Content;
			float num = EditorStyles.wordWrappedLabel.CalcHeight(this.ContentSize, 380f);
			GUI.Box(new Rect(5f, (float)(i * 60 + 25), 390f, 39f + num), "", GUI.skin.window);
			GUILayout.Space(9f);
			GUILayout.Label(this.ContentList[i].Title, EditorStyles.label, new GUILayoutOption[] { GUILayout.Height(15f) });
			GUILayout.Label(this.ContentList[i].Content, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
			{
				GUILayout.Height(num),
				GUILayout.Width(380f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Space(15f);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003F14 File Offset: 0x00002114
	private void OnClose()
	{
		base.Close();
		if (this.Func != null)
		{
			this.Func();
		}
	}

	// Token: 0x04000014 RID: 20
	private string BaseTitle;

	// Token: 0x04000015 RID: 21
	private string Title;

	// Token: 0x04000016 RID: 22
	private GUIContent ContentSize = new GUIContent();

	// Token: 0x04000017 RID: 23
	private Action Func;

	// Token: 0x04000018 RID: 24
	private List<InfoListWindow.ErrorContent> ContentList = new List<InfoListWindow.ErrorContent>();

	// Token: 0x04000019 RID: 25
	private Vector2 Pos;

	// Token: 0x0200008B RID: 139
	private class ErrorContent
	{
		// Token: 0x0400031A RID: 794
		public string Title;

		// Token: 0x0400031B RID: 795
		public string Content;
	}
}
