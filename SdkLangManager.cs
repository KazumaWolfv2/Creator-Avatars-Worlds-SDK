using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class SdkLangManager
{
	// Token: 0x0600026A RID: 618 RVA: 0x0000FC79 File Offset: 0x0000DE79
	public static void Init(string lang)
	{
		if (string.IsNullOrEmpty(lang))
		{
			return;
		}
		SdkLangManager.Load(lang);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000FC8A File Offset: 0x0000DE8A
	public static void Load(string lang)
	{
		SdkLangManager.Lang = lang;
		SdkLangManager.SetLang();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000FC98 File Offset: 0x0000DE98
	public static void SetLang()
	{
		string text = "/com.para.common/Config/Lang/sdk_lang_" + SdkLangManager.Lang + ".json";
		string text2 = Application.dataPath.Replace("Assets", "Packages") + text;
		if (!File.Exists(text2))
		{
			return;
		}
		SdkLangManager.LangDic = JsonMapper.ToObject<Dictionary<string, string>>(File.ReadAllText(text2));
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000FCEE File Offset: 0x0000DEEE
	private static string _Get(string key)
	{
		if (!SdkLangManager.LangDic.ContainsKey(key))
		{
			return null;
		}
		return SdkLangManager.LangDic[key];
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000FD0C File Offset: 0x0000DF0C
	public static string Get(string key)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return text;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000FD28 File Offset: 0x0000DF28
	public static string Get(string key, object arg0)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, arg0);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x0000FD48 File Offset: 0x0000DF48
	public static string Get(string key, object arg0, object arg1)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, arg0, arg1);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000FD6C File Offset: 0x0000DF6C
	public static string Get(string key, object arg0, object arg1, object arg2)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, arg0, arg1, arg2);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000FD90 File Offset: 0x0000DF90
	public static string Get(string key, object arg0, object arg1, object arg2, object arg3)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, new object[] { arg0, arg1, arg2, arg3 });
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000FDC8 File Offset: 0x0000DFC8
	public static string Get(string key, object arg0, object arg1, object arg2, object arg3, object arg4)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, new object[] { arg0, arg1, arg2, arg3, arg4 });
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000FE04 File Offset: 0x0000E004
	public static string Get(string key, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, new object[] { arg0, arg1, arg2, arg3, arg4, arg5 });
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000FE44 File Offset: 0x0000E044
	public static string Get(string key, params object[] args)
	{
		string text = SdkLangManager._Get(key);
		if (text == null)
		{
			return key;
		}
		return string.Format(text, args);
	}

	// Token: 0x0400016B RID: 363
	private static string Lang = "en";

	// Token: 0x0400016C RID: 364
	private static Dictionary<string, string> LangDic = new Dictionary<string, string>();
}
