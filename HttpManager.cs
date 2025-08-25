using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using LitJson;
using TTNetHttp;
using TTNetHttp.Forms;
using UnityEditor;
using UnityEngine;

// Token: 0x0200004B RID: 75
public static class HttpManager
{
	// Token: 0x0600022B RID: 555 RVA: 0x0000E575 File Offset: 0x0000C775
	public static void SetRawAvatarSdkVersion(string avatarSdkVersion)
	{
		HttpManager._rawAvatarSdkVersion = avatarSdkVersion;
		HttpManager.OnRawSdkVersionSet();
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000E582 File Offset: 0x0000C782
	public static void SetRawWorldSdkVersion(string worldSdkVersion)
	{
		HttpManager._rawWorldSdkVersion = worldSdkVersion;
		HttpManager.OnRawSdkVersionSet();
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000E590 File Offset: 0x0000C790
	public static string GetRawSdkVersion()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(HttpManager._rawAvatarSdkVersion))
		{
			stringBuilder.Append("Avatar:");
			stringBuilder.Append(HttpManager._rawAvatarSdkVersion);
		}
		if (!string.IsNullOrEmpty(HttpManager._rawWorldSdkVersion))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append("World:");
			stringBuilder.Append(HttpManager._rawWorldSdkVersion);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000E608 File Offset: 0x0000C808
	private static void OnRawSdkVersionSet()
	{
		SkdPack skdPack = SdkManager.Instance.GetConfig().CalSdkPack();
		if (skdPack != SkdPack.kAvatar && skdPack != SkdPack.kWorld && (string.IsNullOrEmpty(HttpManager._rawAvatarSdkVersion) || string.IsNullOrEmpty(HttpManager._rawWorldSdkVersion)))
		{
			return;
		}
		if (!HttpManager._isTTNetHttpManagerInited)
		{
			HttpManager._isTTNetHttpManagerInited = true;
			HttpManager.InitTTNetHttpManager();
		}
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000E658 File Offset: 0x0000C858
	private static void InitTTNetHttpManager()
	{
		TTNetHttpManager.Init(518836L, "https://pc-mon-va.byteoversea.com", HttpDefine.TTNetHttpRootPathName, SystemInfo.deviceUniqueIdentifier, SdkManager.Instance.GetAccount().GetOpenId(), HttpManager.GetRawSdkVersion(), false);
		EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(HttpManager.OnEditorUpdate));
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
	private static void OnEditorUpdate()
	{
		TTNetHttpManager.Tick();
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000E6BB File Offset: 0x0000C8BB
	public static void Release()
	{
		TTNetHttpManager.ReleaseInstance();
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000E6C4 File Offset: 0x0000C8C4
	private static string RedirectionPlatform(string plat)
	{
		if (plat == "OSXEditor")
		{
			return "macOS";
		}
		if (plat == "WindowsEditor")
		{
			return "windows";
		}
		if (!(plat == "LinuxEditor"))
		{
			return "unknown";
		}
		return "linux";
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000E714 File Offset: 0x0000C914
	public static string GetClientInfo()
	{
		JsonData jsonData = new JsonData();
		jsonData["device_id"] = SystemInfo.deviceUniqueIdentifier;
		jsonData["platform"] = HttpManager.RedirectionPlatform(Application.platform.ToString());
		DateTime now = DateTime.Now;
		jsonData["time_diff"] = (int)(now - now.ToUniversalTime()).TotalSeconds;
		string text = jsonData.ToJson();
		string text2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(text2));
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000E7B7 File Offset: 0x0000C9B7
	public static string GetHttpsUrl(string url)
	{
		return SdkManager.Instance.GetConfig().GetBaseUrl() + url;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000E7CE File Offset: 0x0000C9CE
	public static Uri CreateUri(string url)
	{
		return new Uri(HttpManager.GetHttpsUrl(url));
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000E7DC File Offset: 0x0000C9DC
	private static void SetHeaderSdkVersion(TTNetHttpRequest request)
	{
		string text;
		string text2;
		if (SdkManager.Instance.GetConfig().IsDisabeUpdate())
		{
			text = "default";
			text2 = "default";
		}
		else
		{
			switch (SdkManager.Instance.GetConfig().CalSdkPack())
			{
			case SkdPack.kWorld:
				text2 = "default";
				text = HttpManager.GetWorldSdkVersion();
				break;
			case SkdPack.kAvatar:
				text2 = HttpManager.GetAvatarSdkVersion();
				text = "default";
				break;
			case SkdPack.kBoth:
				text2 = HttpManager.GetAvatarSdkVersion();
				text = HttpManager.GetWorldSdkVersion();
				break;
			default:
				text = "default";
				text2 = "default";
				break;
			}
		}
		request.SetHeader("Hamlet-Avatar-sdkVersion", text2);
		request.SetHeader("Hamlet-World-sdkVersion", text);
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000E880 File Offset: 0x0000CA80
	public static string GetAvatarSdkVersion()
	{
		return SdkManager.Instance.GetConfig().GetAvatarSdkVersion();
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000E891 File Offset: 0x0000CA91
	public static string GetWorldSdkVersion()
	{
		return SdkManager.Instance.GetConfig().GetWorldSdkVersion();
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
	private static void SetHeaderAuth(TTNetHttpRequest request, JsonData jsonData, string contentType, long timestamp)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!(contentType == "application/x-www-form-urlencoded"))
		{
			if (!(contentType == "application/json"))
			{
				if (!(contentType == "multipart/form-jsonData"))
				{
					goto IL_016D;
				}
				stringBuilder.Append("file_desc=");
				if (jsonData != null)
				{
					stringBuilder.Append(jsonData.ToJson());
				}
				else
				{
					stringBuilder.Append("");
				}
				stringBuilder.Append("&");
				goto IL_016D;
			}
		}
		else
		{
			List<string> list = new List<string>();
			if (jsonData == null)
			{
				goto IL_016D;
			}
			if (jsonData.Keys.Count > 0)
			{
				foreach (string text in jsonData.Keys)
				{
					list.Add(text.ToString());
				}
				list.Sort();
			}
			using (List<string>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string text2 = enumerator2.Current;
					stringBuilder.Append(text2);
					stringBuilder.Append("=");
					stringBuilder.Append(jsonData[text2].ToString());
					stringBuilder.Append("&");
				}
				goto IL_016D;
			}
		}
		stringBuilder.Append("body");
		stringBuilder.Append("=");
		if (jsonData != null)
		{
			stringBuilder.Append(jsonData.ToJson());
		}
		else
		{
			stringBuilder.Append("");
		}
		stringBuilder.Append("&");
		IL_016D:
		request.SetHeader("Content-Type", contentType);
		stringBuilder.Append("timestamp=");
		stringBuilder.Append(timestamp.ToString());
		string md = MD5Util.GetMD5(stringBuilder.ToString());
		request.SetHeader("Hamlet-Auth", md);
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000EA78 File Offset: 0x0000CC78
	private static void SetHeaderTimestamp(TTNetHttpRequest request, long timestamp)
	{
		request.SetHeader("Hamlet-Timestamp", timestamp.ToString());
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000EA8C File Offset: 0x0000CC8C
	public static void Send(string url, HTTPMethods methods, JsonData data, Action<int, string, JsonData> callback)
	{
		HttpManager.Send(url, methods, data, null, callback);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000EA98 File Offset: 0x0000CC98
	public static void SendWithUploadToken(string url, HTTPMethods methods, JsonData data, string uploadToken, Action<int, string, JsonData> callback)
	{
		HttpManager.Send(url, methods, data, new Dictionary<string, string> { { "Hamlet-UP-OP-INFO", uploadToken } }, callback);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
	public static void SendWithUploadTokenAndFeature(string url, HTTPMethods methods, JsonData data, string uploadToken, Action<int, string, JsonData> callback)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Hamlet-UP-OP-INFO", uploadToken);
		string feature = SdkManager.Instance.GetConfig().GetFeature();
		if (!string.IsNullOrEmpty(feature))
		{
			dictionary.Add("Tt-Hamlet-packer", feature);
		}
		HttpManager.Send(url, methods, data, dictionary, callback);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000EB14 File Offset: 0x0000CD14
	public static void Send(string url, HTTPMethods methodsBestHTTP, JsonData data, Dictionary<string, string> headerDict, Action<int, string, JsonData> callback)
	{
		TTNetHttpMethods ttnetHttpMethods = methodsBestHTTP;
		TTNetHttpRequest ttnetHttpRequest = new TTNetHttpRequest(HttpManager.CreateUri(url), ttnetHttpMethods, null);
		string text = ((ttnetHttpMethods == TTNetHttpMethods.Get) ? "application/x-www-form-urlencoded" : "application/json");
		HttpManager.SetBaseHeader(ttnetHttpRequest, data, text);
		if (headerDict != null)
		{
			foreach (KeyValuePair<string, string> keyValuePair in headerDict)
			{
				ttnetHttpRequest.SetHeader(keyValuePair.Key, keyValuePair.Value);
			}
		}
		if (data != null && ttnetHttpMethods != TTNetHttpMethods.Get)
		{
			string text2 = data.ToJson();
			ttnetHttpRequest.RawData = Encoding.UTF8.GetBytes(text2);
		}
		ttnetHttpRequest.DisableCache = false;
		ttnetHttpRequest.ConnectTimeout = TimeSpan.FromSeconds(20.0);
		ttnetHttpRequest.Timeout = TimeSpan.FromSeconds(0.0);
		ttnetHttpRequest.Tag = 3;
		ttnetHttpRequest.IsCookiesEnabled = false;
		ttnetHttpRequest.IsKeepAlive = false;
		HttpManager._Send(ttnetHttpRequest, callback);
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000EC10 File Offset: 0x0000CE10
	private static void _Process(TTNetHttpRequest request, TTNetHttpResponse response, Action<int, string, JsonData> callback)
	{
		ParaLog.Log("request.State:" + request.State.ToString());
		switch (request.State)
		{
		case HTTPRequestStates.Finished:
			HttpManager.OnRequestFinished(request, response, callback);
			return;
		case HTTPRequestStates.Error:
		{
			string text = "HTTPRequestStates  Error url:";
			Uri uri = request.Uri;
			ParaLog.Log(text + ((uri != null) ? uri.ToString() : null) + "  count:" + ((int)request.Tag).ToString());
			ParaLog.Log("HTTPRequestStates.Error  :" + request.Exception);
			goto IL_0136;
		}
		case HTTPRequestStates.ConnectionTimedOut:
		case HTTPRequestStates.TimedOut:
		{
			string text2 = "ConnectionTimedOut   url:";
			Uri uri2 = request.Uri;
			ParaLog.Log(text2 + ((uri2 != null) ? uri2.ToString() : null) + "  count:" + ((int)request.Tag).ToString());
			if (response != null)
			{
				ParaLog.Log("response:" + response.DataAsText);
				goto IL_0136;
			}
			goto IL_0136;
		}
		}
		string text3 = "HTTPRequestStates  Default url:";
		Uri uri3 = request.Uri;
		ParaLog.Log(text3 + ((uri3 != null) ? uri3.ToString() : null) + "  count:" + ((int)request.Tag).ToString());
		IL_0136:
		HttpManager._Send(request, callback);
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000ED5C File Offset: 0x0000CF5C
	private static void _Send(TTNetHttpRequest request, Action<int, string, JsonData> callback)
	{
		if ((int)request.Tag < 0)
		{
			ParaLog.Log("_Send   Tag < 0");
			HttpManager._Dispose(request);
			SdkManager.Instance.CommonHandle(-101, SdkLangManager.Get("str_sdk_sum_failed"));
			return;
		}
		request.Callback = delegate(TTNetHttpRequest request, TTNetHttpResponse response)
		{
			HttpManager.PrintResponse(request, response);
			HttpManager._Process(request, response, callback);
		};
		request.Tag = (int)request.Tag - 1;
		HttpManager.PrintRequest(request);
		request.Send();
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000EDE2 File Offset: 0x0000CFE2
	private static void _Dispose(TTNetHttpRequest request)
	{
		if (request == null)
		{
			return;
		}
		request.Clear();
		request.Dispose();
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000EDF4 File Offset: 0x0000CFF4
	private static void OnRequestFinished(TTNetHttpRequest request, TTNetHttpResponse response, Action<int, string, JsonData> callback)
	{
		if (response == null)
		{
			HttpManager._Dispose(request);
			if (SdkManager.Instance.IsAigcCloud())
			{
				callback(-100, "time out", null);
				return;
			}
			SdkManager.Instance.CommonHandle(-100, SdkLangManager.Get("str_sdk_net_error"));
			return;
		}
		else
		{
			ParaLog.Log(response.GetFirstHeaderValue("X-Tt-Logid"));
			if (response.StatusCode != 200)
			{
				int statusCode = response.StatusCode;
				string dataAsText = response.DataAsText;
				HttpManager._Dispose(request);
				if (callback != null)
				{
					callback(statusCode, dataAsText, null);
				}
				ParaLog.Log("Network error. Please try again. the content is " + dataAsText);
				return;
			}
			if (response.HasHeader("Hamlet-JWT"))
			{
				string firstHeaderValue = response.GetFirstHeaderValue("Hamlet-JWT");
				if (!string.IsNullOrEmpty(firstHeaderValue))
				{
					SdkManager.Instance.GetAccount().SetJwt(firstHeaderValue, false);
				}
			}
			JsonData jsonData = JsonMapper.ToObject(response.DataAsText);
			HttpManager._Dispose(request);
			string text = jsonData["message"].ToString();
			int num = int.Parse(jsonData["code"].ToString());
			text = SdkManager.Instance.GetErrorCode(num, text);
			if (SdkManager.Instance.IsAigcCloud() && num != 0)
			{
				callback(-100, "time out", null);
			}
			else if (SdkManager.Instance.CommonHandle(num, text))
			{
				return;
			}
			if (jsonData.Keys.Contains("data"))
			{
				callback(num, text, jsonData["data"]);
				return;
			}
			callback(num, text, null);
			return;
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000EF68 File Offset: 0x0000D168
	private static void _Upload(string url, string resType, string name, byte[] data, string uploadToken, string jobId, string partTaskId, int partIndex, Action<int, JsonData> callback)
	{
		TTNetHttpRequest ttnetHttpRequest = new TTNetHttpRequest(HttpManager.CreateUri(url), TTNetHttpMethods.Post, null);
		ttnetHttpRequest.DisableCache = false;
		ttnetHttpRequest.ConnectTimeout = TimeSpan.FromSeconds(20.0);
		ttnetHttpRequest.Tag = new HttpManager.FileInfo
		{
			Name = name,
			Count = 3,
			Url = url
		};
		ttnetHttpRequest.IsKeepAlive = false;
		ttnetHttpRequest.FormUsage = HTTPFormUsage.Multipart;
		HTTPMultiPartForm httpmultiPartForm = new HTTPMultiPartForm();
		JsonData jsonData = new JsonData();
		if (partIndex < 0)
		{
			jsonData["res_type"] = resType;
		}
		else
		{
			jsonData["part_task_id"] = partTaskId;
			jsonData["part_index"] = partIndex;
		}
		ttnetHttpRequest.SetHeader("Hamlet-UP-OP-INFO", uploadToken);
		HttpManager.SetBaseHeader(ttnetHttpRequest, jsonData, "multipart/form-jsonData");
		httpmultiPartForm.AddField("file_desc", jsonData.ToJson());
		httpmultiPartForm.AddBinaryData("file", data, name);
		ttnetHttpRequest.SetForm(httpmultiPartForm);
		string text = "_Upload url:";
		Uri uri = ttnetHttpRequest.Uri;
		ParaLog.Log(text + ((uri != null) ? uri.ToString() : null) + "  jsonData:" + jsonData.ToJson());
		HttpManager._Upload(ttnetHttpRequest, callback);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000F088 File Offset: 0x0000D288
	private static void SetBaseHeader(TTNetHttpRequest request, JsonData jsonData, string contentType)
	{
		string jwt = SdkManager.Instance.GetAccount().GetJwt();
		if (!string.IsNullOrEmpty(jwt))
		{
			request.SetHeader("Hamlet-JWT", jwt);
		}
		string env = SdkManager.Instance.GetConfig().GetEnv();
		if (!string.IsNullOrEmpty(env))
		{
			request.SetHeader("x-tt-env", env);
			Debug.Log("This is add x-tt-env :" + env);
		}
		request.SetHeader("Hamlet-Client-Info", HttpManager.GetClientInfo());
		HttpManager.SetHeaderSdkVersion(request);
		long serverCurrentTimestamp = HttpUtil.GetServerCurrentTimestamp();
		HttpManager.SetHeaderAuth(request, jsonData, contentType, serverCurrentTimestamp);
		HttpManager.SetHeaderTimestamp(request, serverCurrentTimestamp);
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000F11C File Offset: 0x0000D31C
	private static void _Upload(TTNetHttpRequest request, Action<int, JsonData> callback)
	{
		HttpManager.FileInfo fileInfo = (HttpManager.FileInfo)request.Tag;
		if (fileInfo.Count < 0)
		{
			ParaLog.Log("_Send   Tag < 0");
			HttpManager._Dispose(request);
			callback(-1, null);
			return;
		}
		request.Callback = delegate(TTNetHttpRequest request, TTNetHttpResponse response)
		{
			HttpManager.PrintResponse(request, response);
			HttpManager._UploadProcess(request, response, callback);
		};
		fileInfo.Count--;
		HttpManager.PrintRequest(request);
		request.Send();
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000F198 File Offset: 0x0000D398
	private static void PrintRequest(TTNetHttpRequest request)
	{
		if (request == null)
		{
			ParaLog.Log("request -----> PrintRequest Error, request is null");
			return;
		}
		string[] array = new string[6];
		array[0] = "request -----> url:";
		int num = 1;
		Uri uri = request.Uri;
		array[num] = ((uri != null) ? uri.ToString() : null);
		array[2] = "  data.Length:";
		array[3] = ((request.GetEntityBody() != null) ? ((float)request.GetEntityBody().Length / 1048576f) : 0f).ToString();
		array[4] = "\nrequest headers:\n";
		array[5] = request.GetHeadersFormatText();
		ParaLog.Log(string.Concat(array));
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000F224 File Offset: 0x0000D424
	private static void PrintResponse(TTNetHttpRequest request, TTNetHttpResponse response)
	{
		if (request == null)
		{
			ParaLog.Log("response --------------> PrintResponse Error, request is null");
			return;
		}
		string text;
		if (response != null)
		{
			text = response.DataAsText + "\nresponse headers:\n" + response.GetHeadersFormatText();
		}
		else
		{
			text = "null";
		}
		string[] array = new string[6];
		array[0] = "response --------------> url:";
		int num = 1;
		Uri uri = request.Uri;
		array[num] = ((uri != null) ? uri.ToString() : null);
		array[2] = ", request.State :";
		array[3] = request.State.ToString();
		array[4] = ", response = ";
		array[5] = text;
		ParaLog.Log(string.Concat(array));
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000F2BC File Offset: 0x0000D4BC
	private static void _UploadProcess(TTNetHttpRequest request, TTNetHttpResponse response, Action<int, JsonData> callback)
	{
		ParaLog.Log("_UploadProcess request.State:" + request.State.ToString());
		HTTPRequestStates state = request.State;
		if (state > HTTPRequestStates.Processing)
		{
			if (state == HTTPRequestStates.Finished)
			{
				HttpManager.OnUploadFinished(request, response, callback);
				return;
			}
			ParaLog.Log("OnUploadFinished   error 2 .....");
			HttpManager.FileInfo fileInfo = (HttpManager.FileInfo)request.Tag;
			ParaLog.Log(string.Concat(new string[]
			{
				"OnUploadFinished   error 3 .....",
				fileInfo.Count.ToString(),
				" name :",
				fileInfo.Name,
				" url:",
				fileInfo.Url
			}));
			HttpManager._Upload(request, callback);
		}
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000F36C File Offset: 0x0000D56C
	private static void OnUploadFinished(TTNetHttpRequest request, TTNetHttpResponse response, Action<int, JsonData> callback)
	{
		if (response != null && response.IsSuccess)
		{
			string text = "url:";
			Uri uri = request.Uri;
			ParaLog.Log(text + ((uri != null) ? uri.ToString() : null) + "  response:" + response.DataAsText);
			ParaLog.Log(response.GetFirstHeaderValue("X-Tt-Logid"));
			if (response.HasHeader("Hamlet-JWT"))
			{
				string firstHeaderValue = response.GetFirstHeaderValue("Hamlet-JWT");
				if (!string.IsNullOrEmpty(firstHeaderValue))
				{
					SdkManager.Instance.GetAccount().SetJwt(firstHeaderValue, false);
				}
			}
			JsonData jsonData = JsonMapper.ToObject(response.DataAsText);
			callback(0, jsonData);
			return;
		}
		ParaLog.Log("OnUploadFinished   error 1 .....");
		HttpManager._Upload(request, callback);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000F420 File Offset: 0x0000D620
	public static void Upload(string url, string resType, string name, byte[] data, string uploadToken, string jobId, string partTaskId, int partIndex, Action<int, string, JsonData> callback)
	{
		HttpManager._Upload(url, resType, name, data, uploadToken, jobId, partTaskId, partIndex, delegate(int code, JsonData response)
		{
			if (code == 0)
			{
				string text = response["message"].ToString();
				int num = int.Parse(response["code"].ToString());
				text = SdkManager.Instance.GetErrorCode(num, text);
				JsonData jsonData = null;
				if (partIndex < 0)
				{
					if (response.ContainsKey("data"))
					{
						jsonData = response["data"].ToString();
					}
					else
					{
						ParaLog.LogError("Upload response not Contains key 'data', url:" + url + ", response : " + response.ToString());
					}
				}
				callback(num, text, jsonData);
				return;
			}
			if (response != null && response.ContainsKey("message"))
			{
				string text2 = response["message"].ToString();
				int num2 = int.Parse(response["code"].ToString());
				callback(num2, text2, null);
				return;
			}
			callback(-1, SdkLangManager.Get("str_sdk_bottomMessage_text41") + code.ToString(), null);
		});
	}

	// Token: 0x0400014A RID: 330
	private static string _rawAvatarSdkVersion = "";

	// Token: 0x0400014B RID: 331
	private static string _rawWorldSdkVersion = "";

	// Token: 0x0400014C RID: 332
	private const float CONNECT_TIME_OUT_SECONDS = 20f;

	// Token: 0x0400014D RID: 333
	private const float REQUEST_TIME_OUT_SECONDS = 0f;

	// Token: 0x0400014E RID: 334
	private static bool _isTTNetHttpManagerInited = false;

	// Token: 0x0400014F RID: 335
	public const int TRY_REQUEST_COUNT = 3;

	// Token: 0x020000A7 RID: 167
	private class FileInfo
	{
		// Token: 0x04000377 RID: 887
		public string Name;

		// Token: 0x04000378 RID: 888
		public int Count;

		// Token: 0x04000379 RID: 889
		public string Url;
	}
}
