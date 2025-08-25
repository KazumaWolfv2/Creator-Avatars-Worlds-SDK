using System;
using System.Collections.Generic;
using UnityEditor;

// Token: 0x02000074 RID: 116
public class ErrorCodeManager
{
	// Token: 0x060003F4 RID: 1012 RVA: 0x0001B3F1 File Offset: 0x000195F1
	public void Init()
	{
		this._errorCodeDic.Add(0, SdkLangManager.Get("str_sdk_errorCode_text0"));
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001B409 File Offset: 0x00019609
	public string GetErrorMessage(int code, string message)
	{
		if (this._errorCodeDic.ContainsKey(code))
		{
			return this._errorCodeDic[code];
		}
		return message;
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001B428 File Offset: 0x00019628
	public bool CommonHandle(int code, string message, SdkManager manager)
	{
		if (code == 0)
		{
			return false;
		}
		EditorUtility.ClearProgressBar();
		if (code <= -50006)
		{
			if (code == -50032)
			{
				EditorUtil.OpenActionInfo(SdkLangManager.Get("str_sdk_tips"), SdkLangManager.Get("str_sdk_QzUpdateSdk"), SdkLangManager.Get("str_sdk_yes"), delegate
				{
					MenuBars.Upload(false);
				}, null, null, null, null, false);
				return true;
			}
			if (code == -50006)
			{
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), message, "OK", null);
				ParaLog.Log(message);
				return false;
			}
		}
		else
		{
			if (code == -50005)
			{
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), message, "OK", null);
				ParaLog.Log(message);
				return false;
			}
			switch (code)
			{
			case -9:
			case -8:
			case -5:
				manager.ClearWindows();
				manager.GetAccount().Logout();
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), message, "OK", null);
				MenuBars.Login(null);
				return true;
			case -4:
			case -3:
				manager.ClearWindows();
				ParaLog.LogError("Login expired. Please log in again.");
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), "Login expired. Please log in again.", "OK", null);
				MenuBars.Login(null);
				return true;
			case -2:
				manager.ClearWindows();
				ParaLog.LogError(message);
				EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), SdkLangManager.Get("str_sdk_errorCode_text2"), "OK", null);
				MenuBars.Login(null);
				return true;
			}
		}
		EditorUtil.OpenInfo(SdkLangManager.Get("str_sdk_alert"), message, "OK", null);
		return true;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001B5CC File Offset: 0x000197CC
	public string ParseErrorCode(int code)
	{
		if (code <= -31201)
		{
			if (code == -31210)
			{
				return SdkLangManager.Get("str_sdk_errorCode_text14");
			}
			if (code == -31209)
			{
				return SdkLangManager.Get("str_sdk_errorCode_text13");
			}
			switch (code)
			{
			case -31203:
				return SdkLangManager.Get("str_sdk_errorCode_text12");
			case -31202:
				return SdkLangManager.Get("str_sdk_errorCode_text11");
			case -31201:
				return SdkLangManager.Get("str_sdk_errorCode_text10");
			}
		}
		else
		{
			if (code == -5000)
			{
				return SdkLangManager.Get("str_sdk_errorCode_text9");
			}
			if (code == -1001)
			{
				return SdkLangManager.Get("str_sdk_errorCode_text8");
			}
			if (code == 0)
			{
				return null;
			}
		}
		return SdkLangManager.Get("str_sdk_errorCode_text15");
	}

	// Token: 0x0400023C RID: 572
	private Dictionary<int, string> _errorCodeDic = new Dictionary<int, string>();
}
