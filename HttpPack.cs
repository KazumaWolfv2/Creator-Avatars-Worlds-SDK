using System;
using System.Collections.Generic;
using LitJson;
using UnityEditor;

// Token: 0x0200004C RID: 76
public class HttpPack
{
	// Token: 0x0600024C RID: 588 RVA: 0x0000F48C File Offset: 0x0000D68C
	public static bool Send(string url, string uploadToken, string jobId, string resType, string name, byte[] data, bool allowPartUpload, Action<int, string, JsonData> callback)
	{
		HttpPack httpPack = new HttpPack();
		if (httpPack.Init(url, uploadToken, jobId, resType, name, data, allowPartUpload, callback))
		{
			httpPack.RealSend();
			return true;
		}
		return false;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000F4BC File Offset: 0x0000D6BC
	private bool Init(string url, string uploadToken, string jobId, string resType, string name, byte[] data, bool allowPartUpload, Action<int, string, JsonData> callback)
	{
		this.UploadToken = uploadToken;
		this.JobId = jobId;
		this.Name = name;
		this.Data = data;
		this.Url = url;
		this.ResType = resType;
		this.AllowPartUpload = allowPartUpload;
		this.Callback = callback;
		return true;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000F4FC File Offset: 0x0000D6FC
	private void RealSend()
	{
		ParaLog.Log("##########################RealSend----------" + ((float)this.Data.Length / (float)this.ChunkSize).ToString());
		if (!this.AllowPartUpload || this.Data.Length < this.ChunkSize * 50)
		{
			this.SendSign();
			return;
		}
		this.SendParts();
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000F55C File Offset: 0x0000D75C
	private void SendSign()
	{
		ParaLog.Log("##########################SendSign----------" + this.Name);
		HttpManager.Upload(this.Url + "/upload", this.ResType, this.Name, this.Data, this.UploadToken, this.JobId, null, -1, delegate(int code, string message, JsonData result)
		{
			this.Callback(code, message, result);
		});
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
	private void SendParts()
	{
		ParaLog.Log("##########################SendParts----------" + this.Name);
		int num = this.Data.Length / (this.ChunkSize * this.kChunkPartSize);
		if (this.Data.Length % (this.ChunkSize * this.kChunkPartSize) > this.ChunkSize * 5)
		{
			num++;
		}
		this.ChunkTotalPart = num;
		this.SendPartsCreate();
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000F62C File Offset: 0x0000D82C
	private void SendPartsCreate()
	{
		JsonData jsonData = new JsonData();
		jsonData["res_type"] = this.ResType;
		jsonData["part_num"] = this.ChunkTotalPart;
		jsonData["total_size"] = this.Data.Length;
		jsonData["file_ext"] = this.Name.Substring(this.Name.LastIndexOf(".") + 1);
		HttpManager.SendWithUploadToken(this.Url + "/part/create", 2, jsonData, this.UploadToken, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				this.PartTaskId = result.ToString();
				this.StartSendParts();
				return;
			}
			ParaLog.LogError("SendPartsCreate error:" + message);
			this.Callback(code, message, result);
		});
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000F6DC File Offset: 0x0000D8DC
	private void SendPartsComplete()
	{
		JsonData jsonData = new JsonData();
		jsonData["part_task_id"] = this.PartTaskId;
		HttpManager.SendWithUploadToken(this.Url + "/part/complete", 2, jsonData, this.UploadToken, delegate(int code, string message, JsonData result)
		{
			if (code == 0)
			{
				ParaLog.Log("=========SendPartsComplete");
				this.IsPartTaskComplete = true;
				this.Callback(code, message, result);
				return;
			}
			ParaLog.LogError("SendPartsComplete error:" + message);
			this.Callback(code, message, result);
		});
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000F72E File Offset: 0x0000D92E
	private void StartSendParts()
	{
		EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
		this.ThreadCount = 0;
		this.IsUploading = true;
		this.IsPartTaskComplete = false;
		this.UploadPartSuccessCount = 0;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000F76C File Offset: 0x0000D96C
	private void Update()
	{
		this.OnThread();
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000F774 File Offset: 0x0000D974
	private void OnThread()
	{
		if (!this.IsUploading)
		{
			return;
		}
		if (this.ThreadCount >= 3)
		{
			return;
		}
		if (this.ThreadCount == 0 && this.CheckAllPartsUploadSuccess())
		{
			return;
		}
		int num = this.CalPartIndex();
		if (num < 0)
		{
			return;
		}
		this.SendPartsData(num);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
	private void SendPartsData(int i)
	{
		ParaLog.Log("=========SendPartsData:" + i.ToString());
		int num = i * this.ChunkSize * this.kChunkPartSize;
		int num2 = (i + 1) * this.ChunkSize * this.kChunkPartSize;
		if (num2 > this.Data.Length || i == this.ChunkTotalPart - 1)
		{
			num2 = this.Data.Length;
		}
		byte[] array = new byte[num2 - num];
		Array.Copy(this.Data, num, array, 0, num2 - num);
		this.SendPart(array, i);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000F840 File Offset: 0x0000DA40
	private int CalPartIndex()
	{
		for (int i = 0; i < this.ChunkTotalPart; i++)
		{
			if (!this.ChunkStatusList.Contains(i))
			{
				this.ChunkStatusList.Add(i);
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000F87C File Offset: 0x0000DA7C
	private void SendPart(byte[] chunk, int index)
	{
		this.ThreadCount++;
		HttpManager.Upload(this.Url + "/part/upload", this.ResType, this.Name, chunk, this.UploadToken, this.JobId, this.PartTaskId, index + 1, delegate(int code, string message, JsonData result)
		{
			ParaLog.Log("=========SendPart Upload:" + index.ToString() + " ThreadCount:" + this.ThreadCount.ToString());
			if (code == 0)
			{
				this.ThreadCount--;
				this.UploadPartSuccessCount++;
				ParaLog.Log(string.Concat(new string[]
				{
					index.ToString(),
					"=========SendPartsData UploadPartSuccessCount:",
					this.UploadPartSuccessCount.ToString(),
					" ChunkTotalPart:",
					this.ChunkTotalPart.ToString()
				}));
				if (this.UploadPartSuccessCount == this.ChunkTotalPart)
				{
					this.SendPartsComplete();
					return;
				}
			}
			else
			{
				this.UploadFailed(code, message);
			}
		});
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000F8F3 File Offset: 0x0000DAF3
	private void UploadFailed(int code, string message)
	{
		this.IsUploading = false;
		EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
		this.Callback(code, message, null);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000F92A File Offset: 0x0000DB2A
	private bool CheckAllPartsUploadSuccess()
	{
		if (this.IsPartTaskComplete)
		{
			this.IsUploading = false;
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
			return true;
		}
		return false;
	}

	// Token: 0x04000150 RID: 336
	private string UploadToken;

	// Token: 0x04000151 RID: 337
	private bool AllowPartUpload;

	// Token: 0x04000152 RID: 338
	private string JobId;

	// Token: 0x04000153 RID: 339
	private string ResType;

	// Token: 0x04000154 RID: 340
	private string Name;

	// Token: 0x04000155 RID: 341
	private string PartTaskId;

	// Token: 0x04000156 RID: 342
	private byte[] Data;

	// Token: 0x04000157 RID: 343
	private string Url;

	// Token: 0x04000158 RID: 344
	private Action<int, string, JsonData> Callback;

	// Token: 0x04000159 RID: 345
	private int ChunkSize = 1048576;

	// Token: 0x0400015A RID: 346
	private int ChunkTotalPart;

	// Token: 0x0400015B RID: 347
	private int kChunkPartSize = 40;

	// Token: 0x0400015C RID: 348
	private object lockObj = new object();

	// Token: 0x0400015D RID: 349
	private int ThreadCount;

	// Token: 0x0400015E RID: 350
	private int UploadPartSuccessCount;

	// Token: 0x0400015F RID: 351
	private bool IsUploading;

	// Token: 0x04000160 RID: 352
	private bool IsPartTaskComplete;

	// Token: 0x04000161 RID: 353
	private const int MAX_THREAD_COUNT = 3;

	// Token: 0x04000162 RID: 354
	private List<int> ChunkStatusList = new List<int>();
}
