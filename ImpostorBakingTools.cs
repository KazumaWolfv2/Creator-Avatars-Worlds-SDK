using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000068 RID: 104
	public static class ImpostorBakingTools
	{
		// Token: 0x0600021A RID: 538 RVA: 0x0000B1B8 File Offset: 0x000093B8
		public static string OpenFolderForImpostor(this ParaImpostor instance)
		{
			string text = Path.GetFullPath(Application.dataPath + "/../").Replace("\\", "/");
			string text2 = AssetDatabase.GetAssetPath(instance.RootTransform);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource<Transform>(instance.RootTransform));
			}
			ImpostorBakingTools.GlobalRelativeFolder = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalRelativeFolder, "");
			string text3 = string.Empty;
			string text4 = text + text2;
			if (string.IsNullOrEmpty(text2))
			{
				text4 = Application.dataPath;
			}
			else
			{
				text4 = Path.GetDirectoryName(text4).Replace("\\", "/");
			}
			ImpostorBakingTools.GlobalFolder = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalFolder, "");
			if (ImpostorBakingTools.GlobalDefaultMode && AssetDatabase.IsValidFolder(ImpostorBakingTools.GlobalFolder.TrimStart('/')))
			{
				text3 = text + ImpostorBakingTools.GlobalFolder;
			}
			else if (AssetDatabase.IsValidFolder(FileUtil.GetProjectRelativePath(text4 + ImpostorBakingTools.GlobalRelativeFolder).TrimEnd('/')))
			{
				text3 = text4 + ImpostorBakingTools.GlobalRelativeFolder;
			}
			else if (AssetDatabase.IsValidFolder(FileUtil.GetProjectRelativePath(text4).TrimEnd('/')))
			{
				text3 = text4;
			}
			else
			{
				text3 = Application.dataPath;
			}
			string text5 = instance.name + "_Impostor";
			if (!string.IsNullOrEmpty(instance.m_impostorName))
			{
				text5 = instance.m_impostorName;
			}
			string text6 = EditorUtility.SaveFilePanelInProject("Save Impostor to folder", text5, "asset", "", FileUtil.GetProjectRelativePath(text3));
			text5 = Path.GetFileNameWithoutExtension(text6);
			if (!string.IsNullOrEmpty(text5))
			{
				text6 = Path.GetDirectoryName(text6).Replace("\\", "/");
				if (!string.IsNullOrEmpty(text6))
				{
					text6 += "/";
					if (!ImpostorBakingTools.GlobalDefaultMode)
					{
						instance.m_folderPath = text6;
					}
					else
					{
						ImpostorBakingTools.GlobalFolder = text6;
						EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalFolder, ImpostorBakingTools.GlobalFolder);
					}
					instance.m_impostorName = text5;
				}
			}
			return text6;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B393 File Offset: 0x00009593
		[SettingsProvider]
		public static SettingsProvider ImpostorsSettings()
		{
			SettingsProvider settingsProvider = new SettingsProvider("Preferences/Amplify Impostors", 0, null);
			settingsProvider.guiHandler = delegate(string searchContext)
			{
				ImpostorBakingTools.PreferencesGUI();
			};
			return settingsProvider;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B3C8 File Offset: 0x000095C8
		public static void PreferencesGUI()
		{
			if (!ImpostorBakingTools.PrefsLoaded)
			{
				ImpostorBakingTools.LoadDefaults();
				ImpostorBakingTools.PrefsLoaded = true;
			}
			ImpostorBakingTools.PathButtonContent.text = (string.IsNullOrEmpty(ImpostorBakingTools.GlobalFolder) ? "Click to select folder" : ImpostorBakingTools.GlobalFolder);
			ImpostorBakingTools.GlobalDefaultMode = (FolderMode)EditorGUILayout.EnumPopup("New Impostor Default Path", ImpostorBakingTools.GlobalDefaultMode ? FolderMode.Global : FolderMode.RelativeToPrefab, Array.Empty<GUILayoutOption>()) == FolderMode.Global;
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (ImpostorBakingTools.GlobalDefaultMode)
			{
				EditorGUI.BeginChangeCheck();
				ImpostorBakingTools.GlobalFolder = EditorGUILayout.TextField("Global Folder", ImpostorBakingTools.GlobalFolder, Array.Empty<GUILayoutOption>());
				if (EditorGUI.EndChangeCheck())
				{
					ImpostorBakingTools.GlobalFolder = ImpostorBakingTools.GlobalFolder.TrimStart(new char[] { '/', '*', '.', ' ' });
					ImpostorBakingTools.GlobalFolder = "/" + ImpostorBakingTools.GlobalFolder;
					ImpostorBakingTools.GlobalFolder = ImpostorBakingTools.GlobalFolder.TrimEnd(new char[] { '/', '*', '.', ' ' });
					EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalFolder, ImpostorBakingTools.GlobalFolder);
				}
				if (GUILayout.Button("...", "minibutton", new GUILayoutOption[] { GUILayout.Width(20f) }))
				{
					string text = Path.GetFullPath(Application.dataPath + "/../").Replace("\\", "/") + ImpostorBakingTools.GlobalFolder;
					string text2 = EditorUtility.SaveFolderPanel("Save Impostor to folder", FileUtil.GetProjectRelativePath(text), null);
					text2 = FileUtil.GetProjectRelativePath(text2);
					if (!string.IsNullOrEmpty(text2))
					{
						ImpostorBakingTools.GlobalFolder = text2;
						ImpostorBakingTools.GlobalFolder = ImpostorBakingTools.GlobalFolder.TrimStart(new char[] { '/', '*', '.', ' ' });
						ImpostorBakingTools.GlobalFolder = "/" + ImpostorBakingTools.GlobalFolder;
						ImpostorBakingTools.GlobalFolder = ImpostorBakingTools.GlobalFolder.TrimEnd(new char[] { '/', '*', '.', ' ' });
						EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalFolder, ImpostorBakingTools.GlobalFolder);
					}
				}
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				ImpostorBakingTools.GlobalRelativeFolder = EditorGUILayout.TextField("Relative to Prefab Folder", ImpostorBakingTools.GlobalRelativeFolder, Array.Empty<GUILayoutOption>());
				if (EditorGUI.EndChangeCheck())
				{
					ImpostorBakingTools.GlobalRelativeFolder = ImpostorBakingTools.GlobalRelativeFolder.TrimStart(new char[] { '/', '*', '.', ' ' });
					ImpostorBakingTools.GlobalRelativeFolder = "/" + ImpostorBakingTools.GlobalRelativeFolder;
					ImpostorBakingTools.GlobalRelativeFolder = ImpostorBakingTools.GlobalRelativeFolder.TrimEnd(new char[] { '/', '*', '.', ' ' });
					EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalRelativeFolder, ImpostorBakingTools.GlobalRelativeFolder);
				}
				EditorGUI.BeginDisabledGroup(true);
				GUILayout.Button("...", "minibutton", new GUILayoutOption[] { GUILayout.Width(20f) });
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndHorizontal();
			ImpostorBakingTools.GlobalTexImport = EditorGUILayout.Popup("Texture Importer Settings", ImpostorBakingTools.GlobalTexImport, new string[] { "Ask if resolution is different", "Don't ask, always change", "Don't ask, never change" }, Array.Empty<GUILayoutOption>());
			ImpostorBakingTools.GlobalCreateLodGroup = EditorGUILayout.Toggle("Create LODGroup if not present", ImpostorBakingTools.GlobalCreateLodGroup, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			GUILayout.Label(ImpostorBakingTools.DefaultSuffixesLabel, "boldlabel", Array.Empty<GUILayoutOption>());
			ImpostorBakingTools.GlobalAlbedoAlpha = EditorGUILayout.TextField("Albedo & Alpha", ImpostorBakingTools.GlobalAlbedoAlpha, Array.Empty<GUILayoutOption>());
			ImpostorBakingTools.GlobalSpecularSmoothness = EditorGUILayout.TextField("Specular & Smoothness", ImpostorBakingTools.GlobalSpecularSmoothness, Array.Empty<GUILayoutOption>());
			ImpostorBakingTools.GlobalNormalDepth = EditorGUILayout.TextField("Normal & Depth", ImpostorBakingTools.GlobalNormalDepth, Array.Empty<GUILayoutOption>());
			ImpostorBakingTools.GlobalEmissionOcclusion = EditorGUILayout.TextField("Emission & Occlusion", ImpostorBakingTools.GlobalEmissionOcclusion, Array.Empty<GUILayoutOption>());
			if (GUI.changed)
			{
				EditorPrefs.SetBool(ImpostorBakingTools.PrefGlobalDefault, ImpostorBakingTools.GlobalDefaultMode);
				EditorPrefs.SetInt(ImpostorBakingTools.PrefGlobalTexImport, ImpostorBakingTools.GlobalTexImport);
				EditorPrefs.SetBool(ImpostorBakingTools.PrefGlobalCreateLodGroup, ImpostorBakingTools.GlobalCreateLodGroup);
				EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalGBuffer0Name, ImpostorBakingTools.GlobalAlbedoAlpha);
				EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalGBuffer1Name, ImpostorBakingTools.GlobalSpecularSmoothness);
				EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalGBuffer2Name, ImpostorBakingTools.GlobalNormalDepth);
				EditorPrefs.SetString(ImpostorBakingTools.PrefGlobalGBuffer3Name, ImpostorBakingTools.GlobalEmissionOcclusion);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B7C8 File Offset: 0x000099C8
		public static void LoadDefaults()
		{
			ImpostorBakingTools.GlobalFolder = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalFolder, "");
			ImpostorBakingTools.GlobalRelativeFolder = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalRelativeFolder, "");
			ImpostorBakingTools.GlobalDefaultMode = EditorPrefs.GetBool(ImpostorBakingTools.PrefGlobalDefault, false);
			ImpostorBakingTools.GlobalTexImport = EditorPrefs.GetInt(ImpostorBakingTools.PrefGlobalTexImport, 0);
			ImpostorBakingTools.GlobalCreateLodGroup = EditorPrefs.GetBool(ImpostorBakingTools.PrefGlobalCreateLodGroup, false);
			ImpostorBakingTools.GlobalBakingOptions = EditorPrefs.GetBool(ImpostorBakingTools.PrefGlobalBakingOptions, true);
			ImpostorBakingTools.GlobalAlbedoAlpha = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalGBuffer0Name, "_AlbedoAlpha");
			ImpostorBakingTools.GlobalSpecularSmoothness = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalGBuffer1Name, "_SpecularSmoothness");
			ImpostorBakingTools.GlobalNormalDepth = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalGBuffer2Name, "_NormalDepth");
			ImpostorBakingTools.GlobalEmissionOcclusion = EditorPrefs.GetString(ImpostorBakingTools.PrefGlobalGBuffer3Name, "_EmissionOcclusion");
		}

		// Token: 0x0400021C RID: 540
		public static readonly string PrefGlobalFolder = "IMPOSTORS_GLOBALFOLDER";

		// Token: 0x0400021D RID: 541
		public static readonly string PrefGlobalRelativeFolder = "IMPOSTORS_GLOBALRELATIVEFOLDER";

		// Token: 0x0400021E RID: 542
		public static readonly string PrefGlobalDefault = "IMPOSTORS_GLOBALDEFAULT";

		// Token: 0x0400021F RID: 543
		public static readonly string PrefGlobalTexImport = "IMPOSTORS_GLOBALTEXIMPORT";

		// Token: 0x04000220 RID: 544
		public static readonly string PrefGlobalCreateLodGroup = "IMPOSTORS_GLOBALCREATELODGROUP ";

		// Token: 0x04000221 RID: 545
		public static readonly string PrefGlobalGBuffer0Name = "IMPOSTORS_GLOBALGBUFFER0SUFFIX";

		// Token: 0x04000222 RID: 546
		public static readonly string PrefGlobalGBuffer1Name = "IMPOSTORS_GLOBALGBUFFER1SUFFIX";

		// Token: 0x04000223 RID: 547
		public static readonly string PrefGlobalGBuffer2Name = "IMPOSTORS_GLOBALGBUFFER2SUFFIX";

		// Token: 0x04000224 RID: 548
		public static readonly string PrefGlobalGBuffer3Name = "IMPOSTORS_GLOBALGBUFFER3SUFFIX";

		// Token: 0x04000225 RID: 549
		public static readonly string PrefGlobalBakingOptions = "IMPOSTORS_GLOBALBakingOptions";

		// Token: 0x04000226 RID: 550
		public static readonly string PrefDataImpType = "IMPOSTORS_DATAIMPTYPE";

		// Token: 0x04000227 RID: 551
		public static readonly string PrefDataTexSizeLocked = "IMPOSTORS_DATATEXSIZEXLOCKED";

		// Token: 0x04000228 RID: 552
		public static readonly string PrefDataTexSizeSelected = "IMPOSTORS_DATATEXSIZEXSELECTED";

		// Token: 0x04000229 RID: 553
		public static readonly string PrefDataTexSizeX = "IMPOSTORS_DATATEXSIZEX";

		// Token: 0x0400022A RID: 554
		public static readonly string PrefDataTexSizeY = "IMPOSTORS_DATATEXSIZEY";

		// Token: 0x0400022B RID: 555
		public static readonly string PrefDataDecoupledFrames = "IMPOSTORS_DATADECOUPLEDFRAMES";

		// Token: 0x0400022C RID: 556
		public static readonly string PrefDataXFrames = "IMPOSTORS_DATAXFRAMES";

		// Token: 0x0400022D RID: 557
		public static readonly string PrefDataYFrames = "IMPOSTORS_DATAYFRAMES";

		// Token: 0x0400022E RID: 558
		public static readonly string PrefDataPixelBleeding = "IMPOSTORS_DATAPIXELBLEEDING";

		// Token: 0x0400022F RID: 559
		public static readonly string PrefDataTolerance = "IMPOSTORS_DATATOLERANCE ";

		// Token: 0x04000230 RID: 560
		public static readonly string PrefDataNormalScale = "IMPOSTORS_DATANORMALSCALE";

		// Token: 0x04000231 RID: 561
		public static readonly string PrefDataMaxVertices = "IMPOSTORS_DATAMAXVERTICES";

		// Token: 0x04000232 RID: 562
		public static bool GlobalDefaultMode = false;

		// Token: 0x04000233 RID: 563
		public static string GlobalFolder = string.Empty;

		// Token: 0x04000234 RID: 564
		public static string GlobalRelativeFolder = string.Empty;

		// Token: 0x04000235 RID: 565
		public static int GlobalTexImport = 0;

		// Token: 0x04000236 RID: 566
		public static bool GlobalCreateLodGroup = false;

		// Token: 0x04000237 RID: 567
		public static string GlobalAlbedoAlpha = string.Empty;

		// Token: 0x04000238 RID: 568
		public static string GlobalSpecularSmoothness = string.Empty;

		// Token: 0x04000239 RID: 569
		public static string GlobalNormalDepth = string.Empty;

		// Token: 0x0400023A RID: 570
		public static string GlobalEmissionOcclusion = string.Empty;

		// Token: 0x0400023B RID: 571
		public static bool GlobalBakingOptions = true;

		// Token: 0x0400023C RID: 572
		private static readonly GUIContent DefaultSuffixesLabel = new GUIContent("Default Suffixes", "Default Suffixes for new Bake Presets");

		// Token: 0x0400023D RID: 573
		private static bool PrefsLoaded = false;

		// Token: 0x0400023E RID: 574
		private static GUIContent PathButtonContent = new GUIContent();
	}
}
