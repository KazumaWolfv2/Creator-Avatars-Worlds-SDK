using System;
using System.Reflection;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x02000075 RID: 117
[InitializeOnLoad]
public static class MenuBars
{
	// Token: 0x060003F9 RID: 1017 RVA: 0x0001B6A4 File Offset: 0x000198A4
	static MenuBars()
	{
		MenuBars.Init();
		EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(MenuBars.OnUpdate));
		EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(MenuBars.OnUpdate));
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0001B720 File Offset: 0x00019920
	public static void Init()
	{
		Debug.Log("MenuBars  init.");
		MenuBars.IsUpdate = false;
		SdkManager.Instance.Init();
		SdkLangManager.Init("en");
		ParaLog.Init(7);
		if (Time.realtimeSinceStartup > 30f)
		{
			return;
		}
		if (EditorApplication.isPlayingOrWillChangePlaymode)
		{
			return;
		}
		EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.delayCall, new EditorApplication.CallbackFunction(delegate
		{
			if (MenuBars.IsCheckUpdate)
			{
				return;
			}
			MenuBars.IsCheckUpdate = true;
			MenuBars.Upload(true);
		}));
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0001B79F File Offset: 0x0001999F
	public static void SetIsAvatarSdk(Action avatarAction)
	{
		MenuBars.AvatarFunc = avatarAction;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001B7A7 File Offset: 0x000199A7
	public static void SetIsWorldSdk(Action worldAction)
	{
		MenuBars.WorldFunc = worldAction;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0001B7AF File Offset: 0x000199AF
	public static void SetAvatarSimulator(Action simulator)
	{
		MenuBars.AvatarSimulator = simulator;
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0001B7B7 File Offset: 0x000199B7
	private static bool IsAvatarSimulator()
	{
		return MenuBars.AvatarSimulator != null;
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0001B7C1 File Offset: 0x000199C1
	public static void SetWorldSimulator(Action simulator)
	{
		MenuBars.WorldSimulator = simulator;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0001B7C9 File Offset: 0x000199C9
	private static bool IsAvatarSdk()
	{
		return MenuBars.AvatarFunc != null;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001B7D3 File Offset: 0x000199D3
	private static bool IsWorldSdk()
	{
		return MenuBars.WorldFunc != null;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001B7DD File Offset: 0x000199DD
	private static bool IsWorldSimulator()
	{
		return MenuBars.WorldSimulator != null;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0001B7E7 File Offset: 0x000199E7
	private static bool IsCsToDllSdk()
	{
		return SdkManager.Instance.GetConfig().IsShowCsToDll();
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0001B7F8 File Offset: 0x000199F8
	private static void CsToDllSdk()
	{
		SdkManager.Instance.GetConfig().CsToDllSdk();
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001B809 File Offset: 0x00019A09
	private static bool IsShowReLoadConfig()
	{
		return SdkManager.Instance.GetConfig().IsShowReLoadConfig();
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001B81A File Offset: 0x00019A1A
	private static void ReLoadConfig()
	{
		SdkManager.Instance.GetConfig().ReLoadConfig();
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001B82B File Offset: 0x00019A2B
	private static void DeleteCsCode()
	{
		SdkManager.Instance.GetConfig().DeleteCsCode();
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001B83C File Offset: 0x00019A3C
	private static void OnUpdate()
	{
		SdkManager.Instance.Update();
		MenuBars.SetupBar();
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001B850 File Offset: 0x00019A50
	private static void SetupBar()
	{
		if (MenuBars.CurrentToolbar == null)
		{
			Object[] array = Resources.FindObjectsOfTypeAll(MenuBars.KToolbarType);
			MenuBars.CurrentToolbar = ((array.Length != 0) ? ((ScriptableObject)array[0]) : null);
			if (MenuBars.CurrentToolbar != null)
			{
				VisualElement visualElement = MenuBars.CurrentToolbar.GetType().GetField("m_Root", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(MenuBars.CurrentToolbar) as VisualElement;
				VisualElement visualElement2 = UQueryExtensions.Q(visualElement, "ToolbarZoneLeftAlign", null);
				VisualElement visualElement3 = new VisualElement
				{
					style = 
					{
						flexGrow = 1f,
						flexDirection = 3
					}
				};
				IMGUIContainer imguicontainer = new IMGUIContainer();
				IMGUIContainer imguicontainer2 = imguicontainer;
				imguicontainer2.onGUIHandler = (Action)Delegate.Combine(imguicontainer2.onGUIHandler, new Action(MenuBars.OnGuiLeftBody));
				visualElement3.Add(imguicontainer);
				visualElement2.Add(visualElement3);
				VisualElement visualElement4 = UQueryExtensions.Q(visualElement, "ToolbarZoneRightAlign", null);
				VisualElement visualElement5 = new VisualElement
				{
					style = 
					{
						flexGrow = 1f,
						flexDirection = 2
					}
				};
				IMGUIContainer imguicontainer3 = new IMGUIContainer();
				IMGUIContainer imguicontainer4 = imguicontainer3;
				imguicontainer4.onGUIHandler = (Action)Delegate.Combine(imguicontainer4.onGUIHandler, new Action(MenuBars.OnGuiBody));
				visualElement5.Add(imguicontainer3);
				visualElement4.Add(visualElement5);
			}
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0001B9A0 File Offset: 0x00019BA0
	private static void OnGuiLeftBody()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (MenuBars.IsAvatarSdk())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text4"), Array.Empty<GUILayoutOption>()))
			{
				JsonData jsonData = new JsonData();
				jsonData["event"] = "unity_editor_button_click";
				jsonData["function_type"] = 1;
				jsonData["function_name"] = "editor_interface";
				jsonData["button_type"] = 1;
				jsonData["button_id"] = 401;
				jsonData["tab_name"] = "";
				AppLogService.PushAppLog(jsonData);
				MenuBars.Sign(delegate
				{
					MenuBars.AvatarPublish();
				});
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		if (MenuBars.IsWorldSdk())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text5"), Array.Empty<GUILayoutOption>()))
			{
				JsonData jsonData2 = new JsonData();
				jsonData2["event"] = "unity_editor_button_click";
				jsonData2["function_type"] = 1;
				jsonData2["function_name"] = "editor_interface";
				jsonData2["button_type"] = 1;
				jsonData2["button_id"] = 402;
				jsonData2["tab_name"] = "";
				AppLogService.PushAppLog(jsonData2);
				MenuBars.Sign(delegate
				{
					MenuBars.WorldPublish();
				});
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		if (MenuBars.IsCsToDllSdk())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text6"), Array.Empty<GUILayoutOption>()))
			{
				MenuBars.CsToDllSdk();
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		if (MenuBars.IsShowReLoadConfig())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text7"), Array.Empty<GUILayoutOption>()))
			{
				MenuBars.ReLoadConfig();
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text8"), Array.Empty<GUILayoutOption>()))
			{
				MenuBars.DeleteCsCode();
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0001BBF8 File Offset: 0x00019DF8
	private static void OnGuiBody()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		if (MenuBars.IsWorldSdk() || MenuBars.IsAvatarSdk())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text9"), Array.Empty<GUILayoutOption>()))
			{
				JsonData jsonData = new JsonData();
				jsonData["event"] = "unity_editor_button_click";
				jsonData["function_type"] = 1;
				jsonData["function_name"] = "editor_interface";
				jsonData["button_type"] = 1;
				jsonData["button_id"] = 403;
				jsonData["tab_name"] = "";
				AppLogService.PushAppLog(jsonData);
				MenuBars.Sign(delegate
				{
					MenuBars.ContentManager();
				});
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		if (MenuBars.IsAvatarSdk() && MenuBars.IsAvatarSimulator())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text10"), Array.Empty<GUILayoutOption>()))
			{
				JsonData jsonData2 = new JsonData();
				jsonData2["event"] = "unity_editor_button_click";
				jsonData2["function_type"] = 1;
				jsonData2["function_name"] = "editor_interface";
				jsonData2["button_type"] = 1;
				jsonData2["button_id"] = 407;
				jsonData2["tab_name"] = "";
				AppLogService.PushAppLog(jsonData2);
				MenuBars.Sign(delegate
				{
					MenuBars.AvatarDebugger();
				});
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		if (MenuBars.IsWorldSdk() && MenuBars.IsWorldSimulator())
		{
			if (GUILayout.Button(SdkLangManager.Get("str_sdk_menuBars_text11"), Array.Empty<GUILayoutOption>()))
			{
				JsonData jsonData3 = new JsonData();
				jsonData3["event"] = "unity_editor_button_click";
				jsonData3["function_type"] = 1;
				jsonData3["function_name"] = "editor_interface";
				jsonData3["button_type"] = 1;
				jsonData3["button_id"] = 406;
				jsonData3["tab_name"] = "";
				AppLogService.PushAppLog(jsonData3);
				MenuBars.Sign(delegate
				{
					MenuBars.WorldDebugger();
				});
				GUIUtility.ExitGUI();
			}
			GUILayout.Space(10f);
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
	public static void Sign(Action action)
	{
		if (SdkManager.Instance.GetAccount().IsLogin())
		{
			if (!SdkManager.Instance.GetAccount().IsUserInfoInit())
			{
				CommonNetProxy.FetchAccountInfo(action);
				return;
			}
			if (action != null)
			{
				action();
				return;
			}
		}
		else
		{
			MenuBars.Login(action);
		}
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0001BEF0 File Offset: 0x0001A0F0
	public static void Login(Action action)
	{
		DateTime now = DateTime.Now;
		SdkManager.Instance.GetAccount().Logout();
		SdkAccountWindow window = EditorWindow.GetWindow<SdkAccountWindow>();
		window.SetAction(action);
		window.Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0001BF8F File Offset: 0x0001A18F
	private static void AvatarPublish()
	{
		MenuBars.AvatarFunc();
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0001BF9B File Offset: 0x0001A19B
	private static void AvatarDebugger()
	{
		Action avatarSimulator = MenuBars.AvatarSimulator;
		if (avatarSimulator == null)
		{
			return;
		}
		avatarSimulator();
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0001BFAC File Offset: 0x0001A1AC
	private static void WorldPublish()
	{
		MenuBars.WorldFunc();
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001BFB8 File Offset: 0x0001A1B8
	private static void WorldDebugger()
	{
		Action worldSimulator = MenuBars.WorldSimulator;
		if (worldSimulator == null)
		{
			return;
		}
		worldSimulator();
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001BFC9 File Offset: 0x0001A1C9
	public static void SetSdkUpdateAction(Action<bool> action)
	{
		MenuBars.SdkUpdateAction = action;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0001BFD4 File Offset: 0x0001A1D4
	public static void CheckUpdateSdk(bool isAvatar, string ver, Action<JsonData> action)
	{
		CommonNetProxy.UpdateSdk(isAvatar ? "avatar" : "world", ver, delegate(int code, string message, JsonData result)
		{
			if (code != 0)
			{
				EditorUtility.DisplayDialog(SdkLangManager.Get("str_sdk_tips"), message, SdkLangManager.Get("str_sdk_confirm"));
				return;
			}
			if (result["version"].ToString() == ver)
			{
				action(null);
				return;
			}
			action(result);
		});
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001C01B File Offset: 0x0001A21B
	public static void Upload(bool isAuto = false)
	{
		if (MenuBars.SdkUpdateAction != null)
		{
			MenuBars.SdkUpdateAction(isAuto);
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001C030 File Offset: 0x0001A230
	public static void ContentManager()
	{
		DateTime now = DateTime.Now;
		EditorWindow.GetWindow<ContentManagerWindow>().Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0001C0BC File Offset: 0x0001A2BC
	public static void SdkAccountWindow()
	{
		DateTime now = DateTime.Now;
		EditorWindow.GetWindow<SdkAccountWindow>().Show();
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - now.Ticks);
		JsonData jsonData = new JsonData();
		jsonData["event"] = "unity_editor_page_show";
		jsonData["loading_time"] = (int)(timeSpan.Ticks / 10000L);
		jsonData["is_sdk_init"] = 0;
		AppLogService.PushAppLog(jsonData);
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001C145 File Offset: 0x0001A345
	public static void RefreshAssetsGuid()
	{
		RefreshGuid.Refresh();
		RevertDLLMeta.Refresh(false);
	}

	// Token: 0x0400023D RID: 573
	private static readonly Type KToolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

	// Token: 0x0400023E RID: 574
	private static ScriptableObject CurrentToolbar;

	// Token: 0x0400023F RID: 575
	private static Action AvatarFunc;

	// Token: 0x04000240 RID: 576
	private static Action AvatarSimulator;

	// Token: 0x04000241 RID: 577
	private static Action WorldFunc;

	// Token: 0x04000242 RID: 578
	private static Action WorldSimulator;

	// Token: 0x04000243 RID: 579
	private static bool IsUpdate = false;

	// Token: 0x04000244 RID: 580
	private static Action<bool> SdkUpdateAction;

	// Token: 0x04000245 RID: 581
	private static bool IsCheckUpdate = false;
}
