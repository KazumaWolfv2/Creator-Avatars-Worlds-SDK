using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class SdkConfig
{
	// Token: 0x06000442 RID: 1090 RVA: 0x0001D1E0 File Offset: 0x0001B3E0
	public void SetIsDisabeUpdate(bool disable)
	{
		this._isDisabeUpdate = disable;
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0001D1E9 File Offset: 0x0001B3E9
	public bool IsDisabeUpdate()
	{
		return this._isDisabeUpdate;
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0001D1F1 File Offset: 0x0001B3F1
	public string GetUrlEnv()
	{
		return this.UrlEnv;
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0001D1F9 File Offset: 0x0001B3F9
	public string GetServerAddr()
	{
		return null;
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0001D1F9 File Offset: 0x0001B3F9
	public string GetServerRegion()
	{
		return null;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0001D1FC File Offset: 0x0001B3FC
	public string GetBaseUrl()
	{
		return HttpSDKConfig.BASE_URL;
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0001D203 File Offset: 0x0001B403
	public string GetGSDKUrl()
	{
		return HttpSDKConfig.ACCOUNT_URL;
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0001D20A File Offset: 0x0001B40A
	public string GetLogUrl()
	{
		return HttpSDKConfig.APPLOG_URL;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001D211 File Offset: 0x0001B411
	public string GetWebUrl()
	{
		return HttpSDKConfig.Web_URL;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0001D218 File Offset: 0x0001B418
	public string GetServerId()
	{
		return HttpSDKConfig.ServerID;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00003394 File Offset: 0x00001594
	public void Init()
	{
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0001D21F File Offset: 0x0001B41F
	public string GetFeature()
	{
		return this.kFeature;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0001D227 File Offset: 0x0001B427
	public void SetEnv(string env)
	{
		this._env = env;
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0001D230 File Offset: 0x0001B430
	public string GetEnv()
	{
		return this._env;
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00006FEA File Offset: 0x000051EA
	public bool IsShowReLoadConfig()
	{
		return false;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00003394 File Offset: 0x00001594
	public void ReLoadConfig()
	{
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00006FEA File Offset: 0x000051EA
	public bool IsShowCsToDll()
	{
		return false;
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00003394 File Offset: 0x00001594
	public void CsToDllSdk()
	{
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001D238 File Offset: 0x0001B438
	public void DeleteCsCode()
	{
		string text = Application.dataPath.Replace("Assets", "Packages");
		this.SafeRemoveDir(text + "/com.para.avatar/Editor");
		this.SafeRemoveDir(text + "/com.para.avatar/Runtime");
		this.SafeRemoveDir(text + "/com.para.common/Editor");
		this.SafeRemoveDir(text + "/com.para.common/Runtime");
		this.SafeRemoveDir(text + "/com.para.internal/Editor");
		this.SafeRemoveDir(text + "/com.para.internal/Runtime");
		this.SafeRemoveDir(text + "/com.para.world/Editor");
		this.SafeRemoveDir(text + "/com.para.world/Runtime");
		this.SafeRemoveDir(text + "/com.para.object/Editor");
		this.SafeRemoveDir(text + "/com.para.object/Runtime");
		this.SafeRemoveDir(text + "/com.para.services/Editor");
		this.SafeRemoveDir(text + "/com.para.services/Runtime");
		this.SafeRemoveDir(text + "/com.para.script/Editor");
		this.SafeRemoveDir(text + "/com.para.script/Runtime");
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001D348 File Offset: 0x0001B548
	private void SafeRemoveDir(string path)
	{
		if (Directory.Exists(path))
		{
			Directory.Delete(path, true);
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0001D35C File Offset: 0x0001B55C
	private static SdkVersionFile ReadFileList(bool isAvatar)
	{
		string text = (isAvatar ? "Packages/com.para.common/Config/AvatarSdkFileList.json" : "Packages/com.para.common/Config/WorldSdkFileList.json");
		string text2 = Application.dataPath.Replace("Assets", text);
		if (!File.Exists(text2))
		{
			EditorUtility.DisplayDialog(SdkLangManager.Get("str_sdk_tips"), SdkLangManager.Get("str_sdk_bottomMessage_text36") + text2, SdkLangManager.Get("str_sdk_confirm"));
			return null;
		}
		return SdkConfig.ReadFile(text2);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0001D3C4 File Offset: 0x0001B5C4
	private static SdkVersionFile ReadFile(string path)
	{
		return JsonUtility.FromJson<SdkVersionFile>(File.ReadAllText(path));
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0001D3D4 File Offset: 0x0001B5D4
	public string GetAvatarSdkVersion()
	{
		if (string.IsNullOrEmpty(this.AvatarSdkVersion))
		{
			SdkVersionFile sdkVersionFile = SdkConfig.ReadFileList(true);
			this.AvatarSdkVersion = sdkVersionFile.Version;
		}
		return this.AvatarSdkVersion;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0001D408 File Offset: 0x0001B608
	public SkdPack CalSdkPack()
	{
		SkdPack skdPack = SkdPack.KNone;
		string text = Application.dataPath.Replace("Assets", "Packages");
		if (File.Exists(text + "/com.para.world/Plugins/Editor/com.para.world.editor.dll"))
		{
			skdPack = SkdPack.kWorld;
		}
		if (File.Exists(text + "/com.para.avatar/Plugins/Editor/com.para.avatar.editor.dll"))
		{
			if (skdPack == SkdPack.kWorld)
			{
				skdPack = SkdPack.kBoth;
			}
			else
			{
				skdPack = SkdPack.kAvatar;
			}
		}
		return skdPack;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0001D45B File Offset: 0x0001B65B
	public void ClearVersion()
	{
		this.AvatarSdkVersion = null;
		this.WorldSdkVersion = null;
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0001D46C File Offset: 0x0001B66C
	public string GetWorldSdkVersion()
	{
		if (string.IsNullOrEmpty(this.WorldSdkVersion))
		{
			SdkVersionFile sdkVersionFile = SdkConfig.ReadFileList(false);
			this.WorldSdkVersion = sdkVersionFile.Version;
		}
		return this.WorldSdkVersion;
	}

	// Token: 0x04000273 RID: 627
	private SdkConfigData _data;

	// Token: 0x04000274 RID: 628
	private string UrlEnv = "online";

	// Token: 0x04000275 RID: 629
	private string kFeature = "dev";

	// Token: 0x04000276 RID: 630
	private bool _isDisabeUpdate;

	// Token: 0x04000277 RID: 631
	private string _env;

	// Token: 0x04000278 RID: 632
	private string AvatarSdkVersion = "";

	// Token: 0x04000279 RID: 633
	private string WorldSdkVersion = "";
}
