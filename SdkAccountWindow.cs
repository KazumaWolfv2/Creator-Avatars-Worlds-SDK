using System;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class SdkAccountWindow : EditorWindow
{
	// Token: 0x06000428 RID: 1064 RVA: 0x0001C38C File Offset: 0x0001A58C
	private void OnEnable()
	{
		this.enableTime = DateTime.Now;
		base.minSize = new Vector2(500f, 775f);
		base.maxSize = new Vector2(500f, 775f);
		this.IsLogin = SdkManager.Instance.GetAccount().IsLogin();
		this.UserName = PlayerPrefs.GetString("UserName");
		this.ToggleValueAccept = false;
		this.DisplayName = null;
		this.Info = new SdkAccount.SdkAccountInfo();
		base.titleContent.text = SdkLangManager.Get("str_sdk_accountWindow_text1");
		this.Texture = EditorUtil.LoadTexture("Packages/com.para.common/Config/Texture/EditorSdkTopBar.png");
		this.TitleStyle = new GUIStyle(EditorStyles.boldLabel);
		this.TitleStyle.fontSize = 18;
		this.ErrorInfo = SdkLangManager.Get("str_sdk_accountWindow_text2");
		if (this.IsLogin)
		{
			this.FetchAccountInfo();
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001C46C File Offset: 0x0001A66C
	private void OnDisable()
	{
		this.TitleStyle = null;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001C475 File Offset: 0x0001A675
	private void FetchAccountInfo()
	{
		CommonNetProxy.FetchAccountInfo(delegate
		{
			this.Info = SdkManager.Instance.GetAccount().GetInfo();
			SdkManager.Instance.GetTextureDownload().DownLoadTexture(this.Info.Uri, this.Info.Url, SdkLangManager.Get("str_sdk_accountWindow_title3"));
		});
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001C488 File Offset: 0x0001A688
	private void OnGUI()
	{
		GUILayout.Label(this.Texture, new GUILayoutOption[]
		{
			GUILayout.Width(500f),
			GUILayout.Height(125f),
			GUILayout.ExpandWidth(true),
			GUILayout.ExpandHeight(false)
		});
		if (SdkManager.Instance.GetAccount().IsLogin())
		{
			this.OnGUILogIn();
			return;
		}
		this.OnGUILoginOut();
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
	private void OnGUILogIn()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(22f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text4"), this.TitleStyle, Array.Empty<GUILayoutOption>());
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(185f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-185f);
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(25f);
		this.OnGUILoginTexture();
		GUILayout.Space(10f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		if (string.IsNullOrEmpty(this.DisplayName))
		{
			if (EditorUtil.GetRealStringCount(this.Info.NickName) <= 18)
			{
				this.DisplayName = this.Info.NickName;
			}
			else
			{
				this.DisplayName = EditorUtil.Clamp(this.Info.NickName, 18) + SdkLangManager.Get("str_sdk_CheckDetailAvatar_text0");
			}
		}
		GUILayout.Label(this.DisplayName, this.TitleStyle, new GUILayoutOption[]
		{
			GUILayout.Width(250f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(40f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text6") + this.Info.AvatarCount.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(15f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text7") + this.Info.WorldCount.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(15f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text8") + this.Info.ReviewCount.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(15f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(150f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_accountWindow_btn0"), new GUILayoutOption[]
		{
			GUILayout.Width(100f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.OnLogout();
			this.Info.Clear();
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(40f);
		GUILayout.Box("", GUI.skin.button, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(1f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0001C81C File Offset: 0x0001AA1C
	private void OnGUILoginTexture()
	{
		if (this.IconTexture == null && SdkManager.Instance.GetTextureDownload().IsDownLoaded(this.Info.Uri))
		{
			this.IconTexture = SdkManager.Instance.GetTextureDownload().LoadTexture(this.Info.Uri);
			if (this.IconTexture == null)
			{
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(this.Info.Uri, this.Info.Url, SdkLangManager.Get("str_sdk_accountWindow_title3"));
			}
			else
			{
				base.Repaint();
			}
		}
		EditorUtil.DrawTexture(150, 150, this.IconTexture, SdkLangManager.Get("str_sdk_accountWindow_text10"));
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001C8DC File Offset: 0x0001AADC
	private void OnGUILoginOut()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(22f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("tr_sdk_accountWindow_btn1"), this.TitleStyle, Array.Empty<GUILayoutOption>());
		GUILayout.Space(4f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(240f),
			GUILayout.ExpandHeight(false),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-230f);
		GUILayout.Space(15f);
		this.OnGUIUserName();
		GUILayout.Space(10f);
		this.OnGUIPassword();
		GUILayout.Space(10f);
		this.OnGUITerm();
		GUILayout.Space(4f);
		this.OnGUILoginButton();
		GUILayout.Space(15f);
		this.OnGUIDes();
		GUILayout.Space(40f);
		GUILayout.Box("", GUI.skin.button, new GUILayoutOption[]
		{
			GUILayout.Width(450f),
			GUILayout.Height(1f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0001CA1C File Offset: 0x0001AC1C
	private void OnGUITerm()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(90f);
		this.ToggleValueAccept = GUILayout.Toggle(this.ToggleValueAccept, "", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.Label(SdkLangManager.Get("tr_sdk_accountWindow_btn2"), new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
		GUILayout.Space(-2f);
		GUI.color = Color.red;
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text13"), new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(false),
			GUILayout.Width(160f)
		});
		GUI.color = Color.white;
		GUILayout.Space(-85f);
		if (GUILayout.Button("", GUIStyle.none, new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(22f),
			GUILayout.ExpandWidth(false)
		}))
		{
			Application.OpenURL("https://www.paraspace.tech/terms-of-service");
			GUIUtility.ExitGUI();
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0001CB24 File Offset: 0x0001AD24
	private void OnGUIUserName()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text14"), new GUILayoutOption[]
		{
			GUILayout.Width(65f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		this.UserName = GUILayout.TextField(this.UserName, new GUILayoutOption[]
		{
			GUILayout.Width(340f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-340f);
		if (string.IsNullOrEmpty(this.UserName))
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text14"), new GUILayoutOption[]
			{
				GUILayout.Width(340f),
				GUILayout.Height(20f),
				GUILayout.ExpandWidth(false)
			});
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001CC48 File Offset: 0x0001AE48
	private void OnGUIPassword()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text16"), new GUILayoutOption[]
		{
			GUILayout.Width(65f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		this.Password = GUILayout.PasswordField(this.Password, "*"[0], 64, new GUILayoutOption[]
		{
			GUILayout.Width(340f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-340f);
		if (string.IsNullOrEmpty(this.Password))
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_accountWindow_text16"), new GUILayoutOption[]
			{
				GUILayout.Width(340f),
				GUILayout.Height(20f),
				GUILayout.ExpandWidth(false)
			});
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001CD78 File Offset: 0x0001AF78
	private void OnGUILoginButton()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(150f);
		GUI.enabled = this.ToggleValueAccept;
		if (!this.ToggleValueAccept)
		{
			GUI.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		}
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_accountWindow_text18"), new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(30f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.Login();
			GUIUtility.ExitGUI();
		}
		GUI.enabled = true;
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0001CE38 File Offset: 0x0001B038
	private void OnGUIDes()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(415f),
			GUILayout.Height(50f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(-415f);
		GUILayout.Label(this.ErrorInfo, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
		{
			GUILayout.Width(415f),
			GUILayout.Height(50f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0001CEDE File Offset: 0x0001B0DE
	public void SetAction(Action action)
	{
		this.PostAction = action;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0001CEE8 File Offset: 0x0001B0E8
	private async void Login()
	{
		EditorUtility.DisplayProgressBar(SdkLangManager.Get("tr_sdk_accountWindow_btn3"), SdkLangManager.Get("str_sdk_accountWindow_text20"), 0.3f);
		this.DisplayName = null;
		DateTime startTime = DateTime.Now;
		TimeSpan timeSpan = new TimeSpan(startTime.Ticks - this.enableTime.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_login";
		jsonData["login_way"] = 1;
		jsonData["duration"] = (int)(timeSpan.Ticks / 10000L);
		AppLogService.PushAppLog(jsonData);
		try
		{
			await CommonNetProxy.LoginAsync(this.UserName, this.Password, delegate(string message, string openID, string tokenId, string paraID)
			{
				if (string.IsNullOrEmpty(message))
				{
					PlayerPrefs.SetString("UserName", this.UserName);
					SdkManager.Instance.GetAccount().Login(openID, tokenId, paraID);
					this.OnLogin();
					TimeSpan timeSpan3 = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
					JsonData jsonData3 = new JsonData();
					jsonData3["event"] = "unity_editor_login_result";
					jsonData3["is_success"] = 1;
					jsonData3["duration"] = timeSpan3.Milliseconds;
					jsonData3["error_code"] = "null";
					jsonData3["error_message"] = "null";
					AppLogService.PushAppLog(jsonData3);
					return;
				}
				EditorUtility.ClearProgressBar();
				TimeSpan timeSpan4 = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
				JsonData jsonData4 = new JsonData();
				jsonData4["event"] = "unity_editor_login_result";
				jsonData4["is_success"] = 0;
				jsonData4["duration"] = timeSpan4.Milliseconds;
				jsonData4["error_code"] = "2";
				jsonData4["error_message"] = message;
				AppLogService.PushAppLog(jsonData4);
				this.ErrorInfo = message;
			});
		}
		catch (Exception ex)
		{
			EditorUtility.ClearProgressBar();
			TimeSpan timeSpan2 = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
			JsonData jsonData2 = new JsonData();
			jsonData2["event"] = "unity_editor_login_result";
			jsonData2["is_success"] = 0;
			jsonData2["duration"] = timeSpan2.Milliseconds;
			jsonData2["error_code"] = "1";
			jsonData2["error_message"] = ex.Message;
			AppLogService.PushAppLog(jsonData2);
			EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_accountWindow_title4"), ex.Message, "OK", null);
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x0001CF1F File Offset: 0x0001B11F
	private void OnLogin()
	{
		EditorUtility.DisplayProgressBar(SdkLangManager.Get("tr_sdk_accountWindow_btn3"), SdkLangManager.Get("str_sdk_accountWindow_text20"), 0.8f);
		this.LoginWithToken(delegate(bool isSuccess, string message)
		{
			if (!isSuccess)
			{
				if (!string.IsNullOrEmpty(message))
				{
					this.ErrorInfo = message;
				}
				EditorUtility.ClearProgressBar();
				MenuBars.Login(this.PostAction);
				return;
			}
			this.IsLogin = SdkManager.Instance.GetAccount().IsLogin();
			this.FetchAccountInfo();
			EditorUtility.ClearProgressBar();
			EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_accountWindow_text26"), "OK", null);
			Action postAction = this.PostAction;
			if (postAction == null)
			{
				return;
			}
			postAction();
		});
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0001CF54 File Offset: 0x0001B154
	private void LoginWithToken(Action<bool, string> action)
	{
		SdkAccount account = SdkManager.Instance.GetAccount();
		string openId = account.GetOpenId();
		string token = account.GetToken();
		if (string.IsNullOrEmpty(openId) || string.IsNullOrEmpty(token))
		{
			if (action != null)
			{
				action(false, null);
			}
			return;
		}
		DateTime startTime = DateTime.Now;
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_login";
		jsonData["login_way"] = 2;
		jsonData["duration"] = 0;
		AppLogService.PushAppLog(jsonData);
		CommonNetProxy.LoginWithToken(openId, token, delegate(int code, string message, JsonData data)
		{
			if (code != 0)
			{
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_sdkMessage_text0"), message, "OK", null);
				if (action != null)
				{
					action(false, message);
				}
				TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
				JsonData jsonData2 = new JsonData();
				jsonData2["event"] = "unity_editor_login_result";
				jsonData2["is_success"] = 0;
				jsonData2["duration"] = (int)(timeSpan.Ticks / 10000L);
				jsonData2["error_code"] = code.ToString();
				jsonData2["error_message"] = message;
				AppLogService.PushAppLog(jsonData2);
				return;
			}
			account.SetJwt(data["user_token"].ToString(), false);
			TimeSpan timeSpan2 = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
			JsonData jsonData3 = new JsonData();
			jsonData3["event"] = "unity_editor_login_result";
			jsonData3["is_success"] = 1;
			jsonData3["duration"] = timeSpan2.Milliseconds;
			jsonData3["error_code"] = "null";
			jsonData3["error_message"] = "null";
			AppLogService.PushAppLog(jsonData3);
			if (action != null)
			{
				action(true, null);
			}
		});
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0001D01C File Offset: 0x0001B21C
	private void OnLogout()
	{
		this.DisplayName = null;
		this.ErrorInfo = SdkLangManager.Get("str_sdk_accountWindow_text27");
		SdkManager.Instance.GetAccount().Logout();
		this.IsLogin = SdkManager.Instance.GetAccount().IsLogin();
		this.IconTexture = null;
	}

	// Token: 0x0400024B RID: 587
	private bool IsLogin;

	// Token: 0x0400024C RID: 588
	private Action PostAction;

	// Token: 0x0400024D RID: 589
	private string UserName = "";

	// Token: 0x0400024E RID: 590
	private string Password = "";

	// Token: 0x0400024F RID: 591
	private Texture2D Texture;

	// Token: 0x04000250 RID: 592
	private Texture2D IconTexture;

	// Token: 0x04000251 RID: 593
	private GUIStyle TitleStyle;

	// Token: 0x04000252 RID: 594
	private string DisplayName;

	// Token: 0x04000253 RID: 595
	private DateTime enableTime;

	// Token: 0x04000254 RID: 596
	private string ErrorInfo;

	// Token: 0x04000255 RID: 597
	private bool ToggleValueAccept;

	// Token: 0x04000256 RID: 598
	private SdkAccount.SdkAccountInfo Info;
}
