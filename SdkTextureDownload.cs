using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class SdkTextureDownload
{
	// Token: 0x06000471 RID: 1137 RVA: 0x0001D753 File Offset: 0x0001B953
	public void Init()
	{
		this._DownloadList.Clear();
		this._TextureList.Clear();
		this._UriDict.Clear();
		this._IsDownloading = false;
		this.CheckDir();
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001D784 File Offset: 0x0001B984
	private void CheckDir()
	{
		int num = ParaPathDefine.kSdkDownLoadPath.LastIndexOf("/");
		string text = ParaPathDefine.kSdkDownLoadPath.Substring(0, num);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (!Directory.Exists(ParaPathDefine.kSdkDownLoadPath))
		{
			Directory.CreateDirectory(ParaPathDefine.kSdkDownLoadPath);
		}
		if (!Directory.Exists(ParaPathDefine.kSdkDownLoadPath + "/Snapshot/"))
		{
			Directory.CreateDirectory(ParaPathDefine.kSdkDownLoadPath + "/Snapshot/");
		}
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001D7FF File Offset: 0x0001B9FF
	public bool IsDownLoaded(string uri)
	{
		return !string.IsNullOrEmpty(uri) && this._UriDict.ContainsKey(uri) && this._TextureList.Contains(this._UriDict[uri]);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001D834 File Offset: 0x0001BA34
	public Texture2D LoadTexture(string uri)
	{
		string text = this._UriDict[uri];
		if (!File.Exists(text))
		{
			if (this._TextureList.Contains(text))
			{
				this._TextureList.Remove(text);
			}
			this._UriDict.Remove(uri);
			return null;
		}
		return EditorUtil.LoadTexture(this._UriDict[uri]);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001D891 File Offset: 0x0001BA91
	private bool IsExist(string file)
	{
		if (this._TextureList.Contains(file))
		{
			return true;
		}
		if (File.Exists(file))
		{
			this._TextureList.Add(file);
			return true;
		}
		return false;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001D8BC File Offset: 0x0001BABC
	public void DownLoadTexture(string uri, string remotePath, string subPath)
	{
		string text = this.TransformPngPath(uri, subPath);
		if (this._UriDict.ContainsKey(uri))
		{
			uri = this._UriDict[uri];
		}
		else
		{
			this._UriDict.Add(uri, text);
		}
		if (this.IsExist(text))
		{
			return;
		}
		SdkTextureDownload.SdkTextureDownloadItem sdkTextureDownloadItem = new SdkTextureDownload.SdkTextureDownloadItem(remotePath, text);
		this._DownloadList.Add(sdkTextureDownloadItem);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0001D91C File Offset: 0x0001BB1C
	public string TransformPngPath(string path, string subPath)
	{
		if (string.IsNullOrEmpty(path))
		{
			EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_error"), SdkLangManager.Get("str_sdk_textureDownload_text0"), "OK", null);
			return "";
		}
		int num = path.LastIndexOf("/");
		if (num == -1)
		{
			EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_error"), SdkLangManager.Get("str_sdk_textureDownload_text0"), "OK", null);
			return "";
		}
		string text = path.Substring(num + 1);
		return ParaPathDefine.kSdkDownLoadPath + "/Snapshot/" + subPath + text;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0001D9A8 File Offset: 0x0001BBA8
	public void Update()
	{
		if (!this._IsDownloading && this._DownloadList.Count > 0)
		{
			SdkTextureDownload.SdkTextureDownloadItem sdkTextureDownloadItem = this._DownloadList[0];
			this._DownloadList.RemoveAt(0);
			this.DownloadTexture(sdkTextureDownloadItem);
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001D9EC File Offset: 0x0001BBEC
	private void DownloadTexture(SdkTextureDownload.SdkTextureDownloadItem item)
	{
		string url = item.Url;
		string path = item.Path;
		ThreadPool.QueueUserWorkItem(delegate(object pngPath)
		{
			DownLoadProxy.DownloadPng(pngPath as string, path, delegate(int code)
			{
				this._IsDownloading = false;
				if (code == 0)
				{
					this._TextureList.Add(item.Path);
					return;
				}
				if (item.Count >= 3)
				{
					EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_error"), SdkLangManager.Get("str_sdk_textureDownload_text2"), "OK", null);
					return;
				}
				item.Count++;
				this._DownloadList.Add(item);
			});
		}, url);
	}

	// Token: 0x04000283 RID: 643
	private List<SdkTextureDownload.SdkTextureDownloadItem> _DownloadList = new List<SdkTextureDownload.SdkTextureDownloadItem>();

	// Token: 0x04000284 RID: 644
	private List<string> _TextureList = new List<string>();

	// Token: 0x04000285 RID: 645
	private Dictionary<string, string> _UriDict = new Dictionary<string, string>();

	// Token: 0x04000286 RID: 646
	private bool _IsDownloading;

	// Token: 0x020000D7 RID: 215
	private class SdkTextureDownloadItem
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x000264DA File Offset: 0x000246DA
		public SdkTextureDownloadItem(string remotePath, string localPath)
		{
			this.Url = remotePath;
			this.Path = localPath;
			this.Count = 0;
		}

		// Token: 0x04000400 RID: 1024
		public string Url;

		// Token: 0x04000401 RID: 1025
		public string Path;

		// Token: 0x04000402 RID: 1026
		public int Count;
	}
}
