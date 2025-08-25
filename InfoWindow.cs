using System;
using UnityEditor;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class InfoWindow : EditorWindow
{
	// Token: 0x06000043 RID: 67 RVA: 0x00003F4D File Offset: 0x0000214D
	private void OnEnable()
	{
		base.minSize = new Vector2(350f, 130f);
		base.maxSize = new Vector2(350f, 130f);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003F7C File Offset: 0x0000217C
	public void SetInfo(string baseTitle, string content, string btnName, string btnExtra = null, string extraBaseTitle = null, string image = null, bool isBox = false)
	{
		this.BaseTitle = baseTitle;
		this.ExtraBaseTitle = extraBaseTitle;
		this.Content = content;
		this.Image = image;
		this.IsBox = isBox;
		this.BtnName = btnName;
		this.BtnExtra = btnExtra;
		base.titleContent.text = this.BaseTitle;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003FCF File Offset: 0x000021CF
	public void SetCallBack(Action funcName, Action funcExtra = null)
	{
		this.FuncName = funcName;
		this.FuncExtra = funcExtra;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003FE0 File Offset: 0x000021E0
	private void OnGUI()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		if (this.IsBox)
		{
			GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
			{
				GUILayout.Height(340f),
				GUILayout.Height(125f)
			});
			GUILayout.Space(-125f);
		}
		if (this.ExtraBaseTitle != null)
		{
			GUILayout.Label(this.ExtraBaseTitle, EditorStyles.wordWrappedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.Space(10f);
		}
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (this.Image != null)
		{
			GUILayout.Box(SdkLangManager.Get("str_sdk_bundleSettings_windowtext1"), new GUILayoutOption[]
			{
				GUILayout.Width(50f),
				GUILayout.Height(50f)
			});
			GUILayout.Space(10f);
		}
		GUILayout.Label(this.Content, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width((float)(330 - ((this.Image != null) ? 70 : 0))),
			GUILayout.Height(50f)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (this.BtnExtra != null)
		{
			GUILayout.Space(170f);
			if (GUILayout.Button(this.BtnExtra, new GUILayoutOption[]
			{
				GUILayout.Width(70f),
				GUILayout.Height(30f)
			}))
			{
				this.OnExtra();
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
			if (GUILayout.Button(this.BtnName, new GUILayoutOption[]
			{
				GUILayout.Width(70f),
				GUILayout.Height(30f)
			}))
			{
				this.OnClose();
				GUIUtility.ExitGUI();
			}
		}
		else
		{
			GUILayout.Space(250f);
			if (GUILayout.Button(this.BtnName, new GUILayoutOption[]
			{
				GUILayout.Width(70f),
				GUILayout.Height(30f)
			}))
			{
				this.OnClose();
				GUIUtility.ExitGUI();
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00004207 File Offset: 0x00002407
	private void OnClose()
	{
		base.Close();
		if (this.FuncName != null)
		{
			this.FuncName();
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00004222 File Offset: 0x00002422
	private void OnExtra()
	{
		base.Close();
		if (this.FuncExtra != null)
		{
			this.FuncExtra();
		}
	}

	// Token: 0x0400001A RID: 26
	private string BaseTitle;

	// Token: 0x0400001B RID: 27
	private string ExtraBaseTitle;

	// Token: 0x0400001C RID: 28
	private string Content;

	// Token: 0x0400001D RID: 29
	private string Image;

	// Token: 0x0400001E RID: 30
	private bool IsBox;

	// Token: 0x0400001F RID: 31
	private string BtnName;

	// Token: 0x04000020 RID: 32
	private string BtnExtra;

	// Token: 0x04000021 RID: 33
	private Action FuncName;

	// Token: 0x04000022 RID: 34
	private Action FuncExtra;
}
