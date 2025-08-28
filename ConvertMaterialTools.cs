using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class ConvertMaterialTools
{
	// Token: 0x0600000B RID: 11 RVA: 0x000020BC File Offset: 0x000002BC
	public static bool IsLibriyShader(string shaderName)
	{
		if (ConvertMaterialTools.shaderLibs.Count <= 0)
		{
			ConvertMaterialTools.ReadShaderLibs();
		}
		using (List<string>.Enumerator enumerator = ConvertMaterialTools.shaderLibs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Equals(shaderName))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002128 File Offset: 0x00000328
	public static void ReadShaderLibs()
	{
		ConvertMaterialTools.shaderLibs.Clear();
		string[] array = new StreamReader(new FileStream(Path.Combine(Path.Combine(Application.dataPath, "../"), "Packages/com.para.common/Config/configshader.txt"), FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd().Split('\n', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Replace("\r", "").Split('\t', StringSplitOptions.None);
			if (array2 != null)
			{
				string text = array2[0];
				ConvertMaterialTools.shaderLibs.Add(text);
			}
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000021B4 File Offset: 0x000003B4
	public void Readconfig()
	{
		using (FileStream fileStream = new FileStream(Path.Combine(Application.dataPath, "../") + "Packages/com.para.common/Config/coifgshaderpropertys.txt", FileMode.Open, FileAccess.Read))
		{
			using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				string text = streamReader.ReadToEnd();
				this.shaderPropertys.Clear();
				string[] array = text.Split('\n', StringSplitOptions.None);
				for (int i = 2; i < array.Length; i++)
				{
					string[] array2 = array[i].Replace("\r", "").Split('\t', StringSplitOptions.None);
					if (array2 != null)
					{
						ConvertMaterialTools.CustomShaderProperty customShaderProperty = this.findshader(array2[0]);
						if (customShaderProperty == null)
						{
							customShaderProperty = new ConvertMaterialTools.CustomShaderProperty();
							customShaderProperty.shaderKey = array2[0];
							this.shaderPropertys.Add(customShaderProperty.shaderKey, customShaderProperty);
						}
						if (array2 != null)
						{
							for (int j = 1; j < array2.Length; j++)
							{
								if (!string.IsNullOrEmpty(array2[j]))
								{
									customShaderProperty.AddOne(array2[j]);
								}
							}
						}
					}
				}
				ConvertMaterialTools.shaderLibs.Clear();
				array = new StreamReader(new FileStream(Path.Combine(Path.Combine(Application.dataPath, "../"), "Packages/com.para.common/Config/configshader.txt"), FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd().Split('\n', StringSplitOptions.None);
				for (int k = 0; k < array.Length; k++)
				{
					string[] array3 = array[k].Replace("\r", "").Split('\t', StringSplitOptions.None);
					if (array3 != null)
					{
						string text2 = array3[0];
						ConvertMaterialTools.shaderLibs.Add(text2);
					}
				}
			}
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002374 File Offset: 0x00000574
	private ConvertMaterialTools.CustomShaderProperty findshader(string key)
	{
		ConvertMaterialTools.CustomShaderProperty customShaderProperty = null;
		if (this.shaderPropertys.TryGetValue(key, out customShaderProperty))
		{
			return customShaderProperty;
		}
		return null;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002398 File Offset: 0x00000598
	public void ConverPropertys(Material from, Material to)
	{
		if (from.shader != null && to.shader != null && from.shader == to.shader)
		{
			return;
		}
		ConvertMaterialTools.<>c__DisplayClass8_0 CS$<>8__locals1;
		CS$<>8__locals1.srcTexs = MaterialExtensions.CollectTextures(from);
		List<MaterialExtensions.CustomPropertys> list = MaterialExtensions.CollectTextures(to);
		CS$<>8__locals1.srcPropertys = MaterialExtensions.CollectPropertysOutwithTexture(from);
		List<MaterialExtensions.CustomPropertys> list2 = MaterialExtensions.CollectPropertysOutwithTexture(to);
		if (CS$<>8__locals1.srcTexs.Count <= 0 || list.Count <= 0)
		{
			return;
		}
		foreach (MaterialExtensions.CustomPropertys customPropertys in list)
		{
			foreach (MaterialExtensions.CustomPropertys customPropertys2 in CS$<>8__locals1.srcTexs)
			{
				if (!(customPropertys2.property == null) && customPropertys.propertyName.Equals(customPropertys2.propertyName))
				{
					MaterialExtensions.mySetPropertyEX(to, customPropertys.nPropertyID, customPropertys2.property);
					break;
				}
			}
		}
		foreach (MaterialExtensions.CustomPropertys customPropertys3 in list)
		{
			ConvertMaterialTools.CustomShaderProperty customShaderProperty = this.findshader(customPropertys3.propertyName);
			if (customShaderProperty != null)
			{
				for (int i = 0; i < customShaderProperty.keys.Count; i++)
				{
					MaterialExtensions.CustomPropertys customPropertys4 = ConvertMaterialTools.<ConverPropertys>g__FindProperty|8_0(customShaderProperty.keys[i], ref CS$<>8__locals1);
					if (customPropertys4 != null && customPropertys4.property != null)
					{
						MaterialExtensions.mySetPropertyEX(to, customPropertys3.nPropertyID, customPropertys4.property);
						break;
					}
				}
			}
		}
		if (CS$<>8__locals1.srcPropertys.Count <= 0 || list2.Count <= 0)
		{
			return;
		}
		foreach (MaterialExtensions.CustomPropertys customPropertys5 in list2)
		{
			foreach (MaterialExtensions.CustomPropertys customPropertys6 in CS$<>8__locals1.srcPropertys)
			{
				if (!(customPropertys6.property == null) && customPropertys5.propertyName.Equals(customPropertys6.propertyName) && customPropertys5.propertyType == customPropertys6.propertyType)
				{
					if (customPropertys5.propertyType == MaterialExtensions.PropertyType.COLOR)
					{
						MaterialExtensions.mySetPropertyEX(to, customPropertys5.nPropertyID, customPropertys6.propertyColor);
					}
					if (customPropertys5.propertyType == MaterialExtensions.PropertyType.VECTOR4)
					{
						MaterialExtensions.mySetPropertyEX(to, customPropertys5.nPropertyID, customPropertys6.propertyVector);
					}
					if (customPropertys5.propertyType == MaterialExtensions.PropertyType.FLOAT)
					{
						MaterialExtensions.mySetPropertyEX(to, customPropertys5.nPropertyID, customPropertys6.propertyFloat);
					}
				}
			}
		}
		foreach (MaterialExtensions.CustomPropertys customPropertys7 in list2)
		{
			ConvertMaterialTools.CustomShaderProperty customShaderProperty2 = this.findshader(customPropertys7.propertyName);
			if (customShaderProperty2 != null)
			{
				for (int j = 0; j < customShaderProperty2.keys.Count; j++)
				{
					MaterialExtensions.CustomPropertys customPropertys8 = ConvertMaterialTools.<ConverPropertys>g__FindPropertyWithOutTexture|8_1(customShaderProperty2.keys[j], ref CS$<>8__locals1);
					if (customPropertys8 != null)
					{
						if (customPropertys7.propertyType == MaterialExtensions.PropertyType.COLOR)
						{
							MaterialExtensions.mySetPropertyEX(to, customPropertys7.nPropertyID, customPropertys8.propertyColor);
						}
						if (customPropertys7.propertyType == MaterialExtensions.PropertyType.VECTOR4)
						{
							MaterialExtensions.mySetPropertyEX(to, customPropertys7.nPropertyID, customPropertys8.propertyVector);
						}
						if (customPropertys7.propertyType == MaterialExtensions.PropertyType.FLOAT)
						{
							MaterialExtensions.mySetPropertyEX(to, customPropertys7.nPropertyID, customPropertys8.propertyFloat);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000027A8 File Offset: 0x000009A8
	public string Remapshader(Material srcMat, bool isPBR = false, bool translucent = false)
	{
		string text = string.Empty;
		if (!isPBR)
		{
			if (!translucent)
			{
				text = "ParaSpace/Avatar/Cartoon/AvatarNPR_HD";
			}
			else
			{
				text = "ParaSpace/Avatar/Cartoon/AvatarNPR_HD_Translucent";
			}
		}
		else if (!translucent)
		{
			text = "ParaSpace/Avatar/PBR/AvatarPBR";
		}
		else
		{
			text = "ParaSpace/Avatar/PBR/AvatarPBR_Translucent";
		}
		return text;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000027E4 File Offset: 0x000009E4
	public Material SaveNewMaterial(Material newMaterial)
	{
		if (newMaterial != null)
		{
			string assetPath = AssetDatabase.GetAssetPath(newMaterial);
			string text = assetPath.ToLower();
			if (!string.IsNullOrEmpty(assetPath) && !text.EndsWith(".fbx") && !text.EndsWith(".FBX") && !text.EndsWith(".Fbx"))
			{
				AssetDatabase.SaveAssets();
				newMaterial = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
			}
		}
		return newMaterial;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002848 File Offset: 0x00000A48
	public string BackupMaterial(Material oldMaterial)
	{
		string text = string.Empty;
		if (oldMaterial != null && oldMaterial != null)
		{
			string assetPath = AssetDatabase.GetAssetPath(oldMaterial);
			string text2 = assetPath.ToLower();
			if (!string.IsNullOrEmpty(assetPath))
			{
				Material material = new Material(oldMaterial);
				int num = assetPath.LastIndexOf('.');
				int length = assetPath.Length;
				string text3 = assetPath.Substring(num, length - num);
				string text4;
				if (!text2.EndsWith(".fbx"))
				{
					text4 = assetPath.Substring(0, num) + "_backup" + text3;
				}
				else
				{
					text4 = assetPath.Substring(0, num) + "_" + oldMaterial.name + "_backup.mat";
				}
				text = text4;
				if (AssetDatabase.LoadMainAssetAtPath(text4) as Material == null)
				{
					AssetDatabase.CreateAsset(material, text4);
				}
			}
		}
		return text;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002923 File Offset: 0x00000B23
	public Material GetExitMaterial(Material src, string Name)
	{
		return null;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002948 File Offset: 0x00000B48
	[CompilerGenerated]
	internal static MaterialExtensions.CustomPropertys <ConverPropertys>g__FindProperty|8_0(string key, ref ConvertMaterialTools.<>c__DisplayClass8_0 A_1)
	{
		foreach (MaterialExtensions.CustomPropertys customPropertys in A_1.srcTexs)
		{
			if (customPropertys.propertyName.Equals(key))
			{
				return customPropertys;
			}
		}
		return null;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000029AC File Offset: 0x00000BAC
	[CompilerGenerated]
	internal static MaterialExtensions.CustomPropertys <ConverPropertys>g__FindPropertyWithOutTexture|8_1(string key, ref ConvertMaterialTools.<>c__DisplayClass8_0 A_1)
	{
		foreach (MaterialExtensions.CustomPropertys customPropertys in A_1.srcPropertys)
		{
			if (customPropertys.propertyName.Equals(key))
			{
				return customPropertys;
			}
		}
		return null;
	}

	// Token: 0x04000004 RID: 4
	private static List<string> shaderLibs = new List<string>();

	// Token: 0x04000005 RID: 5
	private Dictionary<string, ConvertMaterialTools.CustomShaderProperty> shaderPropertys = new Dictionary<string, ConvertMaterialTools.CustomShaderProperty>();

	// Token: 0x0200007D RID: 125
	public class CustomShaderProperty
	{
		// Token: 0x06000257 RID: 599 RVA: 0x00010413 File Offset: 0x0000E613
		public void AddOne(string key)
		{
			this.keys.Add(key);
		}

		// Token: 0x040002DA RID: 730
		public string shaderKey = string.Empty;

		// Token: 0x040002DB RID: 731
		public List<string> keys = new List<string>();
	}

	// Token: 0x0200007E RID: 126
	public class CustomShaderName
	{
		// Token: 0x040002DC RID: 732
		public string dstShaderName = string.Empty;

		// Token: 0x040002DD RID: 733
		public string srcShaderName = string.Empty;
	}
}
