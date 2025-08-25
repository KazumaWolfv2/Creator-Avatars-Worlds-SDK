using System;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class ContentManagerWindow : EditorWindow
{
	// Token: 0x06000386 RID: 902 RVA: 0x0001782A File Offset: 0x00015A2A
	public void OnEnableCloudAll()
	{
		this.IsFetchCloudAll = false;
		this.AllSearchAvatarJobList = new List<AvatarJobData>();
		this.AllSearchWorldJobList = new List<WorldJobData>();
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001784C File Offset: 0x00015A4C
	private void ClearCloudAll()
	{
		this.IsFetchCloudAll = false;
		List<TypePair> indexList = this.IndexList;
		if (indexList != null)
		{
			indexList.Clear();
		}
		List<TypePair> searchIndexList = this.SearchIndexList;
		if (searchIndexList != null)
		{
			searchIndexList.Clear();
		}
		List<AvatarJobData> allAvatarJobList = this.AllAvatarJobList;
		if (allAvatarJobList != null)
		{
			allAvatarJobList.Clear();
		}
		List<AvatarJobData> allSearchAvatarJobList = this.AllSearchAvatarJobList;
		if (allSearchAvatarJobList != null)
		{
			allSearchAvatarJobList.Clear();
		}
		this.AllSearchAvatarJobList.Clear();
		this.AllSearchWorldJobList.Clear();
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000178BC File Offset: 0x00015ABC
	private void DrawCloudAll()
	{
		if (!this.IsFetchCloudAll)
		{
			this.IsFetchCloudAll = true;
			this.UpdateAllCloudList();
		}
		List<TypePair> list;
		if (this.IsSearch)
		{
			list = this.SearchIndexList;
		}
		else
		{
			list = this.IndexList;
		}
		if (list == null)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(130f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text13"), this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(90f);
			return;
		}
		if (list.Count != 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.DrawJobItem(i, list[i]);
			}
			GUILayout.Space(5f);
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00017974 File Offset: 0x00015B74
	private void DrawJobItem(int index, TypePair type)
	{
		if (type.Type == 0)
		{
			AvatarJobData avatarJobData = (this.IsSearch ? this.AllSearchAvatarJobList[type.Index] : this.AllAvatarJobList[type.Index]);
			this.DrawAvatarJobItem(index, avatarJobData);
			return;
		}
		WorldJobData worldJobData = (this.IsSearch ? this.AllSearchWorldJobList[type.Index] : this.AllWorldJobList[type.Index]);
		this.DrawWorldJobItem(index, worldJobData);
	}

	// Token: 0x0600038A RID: 906 RVA: 0x000179F4 File Offset: 0x00015BF4
	private void UpdateAllCloudList()
	{
		CommonNetProxy.FetchAllJobList(null, this.GetProcessKey(this.CloudProcessIndex), delegate(List<AvatarJobData> avatarDataList, List<WorldJobData> worldDataList, List<TypePair> indexList)
		{
			if (this.IndexList == null)
			{
				this.IndexList = new List<TypePair>();
			}
			else
			{
				this.IndexList.Clear();
			}
			if (this.AllAvatarJobList == null)
			{
				this.AllAvatarJobList = new List<AvatarJobData>();
			}
			else
			{
				this.AllAvatarJobList.Clear();
			}
			if (this.AllWorldJobList == null)
			{
				this.AllWorldJobList = new List<WorldJobData>();
			}
			else
			{
				this.AllWorldJobList.Clear();
			}
			this.IndexList = indexList;
			this.AllAvatarJobList.AddRange(avatarDataList);
			this.AllWorldJobList.AddRange(worldDataList);
			this.DownLoadAllCloudJobList();
		});
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00017A14 File Offset: 0x00015C14
	private void OnCloudSelectAll()
	{
		this.ClearCloudAvatar();
		this.ClearCloudWorld();
		this.ClearCloudAll();
		this.UpdateAllCloudList();
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00017A2E File Offset: 0x00015C2E
	public void UpdateCloudSearchAll()
	{
		CommonNetProxy.FetchAllJobList(this.SearchName, this.GetProcessKey(this.CloudProcessIndex), delegate(List<AvatarJobData> avatarDataList, List<WorldJobData> worldDataList, List<TypePair> indexList)
		{
			if (this.SearchIndexList == null)
			{
				this.SearchIndexList = new List<TypePair>();
			}
			this.SearchIndexList.Clear();
			this.AllSearchAvatarJobList.Clear();
			this.AllSearchWorldJobList.Clear();
			this.AllSearchAvatarJobList.AddRange(avatarDataList);
			this.AllSearchWorldJobList.AddRange(worldDataList);
			this.SearchIndexList.AddRange(indexList);
			this.DownLoadAllCloudJobSearchList();
		});
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00003394 File Offset: 0x00001594
	private void CloudUpdateAll()
	{
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00017A54 File Offset: 0x00015C54
	private void DownLoadAllCloudJobList()
	{
		if (this.IndexList != null)
		{
			for (int i = 0; i < this.IndexList.Count; i++)
			{
				TypePair typePair = this.IndexList[i];
				string text;
				string text2;
				string text3;
				if (typePair.Type == 0)
				{
					AvatarJobData avatarJobData = this.AllAvatarJobList[typePair.Index];
					text = avatarJobData.Data.TexturePath;
					text2 = avatarJobData.Data.RemoteTexturePath;
					text3 = SdkLangManager.Get("str_sdk_contentManager_text14");
				}
				else
				{
					WorldJobData worldJobData = this.AllWorldJobList[typePair.Index];
					text = worldJobData.Data.TexturePath;
					text2 = worldJobData.Data.RemoteTexturePath;
					text3 = SdkLangManager.Get("str_sdk_contentManager_text15");
				}
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(text, text2, text3);
			}
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00017B1C File Offset: 0x00015D1C
	private void DownLoadAllCloudJobSearchList()
	{
		if (this.SearchIndexList != null)
		{
			for (int i = 0; i < this.SearchIndexList.Count; i++)
			{
				TypePair typePair = this.SearchIndexList[i];
				string text;
				string text2;
				string text3;
				if (typePair.Type == 0)
				{
					AvatarJobData avatarJobData = this.AllSearchAvatarJobList[typePair.Index];
					text = avatarJobData.Data.TexturePath;
					text2 = avatarJobData.Data.RemoteTexturePath;
					text3 = SdkLangManager.Get("str_sdk_contentManager_text14");
				}
				else
				{
					WorldJobData worldJobData = this.AllSearchWorldJobList[typePair.Index];
					text = worldJobData.Data.TexturePath;
					text2 = worldJobData.Data.RemoteTexturePath;
					text3 = SdkLangManager.Get("str_sdk_contentManager_text15");
				}
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(text, text2, text3);
			}
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00017BE3 File Offset: 0x00015DE3
	public void OnEnableAvatar()
	{
		this.IsFetchAvatar = false;
		this.AvatarDataList = null;
		this.SearchAvatarList = new List<AvatarData>();
		this.IsHaveNextAvatar = false;
		this.AvatarOffset = 0;
		this.AvatarIconDict = new Dictionary<string, Texture2D>();
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00017C18 File Offset: 0x00015E18
	private void DrawAvatar()
	{
		if (!this.IsFetchAvatar)
		{
			this.AvatarOffset = 0;
			this.IsFetchAvatar = true;
			this.UpdateAvatarList(this.AvatarOffset);
		}
		List<AvatarData> list;
		if (this.IsSearch)
		{
			list = this.SearchAvatarList;
		}
		else
		{
			list = this.AvatarDataList;
		}
		if (list == null)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(130f);
			GUILayout.Label("Pulling data, please wait...", this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(90f);
			return;
		}
		if (list.Count == 0)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(50f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text15"), this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(90f);
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.DrawAvatarItem(i, list[i]);
		}
		GUILayout.Space(5f);
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00017D20 File Offset: 0x00015F20
	private void DrawAvatarItem(int index, AvatarData data)
	{
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(460f),
			GUILayout.Height(151f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-147f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		this.DrawTexture(data);
		this.DrawAvatarTakeDown(data);
		this.DrawDetailInfos(data);
		this.DrawButtons(data);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00017DBC File Offset: 0x00015FBC
	private void DrawTexture(AvatarData data)
	{
		if (this.AvatarIconDict.ContainsKey(data.TexturePath))
		{
			if (this.AvatarIconDict[data.TexturePath] == null && SdkManager.Instance.GetTextureDownload().IsDownLoaded(data.TexturePath))
			{
				this.AvatarIconDict[data.TexturePath] = SdkManager.Instance.GetTextureDownload().LoadTexture(data.TexturePath);
				if (this.AvatarIconDict[data.TexturePath] == null)
				{
					SdkManager.Instance.GetTextureDownload().DownLoadTexture(data.TexturePath, data.RemoteTexturePath, SdkLangManager.Get("str_sdk_avatarContent_text16"));
				}
				else
				{
					base.Repaint();
				}
			}
		}
		else
		{
			this.AvatarIconDict.Add(data.TexturePath, null);
		}
		EditorUtil.DrawTexture(100, 136, this.AvatarIconDict[data.TexturePath], SdkLangManager.Get("str_sdk_avatarContent_text17"));
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00017EBC File Offset: 0x000160BC
	private void DrawAvatarTakeDown(AvatarData data)
	{
		this.DrawTakeDown(data.IsTakeDown, -90f, 30f, -50f);
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00017EDC File Offset: 0x000160DC
	private void DrawButtons(AvatarData data)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(50f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarContent_text19"), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		}))
		{
			JsonData jsonData = new JsonData();
			jsonData["event"] = "unity_editor_button_click";
			jsonData["function_type"] = 3;
			jsonData["function_name"] = "content_manager";
			jsonData["button_type"] = 1;
			jsonData["button_id"] = 502;
			jsonData["tab_name"] = "my_avatars";
			AppLogService.PushAppLog(jsonData);
			this.ModifyAvatarContent(data);
			GUIUtility.ExitGUI();
		}
		if (GUILayout.Button(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_avatarContent_text18")), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		}))
		{
			JsonData jsonData2 = new JsonData();
			jsonData2["event"] = "unity_editor_button_click";
			jsonData2["function_type"] = 3;
			jsonData2["function_name"] = "content_manager";
			jsonData2["button_type"] = 1;
			jsonData2["button_id"] = 501;
			jsonData2["tab_name"] = "my_avatars";
			AppLogService.PushAppLog(jsonData2);
			this.CopyId(data.ContentID);
			GUIUtility.ExitGUI();
		}
		if (GUILayout.Button(data.IsShow ? EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_avatarContent_text20")) : EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_avatarContent_text21")), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		}))
		{
			JsonData jsonData3 = new JsonData();
			jsonData3["event"] = "unity_editor_button_click";
			jsonData3["function_type"] = 3;
			jsonData3["function_name"] = "content_manager";
			jsonData3["button_type"] = 1;
			jsonData3["button_id"] = 503;
			jsonData3["tab_name"] = "my_avatars";
			AppLogService.PushAppLog(jsonData3);
			data.IsShow = !data.IsShow;
			GUIUtility.ExitGUI();
		}
		GUILayout.EndVertical();
	}

	// Token: 0x06000396 RID: 918 RVA: 0x000181AC File Offset: 0x000163AC
	private void DrawDetailInfos(AvatarData data)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(data.Name, this.Style18, new GUILayoutOption[]
		{
			GUILayout.Width(160f),
			GUILayout.Height(22f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		string text = SdkLangManager.Get("str_sdk_avatarContent_text22") + (data.IsShow ? data.ContentID : (new string('*', data.ContentID.Length) + SdkLangManager.Get("str_sdk_avatarContent_text23")));
		GUILayout.Space(2f);
		GUILayout.Label(text, this.Style12, new GUILayoutOption[]
		{
			GUILayout.Height(16f),
			GUILayout.Width(160f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(2f);
		GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_avatarContent_text24") + ((data.SharingStauts == 1) ? SdkLangManager.Get("str_sdk_avatarContent_text25") : SdkLangManager.Get("str_sdk_avatarContent_text26"))), this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(220f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-6f);
		if (data.SharingChannel != null && data.SharingChannel.Count >= 3)
		{
			GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_avatarContent_text27") + ((data.SharingChannel[0] == 1) ? SdkLangManager.Get("str_sdk_avatarContent_btn6") : SdkLangManager.Get("str_sdk_no"))), this.Style12, new GUILayoutOption[]
			{
				GUILayout.Width(220f),
				GUILayout.Height(20f),
				GUILayout.ExpandWidth(false),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.Space(-6f);
			GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_avatarContent_text28") + ((data.SharingChannel[1] == 1) ? SdkLangManager.Get("str_sdk_avatarContent_btn6") : SdkLangManager.Get("str_sdk_no"))), this.Style12, new GUILayoutOption[]
			{
				GUILayout.Width(220f),
				GUILayout.Height(20f),
				GUILayout.ExpandWidth(false),
				GUILayout.ExpandHeight(false)
			});
			GUILayout.Space(-6f);
			GUILayout.Space(14f);
		}
		else
		{
			GUILayout.Space(42f);
		}
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text29") + data.CreatTime, this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(220f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-6f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_avatarContent_text30") + data.UpdateTime, this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(220f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndVertical();
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00003394 File Offset: 0x00001594
	public void AvatarUpdate()
	{
	}

	// Token: 0x06000398 RID: 920 RVA: 0x000184F8 File Offset: 0x000166F8
	private void DownLoadAvatarList(int offset = 0)
	{
		if (this.AvatarDataList != null)
		{
			for (int i = offset; i < this.AvatarDataList.Count; i++)
			{
				AvatarData avatarData = this.AvatarDataList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(avatarData.TexturePath, avatarData.RemoteTexturePath, SdkLangManager.Get("str_sdk_avatarContent_text16"));
			}
		}
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00018558 File Offset: 0x00016758
	private void DownLoadAvatarSearchList()
	{
		if (this.SearchAvatarList != null)
		{
			for (int i = 0; i < this.SearchAvatarList.Count; i++)
			{
				AvatarData avatarData = this.SearchAvatarList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(avatarData.TexturePath, avatarData.RemoteTexturePath, SdkLangManager.Get("str_sdk_avatarContent_text16"));
			}
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x000185B8 File Offset: 0x000167B8
	public void ModifyAvatarContent(AvatarData data)
	{
		DateTime now = DateTime.Now;
		CommonNetProxy.FetchWebToken(delegate(string log_code)
		{
			string text = HttpUtil.UrlEncode(log_code);
			string text2 = HttpUtil.UrlEncode(data.ContentID);
			string webUrl = SdkManager.Instance.GetConfig().GetWebUrl();
			Application.OpenURL(string.Concat(new string[] { webUrl, "/api/login/quick?t=", text, "&back=%2Fcreator%2Favatar%2F", text2 }));
		});
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00018654 File Offset: 0x00016854
	private void UpdateAvatarList(int offset = 0)
	{
		CommonNetProxy.RefreshAvatarList(offset, this.LimitAvatar, delegate(List<AvatarData> avatarDataList, JsonData next)
		{
			if (this.AvatarDataList == null)
			{
				this.AvatarDataList = new List<AvatarData>();
			}
			this.AvatarDataList.AddRange(avatarDataList);
			string text = next.ToString();
			this.IsHaveNextAvatar = text.Length > 0;
			if (this.IsHaveNextAvatar)
			{
				this.AvatarOffset += this.LimitAvatar;
			}
			this.IsShowMoreBtn = this.IsHaveNextAvatar;
			this.DownLoadAvatarList(offset);
		});
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00018692 File Offset: 0x00016892
	private void UpdateAvatarContent()
	{
		this.SearchAvatarList.Clear();
		CommonNetProxy.FetchAvatarListWithTag(0, 100, this.SearchName, delegate(List<AvatarData> avatarDataList)
		{
			this.SearchAvatarList.AddRange(avatarDataList);
			this.DownLoadAvatarSearchList();
		});
	}

	// Token: 0x0600039D RID: 925 RVA: 0x000186B9 File Offset: 0x000168B9
	private void OnMoreAvatar()
	{
		this.UpdateAvatarList(this.AvatarOffset);
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000186C7 File Offset: 0x000168C7
	public void OnEnableCloudAvatar()
	{
		this.IsFetchCloudAvatar = false;
		this.SearchAvatarJobList = new List<AvatarJobData>();
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000186DB File Offset: 0x000168DB
	private void ClearCloudAvatar()
	{
		List<AvatarJobData> avatarJobList = this.AvatarJobList;
		if (avatarJobList != null)
		{
			avatarJobList.Clear();
		}
		List<AvatarJobData> searchAvatarJobList = this.SearchAvatarJobList;
		if (searchAvatarJobList == null)
		{
			return;
		}
		searchAvatarJobList.Clear();
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00018700 File Offset: 0x00016900
	private void DrawCloudAvatar()
	{
		if (!this.IsFetchCloudAvatar)
		{
			this.IsFetchCloudAvatar = true;
			this.UpdateAvatarCloudList();
		}
		List<AvatarJobData> list;
		if (this.IsSearch)
		{
			list = this.SearchAvatarJobList;
		}
		else
		{
			list = this.AvatarJobList;
		}
		if (list == null)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(130f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_ContentManager_text29"), this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(90f);
			return;
		}
		if (list.Count != 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.DrawAvatarJobItem(i, list[i]);
			}
			GUILayout.Space(5f);
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000187B8 File Offset: 0x000169B8
	private void DrawAvatarJobItem(int index, AvatarJobData job)
	{
		AvatarData data = job.Data;
		float num = 0f;
		if (!string.IsNullOrEmpty(job.Reason))
		{
			this.ContentInfo.text = job.Reason;
			float num2 = EditorStyles.wordWrappedLabel.CalcHeight(this.ContentInfo, 335f);
			num += num2 + 4f;
		}
		if (!string.IsNullOrEmpty(job.Solution))
		{
			this.ContentInfo.text = job.Solution;
			float num3 = EditorStyles.wordWrappedLabel.CalcHeight(this.ContentInfo, 335f);
			num += num3 + 4f;
		}
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(460f),
			GUILayout.Height(194f + num),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-196f - num);
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		this.DrawTexture(data);
		this.DrawDetailInfos(data);
		GUILayout.EndHorizontal();
		GUILayout.Space(6f);
		this.DrawItemBottom(job, num);
		GUILayout.Space(12f);
		GUILayout.EndVertical();
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000188FE File Offset: 0x00016AFE
	private void UpdateAvatarCloudList()
	{
		CommonNetProxy.FetchAvatarJobList(null, this.GetProcessKey(this.CloudProcessIndex), delegate(List<AvatarJobData> avatarDataList)
		{
			if (this.AvatarJobList == null)
			{
				this.AvatarJobList = new List<AvatarJobData>();
			}
			else
			{
				this.AvatarJobList.Clear();
			}
			this.AvatarJobList.AddRange(avatarDataList);
			this.DownLoadAvatarCloudJobList();
		});
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001891E File Offset: 0x00016B1E
	private void DrawItemBottom(AvatarJobData job, float offset)
	{
		if (!string.IsNullOrEmpty(job.Solution) || !string.IsNullOrEmpty(job.Reason))
		{
			this.DrawItemBottomForNew(job, offset);
			return;
		}
		this.DrawItemBottomForOld(job);
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001894C File Offset: 0x00016B4C
	private void DrawItemBottomForNew(AvatarJobData job, float offset)
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(445f),
			GUILayout.Height(30f + offset),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(-26f - offset);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		if (job.ErrorCode == -1)
		{
			GUI.color = new Color(1f, 0f, 0f, 1f);
		}
		else if (job.ErrorCode == 0)
		{
			GUI.color = new Color(0f, 1f, 0f, 1f);
		}
		GUILayout.Label(CloudJobFailed.GetWorldJobFailed(job.ErrorCode), new GUILayoutOption[]
		{
			GUILayout.Width(300f),
			GUILayout.Height(21f),
			GUILayout.ExpandWidth(false)
		});
		if (job.ErrorCode <= 0)
		{
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
		GUILayout.Space(10f);
		if (offset > 1f)
		{
			if (job.IsOwnDetailInfo)
			{
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				GUILayout.Space(3f);
				if (GUILayout.Button(SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle"), new GUILayoutOption[]
				{
					GUILayout.Width(120f),
					GUILayout.Height(23f),
					GUILayout.ExpandWidth(false)
				}))
				{
					this.HandleDetailInfo(job);
					GUIUtility.ExitGUI();
				}
				GUILayout.EndVertical();
			}
		}
		else if (job.IsOwnDetailInfo)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Space(-3f);
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle"), new GUILayoutOption[]
			{
				GUILayout.Width(120f),
				GUILayout.Height(23f),
				GUILayout.ExpandWidth(false)
			}))
			{
				this.HandleDetailInfo(job);
				GUIUtility.ExitGUI();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		if (!string.IsNullOrEmpty(job.Reason))
		{
			GUILayout.Label(job.Reason, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
			{
				GUILayout.Width(425f),
				GUILayout.ExpandWidth(false)
			});
		}
		if (!string.IsNullOrEmpty(job.Solution))
		{
			GUILayout.Label(job.Solution, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
			{
				GUILayout.Width(425f),
				GUILayout.ExpandWidth(false)
			});
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00018C10 File Offset: 0x00016E10
	private void DrawItemBottomForOld(AvatarJobData job)
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(445f),
			GUILayout.Height(30f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(-26f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		if (job.ErrorCode == -1)
		{
			GUI.color = new Color(1f, 0f, 0f, 1f);
		}
		else if (job.ErrorCode == 0)
		{
			GUI.color = new Color(0f, 1f, 0f, 1f);
		}
		GUILayout.Label(CloudJobFailed.GetAvatarJobFailed(job.ErrorCode), new GUILayoutOption[]
		{
			GUILayout.Width(300f),
			GUILayout.Height(21f),
			GUILayout.ExpandWidth(false)
		});
		if (job.ErrorCode <= 0)
		{
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
		GUILayout.Space(10f);
		if (job.IsOwnDetailInfo)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Space(-3f);
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle"), new GUILayoutOption[]
			{
				GUILayout.Width(120f),
				GUILayout.Height(25f),
				GUILayout.ExpandWidth(false)
			}))
			{
				this.HandleDetailInfo(job);
				GUIUtility.ExitGUI();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00018DBF File Offset: 0x00016FBF
	public void UpdateCloudSearchAvatar()
	{
		CommonNetProxy.FetchAvatarJobList(this.SearchName, this.GetProcessKey(this.CloudProcessIndex), delegate(List<AvatarJobData> avatarDataList)
		{
			this.SearchAvatarJobList.Clear();
			this.SearchAvatarJobList.AddRange(avatarDataList);
			this.DownLoadAvatarCloudJobSearchList();
		});
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00018DE4 File Offset: 0x00016FE4
	private void OnCloudSelectAvatar()
	{
		this.ClearCloudAvatar();
		this.ClearCloudWorld();
		this.ClearCloudAll();
		this.UpdateAvatarCloudList();
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00003394 File Offset: 0x00001594
	private void OnAvatarJobHelp(AvatarJobData job)
	{
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00003394 File Offset: 0x00001594
	private void CloudUpdateAvatar()
	{
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00018E00 File Offset: 0x00017000
	public void DownLoadAvatarCloudJobList()
	{
		if (this.AvatarJobList != null)
		{
			for (int i = 0; i < this.AvatarJobList.Count; i++)
			{
				AvatarJobData avatarJobData = this.AvatarJobList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(avatarJobData.Data.TexturePath, avatarJobData.Data.RemoteTexturePath, "avatar");
			}
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00018E62 File Offset: 0x00017062
	private void HandleDetailInfo(AvatarJobData job)
	{
		CommonNetProxy.FetchJobErrorLogList(job.JobId, delegate(List<string> errorList)
		{
			ContentManagerDetailInfoWindow window = EditorWindow.GetWindow<ContentManagerDetailInfoWindow>();
			window.SetErrorLogList(errorList);
			window.Show();
		});
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00018E90 File Offset: 0x00017090
	private void DownLoadAvatarCloudJobSearchList()
	{
		if (this.SearchAvatarJobList != null)
		{
			for (int i = 0; i < this.SearchAvatarJobList.Count; i++)
			{
				AvatarJobData avatarJobData = this.SearchAvatarJobList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(avatarJobData.Data.TexturePath, avatarJobData.Data.RemoteTexturePath, SdkLangManager.Get("avatar"));
			}
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00018EF7 File Offset: 0x000170F7
	private void OnEnableCloud()
	{
		this.CloudTypeIndex = 0;
		this.CloudProcessIndex = 0;
		this.OnEnableCloudAvatar();
		this.OnEnableCloudWorld();
		this.OnEnableCloudAll();
		this.InitProcessConfig();
		this.CloudProcess = this.CloudProcessConfig.GetNameArray();
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00018F30 File Offset: 0x00017130
	private void InitProcessConfig()
	{
		this.CloudProcessConfig = new CloudProcess();
		this.CloudProcessConfig.Add(SdkLangManager.Get("str_sdk_contentManager_text11"), "all");
		this.CloudProcessConfig.Add(SdkLangManager.Get("str_sdk_contentManager_text6"), "success");
		this.CloudProcessConfig.Add(SdkLangManager.Get("str_sdk_contentManager_text8"), "failed");
		this.CloudProcessConfig.Add(SdkLangManager.Get("str_sdk_contentManager_text10"), "packing");
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00018FB0 File Offset: 0x000171B0
	private string GetProcessKey(int status)
	{
		return this.CloudProcessConfig.GetKey(status);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x00018FBE File Offset: 0x000171BE
	private void DrawCloud()
	{
		EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		if (this.IsCloudSelectAll())
		{
			this.DrawCloudAll();
		}
		else if (this.IsCloudSelectAvatar())
		{
			this.DrawCloudAvatar();
		}
		else
		{
			this.DrawCloudWorld();
		}
		EditorGUILayout.EndVertical();
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00018FF8 File Offset: 0x000171F8
	private void DrawCloudTitle()
	{
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		EditorGUILayout.LabelField(SdkLangManager.Get("str_sdk_contentManager_text12"), EditorStyles.boldLabel, new GUILayoutOption[]
		{
			GUILayout.Width(40f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(335f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_avatarPublish_btnRefresh"), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.OnCloudSelect();
			GUIUtility.ExitGUI();
		}
		GUILayout.Space(-420f);
		EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(-2f);
		this.CloudTypeIndex = EditorGUILayout.Popup(this.CloudTypeIndex, this.CloudType, new GUILayoutOption[]
		{
			GUILayout.Width(113f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		this.CloudProcessIndex = EditorGUILayout.Popup(this.CloudProcessIndex, this.CloudProcess, new GUILayoutOption[]
		{
			GUILayout.Width(220f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0001915F File Offset: 0x0001735F
	private bool IsCloudSelectAvatar()
	{
		return this.SelectCloudTypeIndex == 1;
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001916A File Offset: 0x0001736A
	private bool IsCloudSelectAll()
	{
		return this.SelectCloudTypeIndex == 0;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00019175 File Offset: 0x00017375
	private void OnCloudSelect()
	{
		this.IsSearch = false;
		this.IsShowMoreBtn = false;
		this.SelectCloudTypeIndex = this.CloudTypeIndex;
		if (this.IsCloudSelectAvatar())
		{
			this.OnCloudSelectAvatar();
			return;
		}
		if (this.IsCloudSelectAll())
		{
			this.OnCloudSelectAll();
			return;
		}
		this.OnCloudSelectWorld();
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x000191B5 File Offset: 0x000173B5
	private void DrawMiddleCloudTitle()
	{
		this.DrawCloudTitle();
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x000191BD File Offset: 0x000173BD
	private void CloudUpdate()
	{
		if (this.IsCloudSelectAvatar())
		{
			this.CloudUpdateAvatar();
			return;
		}
		if (this.IsCloudSelectAll())
		{
			this.CloudUpdateAll();
			return;
		}
		this.CloudUpdateWorld();
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x000191E3 File Offset: 0x000173E3
	private void UpdateCloudContent()
	{
		if (this.IsCloudSelectAvatar())
		{
			this.UpdateCloudSearchAvatar();
			return;
		}
		if (this.IsCloudSelectAll())
		{
			this.UpdateCloudSearchAll();
			return;
		}
		this.UpdateCloudSearchWorld();
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00019209 File Offset: 0x00017409
	public void OnEnableCloudWorld()
	{
		this.IsFetchCloudWorld = false;
		this.SearchWorldJobList = new List<WorldJobData>();
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0001921D File Offset: 0x0001741D
	private void ClearCloudWorld()
	{
		List<WorldJobData> worldJobList = this.WorldJobList;
		if (worldJobList != null)
		{
			worldJobList.Clear();
		}
		List<WorldJobData> searchWorldJobList = this.SearchWorldJobList;
		if (searchWorldJobList == null)
		{
			return;
		}
		searchWorldJobList.Clear();
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00019240 File Offset: 0x00017440
	private void DrawCloudWorld()
	{
		if (!this.IsFetchCloudWorld)
		{
			this.IsFetchCloudWorld = true;
			this.UpdateWorldCloudList();
		}
		List<WorldJobData> list;
		if (this.IsSearch)
		{
			list = this.SearchWorldJobList;
		}
		else
		{
			list = this.WorldJobList;
		}
		if (list == null)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(130f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text0"), this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(90f);
			return;
		}
		if (list.Count != 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.DrawWorldJobItem(i, list[i]);
			}
			GUILayout.Space(5f);
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000192F8 File Offset: 0x000174F8
	private void DrawWorldJobItem(int index, WorldJobData job)
	{
		float num = 0f;
		if (!string.IsNullOrEmpty(job.Reason))
		{
			this.ContentInfo.text = job.Reason;
			float num2 = EditorStyles.wordWrappedLabel.CalcHeight(this.ContentInfo, 335f);
			num += num2 + 4f;
		}
		if (!string.IsNullOrEmpty(job.Solution))
		{
			this.ContentInfo.text = job.Solution;
			float num3 = EditorStyles.wordWrappedLabel.CalcHeight(this.ContentInfo, 335f);
			num += num3 + 4f;
		}
		WorldData data = job.Data;
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(460f),
			GUILayout.Height(180f + num),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-182f - num);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(14f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		this.DrawTitle(data);
		GUILayout.Space(-4f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.DrawTexture(data);
		this.DrawDetailInfos(data);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		this.DrawItemBottom(job, num);
		GUILayout.Space(11f);
		GUILayout.EndVertical();
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00019459 File Offset: 0x00017659
	private void DrawItemBottom(WorldJobData job, float offset)
	{
		if (!string.IsNullOrEmpty(job.Solution) || !string.IsNullOrEmpty(job.Reason))
		{
			this.DrawItemBottomForNew(job, offset);
			return;
		}
		this.DrawItemBottomForOld(job);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00019488 File Offset: 0x00017688
	private void DrawItemBottomForOld(WorldJobData job)
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(445f),
			GUILayout.Height(30f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(-26f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		if (job.ErrorCode == -1)
		{
			GUI.color = new Color(1f, 0f, 0f, 1f);
		}
		else if (job.ErrorCode == 0)
		{
			GUI.color = new Color(0f, 1f, 0f, 1f);
		}
		GUILayout.Label(CloudJobFailed.GetWorldJobFailed(job.ErrorCode), new GUILayoutOption[]
		{
			GUILayout.Width(300f),
			GUILayout.Height(21f),
			GUILayout.ExpandWidth(false)
		});
		if (job.ErrorCode <= 0)
		{
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
		GUILayout.Space(10f);
		if (job.IsOwnDetailInfo)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Space(-3f);
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle"), new GUILayoutOption[]
			{
				GUILayout.Width(120f),
				GUILayout.Height(25f),
				GUILayout.ExpandWidth(false)
			}))
			{
				this.HandleDetailInfo(job);
				GUIUtility.ExitGUI();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00019638 File Offset: 0x00017838
	private void DrawItemBottomForNew(WorldJobData job, float offset)
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(445f),
			GUILayout.Height(30f + offset),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.Space(-26f - offset);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		if (job.ErrorCode == -1)
		{
			GUI.color = new Color(1f, 0f, 0f, 1f);
		}
		else if (job.ErrorCode == 0)
		{
			GUI.color = new Color(0f, 1f, 0f, 1f);
		}
		GUILayout.Label(CloudJobFailed.GetWorldJobFailed(job.ErrorCode), new GUILayoutOption[]
		{
			GUILayout.Width(300f),
			GUILayout.Height(21f),
			GUILayout.ExpandWidth(false)
		});
		if (job.ErrorCode <= 0)
		{
			GUI.color = new Color(1f, 1f, 1f, 1f);
		}
		GUILayout.Space(10f);
		if (offset > 1f)
		{
			if (job.IsOwnDetailInfo)
			{
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				GUILayout.Space(3f);
				if (GUILayout.Button(SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle"), new GUILayoutOption[]
				{
					GUILayout.Width(120f),
					GUILayout.Height(23f),
					GUILayout.ExpandWidth(false)
				}))
				{
					this.HandleDetailInfo(job);
					GUIUtility.ExitGUI();
				}
				GUILayout.EndVertical();
			}
		}
		else if (job.IsOwnDetailInfo)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Space(-3f);
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_cloudprocessfailedlogtitle"), new GUILayoutOption[]
			{
				GUILayout.Width(120f),
				GUILayout.Height(23f),
				GUILayout.ExpandWidth(false)
			}))
			{
				this.HandleDetailInfo(job);
				GUIUtility.ExitGUI();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(15f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		if (!string.IsNullOrEmpty(job.Reason))
		{
			GUILayout.Label(job.Reason, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
			{
				GUILayout.Width(425f),
				GUILayout.ExpandWidth(false)
			});
		}
		if (!string.IsNullOrEmpty(job.Solution))
		{
			GUILayout.Label(job.Solution, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
			{
				GUILayout.Width(425f),
				GUILayout.ExpandWidth(false)
			});
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x060003BF RID: 959 RVA: 0x000198F9 File Offset: 0x00017AF9
	private void UpdateWorldCloudList()
	{
		CommonNetProxy.FetchWorldJobList(null, this.GetProcessKey(this.CloudProcessIndex), delegate(List<WorldJobData> worldDataList)
		{
			if (this.WorldJobList == null)
			{
				this.WorldJobList = new List<WorldJobData>();
			}
			else
			{
				this.WorldJobList.Clear();
			}
			this.WorldJobList.AddRange(worldDataList);
			this.DownLoadWorldCloudJobList();
		});
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00019919 File Offset: 0x00017B19
	private void HandleDetailInfo(WorldJobData job)
	{
		CommonNetProxy.FetchJobErrorLogList(job.JobId, delegate(List<string> errorList)
		{
			ContentManagerDetailInfoWindow window = EditorWindow.GetWindow<ContentManagerDetailInfoWindow>();
			window.SetErrorLogList(errorList);
			window.Show();
		});
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00019945 File Offset: 0x00017B45
	private void OnCloudSelectWorld()
	{
		this.ClearCloudAvatar();
		this.ClearCloudWorld();
		this.ClearCloudAll();
		this.UpdateWorldCloudList();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001995F File Offset: 0x00017B5F
	public void UpdateCloudSearchWorld()
	{
		CommonNetProxy.FetchWorldJobList(this.SearchName, this.GetProcessKey(this.CloudProcessIndex), delegate(List<WorldJobData> worldDataList)
		{
			if (this.SearchWorldJobList == null)
			{
				this.SearchWorldJobList = new List<WorldJobData>();
			}
			else
			{
				this.SearchWorldJobList.Clear();
			}
			this.SearchWorldJobList.AddRange(worldDataList);
			this.DownLoadWorldCloudJobSearchList();
		});
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00003394 File Offset: 0x00001594
	private void CloudUpdateWorld()
	{
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00019984 File Offset: 0x00017B84
	private void DownLoadWorldCloudJobList()
	{
		if (this.WorldJobList != null)
		{
			for (int i = 0; i < this.WorldJobList.Count; i++)
			{
				WorldJobData worldJobData = this.WorldJobList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(worldJobData.Data.TexturePath, worldJobData.Data.RemoteTexturePath, SdkLangManager.Get("str_sdk_contentManager_text1"));
			}
		}
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x000199EC File Offset: 0x00017BEC
	private void DownLoadWorldCloudJobSearchList()
	{
		if (this.SearchWorldJobList != null)
		{
			for (int i = 0; i < this.SearchWorldJobList.Count; i++)
			{
				WorldJobData worldJobData = this.SearchWorldJobList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(worldJobData.Data.TexturePath, worldJobData.Data.RemoteTexturePath, SdkLangManager.Get("str_sdk_contentManager_text1"));
			}
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00019A54 File Offset: 0x00017C54
	private void OnEnable()
	{
		base.minSize = new Vector2(500f, 770f);
		base.maxSize = new Vector2(500f, 770f);
		this.Style12 = new GUIStyle(EditorStyles.wordWrappedLabel);
		this.Style12.fontSize = 12;
		this.Style18 = new GUIStyle(EditorStyles.boldLabel);
		this.Style18.fontSize = 18;
		this.Style14 = new GUIStyle();
		this.Style14.fontSize = 14;
		this.Style14.alignment = 4;
		this.Style14.normal.textColor = Color.white;
		this.Pos = Vector2.zero;
		base.titleContent.text = SdkLangManager.Get("str_sdk_contentManager_text18");
		this.IsSearch = false;
		this.IsShowMoreBtn = false;
		this.TakeDownTexture = EditorUtil.LoadTexture("Packages/com.para.common/Config/Texture/EditorTakeDown.png");
		this.ContentViewType = ContentViewType.kAvatar;
		string nickName = SdkManager.Instance.GetAccount().GetInfo().NickName;
		if (!string.IsNullOrEmpty(nickName))
		{
			if (EditorUtil.GetRealStringCount(nickName) <= 18)
			{
				this.NickName = nickName;
			}
			else
			{
				this.NickName = EditorUtil.Clamp(nickName, 18) + SdkLangManager.Get("str_sdk_CheckDetailAvatar_text0");
			}
		}
		this.OnEnableAvatar();
		this.OnEnableWorld();
		this.OnEnableCloud();
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00019BA2 File Offset: 0x00017DA2
	private void OnDisable()
	{
		this.Style12 = null;
		this.Style18 = null;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00019BB4 File Offset: 0x00017DB4
	private void DrawTitle()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.SearchName = EditorGUILayout.TextField("", this.SearchName, new GUILayoutOption[]
		{
			GUILayout.Width(292f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_contentManager_text20"), new GUILayoutOption[]
		{
			GUILayout.Width(79f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.RevertContent();
			GUIUtility.ExitGUI();
		}
		if (this.IsSearch)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
		}
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_contentManager_text21"), new GUILayoutOption[]
		{
			GUILayout.Width(79f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		}))
		{
			this.UpdateContent();
			GUIUtility.ExitGUI();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.EndHorizontal();
		if (string.IsNullOrEmpty(this.SearchName))
		{
			GUILayout.Space(-27f);
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(8f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text22"), new GUILayoutOption[]
			{
				GUILayout.Width(296f),
				GUILayout.Height(25f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndHorizontal();
			GUI.color = Color.white;
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00019D70 File Offset: 0x00017F70
	private void DrawTypeButton()
	{
		GUIStyle guistyle = new GUIStyle(EditorStyles.label);
		guistyle.alignment = 4;
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (this.ContentViewType == ContentViewType.kAvatar)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
		}
		if (GUILayout.Button("", new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		}))
		{
			JsonData jsonData = new JsonData();
			jsonData["event"] = "unity_editor_button_click";
			jsonData["function_type"] = 3;
			jsonData["function_name"] = "content_manager";
			jsonData["button_id"] = 504;
			jsonData["button_type"] = 2;
			jsonData["tab_name"] = "my_avatars";
			AppLogService.PushAppLog(jsonData);
			this.IsSearch = false;
			this.ContentViewType = ContentViewType.kAvatar;
			this.IsShowMoreBtn = this.IsHaveNextAvatar;
			GUIUtility.ExitGUI();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.Space(-150f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text23"), guistyle, new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(0.5f);
		if (this.ContentViewType == ContentViewType.kWorld)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
		}
		if (GUILayout.Button("", new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		}))
		{
			JsonData jsonData2 = new JsonData();
			jsonData2["event"] = "unity_editor_button_click";
			jsonData2["function_type"] = 3;
			jsonData2["function_name"] = "content_manager";
			jsonData2["button_id"] = 505;
			jsonData2["button_type"] = 2;
			jsonData2["tab_name"] = "my_worlds";
			AppLogService.PushAppLog(jsonData2);
			this.IsSearch = false;
			this.ContentViewType = ContentViewType.kWorld;
			this.IsShowMoreBtn = this.IsHaveNextWorld;
			GUIUtility.ExitGUI();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.Space(-150f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text24"), guistyle, new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(0.5f);
		if (this.ContentViewType == ContentViewType.kCloud)
		{
			this.IsShowMoreBtn = false;
		}
		if (this.ContentViewType == ContentViewType.kCloud)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.25f);
		}
		if (GUILayout.Button("", new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		}))
		{
			JsonData jsonData3 = new JsonData();
			jsonData3["event"] = "unity_editor_button_click";
			jsonData3["function_type"] = 3;
			jsonData3["function_name"] = "content_manager";
			jsonData3["button_id"] = 506;
			jsonData3["button_type"] = 2;
			jsonData3["tab_name"] = "cloud_records";
			AppLogService.PushAppLog(jsonData3);
			this.IsSearch = false;
			this.ContentViewType = ContentViewType.kCloud;
			this.IsShowMoreBtn = false;
			GUIUtility.ExitGUI();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		GUILayout.Space(-150f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text25"), guistyle, new GUILayoutOption[]
		{
			GUILayout.Width(150f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001A1E0 File Offset: 0x000183E0
	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(18f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(12f);
		GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_text26", this.NickName), this.Style18, new GUILayoutOption[]
		{
			GUILayout.Width(400f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(8f);
		this.DrawTitle();
		GUILayout.Space(3f);
		this.DrawTypeButton();
		this.DrawMiddleTitle();
		GUILayout.Space(2f);
		this.Pos = this.DrawList(this.Pos, new GUILayoutOption[]
		{
			GUILayout.Width(477f),
			(this.ContentViewType == ContentViewType.kCloud) ? GUILayout.Height(585f) : GUILayout.Height(605f)
		}, new GUILayoutOption[0]);
		GUILayout.Space(10f);
		if (this.IsShowMoreBtn)
		{
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(192f);
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_contentManager_text27"), new GUILayoutOption[]
			{
				GUILayout.Width(80f),
				GUILayout.Height(25f),
				GUILayout.ExpandWidth(false)
			}))
			{
				this.OnMore();
				GUIUtility.ExitGUI();
			}
			EditorGUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001A354 File Offset: 0x00018554
	public Vector2 DrawList(Vector2 listPos, GUILayoutOption[] svLp, GUILayoutOption[] containerLp)
	{
		listPos = GUILayout.BeginScrollView(listPos, svLp);
		GUILayout.BeginVertical(containerLp);
		if (this.ContentViewType == ContentViewType.kAvatar)
		{
			this.DrawAvatar();
		}
		else if (this.ContentViewType == ContentViewType.kWorld)
		{
			this.DrawWorld();
		}
		else if (this.ContentViewType == ContentViewType.kCloud)
		{
			this.DrawCloud();
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
		return listPos;
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001A3AC File Offset: 0x000185AC
	public void Update()
	{
		if (this.ContentViewType == ContentViewType.kAvatar)
		{
			this.AvatarUpdate();
			return;
		}
		if (this.ContentViewType == ContentViewType.kWorld)
		{
			this.WorldUpdate();
			return;
		}
		if (this.ContentViewType == ContentViewType.kCloud)
		{
			this.CloudUpdate();
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001A3E0 File Offset: 0x000185E0
	private void DrawTakeDown(bool isTakeDown, float left, float top, float right = 0f)
	{
		if (!isTakeDown)
		{
			return;
		}
		GUILayout.Space(left);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(top);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(this.TakeDownTexture, new GUILayoutOption[]
		{
			GUILayout.Width(70f),
			GUILayout.Height(70f)
		});
		GUILayout.Space(right);
		GUILayout.EndHorizontal();
		GUILayout.Space(-45f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_contentManager_btn1"), this.Style14, new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Space(right);
		GUILayout.EndHorizontal();
		GUILayout.Space(30f);
		GUILayout.EndVertical();
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0001A4B5 File Offset: 0x000186B5
	private void DrawMiddleTitle()
	{
		if (this.ContentViewType == ContentViewType.kCloud)
		{
			this.DrawMiddleCloudTitle();
		}
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001A4C6 File Offset: 0x000186C6
	private void RevertContent()
	{
		this.IsSearch = false;
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001A4CF File Offset: 0x000186CF
	private void UpdateContent()
	{
		this.IsSearch = true;
		if (this.ContentViewType == ContentViewType.kAvatar)
		{
			this.UpdateAvatarContent();
			return;
		}
		if (this.ContentViewType == ContentViewType.kWorld)
		{
			this.UpdateWorldContent();
			return;
		}
		if (this.ContentViewType == ContentViewType.kCloud)
		{
			this.UpdateCloudContent();
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001A507 File Offset: 0x00018707
	private void CopyId(string id)
	{
		ParaLog.Log("CopyId" + id);
		GUIUtility.systemCopyBuffer = id;
		this.ShowCopySuccess();
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001A525 File Offset: 0x00018725
	private void ShowCopySuccess()
	{
		EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_contentManager_text28"), SdkLangManager.Get("str_sdk_ok"), null);
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001A54B File Offset: 0x0001874B
	private void OnMore()
	{
		if (this.ContentViewType == ContentViewType.kAvatar)
		{
			this.OnMoreAvatar();
			return;
		}
		if (this.ContentViewType == ContentViewType.kWorld)
		{
			this.OnMoreWorld();
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001A56C File Offset: 0x0001876C
	private void OnEnableWorld()
	{
		this.WorldDataList = null;
		this.SearchWorldList = new List<WorldData>();
		this.IsFetchWorld = false;
		this.WorldIconDict = new Dictionary<string, Texture2D>();
		this.IsHaveNextWorld = false;
		this.WorldOffset = 0;
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001A5A0 File Offset: 0x000187A0
	private void DrawWorld()
	{
		if (!this.IsFetchWorld)
		{
			this.WorldOffset = 0;
			this.IsFetchWorld = true;
			this.UpdateWorldList(this.WorldOffset);
		}
		List<WorldData> list;
		if (this.IsSearch)
		{
			list = this.SearchWorldList;
		}
		else
		{
			list = this.WorldDataList;
		}
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		if (list == null)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(130f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_ContentManager_text29"), this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(90f);
			return;
		}
		if (list.Count == 0)
		{
			GUILayout.Space(260f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(50f);
			GUILayout.Label(SdkLangManager.Get("str_sdk_worldContent_text1"), this.Style18, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.DrawWorldItem(i, list[i]);
		}
		GUILayout.Space(5f);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001A6B4 File Offset: 0x000188B4
	private void DrawWorldItem(int index, WorldData data)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Box("", GUI.skin.window, new GUILayoutOption[]
		{
			GUILayout.Width(460f),
			GUILayout.Height(155f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-157f);
		GUILayout.Space(6f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(8f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		this.DrawTitle(data);
		GUILayout.Space(-4f);
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		this.DrawTexture(data);
		this.DrawWorldTakeDown(data);
		this.DrawDetailInfos(data);
		GUILayout.Space(2f);
		this.DrawButtons(data);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(15f);
		GUILayout.EndVertical();
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001A7A2 File Offset: 0x000189A2
	private void DrawWorldTakeDown(WorldData data)
	{
		this.DrawTakeDown(data.IsTakeDown, -130f, 20f, 30f);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001A7C0 File Offset: 0x000189C0
	private void DrawTitle(WorldData data)
	{
		GUILayout.Label(data.Name, this.Style18, new GUILayoutOption[]
		{
			GUILayout.Width(300f),
			GUILayout.Height(22f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.Label(SdkLangManager.Get("str_sdk_worldContent_field0") + (data.IsShow ? data.ContentID : (new string('*', data.ContentID.Length) + SdkLangManager.Get("str_sdk_worldContent_field1"))), this.Style12, new GUILayoutOption[]
		{
			GUILayout.Height(16f),
			GUILayout.Width(160f),
			GUILayout.ExpandWidth(false)
		});
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001A87C File Offset: 0x00018A7C
	private void DrawButtons(WorldData data)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		if (GUILayout.Button(SdkLangManager.Get("str_sdk_worldContent_field3"), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		}))
		{
			JsonData jsonData = new JsonData();
			jsonData["event"] = "unity_editor_button_click";
			jsonData["function_type"] = 3;
			jsonData["function_name"] = "content_manager";
			jsonData["button_type"] = 1;
			jsonData["button_id"] = 502;
			jsonData["tab_name"] = "my_worlds";
			AppLogService.PushAppLog(jsonData);
			this.ModifyWorldContent(data);
			GUIUtility.ExitGUI();
		}
		if (GUILayout.Button(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field2")), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		}))
		{
			JsonData jsonData2 = new JsonData();
			jsonData2["event"] = "unity_editor_button_click";
			jsonData2["function_type"] = 3;
			jsonData2["function_name"] = "content_manager";
			jsonData2["button_type"] = 1;
			jsonData2["button_id"] = 501;
			jsonData2["tab_name"] = "my_worlds";
			AppLogService.PushAppLog(jsonData2);
			this.CopyId(data.ContentID);
			GUIUtility.ExitGUI();
		}
		if (GUILayout.Button(data.IsShow ? EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field4")) : EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field5")), new GUILayoutOption[]
		{
			GUILayout.Width(80f),
			GUILayout.Height(25f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		}))
		{
			JsonData jsonData3 = new JsonData();
			jsonData3["event"] = "unity_editor_button_click";
			jsonData3["function_type"] = 3;
			jsonData3["function_name"] = "content_manager";
			jsonData3["button_type"] = 1;
			jsonData3["button_id"] = 503;
			jsonData3["tab_name"] = "my_worlds";
			AppLogService.PushAppLog(jsonData3);
			data.IsShow = !data.IsShow;
			GUIUtility.ExitGUI();
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0001AB4C File Offset: 0x00018D4C
	private void DrawDetailInfos(WorldData data)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field6") + (data.IsPublic ? SdkLangManager.Get("str_sdk_worldContent_text2") : SdkLangManager.Get("str_sdk_worldContent_text3"))), this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(170f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-6f);
		GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field7") + data.Capacity.ToString()), this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(170f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(30f);
		GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field8") + data.CreatTime), this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(170f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.Space(-6f);
		GUILayout.Label(EditorUtil.ReplaceSpace(SdkLangManager.Get("str_sdk_worldContent_field9") + data.UpdateTime), this.Style12, new GUILayoutOption[]
		{
			GUILayout.Width(170f),
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false),
			GUILayout.ExpandHeight(false)
		});
		GUILayout.EndVertical();
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001AD00 File Offset: 0x00018F00
	private void DrawTexture(WorldData data)
	{
		if (this.WorldIconDict.ContainsKey(data.TexturePath))
		{
			if (this.WorldIconDict[data.TexturePath] == null && SdkManager.Instance.GetTextureDownload().IsDownLoaded(data.TexturePath))
			{
				this.WorldIconDict[data.TexturePath] = SdkManager.Instance.GetTextureDownload().LoadTexture(data.TexturePath);
				if (this.WorldIconDict[data.TexturePath] == null)
				{
					SdkManager.Instance.GetTextureDownload().DownLoadTexture(data.TexturePath, data.RemoteTexturePath, SdkLangManager.Get("str_sdk_worldContent_text4"));
				}
				else
				{
					base.Repaint();
				}
			}
		}
		else
		{
			this.WorldIconDict.Add(data.TexturePath, null);
		}
		EditorUtil.DrawTexture(172, 96, this.WorldIconDict[data.TexturePath], SdkLangManager.Get("str_sdk_worldContent_field11"));
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00003394 File Offset: 0x00001594
	public void WorldUpdate()
	{
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001AE00 File Offset: 0x00019000
	private void DownLoadWorldList(int offset = 0)
	{
		if (this.WorldDataList != null)
		{
			for (int i = offset; i < this.WorldDataList.Count; i++)
			{
				WorldData worldData = this.WorldDataList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(worldData.TexturePath, worldData.RemoteTexturePath, SdkLangManager.Get("str_sdk_worldContent_text4"));
			}
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001AE60 File Offset: 0x00019060
	private void DownLoadWorldSearchList()
	{
		if (this.SearchWorldList != null)
		{
			for (int i = 0; i < this.SearchWorldList.Count; i++)
			{
				WorldData worldData = this.SearchWorldList[i];
				SdkManager.Instance.GetTextureDownload().DownLoadTexture(worldData.TexturePath, worldData.RemoteTexturePath, SdkLangManager.Get("str_sdk_worldContent_text4"));
			}
		}
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001AEC0 File Offset: 0x000190C0
	public void ModifyWorldContent(WorldData data)
	{
		DateTime now = DateTime.Now;
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		CommonNetProxy.FetchWebToken(delegate(string log_code)
		{
			string text = HttpUtil.UrlEncode(log_code);
			string text2 = HttpUtil.UrlEncode(data.ContentID);
			string webUrl = SdkManager.Instance.GetConfig().GetWebUrl();
			Application.OpenURL(string.Concat(new string[] { webUrl, "/api/login/quick?t=", text, "&back=%2Fcreator%2Fworld%2F", text2 }));
		});
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0001AF5C File Offset: 0x0001915C
	private void UpdateWorldList(int offset = 0)
	{
		CommonNetProxy.RefreshWorldList(offset, this.LimitWorld, delegate(List<WorldData> worldDataList, JsonData next)
		{
			if (this.WorldDataList == null)
			{
				this.WorldDataList = new List<WorldData>();
			}
			this.WorldDataList.AddRange(worldDataList);
			string text = next.ToString();
			this.IsHaveNextWorld = text.Length > 0;
			if (this.IsHaveNextWorld)
			{
				this.WorldOffset += this.LimitWorld;
			}
			this.IsShowMoreBtn = this.IsHaveNextWorld;
			this.DownLoadWorldList(offset);
		});
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001AF9A File Offset: 0x0001919A
	private void UpdateWorldContent()
	{
		this.SearchWorldList.Clear();
		CommonNetProxy.FetchWorldListWithTag(0, 100, this.SearchName, delegate(List<WorldData> worldDataList)
		{
			this.SearchWorldList.AddRange(worldDataList);
			this.DownLoadWorldSearchList();
		});
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0001AFC1 File Offset: 0x000191C1
	private void OnMoreWorld()
	{
		this.UpdateWorldList(this.WorldOffset);
	}

	// Token: 0x040001E1 RID: 481
	private List<WorldJobData> AllWorldJobList;

	// Token: 0x040001E2 RID: 482
	private List<AvatarJobData> AllAvatarJobList;

	// Token: 0x040001E3 RID: 483
	private List<TypePair> IndexList;

	// Token: 0x040001E4 RID: 484
	private List<AvatarJobData> AllSearchAvatarJobList;

	// Token: 0x040001E5 RID: 485
	private List<WorldJobData> AllSearchWorldJobList;

	// Token: 0x040001E6 RID: 486
	private List<TypePair> SearchIndexList;

	// Token: 0x040001E7 RID: 487
	private bool IsFetchCloudAll;

	// Token: 0x040001E8 RID: 488
	private List<AvatarData> AvatarDataList;

	// Token: 0x040001E9 RID: 489
	private List<AvatarData> SearchAvatarList;

	// Token: 0x040001EA RID: 490
	private bool IsFetchAvatar;

	// Token: 0x040001EB RID: 491
	private Dictionary<string, Texture2D> AvatarIconDict;

	// Token: 0x040001EC RID: 492
	private bool IsHaveNextAvatar;

	// Token: 0x040001ED RID: 493
	private int AvatarOffset;

	// Token: 0x040001EE RID: 494
	private int LimitAvatar = 100;

	// Token: 0x040001EF RID: 495
	private List<AvatarJobData> AvatarJobList;

	// Token: 0x040001F0 RID: 496
	private List<AvatarJobData> SearchAvatarJobList;

	// Token: 0x040001F1 RID: 497
	private bool IsFetchCloudAvatar;

	// Token: 0x040001F2 RID: 498
	private string[] CloudType = new string[]
	{
		SdkLangManager.Get("str_sdk_contentManager_text3"),
		SdkLangManager.Get("str_sdk_contentManager_text4"),
		SdkLangManager.Get("str_sdk_contentManager_text5")
	};

	// Token: 0x040001F3 RID: 499
	private string[] CloudProcess;

	// Token: 0x040001F4 RID: 500
	private int CloudTypeIndex;

	// Token: 0x040001F5 RID: 501
	private int CloudProcessIndex;

	// Token: 0x040001F6 RID: 502
	private int SelectCloudTypeIndex;

	// Token: 0x040001F7 RID: 503
	private CloudProcess CloudProcessConfig;

	// Token: 0x040001F8 RID: 504
	private List<WorldJobData> WorldJobList;

	// Token: 0x040001F9 RID: 505
	private List<WorldJobData> SearchWorldJobList;

	// Token: 0x040001FA RID: 506
	private bool IsFetchCloudWorld;

	// Token: 0x040001FB RID: 507
	private GUIContent ContentInfo = new GUIContent();

	// Token: 0x040001FC RID: 508
	private ContentViewType ContentViewType;

	// Token: 0x040001FD RID: 509
	private string NickName = "";

	// Token: 0x040001FE RID: 510
	private string SearchName;

	// Token: 0x040001FF RID: 511
	private Vector2 Pos;

	// Token: 0x04000200 RID: 512
	private bool IsSearch;

	// Token: 0x04000201 RID: 513
	private GUIStyle Style12;

	// Token: 0x04000202 RID: 514
	private GUIStyle Style14;

	// Token: 0x04000203 RID: 515
	private GUIStyle Style18;

	// Token: 0x04000204 RID: 516
	private bool IsShowMoreBtn;

	// Token: 0x04000205 RID: 517
	private Texture2D TakeDownTexture;

	// Token: 0x04000206 RID: 518
	private List<WorldData> WorldDataList;

	// Token: 0x04000207 RID: 519
	private List<WorldData> SearchWorldList;

	// Token: 0x04000208 RID: 520
	private bool IsFetchWorld;

	// Token: 0x04000209 RID: 521
	private bool IsHaveNextWorld;

	// Token: 0x0400020A RID: 522
	private int WorldOffset;

	// Token: 0x0400020B RID: 523
	private Dictionary<string, Texture2D> WorldIconDict;

	// Token: 0x0400020C RID: 524
	private int LimitWorld = 100;
}
