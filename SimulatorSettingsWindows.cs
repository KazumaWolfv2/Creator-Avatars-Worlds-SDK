using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class SimulatorSettingsWindows : EditorWindow
{
	// Token: 0x060002E8 RID: 744 RVA: 0x00012864 File Offset: 0x00010A64
	private void OnEnable()
	{
		this._firstShow = true;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0001286D File Offset: 0x00010A6D
	private void OnDisable()
	{
		this._firstShow = false;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00012878 File Offset: 0x00010A78
	private void OnGUI()
	{
		Rect rect = EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		float num = rect.width - 1f;
		rect.width = num;
		EditorGUI.BeginChangeCheck();
		GUILayout.BeginArea(new Rect(0f, 4f, SimulatorSettingsWindows.Styles.WindowSizeX / 2f, (float)SimulatorSettingsWindows.Styles.TagHeight), GUI.skin.window);
		EditorGUI.BeginDisabledGroup(!this._enableTabChange);
		bool flag = this.CheckValidate();
		if (this._firstShow)
		{
			this._selectedTab = (flag ? SimulatorSettingsWindows.Tabs.Debug : SimulatorSettingsWindows.Tabs.Install);
			this._firstShow = false;
		}
		for (int i = 0; i < this._tabs.Length; i++)
		{
			int num2 = Mathf.RoundToInt((float)i * rect.width / 4f);
			int num3 = Mathf.RoundToInt((float)(i + 1) * rect.width / 4f);
			Rect rect2 = new Rect(rect.x + (float)num2, rect.y, (float)(num3 - num2), (float)SimulatorSettingsWindows.Styles.TagHeight);
			GUI.enabled = i == 0 || flag;
			if (GUI.Toggle(rect2, this._selectedTab == (SimulatorSettingsWindows.Tabs)i, new GUIContent(this._tabs[i]), SimulatorSettingsWindows.Styles.TagToggleStyle))
			{
				this._selectedTab = (SimulatorSettingsWindows.Tabs)i;
			}
			GUI.enabled = true;
		}
		EditorGUI.EndDisabledGroup();
		GUILayout.EndArea();
		EditorGUI.EndChangeCheck();
		GUILayout.BeginArea(new Rect(0f, (float)SimulatorSettingsWindows.Styles.TagHeight, SimulatorSettingsWindows.Styles.WindowSizeX, SimulatorSettingsWindows.Styles.WindowSizeY - (float)SimulatorSettingsWindows.Styles.TagHeight), SimulatorSettingsWindows.Styles.ContentStyle);
		SimulatorSettingsWindows.Tabs selectedTab = this._selectedTab;
		if (selectedTab != SimulatorSettingsWindows.Tabs.Install)
		{
			if (selectedTab == SimulatorSettingsWindows.Tabs.Debug)
			{
				this.DrawTabDebug();
			}
		}
		else
		{
			this.DrawTabInstall();
		}
		GUILayout.EndArea();
		if (EditorGUI.LinkButton(new Rect(SimulatorSettingsWindows.Styles.WindowSizeX - 320f, SimulatorSettingsWindows.Styles.WindowSizeY - 36f, 312f, 18f), "Learn About ParaSpace SDK Debugger Configuration"))
		{
			Application.OpenURL("https://docs.paraspace.tech/docs/how-to-debug");
		}
		EditorGUILayout.EndVertical();
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00012A54 File Offset: 0x00010C54
	private void DrawTabInstall()
	{
		EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label("ParaSpace SDK Debugger", SimulatorSettingsWindows.Styles.InspectorHeaderStyle, Array.Empty<GUILayoutOption>());
		GUILayout.Space((float)SimulatorSettingsWindows.Styles.HeadTextSize);
		GUILayout.Label("The debugger has not been downloaded: please click the button to download first", SimulatorSettingsWindows.Styles.InspectorContentStyle, Array.Empty<GUILayoutOption>());
		GUILayout.Space((float)SimulatorSettingsWindows.Styles.ContentTextSize);
		if (GUILayout.Button("Open Download Page", new GUILayoutOption[]
		{
			GUILayout.Height((float)SimulatorSettingsWindows.Styles.HeadTextSize),
			GUILayout.Width(200f)
		}))
		{
			Application.OpenURL("https://docs.paraspace.tech/docs/how-to-debug");
		}
		GUILayout.Space((float)SimulatorSettingsWindows.Styles.HeadTextSize);
		GUILayout.Label("Downloaded Debugger: Please add SDK Debugger folder path under Debugger", SimulatorSettingsWindows.Styles.InspectorContentStyle, Array.Empty<GUILayoutOption>());
		GUILayout.Space((float)SimulatorSettingsWindows.Styles.ContentTextSize);
		EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		string @string = EditorPrefs.GetString("ParaSpaceSimulator");
		GUILayout.TextField(@string, new GUILayoutOption[]
		{
			GUILayout.Height((float)SimulatorSettingsWindows.Styles.HeadTextSize),
			GUILayout.Width(SimulatorSettingsWindows.Styles.WindowSizeX - 150f)
		});
		if (GUILayout.Button("Browse", new GUILayoutOption[]
		{
			GUILayout.Height((float)SimulatorSettingsWindows.Styles.HeadTextSize),
			GUILayout.Width(120f)
		}))
		{
			string text = "";
			if (Application.platform == null)
			{
				text = EditorUtility.OpenFilePanel("Select ParaSpace Simulaotr", @string, "");
				if (text.EndsWith(".app"))
				{
					text += "/Contents/MacOS/ParaSpace";
				}
				EditorPrefs.SetString("ParaSpaceSimulator", text);
			}
			else if (Application.platform == 7)
			{
				text = EditorUtility.OpenFilePanel("Select ParaSpace Simulaotr", @string, "exe");
			}
			EditorPrefs.SetString("ParaSpaceSimulator", text);
			if (Application.platform == null)
			{
				try
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo
					{
						FileName = "/bin/bash",
						Arguments = "-c \" chmod +x  " + text + "\" ",
						CreateNoWindow = true
					};
					new Process
					{
						StartInfo = processStartInfo
					}.Start();
				}
				catch (Exception ex)
				{
					ParaLog.LogError(string.Format("SimulatorSettingsWindows.DrawTabInstall Exception{0}", ex));
				}
			}
			GUIUtility.ExitGUI();
		}
		EditorGUILayout.EndHorizontal();
		if (!this.CheckValidate())
		{
			GUILayout.Space((float)SimulatorSettingsWindows.Styles.ContentTextSize);
			EditorGUILayout.HelpBox("This path is not a valid path for the debugger. If it has not been downloaded, please download it first. After downloading, please add the debugger startup file to the path.", 1);
		}
		EditorGUILayout.EndVertical();
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00012C80 File Offset: 0x00010E80
	private bool CheckValidate()
	{
		string @string = EditorPrefs.GetString("ParaSpaceSimulator");
		if (!File.Exists(@string))
		{
			return false;
		}
		if (Application.platform == 7)
		{
			if (!@string.EndsWith("ParaSpace.exe"))
			{
				return false;
			}
		}
		else if (Application.platform == null && !@string.EndsWith("ParaSpace"))
		{
			return false;
		}
		return true;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00003394 File Offset: 0x00001594
	protected virtual void DrawTabDebug()
	{
	}

	// Token: 0x04000186 RID: 390
	private bool _enableTabChange = true;

	// Token: 0x04000187 RID: 391
	private string[] _tabs = (from s in Enum.GetNames(typeof(SimulatorSettingsWindows.Tabs))
		select s.Replace('_', ' ')).ToArray<string>();

	// Token: 0x04000188 RID: 392
	private SimulatorSettingsWindows.Tabs _selectedTab;

	// Token: 0x04000189 RID: 393
	private bool _firstShow;

	// Token: 0x020000B3 RID: 179
	private enum Tabs
	{
		// Token: 0x040003A4 RID: 932
		Install,
		// Token: 0x040003A5 RID: 933
		Debug
	}

	// Token: 0x020000B4 RID: 180
	public static class Styles
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00024C54 File Offset: 0x00022E54
		public static GUIStyle TagToggleStyle
		{
			get
			{
				GUIStyle guistyle;
				if ((guistyle = SimulatorSettingsWindows.Styles._tapToggleStyle) == null)
				{
					(guistyle = new GUIStyle(EditorStyles.toolbarButton)).fixedHeight = (float)SimulatorSettingsWindows.Styles.TagHeight;
				}
				return guistyle;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00024C78 File Offset: 0x00022E78
		public static GUIStyle ContentStyle
		{
			get
			{
				GUIStyle guistyle;
				if ((guistyle = SimulatorSettingsWindows.Styles._contentStyle) == null)
				{
					GUIStyle guistyle2 = new GUIStyle(GUI.skin.window);
					guistyle2.contentOffset = new Vector2(10f, 0f);
					guistyle = guistyle2;
					guistyle2.padding = new RectOffset(GUI.skin.window.padding.left + 10, GUI.skin.window.padding.right + 10, GUI.skin.window.padding.top, GUI.skin.window.padding.bottom);
				}
				return guistyle;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00024D16 File Offset: 0x00022F16
		public static GUIStyle InspectorHeaderStyle
		{
			get
			{
				GUIStyle guistyle;
				if ((guistyle = SimulatorSettingsWindows.Styles._inspectorHeaderStyle) == null)
				{
					GUIStyle guistyle2 = new GUIStyle(EditorStyles.boldLabel);
					guistyle2.fontSize = SimulatorSettingsWindows.Styles.HeadTextSize;
					guistyle = guistyle2;
					SimulatorSettingsWindows.Styles._inspectorHeaderStyle = guistyle2;
				}
				return guistyle;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00024D3C File Offset: 0x00022F3C
		public static GUIStyle InspectorContentStyle
		{
			get
			{
				GUIStyle guistyle;
				if ((guistyle = SimulatorSettingsWindows.Styles._inspectorContentStyle) == null)
				{
					GUIStyle guistyle2 = new GUIStyle(EditorStyles.wordWrappedLabel);
					guistyle2.fontSize = SimulatorSettingsWindows.Styles.ContentTextSize;
					guistyle = guistyle2;
					SimulatorSettingsWindows.Styles._inspectorContentStyle = guistyle2;
				}
				return guistyle;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00024D62 File Offset: 0x00022F62
		public static GUIStyle InspectorTipsStyle
		{
			get
			{
				GUIStyle guistyle;
				if ((guistyle = SimulatorSettingsWindows.Styles._inspectorTipsStyle) == null)
				{
					GUIStyle guistyle2 = new GUIStyle(EditorStyles.label);
					guistyle2.fontSize = SimulatorSettingsWindows.Styles.TipTextSize;
					guistyle = guistyle2;
					SimulatorSettingsWindows.Styles._inspectorTipsStyle = guistyle2;
				}
				return guistyle;
			}
		}

		// Token: 0x040003A6 RID: 934
		public static float WindowSizeX = 449f;

		// Token: 0x040003A7 RID: 935
		public static float WindowSizeY = 620f;

		// Token: 0x040003A8 RID: 936
		public static int TagHeight = 32;

		// Token: 0x040003A9 RID: 937
		public static int HeadTextSize = 24;

		// Token: 0x040003AA RID: 938
		public static int ContentTextSize = 14;

		// Token: 0x040003AB RID: 939
		public static int TipTextSize = 12;

		// Token: 0x040003AC RID: 940
		private static GUIStyle _tapToggleStyle;

		// Token: 0x040003AD RID: 941
		private static GUIStyle _contentStyle;

		// Token: 0x040003AE RID: 942
		private static GUIStyle _inspectorHeaderStyle;

		// Token: 0x040003AF RID: 943
		private static GUIStyle _inspectorContentStyle;

		// Token: 0x040003B0 RID: 944
		private static GUIStyle _inspectorTipsStyle;
	}
}
