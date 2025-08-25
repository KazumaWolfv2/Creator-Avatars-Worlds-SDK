using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class AvatarUploadWindow : UploadWindowBase
{
	// Token: 0x060002EF RID: 751 RVA: 0x00012D28 File Offset: 0x00010F28
	protected void OnEnable()
	{
		float num = (float)Screen.currentResolution.height * 5f / 12f;
		if (num > 640f)
		{
			base.minSize = new Vector2(500f, 640f);
			base.maxSize = new Vector2(500f, 640f);
		}
		else
		{
			base.minSize = new Vector2(500f, num);
			base.maxSize = new Vector2(500f, num + 20f);
		}
		base.titleContent.text = SdkLangManager.Get("str_sdk_avatarPublish_title0");
		this.IsUpLoadTexture = false;
		this.IsUpLoadPackage = false;
		this.IsModifyTexture = false;
		this.JobId = null;
		this.ContentId = "";
		this.Pos = Vector2.zero;
		this.SvLp = new GUILayoutOption[] { GUILayout.Height(base.minSize.y) };
		this.SvLpTa = new GUILayoutOption[] { GUILayout.Height(152f) };
		this.ContainerLp = new GUILayoutOption[0];
		this.TagList.Clear();
		this.TagList.Add("");
		this.TagList.Add("");
		this.ToggleValue = true;
		this.ToggleValueMarket = true;
		this.ToggleValueClone = true;
		this.DelTagIndex = -1;
		this.RemoteTexture = null;
		this.RemoteTexturePath = null;
		this.ExportAvatarPath = ParaPathDefine.kSdkDownLoadPath;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00012E98 File Offset: 0x00011098
	private void InitGUIStyle()
	{
		if (this.Style9 == null)
		{
			this.Style9 = new GUIStyle(EditorStyles.wordWrappedLabel);
			this.Style9.fontSize = 9;
		}
		if (this.Style10 == null)
		{
			this.Style10 = new GUIStyle(EditorStyles.miniButton);
			this.Style10.fontSize = 9;
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00012EF0 File Offset: 0x000110F0
	public void RefreshAvatarData(AvatarData data)
	{
		this.AvatarName = data.Name;
		this.ContentId = data.ContentID;
		this.TagList = data.HashTags;
		this.Description = data.Description;
		this.ToggleValue = data.SharingStauts == 1;
		this.RemoteTexture = data.RemoteTexturePath;
		this.RemoteTexturePath = data.TexturePath;
		if (data.SharingChannel.Count > 0)
		{
			this.ToggleValueMarket = data.SharingChannel[0] == 1;
		}
		if (data.SharingChannel.Count > 1)
		{
			this.ToggleValueClone = data.SharingChannel[1] == 1;
		}
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00012F9C File Offset: 0x0001119C
	private void OnGUI()
	{
		this.InitGUIStyle();
		this.Pos = GUILayout.BeginScrollView(this.Pos, this.SvLp);
		GUILayout.BeginVertical(this.ContainerLp);
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(25f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		this.OnGuiTop();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.DrawDetailTexture();
		this.OnDrawTakeDown();
		this.DrawLeftDes();
		GUILayout.EndHorizontal();
		this.DrawAvatarTags();
		GUILayout.Space(18f);
		this.DrawPublic();
		GUILayout.Space(6f);
		this.DrawPublicItem();
		GUILayout.Space(4f);
		this.DrawBottom();
		GUILayout.Space(4f);
		this.DrawAiNpcItem();
		GUILayout.Space(5f);
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.OnGuiBottomBtn();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		if (this.DelTagIndex > -1)
		{
			this.TagList.RemoveAt(this.DelTagIndex);
			this.DelTagIndex = -1;
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000130D0 File Offset: 0x000112D0
	private void DrawDetailTexture()
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarPublish_Field0"), EditorStyles.boldLabel, new GUILayoutOption[]
		{
			GUILayout.Width(130f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-2f);
		EditorUtil.DrawTexture(this.TextureRectDisplay, this.AvatarTexture, SdkLangManager.Get("str_sdk_avatarPublish_imageField"));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-3f);
		this.OnGuiMiddleBeginBtn();
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_worldPublish_title0"), this.Style10, new GUILayoutOption[]
		{
			GUILayout.Width(78f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.UpLoadFile();
			GUIUtility.ExitGUI();
		}
		this.OnGuiMiddleEndBtn();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000131D8 File Offset: 0x000113D8
	private void DrawLeftDes()
	{
		GUILayout.Space(-17f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		EditorUtil.DrawRedStar();
		GUILayout.Space(-6f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_field0", EditorUtil.GetRealStringCount(this.AvatarName)), new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.EndHorizontal();
		this.AvatarName = GUILayout.TextField(this.AvatarName, new GUILayoutOption[]
		{
			GUILayout.Width(285f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		this.AvatarName = this.AvatarName.TrimStart();
		if (EditorUtil.GetRealStringCount(this.AvatarName) > 30)
		{
			this.AvatarName = EditorUtil.Clamp(this.AvatarName, 30);
		}
		if (this.AvatarName.Equals(""))
		{
			GUILayout.Space(-27f);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text0"), new GUILayoutOption[]
			{
				GUILayout.Height(25f),
				GUILayout.ExpandWidth(false)
			});
			GUI.color = Color.white;
		}
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_field2", EditorUtil.GetRealStringCount(this.Description)), new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.EndHorizontal();
		this.contentArea.text = this.Description;
		if (GUI.skin.textArea.CalcHeight(this.contentArea, 280f) > 146f)
		{
			this.PosTa = GUILayout.BeginScrollView(this.PosTa, this.SvLpTa);
			GUILayout.BeginVertical(this.ContainerLp);
			this.Description = GUILayout.TextArea(this.Description, 500, new GUILayoutOption[]
			{
				GUILayout.Width(285f),
				GUILayout.ExpandHeight(true),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}
		else
		{
			this.Description = GUILayout.TextArea(this.Description, 500, new GUILayoutOption[]
			{
				GUILayout.Width(285f),
				GUILayout.Height(152f),
				GUILayout.ExpandHeight(false),
				GUILayout.ExpandWidth(false)
			});
		}
		this.Description = this.Description.TrimStart();
		if (EditorUtil.GetRealStringCount(this.Description) > 500)
		{
			EditorUtil.Clamp(this.Description, 500);
		}
		if (this.Description.Equals(""))
		{
			GUILayout.Space(-152f);
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text2"), new GUILayoutOption[]
			{
				GUILayout.Height(285f),
				GUILayout.Height(20f),
				GUILayout.ExpandWidth(false)
			});
			GUI.color = Color.white;
			GUILayout.Space(130f);
		}
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text3"), new GUILayoutOption[]
		{
			GUILayout.Width(280f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text4"), this.Style9, new GUILayoutOption[]
		{
			GUILayout.Width(280f),
			GUILayout.ExpandWidth(false)
		});
		GUI.skin.textField.fontSize = 12;
		GUILayout.EndVertical();
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00013584 File Offset: 0x00011784
	private void DrawAvatarTags()
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(2f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_field3"), Array.Empty<GUILayoutOption>());
		GUILayout.Space(8f);
		int num = ((this.TagList.Count == 6) ? 10 : 40) + this.TagList.Count * 28;
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height((float)num),
			GUILayout.ExpandHeight(false)
		});
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

	// Token: 0x060002F6 RID: 758 RVA: 0x000136D8 File Offset: 0x000118D8
	private void DrawPublic()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		EditorUtil.DrawRedStar();
		GUILayout.Space(-6f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text5"), new GUILayoutOption[]
		{
			GUILayout.Width(250f),
			GUILayout.Height(20f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(22f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		this.ToggleValue = GUILayout.Toggle(this.ToggleValue, SdkLangManager.Get("str_sdk_yes"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(2f);
		this.ToggleValue = !GUILayout.Toggle(!this.ToggleValue, SdkLangManager.Get("str_sdk_no"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x000137EC File Offset: 0x000119EC
	private void DrawPublicItem()
	{
		if (this.ToggleValue)
		{
			GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text6"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.Space(2f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(22f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.ToggleValueMarket = GUILayout.Toggle(this.ToggleValueMarket, SdkLangManager.Get("str_sdk_avatarContent_text7"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.Space(4f);
			this.ToggleValueClone = GUILayout.Toggle(this.ToggleValueClone, SdkLangManager.Get("str_sdk_avatarContent_text8"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(78f);
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x000138E4 File Offset: 0x00011AE4
	private void DrawAiNpcItem()
	{
		if (this.ToggleValue)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text31"), new GUILayoutOption[]
			{
				GUILayout.Width(250f),
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(22f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.ToggleAiNpcValue = GUILayout.Toggle(this.ToggleAiNpcValue, SdkLangManager.Get("str_sdk_yes"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.Space(2f);
			this.ToggleAiNpcValue = !GUILayout.Toggle(!this.ToggleAiNpcValue, SdkLangManager.Get("str_sdk_no"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(78f);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000139FD File Offset: 0x00011BFD
	private void DrawBottom()
	{
		GUILayout.Box("", new GUILayoutOption[]
		{
			GUILayout.Width(370f),
			GUILayout.Height(1f),
			GUILayout.ExpandWidth(true)
		});
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00013A34 File Offset: 0x00011C34
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
		GUILayout.Space(-50f);
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
			GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text9"), new GUILayoutOption[]
			{
				GUILayout.Height(20f),
				GUILayout.ExpandHeight(false),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUI.color = Color.white;
		}
		GUILayout.Space(-28f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(395f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarContent_btn2"), new GUILayoutOption[]
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
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00013CAC File Offset: 0x00011EAC
	private void UpLoadFile()
	{
		ParaLog.Log("UpLoadFile.");
		this.IsModifyTexture = true;
		this.IsActiveCarmera = false;
		DateTime now = DateTime.Now;
		CommonImageSelectWindow window = EditorWindow.GetWindow<CommonImageSelectWindow>();
		Action<Texture2D> action = delegate(Texture2D texture)
		{
			if (texture == null)
			{
				this.IsModifyTexture = false;
				this.IsActiveCarmera = true;
				return;
			}
			this.AvatarTexture = texture;
		};
		window.FetchTexture(this.TextureRect, "Avatar World Upload", "The aspect ratio is fixed at 9:16", action);
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00013D60 File Offset: 0x00011F60
	protected void ShowAvatarName()
	{
		EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_avatarContent_text12"), "OK", null);
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00013D81 File Offset: 0x00011F81
	protected void ShowChannels()
	{
		EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_avatarContent_text13"), "OK", null);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00013DA4 File Offset: 0x00011FA4
	protected bool BuildPackage(string path, string basePath, bool isRetry, out string packagePath, out string fileName)
	{
		if (!isRetry)
		{
			if (!File.Exists(Application.dataPath.Replace("Assets", path)))
			{
				this._lastBuildPackagePath = null;
				this._lastBuildFileName = null;
				packagePath = null;
				fileName = null;
				return false;
			}
			this._lastBuildFileName = PackageProxy.Build(path, basePath);
			this._lastBuildPackagePath = Path.Combine(basePath, this._lastBuildFileName);
		}
		packagePath = this._lastBuildPackagePath;
		fileName = this._lastBuildFileName;
		return true;
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00013E18 File Offset: 0x00012018
	protected bool UploadPackage(string packagePath, string fileName)
	{
		if (!SdkManager.Instance.IsSameJob(this.JobId))
		{
			return false;
		}
		if (!File.Exists(packagePath))
		{
			return false;
		}
		byte[] bytes = File.ReadAllBytes(packagePath);
		CommonNetProxy.UploadPackage(this.UploadToken, this.JobId, bytes, fileName, delegate(int code, string message, JsonData result)
		{
			ParaLog.Log("code---1>: " + code.ToString());
			ParaLog.Log("message---1>: " + message);
			if (code == 0)
			{
				this.IsUpLoadPackage = true;
				CommonNetProxy.AddJobResInfo(ref this.ResInfo, "dev_pack", result, (long)bytes.Length);
				return;
			}
			this.ErrorInfoList.Add(new UploadWindowBase.ErrorInfo(code, message));
		});
		return true;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00013E84 File Offset: 0x00012084
	protected bool UploadPng(string basePath)
	{
		if (!SdkManager.Instance.IsSameJob(this.JobId))
		{
			return false;
		}
		string text = basePath + "/avatar.png";
		EditorUtil.SaveTextureToLocal(this.AvatarTexture, text);
		byte[] bytes = File.ReadAllBytes(text);
		CommonNetProxy.UploadTexture(this.UploadToken, this.JobId, AvatarResType.AvatarResTypeCoverImg, bytes, "avatar.png", delegate(int code, string message, JsonData result)
		{
			ParaLog.Log("code---2>: " + code.ToString());
			ParaLog.Log("message---2>: " + message);
			if (code == 0)
			{
				this.IsUpLoadTexture = true;
				CommonNetProxy.AddJobResInfo(ref this.ResInfo, AvatarResType.AvatarResTypeCoverImg, result, (long)bytes.Length);
				return;
			}
			this.ErrorInfoList.Add(new UploadWindowBase.ErrorInfo(code, message));
		});
		return true;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnSelectTexture(Texture2D texture)
	{
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiBottomBtn()
	{
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiMiddleBeginBtn()
	{
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiMiddleEndBtn()
	{
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnGuiTop()
	{
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void OnDrawTakeDown()
	{
	}

	// Token: 0x04000190 RID: 400
	protected bool ToggleValue;

	// Token: 0x04000191 RID: 401
	protected bool ToggleValueMarket;

	// Token: 0x04000192 RID: 402
	protected bool ToggleValueClone;

	// Token: 0x04000193 RID: 403
	protected Texture2D AvatarTexture;

	// Token: 0x04000194 RID: 404
	protected string AvatarName = "";

	// Token: 0x04000195 RID: 405
	protected string Description = "";

	// Token: 0x04000196 RID: 406
	private Vector2 Pos;

	// Token: 0x04000197 RID: 407
	protected List<string> TagList = new List<string>();

	// Token: 0x04000198 RID: 408
	protected bool IsUpLoadTexture;

	// Token: 0x04000199 RID: 409
	protected bool IsUpLoadPackage;

	// Token: 0x0400019A RID: 410
	protected bool IsActiveCarmera;

	// Token: 0x0400019B RID: 411
	protected Rect TextureRect = new Rect(0f, 0f, 675f, 900f);

	// Token: 0x0400019C RID: 412
	protected Rect TextureRectDisplay = new Rect(0f, 0f, 160f, 210f);

	// Token: 0x0400019D RID: 413
	protected string JobId;

	// Token: 0x0400019E RID: 414
	protected JsonData ResInfo;

	// Token: 0x0400019F RID: 415
	protected string UploadToken;

	// Token: 0x040001A0 RID: 416
	protected bool IsModifyTexture;

	// Token: 0x040001A1 RID: 417
	protected GUIStyle Style9;

	// Token: 0x040001A2 RID: 418
	protected GUIStyle Style10;

	// Token: 0x040001A3 RID: 419
	protected GUILayoutOption[] SvLp;

	// Token: 0x040001A4 RID: 420
	protected GUILayoutOption[] ContainerLp;

	// Token: 0x040001A5 RID: 421
	protected GUILayoutOption[] SvLpTa;

	// Token: 0x040001A6 RID: 422
	protected AvatarUploadWindowState State;

	// Token: 0x040001A7 RID: 423
	protected bool IsUploading;

	// Token: 0x040001A8 RID: 424
	private int DelTagIndex = -1;

	// Token: 0x040001A9 RID: 425
	protected List<UploadWindowBase.ErrorInfo> ErrorInfoList = new List<UploadWindowBase.ErrorInfo>();

	// Token: 0x040001AA RID: 426
	private Vector2 PosTa = Vector2.zero;

	// Token: 0x040001AB RID: 427
	private GUIContent contentArea = new GUIContent();

	// Token: 0x040001AC RID: 428
	protected string ExportAvatarPath;

	// Token: 0x040001AD RID: 429
	protected string ContentId = "";

	// Token: 0x040001AE RID: 430
	protected string RemoteTexture;

	// Token: 0x040001AF RID: 431
	protected string RemoteTexturePath;

	// Token: 0x040001B0 RID: 432
	protected bool ToggleAiNpcValue = true;

	// Token: 0x040001B1 RID: 433
	private string _lastBuildPackagePath;

	// Token: 0x040001B2 RID: 434
	private string _lastBuildFileName;
}
