using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class DllFileInfo
{
	// Token: 0x06000297 RID: 663 RVA: 0x0001095C File Offset: 0x0000EB5C
	public static Dictionary<string, ClassTypeInfo> CalGuidMappingTableOfPath()
	{
		Dictionary<string, ClassTypeInfo> dictionary = new Dictionary<string, ClassTypeInfo>();
		string text = Application.dataPath.Replace("Assets", "Packages");
		int num = text.Length - "Packages".Length;
		string[] array = new string[] { ".dll" };
		foreach (string text2 in FileExtensions.FindAllFileWithSuffixs(text, array))
		{
			if (Path.GetFileName(text2).StartsWith("com.para."))
			{
				Assembly assembly = Assembly.LoadFrom(text2);
				string text3 = AssetDatabase.AssetPathToGUID(text2.Substring(num, text2.Length - num));
				Type[] array2 = null;
				try
				{
					array2 = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					ParaLog.Log("ReflectionTypeLoadException:" + ex.Message);
				}
				foreach (Type type in array2)
				{
					if (dictionary.ContainsKey(type.Name))
					{
						ParaLog.LogWarning(SdkLangManager.Get("str_sdk_bottomMessage_text42") + type.Name);
					}
					else
					{
						ClassTypeInfo classTypeInfo = default(ClassTypeInfo);
						classTypeInfo.dllGuid = text3;
						classTypeInfo.fileId = FileIDUtil.Compute(type).ToString();
						dictionary.Add(type.Name, classTypeInfo);
					}
				}
			}
		}
		return dictionary;
	}
}
