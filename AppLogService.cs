using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;

// Token: 0x02000047 RID: 71
public static class AppLogService
{
	// Token: 0x0600021D RID: 541 RVA: 0x0000E00C File Offset: 0x0000C20C
	public static async Task PushAppLog(JsonData contentData)
	{
		Dictionary<string, string> dictionary = AppLogService.SetHeaderData();
		AppLogService.SetCommonParams(contentData);
		await AppLogService.PostAsync(dictionary, contentData);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000E04F File Offset: 0x0000C24F
	private static Dictionary<string, string> SetHeaderData()
	{
		return new Dictionary<string, string>
		{
			{ "charset", "utf-8" },
			{ "App-Key", "9f5aa8315a04d70f864bca9b07cedae5" }
		};
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000E078 File Offset: 0x0000C278
	private static JsonData SetCommonParams(JsonData jsonData)
	{
		jsonData["time"] = DateTimeOffset.Now.ToUnixTimeSeconds();
		jsonData["server_id"] = SdkManager.Instance.GetConfig().GetServerId();
		jsonData["app_id"] = HttpSDKConfig.AppID;
		jsonData["device_id"] = SystemInfo.deviceUniqueIdentifier;
		jsonData["user_unique_id"] = SdkManager.Instance.GetAccount().GetOpenId();
		jsonData["real_package_name"] = "com.hamlet.sdk";
		jsonData["role_id"] = SdkManager.Instance.GetAccount().GetOpenId();
		jsonData["role_name"] = SdkManager.Instance.GetAccount().GetInfo().NickName;
		jsonData["para_id"] = SdkManager.Instance.GetAccount().GetInfo().ParaID;
		jsonData["uuid"] = Guid.NewGuid().ToString();
		return jsonData;
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000E1AC File Offset: 0x0000C3AC
	private static async Task PostAsync(Dictionary<string, string> headerData, JsonData contentData)
	{
		string logUrl = SdkManager.Instance.GetConfig().GetLogUrl();
		StringContent stringContent = new StringContent(contentData.ToJson(), Encoding.UTF8, "text/plain");
		HttpClient httpClient = new HttpClient();
		foreach (KeyValuePair<string, string> keyValuePair in headerData)
		{
			httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
		}
		HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(logUrl, stringContent);
		if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
		{
			ParaLog.Log(string.Format("AppLogService.Post Failed {0}", httpResponseMessage.StatusCode));
		}
	}
}
