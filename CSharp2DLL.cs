using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class CSharp2DLL
{
	// Token: 0x0600028E RID: 654 RVA: 0x00010638 File Offset: 0x0000E838
	public static void InitGuidMappingTableOfAllScripts()
	{
		CSharp2DLL.guidMappingTableFromScripts = new Dictionary<string, string>();
		CSharp2DLL.InitGuidMappingTableOfPath(Application.dataPath.Replace("Assets", "Packages"), false);
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00010660 File Offset: 0x0000E860
	private static void InitGuidMappingTableOfPath(string path, bool dllToSrc = false)
	{
		string[] array = new string[] { ".cs.meta" };
		foreach (string text in FileExtensions.FindAllFileWithSuffixs(path, array))
		{
			CSharp2DLL.guidMappingTableFromScripts[CSharp2DLL.GetGuidFromMeta(text)] = FileExtensions.getFileNameFromPath(text);
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x000106D4 File Offset: 0x0000E8D4
	private static string GetGuidFromMeta(string filePath)
	{
		string text = "";
		using (StreamReader streamReader = new StreamReader(filePath))
		{
			while (!streamReader.EndOfStream)
			{
				string text2 = streamReader.ReadLine();
				if (text2.StartsWith("guid:"))
				{
					text = text2.Substring(text2.IndexOf(":") + 2);
					break;
				}
			}
			streamReader.Close();
		}
		return text;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00010744 File Offset: 0x0000E944
	public static void ReplaceSriptReferenceOfAllScripts(Dictionary<string, ClassTypeInfo> data, List<string> pathList)
	{
		CSharp2DLL.classTypeMapFromDll = data;
		CSharp2DLL.InitGuidMappingTableOfAllScripts();
		for (int i = 0; i < pathList.Count; i++)
		{
			CSharp2DLL.ReplaceSriptReferenceOfPath(pathList[i]);
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0001077C File Offset: 0x0000E97C
	private static void ReplaceSriptReferenceOfPath(string path)
	{
		string[] array = new string[] { ".asset", ".prefab", ".unity", ".controller" };
		List<string> list = FileExtensions.FindAllFileWithSuffixs(path, array);
		for (int i = 0; i < list.Count; i++)
		{
			EditorUtility.DisplayProgressBar("Replace Dll", list[i], (float)i * 1f / (float)list.Count);
			CSharp2DLL.ReplaceScriptReference(list[i]);
		}
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00010800 File Offset: 0x0000EA00
	private static void ReplaceScriptReference(string filePath)
	{
		ParaLog.Log("Ready to replace:" + filePath);
		string[] array = File.ReadAllLines(filePath);
		int i = 0;
		bool flag = false;
		while (i < array.Length)
		{
			if (array[i].StartsWith("MonoBehaviour:"))
			{
				do
				{
					i++;
				}
				while (!array[i].TrimStart(new char[0]).StartsWith("m_Script:"));
				flag = CSharp2DLL.replaceGUIDAnfFileIDFromSrcToDll(ref array[i]) || flag;
			}
			i++;
		}
		if (flag)
		{
			File.WriteAllLines(filePath, array);
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0001087C File Offset: 0x0000EA7C
	private static string getGUIDFrommScriptReferenceLine(string lineStr)
	{
		int num = lineStr.IndexOf("guid:") + "guid: ".Length;
		int num2 = lineStr.LastIndexOf(",") - num;
		if (num2 <= 0)
		{
			return null;
		}
		return lineStr.Substring(num, num2);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x000108BC File Offset: 0x0000EABC
	private static bool replaceGUIDAnfFileIDFromSrcToDll(ref string lineStr)
	{
		bool flag = false;
		string guidfrommScriptReferenceLine = CSharp2DLL.getGUIDFrommScriptReferenceLine(lineStr);
		if (guidfrommScriptReferenceLine == null)
		{
			return false;
		}
		string text;
		ClassTypeInfo classTypeInfo;
		if (!CSharp2DLL.guidMappingTableFromScripts.TryGetValue(guidfrommScriptReferenceLine, out text))
		{
			ParaLog.LogWarning(SdkLangManager.Get("str_sdk_bottomMessage_text43") + guidfrommScriptReferenceLine);
		}
		else if (!CSharp2DLL.classTypeMapFromDll.TryGetValue(text, out classTypeInfo))
		{
			ParaLog.LogWarning(SdkLangManager.Get("str_sdk_bottomMessage_text44") + text);
		}
		else
		{
			ParaLog.Log("Replacing script reference:" + text);
			lineStr = lineStr.Replace("11500000", classTypeInfo.fileId);
			lineStr = lineStr.Replace(guidfrommScriptReferenceLine, classTypeInfo.dllGuid);
			flag = true;
		}
		return flag;
	}

	// Token: 0x04000170 RID: 368
	public const string DEFAULT_FILE_ID_OF_SCRIPT = "11500000";

	// Token: 0x04000171 RID: 369
	private static Dictionary<string, string> guidMappingTableFromScripts;

	// Token: 0x04000172 RID: 370
	private static Dictionary<string, ClassTypeInfo> classTypeMapFromDll;

	// Token: 0x020000AD RID: 173
	// (Invoke) Token: 0x06000507 RID: 1287
	public delegate string GetDllGUIDFunc(string fileID);
}
