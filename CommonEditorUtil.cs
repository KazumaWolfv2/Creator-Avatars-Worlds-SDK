using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x02000009 RID: 9
public static class CommonEditorUtil
{
	// Token: 0x06000038 RID: 56 RVA: 0x00003B1C File Offset: 0x00001D1C
	public static List<int> GetPublishList(bool isMarket, bool isClone)
	{
		List<int> list = new List<int>();
		if (isMarket)
		{
			list.Add(1);
		}
		if (isClone)
		{
			list.Add(3);
		}
		return list;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003B44 File Offset: 0x00001D44
	public static string GetLinkColor()
	{
		if (!EditorGUIUtility.isProSkin)
		{
			return "#11468aff";
		}
		return "#7faef0ff";
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003B58 File Offset: 0x00001D58
	public static string TextWithColor(string text, string color)
	{
		return string.Concat(new string[] { "<color=", color, ">", text, "</color>" });
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003B88 File Offset: 0x00001D88
	public static void DrawLink(string linkText, string url, int underlineLength)
	{
		GUIStyle guistyle = new GUIStyle
		{
			richText = true,
			padding = new RectOffset
			{
				top = 2,
				bottom = 2
			}
		};
		bool flag = GUILayout.Button(CommonEditorUtil.TextWithColor(linkText, CommonEditorUtil.GetLinkColor()), guistyle, Array.Empty<GUILayoutOption>());
		Rect lastRect = GUILayoutUtility.GetLastRect();
		EditorGUIUtility.AddCursorRect(lastRect, 4);
		string[] array = new string[underlineLength];
		for (int i = 0; i < underlineLength; i++)
		{
			array[i] = "_";
		}
		string text = string.Join("", array);
		GUI.Label(lastRect, CommonEditorUtil.TextWithColor(text, CommonEditorUtil.GetLinkColor()), new GUIStyle
		{
			richText = true,
			padding = new RectOffset
			{
				top = 4,
				bottom = 2
			}
		});
		if (flag)
		{
			Application.OpenURL(url);
		}
	}
}
