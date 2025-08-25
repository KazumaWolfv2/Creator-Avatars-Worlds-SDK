using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

// Token: 0x02000005 RID: 5
public class AssetShaderPackage
{
	// Token: 0x06000016 RID: 22 RVA: 0x00002820 File Offset: 0x00000A20
	public static void HandleShaderListAssetBundle()
	{
		List<string> list = new List<string>();
		List<string> list2 = AssetShaderPackage.FindShaderListPath();
		if (list2 != null)
		{
			foreach (string text in list2)
			{
				string text2 = AssetDatabase.GUIDToAssetPath(text);
				list.Add(text2);
			}
		}
		Singleton<DependencesMgr>.instance.CollectDependenceMap(list);
		Singleton<DependencesMgr>.instance.BuildAssetBundles();
		AssetBundlePackage.SetBundleNames();
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000289C File Offset: 0x00000A9C
	public static List<string> FindShaderListPath()
	{
		List<string> list = new List<string> { "Assets/ParaAvatarSDK/ToolBox/Shaders" };
		return AssetDatabase.FindAssets("t:ShaderVariantCollection", list.ToArray()).ToList<string>();
	}
}
