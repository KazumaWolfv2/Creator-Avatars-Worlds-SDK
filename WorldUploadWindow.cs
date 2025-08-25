using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000084 RID: 132
public class WorldUploadWindow : UploadWindowBase
{
	// Token: 0x06000482 RID: 1154 RVA: 0x0001DCB7 File Offset: 0x0001BEB7
	private void OnEnable()
	{
		this.ResetData();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0001DCC0 File Offset: 0x0001BEC0
	protected void ResetData()
	{
		this.IsUploading = false;
		this.IsTextureChanged = false;
		this.IsIconChanged = false;
		float num = 96f;
		float num2 = (float)Screen.currentResolution.height * 5f / (6f * (Screen.dpi / num));
		if (num2 >= 750f)
		{
			base.minSize = new Vector2(510f, 750f);
			base.maxSize = new Vector2(510f, 750f);
		}
		else
		{
			base.minSize = new Vector2(510f, num2);
			base.maxSize = new Vector2(510f, num2 + 10f);
		}
		base.titleContent.text = SdkLangManager.Get("str_sdk_worldPublish_title1");
		this.IsMainTexture = true;
		this.Capacity = 20;
		this.TagList.Clear();
		this.TagList.Add("");
		this.TagList.Add("");
		this.ErrorInfoList.Clear();
		this.TogglePublic = true;
		this.TogglePrivate = false;
		this.Pos = Vector2.zero;
		this.DesScrollPos = Vector2.zero;
		this.IsModifyTexture = false;
		this.IsModifyIcon = false;
		this.ContainerLp = new GUILayoutOption[0];
		this.SvLp = new GUILayoutOption[] { GUILayout.Height(base.minSize.y) };
		this.SvLpTa = new GUILayoutOption[]
		{
			GUILayout.Width(460f),
			GUILayout.Height(102f)
		};
		this.Style9 = new GUIStyle(EditorStyles.wordWrappedLabel);
		this.Style9.fontSize = 9;
		this.DelTagIndex = -1;
		this.ParaWorldRootKey = "";
		this.ExportWorldPath = ParaPathDefine.kSdkDownLoadPath;
		this.RemoteTexturePath = null;
		this.RemoteIconPath = null;
		this.RemoteTexture = null;
		this.RemoteIcon = null;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0001DE9C File Offset: 0x0001C09C
	public void RefreshWorldData(WorldData data)
	{
		this.Capacity = data.Capacity;
		this.TogglePublic = data.IsPublic;
		this.TogglePrivate = !this.TogglePublic;
		this.WorldTexture16_9 = EditorUtil.LoadTexture(data.TexturePath);
		this.WorldName = data.Name;
		this.Description = data.Description;
		this.RemoteTexturePath = data.RemoteTexturePath;
		this.RemoteIconPath = data.RemoteIconPath;
		this.RemoteTexture = data.TexturePath;
		this.RemoteIcon = data.IconPath;
		this.TagList.Clear();
		for (int i = 0; i < data.HashTags.Count; i++)
		{
			this.TagList.Add(data.HashTags[i]);
		}
		this.ContentId = data.ContentID;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0001DF70 File Offset: 0x0001C170
	private void OnGUI()
	{
		this.Pos = GUILayout.BeginScrollView(this.Pos, false, false, this.SvLp);
		GUILayout.BeginVertical(this.ContainerLp);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(23f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		EditorStyles.label.fontSize = 15;
		this.DrawTitle();
		this.DrawTopTextureBox();
		this.DrawTopTexture();
		GUILayout.Space(22f);
		this.DrawName();
		GUILayout.Space(5f);
		this.DrawDesc();
		GUILayout.Space(5f);
		this.DrawHeaderCount();
		GUILayout.Space(6f);
		this.DrawTags();
		GUILayout.Space(14f);
		this.DrawPublic();
		GUILayout.Space(7f);
		this.DrawLine();
		GUILayout.Space(3f);
		this.DrawBottom();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		EditorStyles.label.fontSize = 12;
		GUILayout.EndVertical();
		GUILayout.Space(35f);
		GUILayout.EndScrollView();
		if (this.DelTagIndex > -1)
		{
			this.TagList.RemoveAt(this.DelTagIndex);
			this.DelTagIndex = -1;
		}
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0001E098 File Offset: 0x0001C298
	private void DrawTitle()
	{
		GUIStyle guistyle = new GUIStyle(EditorStyles.label);
		guistyle.alignment = 4;
		guistyle.fontSize = 12;
		GUILayout.Space(7f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field1"), Array.Empty<GUILayoutOption>());
		GUILayout.Space(40f);
		GUILayout.EndHorizontal();
		GUILayout.Space(2f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUI.color = new Color(1f, 1f, 1f, this.IsMainTexture ? 1f : 0.25f);
		if (GUILayout.Button("", new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(20f)
		}))
		{
			this.IsMainTexture = true;
			GUIUtility.ExitGUI();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.Space(-100f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field2"), guistyle, new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-5f);
		if (!this.IsMainTexture)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
		}
		GUI.color = new Color(1f, 1f, 1f, this.IsMainTexture ? 0.25f : 1f);
		if (GUILayout.Button("", new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(20f)
		}))
		{
			this.IsMainTexture = false;
			GUIUtility.ExitGUI();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.Space(-100f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field3"), guistyle, new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001E2CC File Offset: 0x0001C4CC
	private void DrawTopTextureBox()
	{
		GUILayout.Space(-2f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(185f)
		});
		GUILayout.Space(-170f);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001E324 File Offset: 0x0001C524
	private void DrawTopTexture()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(17f);
		if (this.IsMainTexture)
		{
			EditorUtil.DrawTexture(this.TextureDisplayRect16_9, this.WorldTexture16_9, SdkLangManager.Get("str_sdk_worldPublish_field4"));
		}
		else
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(54f);
			EditorUtil.DrawTexture(this.TextureDisplayRect1_1, this.WorldTexture1_1, SdkLangManager.Get("str_sdk_worldPublish_field4"));
			GUILayout.Space(54f);
			GUILayout.EndHorizontal();
		}
		this.OnDrawTakeDown();
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(27f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_worldPublish_btn0"), new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(20f)
		}))
		{
			this.SelectImage();
			GUIUtility.ExitGUI();
		}
		GUILayout.Space(7f);
		this.OnGuiRevetCamera();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-15f);
		string textureContent = this.GetTextureContent();
		if (this.IsMainTexture)
		{
			GUILayout.Label(textureContent, EditorStyles.wordWrappedLabel, new GUILayoutOption[] { GUILayout.Width(150f) });
		}
		else
		{
			GUILayout.Label(textureContent, EditorStyles.wordWrappedLabel, new GUILayoutOption[] { GUILayout.Width(150f) });
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		EditorStyles.label.fontSize = 4;
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-14f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_text0") + (this.IsMainTexture ? "1600*900." : "900*900."), this.Style9, new GUILayoutOption[] { GUILayout.Width(135f) });
		GUILayout.EndHorizontal();
		EditorStyles.label.fontSize = 12;
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001E524 File Offset: 0x0001C724
	private void DrawName()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-3f);
		EditorUtil.DrawRedStar();
		GUILayout.Space(-5f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field6", EditorUtil.GetRealStringCount(this.WorldName)), new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.EndHorizontal();
		this.WorldName = GUILayout.TextField(this.WorldName, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(25f)
		});
		this.WorldName = this.WorldName.TrimStart();
		if (EditorUtil.GetRealStringCount(this.WorldName) > 50)
		{
			this.WorldName = EditorUtil.Clamp(this.WorldName, 50);
		}
		if (this.WorldName.Equals(""))
		{
			GUILayout.Space(-30f);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_text1"), new GUILayoutOption[]
			{
				GUILayout.Height(28f),
				GUILayout.ExpandWidth(false)
			});
			GUI.color = Color.white;
		}
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001E65C File Offset: 0x0001C85C
	private void DrawDesc()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-2f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field7", EditorUtil.GetRealStringCount(this.Description)), new GUILayoutOption[] { GUILayout.Height(20f) });
		GUILayout.EndHorizontal();
		this.ContentArea.text = this.Description;
		if (GUI.skin.textArea.CalcHeight(this.ContentArea, 440f) > 100f)
		{
			this.DesScrollPos = EditorGUILayout.BeginScrollView(this.DesScrollPos, this.SvLpTa);
			GUILayout.BeginVertical(this.ContainerLp);
			this.Description = GUILayout.TextArea(this.Description, 500, new GUILayoutOption[]
			{
				GUILayout.Width(440f),
				GUILayout.ExpandHeight(true),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}
		else
		{
			this.Description = GUILayout.TextArea(this.Description, 500, new GUILayoutOption[]
			{
				GUILayout.Width(440f),
				GUILayout.Height(102f),
				GUILayout.ExpandHeight(false),
				GUILayout.ExpandWidth(false)
			});
		}
		this.Description = this.Description.TrimStart();
		if (EditorUtil.GetRealStringCount(this.Description) > 500)
		{
			this.Description = EditorUtil.Clamp(this.Description, 500);
		}
		if (this.Description.Equals(""))
		{
			GUILayout.Space(-105f);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_text2"), new GUILayoutOption[]
			{
				GUILayout.Height(28f),
				GUILayout.ExpandWidth(false)
			});
			GUI.color = Color.white;
			GUILayout.Space(75f);
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001E84C File Offset: 0x0001CA4C
	private void DrawHeaderCount()
	{
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field8"), new GUILayoutOption[] { GUILayout.Height(20f) });
		GUILayout.Space(3f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.Capacity = (int)GUILayout.HorizontalSlider((float)this.Capacity, 1f, 30f, new GUILayoutOption[] { GUILayout.Width(370f) });
		GUILayout.Space(25f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-3f);
		this.Capacity = EditorGUILayout.IntField(this.Capacity, new GUILayoutOption[]
		{
			GUILayout.Width(50f),
			GUILayout.Height(25f)
		});
		if (this.Capacity >= 30)
		{
			this.Capacity = 30;
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001E92C File Offset: 0x0001CB2C
	private void DrawTags()
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field9"), Array.Empty<GUILayoutOption>());
		GUILayout.Space(2f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(4f);
		int num = ((this.TagList.Count == 6) ? 10 : 40) + this.TagList.Count * 28;
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height((float)num),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space((float)(-(float)num));
		GUILayout.Space(4f);
		for (int i = 0; i < this.TagList.Count; i++)
		{
			this.DrawTagItem(i);
		}
		GUILayout.Space(2f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(180f);
		if (this.TagList.Count < 6 && GUILayout.Button("+", new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(25f)
		}))
		{
			if (this.TagList.Count < 6)
			{
				this.TagList.Add("");
			}
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0001EA8C File Offset: 0x0001CC8C
	private void DrawPublic()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-4f);
		EditorUtil.DrawRedStar();
		GUILayout.Space(-5f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_field10"), Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(16f);
		this.TogglePublic = GUILayout.Toggle(this.TogglePublic, "", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_text3"), new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		this.TogglePrivate = !this.TogglePublic;
		GUILayout.EndHorizontal();
		GUILayout.Space(7f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(16f);
		this.TogglePrivate = GUILayout.Toggle(this.TogglePrivate, "", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_text4"), new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		this.TogglePublic = !this.TogglePrivate;
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001EBC0 File Offset: 0x0001CDC0
	private void DrawLine()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		GUILayout.Box("", new GUILayoutOption[]
		{
			GUILayout.Width(370f),
			GUILayout.Height(1f),
			GUILayout.ExpandWidth(true)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0001EC19 File Offset: 0x0001CE19
	private void DrawBottom()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		this.OnGuiLeftBottomBtn();
		GUILayout.Space(67f);
		this.OnGuiBottomBtn();
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001EC4C File Offset: 0x0001CE4C
	private void DrawTagItem(int index)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(11f);
		this.TagList[index] = GUILayout.TextField(this.TagList[index], new GUILayoutOption[]
		{
			GUILayout.Width(380f),
			GUILayout.Height(26f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		this.TagList[index] = this.TagList[index].TrimStart();
		int realStringCount = EditorUtil.GetRealStringCount(this.TagList[index]);
		if (realStringCount > 20)
		{
			this.TagList[index] = EditorUtil.Clamp(this.TagList[index], 20);
		}
		GUILayout.Space(2f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_worldPublish_btn1"), new GUILayoutOption[]
		{
			GUILayout.Width(50f),
			GUILayout.Height(26f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.DelTagIndex = index;
			GUIUtility.ExitGUI();
		}
		GUILayout.Space(-100f);
		string text;
		if (realStringCount < 10)
		{
			text = string.Format(" {0}/20", realStringCount);
		}
		else
		{
			text = string.Format("{0}/20", EditorUtil.GetRealStringCount(this.TagList[index]));
		}
		GUILayout.Label(text, new GUILayoutOption[]
		{
			GUILayout.Width(40f),
			GUILayout.Height(26f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
		if (!(this.TagList[index] != ""))
		{
			GUILayout.Space(-28f);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(12f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_worldPublish_text5"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUI.color = Color.white;
		}
		GUILayout.EndVertical();
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
	private void SelectImage()
	{
		DateTime now = DateTime.Now;
		CommonImageSelectWindow window = EditorWindow.GetWindow<CommonImageSelectWindow>();
		Action<Texture2D> action = delegate(Texture2D texture)
		{
			if (texture == null)
			{
				return;
			}
			this.IsActiveCarmera16_9 = false;
			this.IsActiveCarmera1_1 = false;
			if (this.IsMainTexture)
			{
				this.WorldTexture16_9 = texture;
				this.SetSelectImageStatus(this.WorldTexture16_9);
				base.Repaint();
				return;
			}
			this.WorldTexture1_1 = texture;
			this.SetSelectImageStatus(this.WorldTexture1_1);
			base.Repaint();
		};
		window.FetchTexture(this.IsMainTexture ? this.TextureRect16_9 : this.TextureRect1_1, "Para World Upload", "The aspect ratio is fixed at " + (this.IsMainTexture ? "16:9" : "1：1"), action);
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0001EF6C File Offset: 0x0001D16C
	public bool UploadPng16_9(string basePath)
	{
		if (!SdkManager.Instance.IsSameJob(this.JobId))
		{
			return false;
		}
		string text = basePath + "/main.16_9.png";
		EditorUtil.SaveTextureToLocal(this.WorldTexture16_9, text);
		byte[] bytes = File.ReadAllBytes(text);
		CommonNetProxy.UploadTexture(this.UploadToken, this.JobId, WorldJobMask.WorldJobMaskCover, bytes, "16_9.png", delegate(int code, string message, JsonData result)
		{
			ParaLog.Log("code---2>: " + code.ToString());
			ParaLog.Log("message---2>: " + message);
			if (code == 0)
			{
				this.IsUpLoadImage16_9 = true;
				this.IsModifyTexture = true;
				CommonNetProxy.AddJobResInfo(ref this.ResInfo, WorldJobMask.WorldJobMaskCover, result, (long)bytes.Length);
				return;
			}
			this.ErrorInfoList.Add(new UploadWindowBase.ErrorInfo(code, message));
		});
		return true;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0001EFEC File Offset: 0x0001D1EC
	public bool UploadPng1_1(string basePath)
	{
		if (!SdkManager.Instance.IsSameJob(this.JobId))
		{
			return false;
		}
		string text = basePath + "/main.1_1.png";
		EditorUtil.SaveTextureToLocal(this.WorldTexture1_1, text);
		byte[] bytes = File.ReadAllBytes(text);
		CommonNetProxy.UploadTexture(this.UploadToken, this.JobId, WorldJobMask.WorldJobMaskRoomIcon, bytes, "1_1.png", delegate(int code, string message, JsonData result)
		{
			ParaLog.Log("code---3>: " + code.ToString());
			ParaLog.Log("message---3>: " + message);
			if (code == 0)
			{
				this.IsUpLoadImage1_1 = true;
				this.IsModifyIcon = true;
				CommonNetProxy.AddJobResInfo(ref this.ResInfo, WorldJobMask.WorldJobMaskRoomIcon, result, (long)bytes.Length);
				return;
			}
			this.ErrorInfoList.Add(new UploadWindowBase.ErrorInfo(code, message));
		});
		return true;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0001F06C File Offset: 0x0001D26C
	protected void ShowWorldName()
	{
		EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_worldPublish_text14"), "OK", null);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001F08D File Offset: 0x0001D28D
	protected void ShowAcceptTerms()
	{
		EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_worldPublish_text15"), "OK", null);
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiRevetCamera()
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiBottomBtn()
	{
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiLeftBottomBtn()
	{
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0001F0AE File Offset: 0x0001D2AE
	public virtual string GetTextureContent()
	{
		return "";
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void SetSelectImageStatus(Texture2D texture)
	{
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnDrawTakeDown()
	{
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0001F0B8 File Offset: 0x0001D2B8
	protected void UploadReviewImages(Transform trans)
	{
		ParaLog.Log("WorldUploadWindows.GenerateReview");
		string kGeneratePath = ParaPathDefine.kGeneratePath;
		if (!Directory.Exists(kGeneratePath))
		{
			Directory.CreateDirectory(kGeneratePath);
		}
		Bounds bounds = this.CalculateBoundingBox(trans);
		this.CreateReviewCamera();
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.SpawnForward);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.SpawnBack);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.SpawnLeft);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.SpawnRight);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.Top);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.Forward);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.Back);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.Left);
		this.TakeScreenShoot(trans, bounds, kGeneratePath, WorldUploadWindow.ReviewImageType.Right);
		this.CloseReviewCamera();
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0001F154 File Offset: 0x0001D354
	private void TakeScreenShoot(Transform trans, Bounds boundingBox, string basePath, WorldUploadWindow.ReviewImageType type)
	{
		ParaLog.Log(string.Format("WorldUploadWindows.TakeScreenShoot {0} {1}", basePath, type));
		switch (type)
		{
		case WorldUploadWindow.ReviewImageType.SpawnForward:
			this.m_cameraReview.transform.position = trans.position + new Vector3(0f, 1.6f, 0f);
			this.m_cameraReview.transform.rotation = Quaternion.LookRotation(trans.forward) * Quaternion.AngleAxis(0f, Vector3.up);
			break;
		case WorldUploadWindow.ReviewImageType.SpawnBack:
			this.m_cameraReview.transform.position = trans.position + new Vector3(0f, 1.6f, 0f);
			this.m_cameraReview.transform.rotation = Quaternion.LookRotation(trans.forward) * Quaternion.AngleAxis(180f, Vector3.up);
			break;
		case WorldUploadWindow.ReviewImageType.SpawnLeft:
			this.m_cameraReview.transform.position = trans.position + new Vector3(0f, 1.6f, 0f);
			this.m_cameraReview.transform.rotation = Quaternion.LookRotation(trans.forward) * Quaternion.AngleAxis(90f, Vector3.up);
			break;
		case WorldUploadWindow.ReviewImageType.SpawnRight:
			this.m_cameraReview.transform.position = trans.position + new Vector3(0f, 1.6f, 0f);
			this.m_cameraReview.transform.rotation = Quaternion.LookRotation(trans.forward) * Quaternion.AngleAxis(-90f, Vector3.up);
			break;
		case WorldUploadWindow.ReviewImageType.Top:
		{
			float num = Mathf.Sqrt(boundingBox.extents.x * boundingBox.extents.x + boundingBox.extents.z * boundingBox.extents.z);
			float num2 = Mathf.Tan(this.m_cameraReview.fieldOfView / 2f * 0.017453292f);
			float num3 = num / num2;
			this.m_cameraReview.transform.position = boundingBox.center + new Vector3(0f, num3, 0f);
			this.m_cameraReview.transform.rotation = Quaternion.LookRotation(Vector3.down);
			break;
		}
		case WorldUploadWindow.ReviewImageType.Forward:
		{
			float num4 = Mathf.Sqrt(boundingBox.extents.x * boundingBox.extents.x + boundingBox.extents.y * boundingBox.extents.y);
			float num5 = Mathf.Tan(this.m_cameraReview.fieldOfView / 2f * 0.017453292f);
			float num6 = num4 / num5;
			this.m_cameraReview.transform.position = boundingBox.center + new Vector3(0f, 0f, num6) + new Vector3(0f, boundingBox.size.y, 0f);
			this.m_cameraReview.transform.LookAt(boundingBox.center);
			break;
		}
		case WorldUploadWindow.ReviewImageType.Back:
		{
			float num7 = Mathf.Sqrt(boundingBox.extents.x * boundingBox.extents.x + boundingBox.extents.y * boundingBox.extents.y);
			float num8 = Mathf.Tan(this.m_cameraReview.fieldOfView / 2f * 0.017453292f);
			float num9 = num7 / num8;
			this.m_cameraReview.transform.position = boundingBox.center - new Vector3(0f, 0f, num9) + new Vector3(0f, boundingBox.size.y, 0f);
			this.m_cameraReview.transform.LookAt(boundingBox.center);
			break;
		}
		case WorldUploadWindow.ReviewImageType.Left:
		{
			float num10 = Mathf.Sqrt(boundingBox.extents.z * boundingBox.extents.z + boundingBox.extents.y * boundingBox.extents.y);
			float num11 = Mathf.Tan(this.m_cameraReview.fieldOfView / 2f * 0.017453292f);
			float num12 = num10 / num11;
			this.m_cameraReview.transform.position = boundingBox.center - new Vector3(num12, 0f, 0f) + new Vector3(0f, boundingBox.size.y, 0f);
			this.m_cameraReview.transform.LookAt(boundingBox.center);
			break;
		}
		case WorldUploadWindow.ReviewImageType.Right:
		{
			float num13 = Mathf.Sqrt(boundingBox.extents.z * boundingBox.extents.z + boundingBox.extents.y * boundingBox.extents.y);
			float num14 = Mathf.Tan(this.m_cameraReview.fieldOfView / 2f * 0.017453292f);
			float num15 = num13 / num14;
			this.m_cameraReview.transform.position = boundingBox.center + new Vector3(num15, 0f, 0f) + new Vector3(0f, boundingBox.size.y, 0f);
			this.m_cameraReview.transform.LookAt(boundingBox.center);
			break;
		}
		}
		object obj = null;
		string text = this.m_imageName[(int)type];
		EditorUtil.SaveTextureToLocal(EditorUtil.CameraCapture(obj, this.m_cameraReview, this.m_textureRectReview), Path.Combine(basePath, text));
		Object.DestroyImmediate(obj);
		this.UploadReviewImage((int)type, text);
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0001F718 File Offset: 0x0001D918
	private Bounds CalculateBoundingBox(Transform trans)
	{
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		Bounds bounds;
		bounds..ctor(trans.position, Vector3.zero);
		GameObject[] array = rootGameObjects;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Renderer renderer in array[i].GetComponentsInChildren<Renderer>(false))
			{
				bounds.Encapsulate(renderer.bounds);
			}
		}
		return bounds;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0001F788 File Offset: 0x0001D988
	private void CreateReviewCamera()
	{
		GameObject gameObject = GameObject.Find("Review_Camera");
		if (gameObject != null)
		{
			Object.DestroyImmediate(gameObject);
		}
		this.m_cameraReview = EditorUtil.CreateCamera("Review_Camera");
		this.m_cameraReview.gameObject.tag = "EditorOnly";
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001F7D4 File Offset: 0x0001D9D4
	private void CloseReviewCamera()
	{
		if (this.m_cameraReview)
		{
			this.m_cameraReview.enabled = false;
			this.m_cameraReview.targetTexture = null;
			Object.DestroyImmediate(this.m_cameraReview.gameObject);
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001F80C File Offset: 0x0001DA0C
	private void UploadReviewImage(int index, string imageName)
	{
		string text = Path.Combine(ParaPathDefine.kGeneratePath, imageName);
		byte[] bytes = File.ReadAllBytes(text);
		CommonNetProxy.UploadTexture(this.UploadToken, this.JobId, WorldJobMask.WorldJobMaskReviewImg, bytes, imageName, delegate(int code, string message, JsonData result)
		{
			ParaLog.Log(string.Format("WorldUploadWindows.UploadReviewImage image:{0} code: {1} message:{2}", imageName, code, message));
			if (code == 0)
			{
				this.m_uploadImage++;
				CommonNetProxy.AddJobResInfo(ref this.ResInfo, WorldJobMask.WorldJobMaskReviewImg, result, (long)bytes.Length);
				return;
			}
			this.ErrorInfoList.Add(new UploadWindowBase.ErrorInfo(code, message));
		});
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001F878 File Offset: 0x0001DA78
	protected void UploadReviewAudios()
	{
		foreach (string text in this.Audios)
		{
			this.UploadReviewAudios(text);
		}
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0001F8CC File Offset: 0x0001DACC
	private void UploadReviewAudios(string audio)
	{
		string text = Application.dataPath;
		text = text.Substring(0, text.IndexOf("Assets"));
		string text2 = Path.Combine(text, audio);
		string audioName = Path.GetFileName(text2);
		byte[] bytes = File.ReadAllBytes(text2);
		CommonNetProxy.UploadTexture(this.UploadToken, this.JobId, WorldJobMask.WorldJobMaskReviewAudio, bytes, audioName, delegate(int code, string message, JsonData result)
		{
			ParaLog.Log(string.Format("WorldUploadWindows.UploadReviewAudios audio:{0} code: {1} message:{2}", audioName, code, message));
			if (code == 0)
			{
				this.m_uploadAudio++;
				CommonNetProxy.AddJobResInfo(ref this.ResInfo, WorldJobMask.WorldJobMaskReviewAudio, result, (long)bytes.Length);
				return;
			}
			this.ErrorInfoList.Add(new UploadWindowBase.ErrorInfo(code, message));
		});
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0001F94D File Offset: 0x0001DB4D
	protected void ResetReviewFlag()
	{
		this.m_uploadImage = 0;
		this.m_uploadAudio = 0;
		this.Audios.Clear();
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0001F968 File Offset: 0x0001DB68
	protected bool IsUploadImageSuccess()
	{
		return this.m_uploadImage == 9;
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0001F974 File Offset: 0x0001DB74
	protected bool IsUploadAudioSuccess()
	{
		return this.m_uploadAudio == this.Audios.Count;
	}

	// Token: 0x04000290 RID: 656
	protected bool TogglePublic;

	// Token: 0x04000291 RID: 657
	protected bool TogglePrivate;

	// Token: 0x04000292 RID: 658
	protected string WorldName = "";

	// Token: 0x04000293 RID: 659
	protected string Description = "";

	// Token: 0x04000294 RID: 660
	protected Texture2D WorldTexture16_9;

	// Token: 0x04000295 RID: 661
	protected Texture2D WorldTexture1_1;

	// Token: 0x04000296 RID: 662
	protected bool IsActiveCarmera16_9;

	// Token: 0x04000297 RID: 663
	protected bool IsActiveCarmera1_1;

	// Token: 0x04000298 RID: 664
	protected Rect TextureRect16_9 = new Rect(0f, 0f, 1600f, 900f);

	// Token: 0x04000299 RID: 665
	protected Rect TextureRect1_1 = new Rect(0f, 0f, 900f, 900f);

	// Token: 0x0400029A RID: 666
	protected Rect TextureDisplayRect16_9 = new Rect(0f, 0f, 264f, 148f);

	// Token: 0x0400029B RID: 667
	protected Rect TextureDisplayRect1_1 = new Rect(0f, 0f, 148f, 148f);

	// Token: 0x0400029C RID: 668
	protected bool IsUpLoadImage16_9;

	// Token: 0x0400029D RID: 669
	protected bool IsUpLoadImage1_1;

	// Token: 0x0400029E RID: 670
	protected WorldUploadWindowState State;

	// Token: 0x0400029F RID: 671
	protected bool IsMainTexture;

	// Token: 0x040002A0 RID: 672
	protected string JobId;

	// Token: 0x040002A1 RID: 673
	protected JsonData ResInfo;

	// Token: 0x040002A2 RID: 674
	protected string UploadToken;

	// Token: 0x040002A3 RID: 675
	protected bool IsUploading;

	// Token: 0x040002A4 RID: 676
	protected string ContentId;

	// Token: 0x040002A5 RID: 677
	protected int Capacity = 20;

	// Token: 0x040002A6 RID: 678
	protected Vector2 Pos;

	// Token: 0x040002A7 RID: 679
	protected Vector2 DesScrollPos;

	// Token: 0x040002A8 RID: 680
	protected List<string> TagList = new List<string>();

	// Token: 0x040002A9 RID: 681
	protected bool IsTextureChanged;

	// Token: 0x040002AA RID: 682
	protected bool IsIconChanged;

	// Token: 0x040002AB RID: 683
	protected bool IsModifyTexture;

	// Token: 0x040002AC RID: 684
	protected bool IsModifyIcon;

	// Token: 0x040002AD RID: 685
	protected GUIStyle Style9;

	// Token: 0x040002AE RID: 686
	protected GUILayoutOption[] SvLp;

	// Token: 0x040002AF RID: 687
	protected GUILayoutOption[] ContainerLp;

	// Token: 0x040002B0 RID: 688
	protected GUILayoutOption[] SvLpTa;

	// Token: 0x040002B1 RID: 689
	protected string ExportWorldPath;

	// Token: 0x040002B2 RID: 690
	private int DelTagIndex = -1;

	// Token: 0x040002B3 RID: 691
	protected List<UploadWindowBase.ErrorInfo> ErrorInfoList = new List<UploadWindowBase.ErrorInfo>();

	// Token: 0x040002B4 RID: 692
	private GUIContent ContentArea = new GUIContent();

	// Token: 0x040002B5 RID: 693
	protected string ParaWorldRootKey = "";

	// Token: 0x040002B6 RID: 694
	protected string RemoteTexturePath;

	// Token: 0x040002B7 RID: 695
	protected string RemoteIconPath;

	// Token: 0x040002B8 RID: 696
	protected string RemoteTexture;

	// Token: 0x040002B9 RID: 697
	protected string RemoteIcon;

	// Token: 0x040002BA RID: 698
	public List<string> Audios = new List<string>();

	// Token: 0x040002BB RID: 699
	private readonly string[] m_imageName = new string[] { "review_spawn_forward.png", "review_spawn_back.png", "review_spawn_left.png", "review_spawn_right.png", "review_up.png", "review_forward.png", "review_back.png", "review_left.png", "review_right.png" };

	// Token: 0x040002BC RID: 700
	private int m_uploadImage;

	// Token: 0x040002BD RID: 701
	private int m_uploadAudio;

	// Token: 0x040002BE RID: 702
	private Camera m_cameraReview;

	// Token: 0x040002BF RID: 703
	private readonly Rect m_textureRectReview = new Rect(0f, 0f, 1600f, 900f);

	// Token: 0x020000DA RID: 218
	private enum ReviewImageType
	{
		// Token: 0x04000409 RID: 1033
		SpawnForward,
		// Token: 0x0400040A RID: 1034
		SpawnBack,
		// Token: 0x0400040B RID: 1035
		SpawnLeft,
		// Token: 0x0400040C RID: 1036
		SpawnRight,
		// Token: 0x0400040D RID: 1037
		Top,
		// Token: 0x0400040E RID: 1038
		Forward,
		// Token: 0x0400040F RID: 1039
		Back,
		// Token: 0x04000410 RID: 1040
		Left,
		// Token: 0x04000411 RID: 1041
		Right
	}
}
