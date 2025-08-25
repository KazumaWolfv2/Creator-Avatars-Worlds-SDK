using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class ContentManagerDetailInfoWindow : EditorWindow
{
	// Token: 0x0600037D RID: 893 RVA: 0x000175DC File Offset: 0x000157DC
	private void OnEnable()
	{
		base.minSize = new Vector2(350f, 400f);
		base.maxSize = new Vector2(350f, 400f);
		base.titleContent.text = SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle");
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00017628 File Offset: 0x00015828
	public void SetErrorLogList(List<string> logList)
	{
		this._logList = logList;
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00017634 File Offset: 0x00015834
	private void OnGUI()
	{
		if (this._logList == null)
		{
			return;
		}
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(315f),
			GUILayout.Height(370f)
		});
		GUILayout.Space(-310f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		this.DrawMore();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000380 RID: 896 RVA: 0x000176C0 File Offset: 0x000158C0
	private void DrawNormal()
	{
		for (int i = 0; i < this._logList.Count; i++)
		{
			GUILayout.Label(this._logList[i], EditorStyles.wordWrappedLabel, new GUILayoutOption[]
			{
				GUILayout.Width(330f),
				GUILayout.ExpandWidth(false),
				GUILayout.ExpandHeight(false)
			});
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00017720 File Offset: 0x00015920
	private void DrawMore()
	{
		this.Pos = this.ScrollList<string>(this.Pos, this._logList, new Action<int, string>(this.Render<string>), new GUILayoutOption[]
		{
			GUILayout.Width(335f),
			GUILayout.Height(365f)
		}, new GUILayoutOption[0]);
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00017778 File Offset: 0x00015978
	public void Render<T>(int index, T data)
	{
		string text = data as string;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		GUILayout.Label(text, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width(310f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
	}

	// Token: 0x06000383 RID: 899 RVA: 0x000177CC File Offset: 0x000159CC
	public Vector2 ScrollList<T>(Vector2 listPos, List<T> dataList, Action<int, T> renderFunc, GUILayoutOption[] svLp, GUILayoutOption[] containerLp)
	{
		listPos = GUILayout.BeginScrollView(listPos, svLp);
		GUILayout.BeginVertical(containerLp);
		for (int i = 0; i < dataList.Count; i++)
		{
			renderFunc(i, dataList[i]);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		return listPos;
	}

	// Token: 0x040001DD RID: 477
	private List<string> _logList;

	// Token: 0x040001DE RID: 478
	private Vector2 Pos;
}
