using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;
using LitJson;

// Token: 0x0200006A RID: 106
public static class CommonNetProxy
{
	// Token: 0x06000358 RID: 856 RVA: 0x0001620C File Offset: 0x0001440C
	private static void Send(string url, HTTPMethods methods, JsonData data, Dictionary<string, string> headerDict, Action<int, string, JsonData> callback)
	{
		HttpManager.Send(url, methods, data, headerDict, callback);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00016219 File Offset: 0x00014419
	private static void Send(string url, HTTPMethods methods, JsonData data, Action<int, string, JsonData> callback)
	{
		HttpManager.Send(url, methods, data, callback);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00016224 File Offset: 0x00014424
	public static JsonData BuildAvatarInfoData(int job_type, string avatar_id, string job_mask, string name, string des, string assetName, List<string> tags, bool isPublish, List<int> publishs)
	{
		JsonData jsonData = new JsonData();
		jsonData["avatar_id"] = avatar_id;
		jsonData["job_type"] = job_type;
		jsonData["job_mask"] = job_mask;
		JsonData jsonData2 = new JsonData();
		jsonData2["name"] = name;
		jsonData2["desc"] = des;
		JsonData jsonData3 = new JsonData();
		jsonData3["level"] = (isPublish ? AvatarPublishContent.AvatarPublishContentPublic : AvatarPublishContent.AvatarPublishContentPrivate);
		if (isPublish)
		{
			jsonData3["public"] = new JsonData();
			jsonData3["public"]["cloneable"] = (publishs.Contains(3) ? 1 : 0);
			jsonData3["public"]["recommend"] = (publishs.Contains(1) ? 1 : 0);
		}
		jsonData2["privilege"] = jsonData3;
		StringBuilder stringBuilder = new StringBuilder();
		if (tags != null && tags.Count > 0)
		{
			for (int i = 0; i < tags.Count; i++)
			{
				if (!tags[i].Replace(" ", "").Equals(""))
				{
					stringBuilder.Append(tags[i]);
					stringBuilder.Append(",");
				}
			}
		}
		if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ',')
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		jsonData2["tags"] = stringBuilder.ToString();
		jsonData["base_info"] = jsonData2.ToJson();
		jsonData["bundle_asset_name"] = assetName;
		return jsonData;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00016400 File Offset: 0x00014600
	public static JsonData BuildWorldInfoData(int job_type, string world_id, string job_mask, string worldName, int capacity, string desc, string assetName, List<string> tags, bool isPublic)
	{
		JsonData jsonData = new JsonData();
		jsonData["world_id"] = world_id;
		jsonData["job_type"] = job_type;
		jsonData["job_mask"] = job_mask;
		JsonData jsonData2 = new JsonData();
		jsonData2["name"] = worldName;
		jsonData2["desc"] = desc;
		jsonData2["capacity"] = capacity;
		jsonData2["is_public"] = isPublic;
		StringBuilder stringBuilder = new StringBuilder();
		if (tags != null && tags.Count > 0)
		{
			for (int i = 0; i < tags.Count; i++)
			{
				if (!tags[i].Replace(" ", "").Equals(""))
				{
					stringBuilder.Append(tags[i]);
					stringBuilder.Append(",");
				}
			}
		}
		if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ',')
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		jsonData2["tags"] = stringBuilder.ToString();
		jsonData["base_info"] = jsonData2.ToJson();
		jsonData["bundle_asset_name"] = assetName;
		return jsonData;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00016564 File Offset: 0x00014764
	public static void RefreshAvatarList(int offset, int limit, Action<List<AvatarData>, JsonData> func)
	{
		CommonNetProxy.GetAvatarList(offset, limit, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				List<AvatarData> list = new List<AvatarData>();
				if (result.ContainsKey("list") && result["list"] != null)
				{
					foreach (object obj in result["list"])
					{
						AvatarData avatarData = CommonNetProxy.BuildAvatarData((JsonData)obj);
						list.Add(avatarData);
					}
				}
				if (result.ContainsKey("next"))
				{
					JsonData jsonData = result["next"];
				}
				func(list, result["next"]);
				return;
			}
			ParaLog.LogError("code:" + code.ToString() + " message:" + message);
		});
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00016594 File Offset: 0x00014794
	public static void FetchAvatarJobList(string key, string status, Action<List<AvatarJobData>> callback)
	{
		CommonNetProxy.GetJobList("avatar", key, status, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				List<AvatarJobData> list = new List<AvatarJobData>();
				for (int i = 0; i < result.Count; i++)
				{
					JsonData jsonData = result[i];
					if (jsonData["res_type"].ToString() == "avatar")
					{
						AvatarJobData avatarJobData = CommonNetProxy.BuildAvatarJobData(jsonData["avatar"]);
						list.Add(avatarJobData);
					}
				}
				if (callback != null)
				{
					callback(list);
					return;
				}
			}
			else
			{
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000165C8 File Offset: 0x000147C8
	public static void FetchWorldJobList(string key, string status, Action<List<WorldJobData>> callback)
	{
		CommonNetProxy.GetJobList("world", key, status, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				List<WorldJobData> list = new List<WorldJobData>();
				for (int i = 0; i < result.Count; i++)
				{
					JsonData jsonData = result[i];
					if (jsonData["res_type"].ToString() == "world")
					{
						WorldJobData worldJobData = CommonNetProxy.BuildWorldJobData(jsonData["world"]);
						list.Add(worldJobData);
					}
				}
				if (callback != null)
				{
					callback(list);
					return;
				}
			}
			else
			{
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000165FC File Offset: 0x000147FC
	public static void FetchAllJobList(string key, string status, Action<List<AvatarJobData>, List<WorldJobData>, List<TypePair>> callback)
	{
		CommonNetProxy.GetJobList(null, key, status, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				List<WorldJobData> list = new List<WorldJobData>();
				List<AvatarJobData> list2 = new List<AvatarJobData>();
				List<TypePair> list3 = new List<TypePair>();
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < result.Count; i++)
				{
					JsonData jsonData = result[i];
					if (jsonData["res_type"].ToString() == "world")
					{
						WorldJobData worldJobData = CommonNetProxy.BuildWorldJobData(jsonData["world"]);
						list.Add(worldJobData);
						list3.Add(new TypePair(1, num2));
						num2++;
					}
					else if (jsonData["res_type"].ToString() == "avatar")
					{
						AvatarJobData avatarJobData = CommonNetProxy.BuildAvatarJobData(jsonData["avatar"]);
						list2.Add(avatarJobData);
						list3.Add(new TypePair(0, num));
						num++;
					}
				}
				if (callback != null)
				{
					callback(list2, list, list3);
					return;
				}
			}
			else
			{
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0001662C File Offset: 0x0001482C
	private static AvatarJobData BuildAvatarJobData(JsonData data)
	{
		AvatarData avatarData = CommonNetProxy.BuildAvatarData(data);
		string text = "";
		bool flag = false;
		string text2 = "";
		string text3 = "";
		int num;
		if (data.ContainsKey("error_status") && !string.IsNullOrEmpty(data["error_status"].ToString()))
		{
			num = int.Parse(data["error_status"].ToString());
		}
		else
		{
			if (data["status"].ToString() == "success")
			{
				num = 0;
			}
			else if (data["status"].ToString() == "packing")
			{
				num = 1;
			}
			else
			{
				num = -1;
			}
			if (data.ContainsKey("has_log_info"))
			{
				flag = bool.Parse(data["has_log_info"].ToString());
			}
			if (data.ContainsKey("err_reason"))
			{
				text3 = data["err_reason"].ToString();
			}
			if (data.ContainsKey("err_solution"))
			{
				text2 = data["err_solution"].ToString();
			}
			if (data.ContainsKey("job_id"))
			{
				text = data["job_id"].ToString();
			}
		}
		return new AvatarJobData(avatarData, data["status"].ToString(), num, text, flag, text2, text3);
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00016770 File Offset: 0x00014970
	private static WorldJobData BuildWorldJobData(JsonData data)
	{
		WorldData worldData = CommonNetProxy.BuildWorldData(data);
		bool flag = false;
		string text = "";
		string text2 = "";
		string text3 = "";
		int num;
		if (data.ContainsKey("error_status") && !string.IsNullOrEmpty(data["error_status"].ToString()))
		{
			num = int.Parse(data["error_status"].ToString());
		}
		else
		{
			if (data["status"].ToString() == "success")
			{
				num = 0;
			}
			else if (data["status"].ToString() == "packing")
			{
				num = 1;
			}
			else
			{
				num = -1;
			}
			if (data.ContainsKey("has_log_info"))
			{
				flag = bool.Parse(data["has_log_info"].ToString());
			}
			if (data.ContainsKey("err_reason"))
			{
				text2 = data["err_reason"].ToString();
			}
			if (data.ContainsKey("err_solution"))
			{
				text = data["err_solution"].ToString();
			}
			if (data.ContainsKey("job_id"))
			{
				text3 = data["job_id"].ToString();
			}
		}
		return new WorldJobData(worldData, data["status"].ToString(), num, text3, flag, text, text2);
	}

	// Token: 0x06000362 RID: 866 RVA: 0x000168B4 File Offset: 0x00014AB4
	private static WorldData BuildWorldData(JsonData item)
	{
		string text = "";
		if (item.ContainsKey("name"))
		{
			text = item["name"].ToString();
		}
		string text2 = "";
		if (item.ContainsKey("world_id"))
		{
			text2 = item["world_id"].ToString();
		}
		bool flag = false;
		if (item.ContainsKey("is_take_down"))
		{
			flag = (bool)item["is_take_down"];
		}
		string text3 = "";
		if (item.ContainsKey("desc"))
		{
			text3 = item["desc"].ToString();
		}
		string text4 = "";
		if (item.ContainsKey("cover"))
		{
			text4 = item["cover"].ToString();
		}
		string text5 = "";
		if (item.ContainsKey("cover_uri"))
		{
			text5 = item["cover_uri"].ToString();
		}
		string text6 = "";
		if (item.ContainsKey("icon"))
		{
			text6 = item["icon"].ToString();
		}
		string text7 = "";
		if (item.ContainsKey("icon_uri"))
		{
			text7 = item["icon_uri"].ToString();
		}
		List<string> list = new List<string>();
		if (item.ContainsKey("hash_tag"))
		{
			for (int i = 0; i < item["hash_tag"].Count; i++)
			{
				string text8 = item["hash_tag"][i].ToString();
				list.Add(text8);
			}
		}
		int num = 0;
		if (item.ContainsKey("capacity"))
		{
			num = (int)item["capacity"];
		}
		bool flag2 = false;
		if (item.ContainsKey("is_public"))
		{
			flag2 = (bool)item["is_public"];
		}
		string text9 = "";
		if (item.ContainsKey("create_time"))
		{
			text9 = item["create_time"].ToString();
		}
		string text10 = "";
		if (item.ContainsKey("update_time"))
		{
			text10 = item["update_time"].ToString();
		}
		return new WorldData(text, text2, flag, text3, num, flag2, list, text4, text5, text6, text7, text9, text10);
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00016AE8 File Offset: 0x00014CE8
	public static AvatarData BuildAvatarData(JsonData item)
	{
		string text = "";
		if (item.ContainsKey("name"))
		{
			text = item["name"].ToString();
		}
		string text2 = "";
		if (item.ContainsKey("avatar_id"))
		{
			text2 = item["avatar_id"].ToString();
		}
		bool flag = false;
		if (item.ContainsKey("is_take_down"))
		{
			flag = (bool)item["is_take_down"];
		}
		string text3 = "";
		if (item.ContainsKey("desc"))
		{
			text3 = item["desc"].ToString();
		}
		List<string> list = new List<string>();
		if (item.ContainsKey("hash_tags"))
		{
			for (int i = 0; i < item["hash_tags"].Count; i++)
			{
				string text4 = item["hash_tags"][i].ToString();
				list.Add(text4);
			}
		}
		string text5 = "";
		if (item.ContainsKey("cover_image"))
		{
			text5 = item["cover_image"].ToString();
		}
		string text6 = "";
		if (item.ContainsKey("cover_uri"))
		{
			text6 = item["cover_uri"].ToString();
		}
		int num = 0;
		List<int> list2 = new List<int>();
		if (item.ContainsKey("privilege"))
		{
			if (item["privilege"].ContainsKey("level") && item["privilege"]["level"].ToString().Equals("public"))
			{
				num = 1;
			}
			if (item["privilege"].ContainsKey("public"))
			{
				if (item["privilege"]["public"].ContainsKey("recommend"))
				{
					int num2 = (int)item["privilege"]["public"]["recommend"];
					list2.Add(num2);
				}
				else
				{
					list2.Add(0);
				}
				if (item["privilege"]["public"].ContainsKey("cloneable"))
				{
					int num3 = (int)item["privilege"]["public"]["cloneable"];
					list2.Add(num3);
				}
				else
				{
					list2.Add(0);
				}
				if (item["privilege"]["public"].ContainsKey("model_room"))
				{
					int num4 = (int)item["privilege"]["public"]["model_room"];
					list2.Add(num4);
				}
				else
				{
					list2.Add(0);
				}
			}
		}
		string text7 = "";
		if (item.ContainsKey("create_time"))
		{
			text7 = item["create_time"].ToString();
		}
		string text8 = "";
		if (item.ContainsKey("update_time"))
		{
			text8 = item["update_time"].ToString();
		}
		ParaLog.Log("name:" + text);
		ParaLog.Log("avatarId:" + text2);
		ParaLog.Log("desc:" + text3);
		ParaLog.Log("icon:" + text5);
		return new AvatarData(text, text2, flag, text3, list, num, list2, text5, text6, text7, text8);
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00016E48 File Offset: 0x00015048
	public static void RefreshWorldList(int offset, int limit, Action<List<WorldData>, JsonData> func)
	{
		CommonNetProxy.GetWorldList(offset, limit, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				List<WorldData> list = new List<WorldData>();
				JsonData jsonData = null;
				if (result.ContainsKey("list") && result["list"] != null)
				{
					foreach (object obj in result["list"])
					{
						WorldData worldData = CommonNetProxy.BuildWorldData((JsonData)obj);
						list.Add(worldData);
					}
				}
				if (result.ContainsKey("next"))
				{
					jsonData = result["next"];
				}
				func(list, jsonData);
				return;
			}
			ParaLog.LogError("code:" + code.ToString() + " message:" + message);
		});
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00016E75 File Offset: 0x00015075
	public static void FetchAccountInfo(Action callback)
	{
		Action<int, string, JsonData> <>9__1;
		CommonNetProxy.GetServerTimestamp(delegate
		{
			string text = "/platform/sdk/v2/users";
			HTTPMethods httpmethods = 0;
			JsonData jsonData = new JsonData();
			Action<int, string, JsonData> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(int code, string message, JsonData result)
				{
					if (code != 0)
					{
						SdkManager.Instance.GetAccount().Logout();
						MenuBars.Login(callback);
						EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), message, "OK", null);
						return;
					}
					string text2 = result["para_id"].ToString();
					string text3 = result["nickname"].ToString();
					int num = int.Parse(result["avatar_num"].ToString());
					int num2 = int.Parse(result["world_num"].ToString());
					int num3 = int.Parse(result["working_num"].ToString());
					string text4 = result["head_portrait_url"].ToString();
					string text5 = result["head_portrait_uri"].ToString();
					ParaLog.Log("url:" + text4);
					SdkManager.Instance.GetAccount().SetInfo(text2, text3, num, num2, num3, text4, text5);
					if (callback != null)
					{
						callback();
					}
				});
			}
			CommonNetProxy.Send(text, httpmethods, jsonData, action);
		});
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00016E94 File Offset: 0x00015094
	private static void GetServerTimestamp(Action callback)
	{
		if (!HttpUtil.HasSettedServerTimestamp())
		{
			CommonNetProxy.Send("/platform/sdk/v2/pre_login", 0, null, delegate(int code, string message, JsonData result)
			{
				if (code != 0)
				{
					EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), message, "OK", null);
					if (callback != null)
					{
						callback();
					}
					return;
				}
				HttpUtil.SetServerTimestamp(long.Parse(result["server_ts"].ToString()));
				if (callback != null)
				{
					callback();
				}
			});
			return;
		}
		if (callback != null)
		{
			callback();
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00016EE4 File Offset: 0x000150E4
	public static void SendDeleteWorld(string worldId, Action<int, string> callback)
	{
		JsonData jsonData = new JsonData();
		jsonData["world_id"] = worldId;
		CommonNetProxy.Send("/platform/sdk/v2/worlds", 4, jsonData, delegate(int code, string message, JsonData result)
		{
			if (callback != null)
			{
				callback(code, message);
			}
		});
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00016F30 File Offset: 0x00015130
	public static void SendDeleteAvatar(string avatarId, Action<int, string> callback)
	{
		JsonData jsonData = new JsonData();
		jsonData["avatar_id"] = avatarId;
		CommonNetProxy.Send("/platform/sdk/v2/avatars", 4, jsonData, delegate(int code, string message, JsonData result)
		{
			if (callback != null)
			{
				callback(code, message);
			}
		});
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00016F7C File Offset: 0x0001517C
	public static void FetchWorldListWithTag(int offset, int limit, string searchKey, Action<List<WorldData>> callback)
	{
		string openId = SdkManager.Instance.GetAccount().GetOpenId();
		searchKey = HttpUtil.UrlEncode(searchKey);
		JsonData jsonData = new JsonData();
		jsonData["search_key"] = searchKey;
		jsonData["limit"] = limit;
		jsonData["offset"] = offset;
		CommonNetProxy.Send(string.Concat(new string[]
		{
			"/platform/sdk/v2/users/",
			openId,
			"/worlds?offset=",
			offset.ToString(),
			"&limit=",
			limit.ToString(),
			"&search_key=",
			searchKey
		}), 0, jsonData, delegate(int code, string message, JsonData result)
		{
			if (code == 0 && result.ContainsKey("list") && result["list"] != null)
			{
				List<WorldData> list = new List<WorldData>();
				for (int i = 0; i < result["list"].Count; i++)
				{
					WorldData worldData = CommonNetProxy.BuildWorldData(result["list"][i]);
					list.Add(worldData);
				}
				if (callback != null)
				{
					callback(list);
					return;
				}
			}
			else
			{
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00017044 File Offset: 0x00015244
	public static void FetchAvatarListWithTag(int offset, int limit, string searchKey, Action<List<AvatarData>> callback)
	{
		searchKey = HttpUtil.UrlEncode(searchKey);
		string openId = SdkManager.Instance.GetAccount().GetOpenId();
		JsonData jsonData = new JsonData();
		jsonData["search_key"] = searchKey;
		jsonData["limit"] = limit;
		jsonData["offset"] = offset;
		CommonNetProxy.Send(string.Concat(new string[]
		{
			"/platform/sdk/v2/users/",
			openId,
			"/avatars?offset=",
			offset.ToString(),
			"&limit=",
			limit.ToString(),
			"&search_key=",
			searchKey
		}), 0, jsonData, delegate(int code, string message, JsonData result)
		{
			if (code == 0 && result.ContainsKey("list") && result["list"] != null)
			{
				List<AvatarData> list = new List<AvatarData>();
				for (int i = 0; i < result["list"].Count; i++)
				{
					AvatarData avatarData = CommonNetProxy.BuildAvatarData(result["list"][i]);
					list.Add(avatarData);
				}
				if (callback != null)
				{
					callback(list);
					return;
				}
			}
			else
			{
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0001710C File Offset: 0x0001530C
	private static void GetWorldList(int offset, int limit, Action<int, string, JsonData> callback)
	{
		string openId = SdkManager.Instance.GetAccount().GetOpenId();
		JsonData jsonData = new JsonData();
		jsonData["limit"] = limit;
		jsonData["offset"] = offset;
		CommonNetProxy.Send(string.Concat(new string[]
		{
			"/platform/sdk/v2/users/",
			openId,
			"/worlds?offset=",
			offset.ToString(),
			"&limit=",
			limit.ToString()
		}), 0, jsonData, callback);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00017194 File Offset: 0x00015394
	public static void Commit(string uploadToken, string jobId, JsonData resInfo, Action<int, string, JsonData> callback)
	{
		if (resInfo == null)
		{
			ParaLog.LogError("Commit Failed, resInfo is null");
			return;
		}
		JsonData jsonData = new JsonData();
		jsonData["job_id"] = jobId;
		jsonData["res_info"] = resInfo;
		HttpManager.SendWithUploadTokenAndFeature("/platform/sdk/v2/jobs/commit", 2, jsonData, uploadToken, callback);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x000171E0 File Offset: 0x000153E0
	private static void GetAvatarList(int offset, int limit, Action<int, string, JsonData> callback)
	{
		string openId = SdkManager.Instance.GetAccount().GetOpenId();
		JsonData jsonData = new JsonData();
		jsonData["limit"] = limit;
		jsonData["offset"] = offset;
		CommonNetProxy.Send(string.Concat(new string[]
		{
			"/platform/sdk/v2/users/",
			openId,
			"/avatars?offset=",
			offset.ToString(),
			"&limit=",
			limit.ToString()
		}), 0, jsonData, callback);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00017268 File Offset: 0x00015468
	private static void GetJobList(string res_type, string key, string status, Action<int, string, JsonData> callback)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("/platform/sdk/v2/jobs/recent?");
		JsonData jsonData = new JsonData();
		jsonData["status"] = status;
		stringBuilder.Append("status=" + status);
		if (!string.IsNullOrEmpty(res_type))
		{
			jsonData["res_type"] = res_type;
			stringBuilder.Append("&res_type=" + res_type);
		}
		if (!string.IsNullOrEmpty(key))
		{
			jsonData["key"] = key;
			stringBuilder.Append("&key=" + key);
		}
		CommonNetProxy.Send(stringBuilder.ToString(), 0, jsonData, callback);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00017314 File Offset: 0x00015514
	public static void FetchWorldInfo(string worldId, Action<WorldData> callback)
	{
		JsonData jsonData = new JsonData();
		jsonData["world_id"] = worldId;
		JsonData jsonData2 = jsonData;
		CommonNetProxy.Send("/platform/sdk/v2/world/info?world_id=" + worldId, 0, jsonData2, delegate(int code, string message, JsonData data)
		{
			if (code == 0)
			{
				WorldData worldData = CommonNetProxy.BuildWorldData(data);
				if (callback != null)
				{
					callback(worldData);
					return;
				}
			}
			else
			{
				EditorUtil.OpenInfo("Error", message, "OK", null);
				if (callback != null)
				{
					callback(null);
				}
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00017364 File Offset: 0x00015564
	public static void FetchAvatarInfo(string avatarId, Action<AvatarData> callback)
	{
		JsonData jsonData = new JsonData();
		jsonData["avatar_id"] = avatarId;
		JsonData jsonData2 = jsonData;
		CommonNetProxy.Send("/platform/sdk/v2/avatar/info?avatar_id=" + avatarId, 0, jsonData2, delegate(int code, string message, JsonData data)
		{
			if (code == 0)
			{
				AvatarData avatarData = CommonNetProxy.BuildAvatarData(data);
				if (callback != null)
				{
					callback(avatarData);
					return;
				}
			}
			else
			{
				EditorUtil.OpenInfo("Error", message, "OK", null);
				if (callback != null)
				{
					callback(null);
				}
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
			}
		});
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000173B4 File Offset: 0x000155B4
	public static void FetchJobErrorLogList(string JobId, Action<List<string>> callback)
	{
		JsonData jsonData = new JsonData();
		CommonNetProxy.Send("/platform/sdk/v2/jobs/" + JobId + "/error", 0, jsonData, delegate(int code, string message, JsonData data)
		{
			if (code != 0)
			{
				EditorUtil.OpenInfo("Error", message, "OK", null);
				if (callback != null)
				{
					callback(null);
				}
				ParaLog.LogError("code:" + code.ToString() + " message:" + message);
				return;
			}
			if (data.ContainsKey("log"))
			{
				string[] array = data["log"].ToString().Split("\n", StringSplitOptions.None);
				callback(array.ToList<string>());
				return;
			}
			callback(null);
		});
	}

	// Token: 0x06000372 RID: 882 RVA: 0x000173F7 File Offset: 0x000155F7
	public static void LoginWithToken(string openID, string token, Action<int, string, JsonData> callback)
	{
		Action<int, string, JsonData> <>9__1;
		CommonNetProxy.GetServerTimestamp(delegate
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string> { { "Tt-Hamlet-accessTk", token } };
			JsonData jsonData = new JsonData();
			jsonData["open_id"] = openID;
			jsonData["token"] = token;
			JsonData jsonData2 = jsonData;
			string text = "/platform/sdk/v2/users/verify";
			HTTPMethods httpmethods = 2;
			JsonData jsonData3 = jsonData2;
			Dictionary<string, string> dictionary2 = dictionary;
			Action<int, string, JsonData> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(int code, string message, JsonData reselt)
				{
					if (code != 0)
					{
						SdkManager.Instance.GetAccount().Logout();
					}
					if (callback != null)
					{
						callback(code, message, reselt);
					}
				});
			}
			CommonNetProxy.Send(text, httpmethods, jsonData3, dictionary2, action);
		});
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00017424 File Offset: 0x00015624
	public static void UpdateSdk(string uri, string curVersion, Action<int, string, JsonData> callback)
	{
		JsonData jsonData = new JsonData();
		jsonData["current_version"] = curVersion;
		JsonData jsonData2 = jsonData;
		CommonNetProxy.Send("/platform/sdk/v2/version/" + uri + "?current_version=" + curVersion, 0, jsonData2, callback);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00017464 File Offset: 0x00015664
	public static void FetchWebToken(Action<string> callback)
	{
		CommonNetProxy.Send("/platform/sdk/v2/users/quick_login", 0, new JsonData(), delegate(int code, string message, JsonData reselt)
		{
			if (code == 0 && callback != null)
			{
				string text = "";
				if (reselt.ContainsKey("login_code"))
				{
					text = reselt["login_code"].ToString();
				}
				callback(text);
			}
		});
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001749A File Offset: 0x0001569A
	public static void SendWorldInfo(JsonData jsonData, Action<int, string, JsonData> callback)
	{
		HttpManager.Send("/platform/sdk/v2/world_jobs", 2, jsonData, callback);
	}

	// Token: 0x06000376 RID: 886 RVA: 0x000174A9 File Offset: 0x000156A9
	public static void SendAvatarInfo(JsonData jsonData, Action<int, string, JsonData> callback)
	{
		HttpManager.Send("/platform/sdk/v2/avatar_jobs", 2, jsonData, callback);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x000174B8 File Offset: 0x000156B8
	public static void UploadTexture(string uploadToken, string jobId, string mask, byte[] data, string name, Action<int, string, JsonData> callback)
	{
		CommonNetProxy.UploadData(uploadToken, jobId, mask, name, data, CommonNetProxy.AllowPartUpload(mask), callback);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000174CD File Offset: 0x000156CD
	private static bool AllowPartUpload(string mask)
	{
		return !(mask == WorldJobMask.WorldJobMaskCover) && !(mask == WorldJobMask.WorldJobMaskRoomIcon);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x000174EC File Offset: 0x000156EC
	public static void UploadPackage(string uploadToken, string jobId, byte[] data, string name, Action<int, string, JsonData> callback)
	{
		CommonNetProxy.UploadData(uploadToken, jobId, "dev_pack", name, data, true, callback);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x000174FF File Offset: 0x000156FF
	public static void UploadData(string uploadToken, string jobId, string mask, string name, byte[] data, bool allowPartUpload, Action<int, string, JsonData> callback)
	{
		HttpPack.Send("/platform/upload/v1", uploadToken, jobId, mask, name, data, allowPartUpload, callback);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00017518 File Offset: 0x00015718
	public static async Task LoginAsync(string account, string password, Action<string, string, string, string> callback)
	{
		JsonData jsonData = new JsonData();
		jsonData["type"] = "open_pc";
		jsonData["account_type"] = 100;
		jsonData["account"] = account;
		jsonData["password"] = password;
		JsonData jsonData2 = jsonData;
		JsonData jsonData3;
		try
		{
			jsonData3 = await GSDKRequest.PostWithEncryptAsync("/gsdk/account/open/v1/login", jsonData2, null);
		}
		catch (Exception ex)
		{
			EditorUtil.OpenInfo("Error", ex.Message, "OK", null);
			return;
		}
		ParaLog.Log("login in GSDK success:");
		ParaLog.Log(jsonData3.ToJson());
		JsonData jsonData4 = jsonData3["code"];
		string text;
		if (jsonData3.ContainsKey("StatusCode") && (int)jsonData3["StatusCode"] != 200)
		{
			text = SdkLangManager.Get("str_sdk_commonNetProxy_text0");
		}
		else if (!jsonData3.ContainsKey("code"))
		{
			text = SdkLangManager.Get("str_sdk_commonNetProxy_text0");
		}
		else
		{
			text = SdkManager.Instance.LoginHandle(int.Parse(jsonData3["code"].ToString()));
		}
		string text2 = null;
		string text3 = null;
		string text4 = null;
		if (string.IsNullOrEmpty(text))
		{
			text2 = jsonData3["data"]["sdk_open_id"].ToString();
			text3 = jsonData3["data"]["access_token"].ToString();
			text4 = account;
		}
		if (callback != null)
		{
			callback(text, text2, text3, text4);
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001756C File Offset: 0x0001576C
	public static void AddJobResInfo(ref JsonData resInfo, string resType, JsonData result, long resLen)
	{
		if (result == null)
		{
			ParaLog.Log("AddJobResInfo Failed, resUri is null !");
			return;
		}
		string text = result.ToString();
		JsonData jsonData = new JsonData();
		jsonData["res_type"] = resType;
		jsonData["res_name"] = text;
		jsonData["res_len"] = resLen;
		JsonData jsonData2 = jsonData;
		if (resInfo == null)
		{
			resInfo = new JsonData();
		}
		resInfo.Add(jsonData2);
	}
}
