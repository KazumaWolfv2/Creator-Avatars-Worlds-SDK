using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LitJson;

// Token: 0x02000049 RID: 73
public class GSDKRequest
{
	// Token: 0x06000222 RID: 546 RVA: 0x0000E305 File Offset: 0x0000C505
	private static Dictionary<string, string> SetHeaderDict(Dictionary<string, string> headerDict)
	{
		headerDict.Add("charset", "utf-8");
		return headerDict;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000E318 File Offset: 0x0000C518
	private static JsonData SetCommonParams(JsonData jsonData)
	{
		jsonData["seq_id"] = Guid.NewGuid().ToString();
		jsonData["app_id"] = GSDKRequest.AppID;
		jsonData["language"] = "en";
		return jsonData;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000E374 File Offset: 0x0000C574
	private static string Encrypt(string content)
	{
		RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
		rsacryptoServiceProvider.FromXmlString(GSDKRequest.PublicKey);
		byte[] bytes = Encoding.UTF8.GetBytes(content);
		int i = 0;
		int num = 117;
		int num2 = bytes.Length;
		byte[] array = new byte[0];
		while (i < num2)
		{
			byte[] array2 = rsacryptoServiceProvider.Encrypt(bytes.Skip(i).Take(num).ToArray<byte>(), false);
			int num3 = array.Length;
			Array.Resize<byte>(ref array, num3 + array2.Length);
			Array.Copy(array2, 0, array, num3, array2.Length);
			i += num;
		}
		return Convert.ToBase64String(array);
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000E404 File Offset: 0x0000C604
	private static string ConverJsonToForm(JsonData data)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string text in data.Keys)
		{
			stringBuilder.Append(text);
			stringBuilder.Append("=");
			stringBuilder.Append(WebUtility.UrlEncode(data[text].ToString()));
			stringBuilder.Append("&");
		}
		StringBuilder stringBuilder2 = stringBuilder;
		int length = stringBuilder2.Length;
		stringBuilder2.Length = length - 1;
		ParaLog.Log(stringBuilder.ToString());
		return stringBuilder.ToString();
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000E4AC File Offset: 0x0000C6AC
	public static async Task<JsonData> PostAsync(string apiPath, JsonData data, Dictionary<string, string> headerDict)
	{
		JsonData jsonData2;
		try
		{
			string gsdkurl = SdkManager.Instance.GetConfig().GetGSDKUrl();
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(gsdkurl + apiPath);
			foreach (KeyValuePair<string, string> keyValuePair in headerDict)
			{
				request.Headers.Add(keyValuePair.Key, keyValuePair.Value);
			}
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			byte[] bytes = Encoding.UTF8.GetBytes(GSDKRequest.ConverJsonToForm(data));
			request.ContentLength = (long)bytes.Length;
			Stream stream2 = await request.GetRequestStreamAsync();
			Stream stream = stream2;
			await stream.WriteAsync(bytes, 0, bytes.Length);
			await stream.FlushAsync();
			TaskAwaiter<WebResponse> taskAwaiter = request.GetResponseAsync().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<WebResponse> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<WebResponse>);
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)taskAwaiter.GetResult();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
			if (httpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				JsonData jsonData = new JsonData();
				jsonData["StatusCode"] = (int)httpWebResponse.StatusCode;
				jsonData2 = jsonData;
			}
			else
			{
				jsonData2 = JsonMapper.ToObject(await streamReader.ReadToEndAsync());
			}
		}
		catch (Exception ex)
		{
			ParaLog.LogError(ex.Message);
			jsonData2 = new JsonData();
		}
		return jsonData2;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000E500 File Offset: 0x0000C700
	public static async Task<JsonData> PostWithEncryptAsync(string apiPath, JsonData data, Dictionary<string, string> headerDict = null)
	{
		GSDKRequest.SetCommonParams(data);
		ParaLog.Log(data.ToJson());
		JsonData jsonData = new JsonData();
		jsonData["encrypt_msg"] = GSDKRequest.Encrypt(data.ToJson());
		JsonData jsonData2 = jsonData;
		return await GSDKRequest.PostAsync(apiPath, jsonData2, GSDKRequest.SetHeaderDict(headerDict ?? new Dictionary<string, string>()));
	}

	// Token: 0x0400013D RID: 317
	private static readonly int AppID = 371774;

	// Token: 0x0400013E RID: 318
	private static readonly string PublicKey = "<RSAKeyValue><Exponent>AQAB</Exponent><Modulus>vW/luwohBFJOWkKR6kK9mEejzmqk3FewLEGEMQR+TecOEA3OtzFvsxvqEfeBgnzGfFEpbOkB9xUridY+lnlc1pov4v8jPiteKuVOIPT5Osm5Rlcov2lfNBXz8Z2oMQ9ukAJ099CjceMNzAx1fzvvT88tyM+74Om4nfigmUDa040=</Modulus></RSAKeyValue>";
}
