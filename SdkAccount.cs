using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class SdkAccount
{
	// Token: 0x06000418 RID: 1048 RVA: 0x0001C152 File Offset: 0x0001A352
	public void Init()
	{
		this._Info = new SdkAccount.SdkAccountInfo();
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001C15F File Offset: 0x0001A35F
	public bool IsLogin()
	{
		return !string.IsNullOrEmpty(this.GetJwt());
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001C16F File Offset: 0x0001A36F
	public SdkAccount.SdkAccountInfo GetInfo()
	{
		return this._Info;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001C177 File Offset: 0x0001A377
	public bool IsUserInfoInit()
	{
		return this._Info.IsInit;
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001C184 File Offset: 0x0001A384
	public void SetInfo(string ParaID, string nickName, int avatarCount, int worldCount, int reviewCount, string url, string uri)
	{
		this._Info.IsInit = true;
		this._Info.ParaID = ParaID;
		this._Info.NickName = nickName;
		this._Info.AvatarCount = avatarCount;
		this._Info.WorldCount = worldCount;
		this._Info.ReviewCount = reviewCount;
		this._Info.Url = url;
		this._Info.Uri = uri;
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001C1F5 File Offset: 0x0001A3F5
	public void Login(string openId, string token, string paraId)
	{
		this.SetOpenId(openId, false);
		this.SetToken(token, false);
		this.SetParaId(paraId, false);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001C20F File Offset: 0x0001A40F
	public void SetOpenId(string openId, bool isReal = false)
	{
		this._OpenId = openId;
		if (!string.IsNullOrEmpty(this._OpenId) || isReal)
		{
			PlayerPrefs.SetString("SdkOpenId", this._OpenId);
		}
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001C23A File Offset: 0x0001A43A
	public string GetOpenId()
	{
		if (string.IsNullOrEmpty(this._OpenId))
		{
			this._OpenId = PlayerPrefs.GetString("SdkOpenId");
		}
		return this._OpenId;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001C25F File Offset: 0x0001A45F
	public void SetParaId(string paraId, bool isReal = false)
	{
		this._ParaId = paraId;
		if (!string.IsNullOrEmpty(this._ParaId) || isReal)
		{
			PlayerPrefs.SetString("SdkParaId", this._ParaId);
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001C28A File Offset: 0x0001A48A
	public string GetParaId()
	{
		if (string.IsNullOrEmpty(this._ParaId))
		{
			this._ParaId = PlayerPrefs.GetString("SdkParaId");
		}
		return this._ParaId;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0001C2AF File Offset: 0x0001A4AF
	public void SetToken(string token, bool isReal = false)
	{
		this._Token = token;
		if (!string.IsNullOrEmpty(this._Token) || isReal)
		{
			PlayerPrefs.SetString("SdkToken", this._Token);
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001C2DA File Offset: 0x0001A4DA
	public string GetToken()
	{
		if (string.IsNullOrEmpty(this._Token))
		{
			this._Token = PlayerPrefs.GetString("SdkToken");
		}
		return this._Token;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001C2FF File Offset: 0x0001A4FF
	public void SetJwt(string jwt, bool isReal = false)
	{
		this._JWT = jwt;
		if (!string.IsNullOrEmpty(this._JWT) || isReal)
		{
			PlayerPrefs.SetString("SdkJWT", this._JWT);
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001C32A File Offset: 0x0001A52A
	public string GetJwt()
	{
		if (string.IsNullOrEmpty(this._JWT))
		{
			this._JWT = PlayerPrefs.GetString("SdkJWT");
		}
		return this._JWT;
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001C34F File Offset: 0x0001A54F
	public void Logout()
	{
		this.SetJwt("", true);
		this.SetOpenId("", true);
		this.SetToken("", true);
		this.SetParaId("", true);
		this._Info.Clear();
	}

	// Token: 0x04000246 RID: 582
	private string _OpenId;

	// Token: 0x04000247 RID: 583
	private string _ParaId;

	// Token: 0x04000248 RID: 584
	private string _Token;

	// Token: 0x04000249 RID: 585
	private string _JWT;

	// Token: 0x0400024A RID: 586
	private SdkAccount.SdkAccountInfo _Info;

	// Token: 0x020000D2 RID: 210
	public class SdkAccountInfo
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x00025DA4 File Offset: 0x00023FA4
		public void Clear()
		{
			this.IsInit = false;
			this.NickName = "";
			this.ParaID = "";
			this.AvatarCount = 0;
			this.WorldCount = 0;
			this.ReviewCount = 0;
			this.Url = "";
			this.Uri = "";
		}

		// Token: 0x040003EB RID: 1003
		public bool IsInit;

		// Token: 0x040003EC RID: 1004
		public string NickName = "";

		// Token: 0x040003ED RID: 1005
		public string ParaID = "";

		// Token: 0x040003EE RID: 1006
		public int AvatarCount;

		// Token: 0x040003EF RID: 1007
		public int WorldCount;

		// Token: 0x040003F0 RID: 1008
		public int ReviewCount;

		// Token: 0x040003F1 RID: 1009
		public string Url = "";

		// Token: 0x040003F2 RID: 1010
		public string Uri = "";
	}
}
