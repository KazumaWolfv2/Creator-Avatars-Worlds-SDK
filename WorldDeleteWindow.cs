using System;
using UnityEditor;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class WorldDeleteWindow : EditorWindow
{
	// Token: 0x0600047C RID: 1148 RVA: 0x0001DA64 File Offset: 0x0001BC64
	private void OnEnable()
	{
		base.minSize = new Vector2(350f, 240f);
		base.maxSize = new Vector2(350f, 240f);
		base.titleContent.text = SdkLangManager.Get("str_sdk_warning");
		this.Style24 = new GUIStyle(EditorStyles.boldLabel);
		this.Style24.fontSize = 24;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0001DACD File Offset: 0x0001BCCD
	public void SetCallBack(Action callBack)
	{
		this.CallBack = callBack;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0001DAD6 File Offset: 0x0001BCD6
	public void SetContentID(string contentID)
	{
		this.ContentID = contentID;
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
	private void OnGUI()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(160f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_warning"), this.Style24, new GUILayoutOption[] { GUILayout.Height(30f) });
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(60f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldDelete_text0"), EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width(280f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(5f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldDelete_text1"), EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width(280f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(25f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUI.color = Color.red;
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_ok"), new GUILayoutOption[]
		{
			GUILayout.Width(180f),
			GUILayout.Height(35f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.OnConfirm();
			GUIUtility.ExitGUI();
		}
		GUILayout.Space(3f);
		GUI.color = Color.white;
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_worldDelete_btn1"), new GUILayoutOption[]
		{
			GUILayout.Width(180f),
			GUILayout.Height(35f),
			GUILayout.ExpandWidth(false)
		}))
		{
			base.Close();
			GUIUtility.ExitGUI();
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
	private void OnConfirm()
	{
		base.Close();
		this.CallBack();
	}

	// Token: 0x04000287 RID: 647
	private string ContentID;

	// Token: 0x04000288 RID: 648
	private Action CallBack;

	// Token: 0x04000289 RID: 649
	private GUIStyle Style24;
}
