using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200000C RID: 12
[InitializeOnLoad]
[ExecuteInEditMode]
public static class ParaEditorService
{
	// Token: 0x0600004B RID: 75 RVA: 0x00004264 File Offset: 0x00002464
	public static void StartService()
	{
		if (ParaEditorService.ServiceStarted)
		{
			return;
		}
		ParaEditorService.AddService();
		ParaEditorService._reqHandler = new List<Action>();
		EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(ParaEditorService.Update));
		if (Directory.Exists(ParaEditorService.SimulatorDir))
		{
			Directory.Delete(ParaEditorService.SimulatorDir, true);
		}
		if (!Directory.Exists(ParaEditorService.SimulatorDir))
		{
			Directory.CreateDirectory(ParaEditorService.SimulatorDir);
		}
		for (;;)
		{
			try
			{
				ParaEditorService._serviceListener = new HttpListener();
				ParaEditorService._serviceListener.Prefixes.Add("http://127.0.0.1:" + ParaEditorService.Port.ToString() + "/");
				ParaEditorService._serviceListener.Start();
				ParaEditorService.Receive();
				ParaLog.Log(string.Format("begin simulator debugger service port:{0}", ParaEditorService.Port));
				ParaEditorService.ServiceStarted = true;
				string text = string.Format("{0}", ParaEditorService.Port);
				File.WriteAllText(Path.Combine(ParaEditorService.SimulatorDir, "ServiceInfo"), text);
			}
			catch (SocketException)
			{
				ParaEditorService.Port++;
				continue;
			}
			catch (HttpListenerException)
			{
				ParaEditorService.Port++;
				continue;
			}
			catch (Exception ex)
			{
				ParaEditorService._serviceListener = null;
				ParaLog.Log(ex);
			}
			break;
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000043BC File Offset: 0x000025BC
	public static void AddProcess(Process p)
	{
		if (ParaEditorService._lstProcess == null)
		{
			ParaEditorService._lstProcess = new List<Process>();
		}
		ParaEditorService._lstProcess.Add(p);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000043DA File Offset: 0x000025DA
	private static void AddService()
	{
		ParaEditorService._handlers = new Dictionary<string, Action<HttpListenerContext, HttpListenerRequest>>();
		ParaEditorService._handlers.Add("/restartSimulator", new Action<HttpListenerContext, HttpListenerRequest>(ParaEditorService.OnRestartSimulator));
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004401 File Offset: 0x00002601
	private static void Receive()
	{
		ParaEditorService._serviceListener.BeginGetContext(new AsyncCallback(ParaEditorService.ListenerCallback), ParaEditorService._serviceListener);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00004420 File Offset: 0x00002620
	private static void Update()
	{
		Action action = null;
		List<Action> reqHandler = ParaEditorService._reqHandler;
		lock (reqHandler)
		{
			if (ParaEditorService._reqHandler.Count > 0)
			{
				action = ParaEditorService._reqHandler[0];
				ParaEditorService._reqHandler.RemoveAt(0);
			}
		}
		if (action != null)
		{
			action();
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004488 File Offset: 0x00002688
	private static void ListenerCallback(IAsyncResult result)
	{
		if (ParaEditorService._serviceListener.IsListening)
		{
			HttpListenerContext httpListenerContext = ParaEditorService._serviceListener.EndGetContext(result);
			HttpListenerRequest request = httpListenerContext.Request;
			Action<HttpListenerContext, HttpListenerRequest> action;
			if (ParaEditorService._handlers.TryGetValue(request.Url.LocalPath, out action))
			{
				try
				{
					action(httpListenerContext, request);
				}
				catch (Exception ex)
				{
					ParaLog.Log(ex);
				}
			}
			ParaEditorService.Receive();
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000044F4 File Offset: 0x000026F4
	private static void GetWorldAssetBundleName(out string assetBundle, out string assetName)
	{
		Scene activeScene = SceneManager.GetActiveScene();
		string text = ABPackageTools.ParseBundleName(activeScene.path) + ABPackageSetting.abSuffixName;
		assetBundle = Path.Combine(Application.dataPath, "DebugOutput", text);
		assetName = activeScene.path;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000453C File Offset: 0x0000273C
	private static void OnRestartSimulator(HttpListenerContext context, HttpListenerRequest request)
	{
		ParaLog.Log("$On Restart Simulator");
		HttpListenerResponse response = context.Response;
		response.StatusCode = 200;
		response.ContentType = "text/plain";
		response.OutputStream.Write(Encoding.UTF8.GetBytes("Succeed!"));
		response.OutputStream.Close();
		List<Action> reqHandler = ParaEditorService._reqHandler;
		lock (reqHandler)
		{
			ParaEditorService._reqHandler.Add(new Action(ParaEditorService.<OnRestartSimulator>g__DoJob|15_0));
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000045DC File Offset: 0x000027DC
	[CompilerGenerated]
	internal static void <OnRestartSimulator>g__DoJob|15_0()
	{
		if (ParaEditorService._lstProcess != null)
		{
			foreach (Process process in ParaEditorService._lstProcess)
			{
				try
				{
					ParaLog.Log(string.Format("$ restart simulator close pre process pid:{0}", process.Id));
					process.Kill();
				}
				catch (Exception)
				{
				}
			}
			ParaEditorService._lstProcess = null;
		}
		string text;
		string text2;
		ParaEditorService.GetWorldAssetBundleName(out text, out text2);
		string dataPath = Application.dataPath;
		int @int = EditorPrefs.GetInt("ParaSpacePlayerNum");
		string @string = EditorPrefs.GetString("ParaSpaceSimulator");
		string text3 = string.Concat(new string[] { "-debugType world -worldAssetBundle \"", text, "\" -worldAssetName \"", text2, "\" -workDirection \"", dataPath, "\"" });
		if (Application.platform == null)
		{
			text3 = string.Concat(new string[]
			{
				"-debugType world -worldAssetBundle file://\"",
				text,
				"\" -worldAssetName \"",
				text2,
				"\" -workDirection \"",
				dataPath.Replace("/Assets", ""),
				"\""
			});
		}
		ParaEditorService._lstProcess = new List<Process>();
		for (int i = 0; i < @int; i++)
		{
			string openId = SdkManager.Instance.GetAccount().GetOpenId();
			string string2 = EditorPrefs.GetString(string.Format("ParaSpacePlayer{0}", i));
			string text4 = EditorPrefs.GetString("ParaSpaceRoomID");
			if (string.IsNullOrEmpty(text4))
			{
				text4 = GUID.Generate().ToString();
			}
			string text5 = string.Format(" -openId {0} -nickName {1} -mirrorId {2} -cache {3}", new object[] { openId, string2, text4, i });
			ProcessStartInfo processStartInfo = new ProcessStartInfo(@string)
			{
				Arguments = text3 + text5
			};
			ParaLog.Log("WorldSimulator " + @string + " " + processStartInfo.Arguments);
			ParaEditorService._lstProcess.Add(Process.Start(processStartInfo));
		}
	}

	// Token: 0x04000023 RID: 35
	public static int Port = 65331;

	// Token: 0x04000024 RID: 36
	public static bool ServiceStarted = false;

	// Token: 0x04000025 RID: 37
	private static HttpListener _serviceListener;

	// Token: 0x04000026 RID: 38
	private static Dictionary<string, Action<HttpListenerContext, HttpListenerRequest>> _handlers;

	// Token: 0x04000027 RID: 39
	private static List<Action> _reqHandler;

	// Token: 0x04000028 RID: 40
	private static string SimulatorDir = "Library/ParaSimulator";

	// Token: 0x04000029 RID: 41
	private static List<Process> _lstProcess;
}
