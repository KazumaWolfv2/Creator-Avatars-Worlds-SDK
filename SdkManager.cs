using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class SdkManager
{
	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x0001D4D3 File Offset: 0x0001B6D3
	public static SdkManager Instance
	{
		get
		{
			if (SdkManager._Instance == null)
			{
				SdkManager._Instance = new SdkManager();
				SdkManager._Instance.Init();
			}
			return SdkManager._Instance;
		}
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001D4F5 File Offset: 0x0001B6F5
	public void SetAigcCloud()
	{
		this._isAigcCloud = true;
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0001D4FE File Offset: 0x0001B6FE
	public bool IsAigcCloud()
	{
		return this._isAigcCloud;
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0001D508 File Offset: 0x0001B708
	public void Init()
	{
		this.SdkAccount = new SdkAccount();
		this.SdkAccount.Init();
		this.SdkConfig = new SdkConfig();
		this.SdkConfig.Init();
		this._Windows.Clear();
		this.SdkTextureDownload = new SdkTextureDownload();
		this.SdkTextureDownload.Init();
		this.ErrorCodeManager = new ErrorCodeManager();
		this.ErrorCodeManager.Init();
		this._uploadTask = default(SdkManager.Uploadtask);
		this._uploadTask.Init();
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001D58F File Offset: 0x0001B78F
	public SdkAccount GetAccount()
	{
		return this.SdkAccount;
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001D597 File Offset: 0x0001B797
	public SdkConfig GetConfig()
	{
		return this.SdkConfig;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001D59F File Offset: 0x0001B79F
	public void SetUpgradeFunc(Action func)
	{
		SdkManager._upgradeSdkFunc = func;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001D5A7 File Offset: 0x0001B7A7
	public bool CommonHandle(int code, string message)
	{
		return this.ErrorCodeManager.CommonHandle(code, message, this);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001D5B7 File Offset: 0x0001B7B7
	public string LoginHandle(int code)
	{
		return this.ErrorCodeManager.ParseErrorCode(code);
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001D5C5 File Offset: 0x0001B7C5
	public string GetErrorCode(int code, string message)
	{
		return this.ErrorCodeManager.GetErrorMessage(code, message);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0001D5D4 File Offset: 0x0001B7D4
	public void RecordJob(string jobId, bool isAvatar = true)
	{
		this._uploadTask.Reset();
		this._uploadTask.JobId = jobId;
		this._uploadTask.IsAvatar = isAvatar;
		this._uploadTask.Time = DateTime.Now.Ticks;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0001D61C File Offset: 0x0001B81C
	public void ClearRecordJob()
	{
		this._uploadTask.Reset();
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0001D629 File Offset: 0x0001B829
	public bool IsSameJob(string jobId)
	{
		return this._uploadTask.IsSameJob(jobId);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x0001D638 File Offset: 0x0001B838
	public void OpenWindow<T>(Action<T> action) where T : EditorWindow
	{
		if (SdkManager._upgradeSdkFunc != null)
		{
			SdkManager._upgradeSdkFunc();
		}
		if (!this.IsOpenedWindow(typeof(T)))
		{
			T window = EditorWindow.GetWindow<T>();
			if (action != null)
			{
				action(window);
			}
			this._Windows.Add(typeof(T), window);
			window.Show();
		}
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0001D69E File Offset: 0x0001B89E
	public void CloseWindow<T>() where T : EditorWindow
	{
		if (this.IsOpenedWindow(typeof(T)))
		{
			this._Windows.Remove(typeof(T));
		}
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001D6C8 File Offset: 0x0001B8C8
	public void ClearWindows()
	{
		foreach (EditorWindow editorWindow in Resources.FindObjectsOfTypeAll<EditorWindow>())
		{
			if (!editorWindow.GetType().ToString().Contains("UnityEditor"))
			{
				editorWindow.Close();
			}
		}
		this._Windows.Clear();
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001D715 File Offset: 0x0001B915
	public bool IsOpenedWindow(Type type)
	{
		return this._Windows.ContainsKey(type);
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001D723 File Offset: 0x0001B923
	public SdkTextureDownload GetTextureDownload()
	{
		return this.SdkTextureDownload;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001D72B File Offset: 0x0001B92B
	public void Update()
	{
		if (this.SdkTextureDownload != null)
		{
			this.SdkTextureDownload.Update();
		}
	}

	// Token: 0x0400027A RID: 634
	private static SdkManager _Instance;

	// Token: 0x0400027B RID: 635
	private SdkAccount SdkAccount;

	// Token: 0x0400027C RID: 636
	private SdkConfig SdkConfig;

	// Token: 0x0400027D RID: 637
	private ErrorCodeManager ErrorCodeManager;

	// Token: 0x0400027E RID: 638
	private static Action _upgradeSdkFunc;

	// Token: 0x0400027F RID: 639
	private bool _isAigcCloud;

	// Token: 0x04000280 RID: 640
	private SdkManager.Uploadtask _uploadTask;

	// Token: 0x04000281 RID: 641
	private Dictionary<Type, EditorWindow> _Windows = new Dictionary<Type, EditorWindow>();

	// Token: 0x04000282 RID: 642
	private SdkTextureDownload SdkTextureDownload;

	// Token: 0x020000D6 RID: 214
	public struct Uploadtask
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x000263EE File Offset: 0x000245EE
		public void Init()
		{
			this.Reset();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000263F6 File Offset: 0x000245F6
		public void Reset()
		{
			this.JobId = null;
			this.Time = 0L;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00026408 File Offset: 0x00024608
		public void Log(bool isWarning = false)
		{
			if (isWarning)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"This is Update Task. JobId:",
					this.JobId,
					" IsAvatar:",
					this.IsAvatar.ToString(),
					" Time:",
					this.Time.ToString()
				}));
				return;
			}
			Debug.Log(string.Concat(new string[]
			{
				"This is Update Task. JobId:",
				this.JobId,
				" IsAvatar:",
				this.IsAvatar.ToString(),
				" Time:",
				this.Time.ToString()
			}));
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000264B3 File Offset: 0x000246B3
		public bool IsSameJob(string jobId)
		{
			if (!string.Equals(jobId, this.JobId))
			{
				if (!string.IsNullOrEmpty(this.JobId))
				{
					this.Log(true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x040003FD RID: 1021
		public string JobId;

		// Token: 0x040003FE RID: 1022
		public bool IsAvatar;

		// Token: 0x040003FF RID: 1023
		public long Time;
	}
}
