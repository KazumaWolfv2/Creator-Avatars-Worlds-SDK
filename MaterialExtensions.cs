using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000006 RID: 6
public static class MaterialExtensions
{
	// Token: 0x06000018 RID: 24 RVA: 0x00002A10 File Offset: 0x00000C10
	static MaterialExtensions()
	{
		Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetTypes().Any((Type t) => t.Name == "ShaderUtil"));
		if (assembly != null)
		{
			foreach (MethodInfo methodInfo in assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "ShaderUtil").GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				MaterialExtensions.methods[methodInfo.Name] = methodInfo;
			}
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002A98 File Offset: 0x00000C98
	public static List<Texture> GetTextures(Material shader)
	{
		List<Texture> list = new List<Texture>();
		int num = MaterialExtensions.myGetPropertyCountEX(shader);
		for (int i = 0; i < num; i++)
		{
			if (MaterialExtensions.myGetPropertyTypeEX(shader, i) == 4)
			{
				list.Add((Texture)MaterialExtensions.myGetPropertyEX(shader, i));
			}
		}
		return list;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002ADC File Offset: 0x00000CDC
	public static List<MaterialExtensions.CustomPropertys> CollectTextures(Material shader)
	{
		List<MaterialExtensions.CustomPropertys> list = new List<MaterialExtensions.CustomPropertys>();
		int num = MaterialExtensions.myGetPropertyCountEX(shader);
		if (num <= 0)
		{
			list.Add(new MaterialExtensions.CustomPropertys
			{
				propertyName = "_MainTex",
				nPropertyID = 10000,
				property = shader.mainTexture
			});
		}
		for (int i = 0; i < num; i++)
		{
			if (MaterialExtensions.myGetPropertyTypeEX(shader, i) == 4)
			{
				list.Add(new MaterialExtensions.CustomPropertys
				{
					propertyName = MaterialExtensions.myGetPropertyNameEX(shader, i),
					nPropertyID = i,
					property = (Texture)MaterialExtensions.myGetPropertyEX(shader, i)
				});
			}
		}
		return list;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002B78 File Offset: 0x00000D78
	public static List<MaterialExtensions.CustomPropertys> CollectPropertysOutwithTexture(Material shader)
	{
		List<MaterialExtensions.CustomPropertys> list = new List<MaterialExtensions.CustomPropertys>();
		int num = MaterialExtensions.myGetPropertyCountEX(shader);
		for (int i = 0; i < num; i++)
		{
			if (MaterialExtensions.myGetPropertyTypeEX(shader, i) == 0)
			{
				list.Add(new MaterialExtensions.CustomPropertys
				{
					propertyName = MaterialExtensions.myGetPropertyNameEX(shader, i),
					nPropertyID = i,
					propertyType = MaterialExtensions.PropertyType.COLOR,
					propertyColor = (Color)MaterialExtensions.myGetPropertyEX(shader, i)
				});
			}
			if (MaterialExtensions.myGetPropertyTypeEX(shader, i) == 1)
			{
				list.Add(new MaterialExtensions.CustomPropertys
				{
					propertyName = MaterialExtensions.myGetPropertyNameEX(shader, i),
					nPropertyID = i,
					propertyType = MaterialExtensions.PropertyType.VECTOR4,
					propertyVector = (Vector4)MaterialExtensions.myGetPropertyEX(shader, i)
				});
			}
			if (MaterialExtensions.myGetPropertyTypeEX(shader, i) == 2 || MaterialExtensions.myGetPropertyTypeEX(shader, i) == 3)
			{
				list.Add(new MaterialExtensions.CustomPropertys
				{
					propertyName = MaterialExtensions.myGetPropertyNameEX(shader, i),
					nPropertyID = i,
					propertyType = MaterialExtensions.PropertyType.FLOAT,
					propertyFloat = (float)MaterialExtensions.myGetPropertyEX(shader, i)
				});
			}
		}
		return list;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002C86 File Offset: 0x00000E86
	public static int myGetPropertyCountEX(Material shader)
	{
		return MaterialExtensions.Call<int>("GetPropertyCount", new object[] { shader.shader });
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002CA1 File Offset: 0x00000EA1
	public static int myGetPropertyTypeEX(Material shader, int index)
	{
		return MaterialExtensions.Call<int>("GetPropertyType", new object[] { shader.shader, index });
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002CC5 File Offset: 0x00000EC5
	public static string myGetPropertyNameEX(Material shader, int index)
	{
		return MaterialExtensions.Call<string>("GetPropertyName", new object[] { shader.shader, index });
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002CEC File Offset: 0x00000EEC
	public static void mySetPropertyEX(Material material, int index, object value)
	{
		string text = MaterialExtensions.myGetPropertyNameEX(material, index);
		switch (MaterialExtensions.myGetPropertyTypeEX(material, index))
		{
		case 0:
			material.SetColor(text, (Color)value);
			return;
		case 1:
			material.SetVector(text, (Vector4)value);
			return;
		case 2:
			material.SetFloat(text, (float)value);
			return;
		case 3:
			material.SetFloat(text, (float)value);
			return;
		case 4:
			material.SetTexture(text, (Texture)value);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002D6C File Offset: 0x00000F6C
	public static object myGetPropertyEX(Material material, int index)
	{
		string text = MaterialExtensions.myGetPropertyNameEX(material, index);
		switch (MaterialExtensions.myGetPropertyTypeEX(material, index))
		{
		case 0:
			return material.GetColor(text);
		case 1:
			return material.GetVector(text);
		case 2:
		case 3:
			return material.GetFloat(text);
		case 4:
			return material.GetTexture(text);
		default:
			return null;
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002DD5 File Offset: 0x00000FD5
	public static T Call<T>(string name, params object[] parameters)
	{
		return (T)((object)MaterialExtensions.methods[name].Invoke(null, parameters));
	}

	// Token: 0x04000006 RID: 6
	public static Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

	// Token: 0x02000080 RID: 128
	public enum PropertyType
	{
		// Token: 0x040002E1 RID: 737
		TEXTURE,
		// Token: 0x040002E2 RID: 738
		COLOR,
		// Token: 0x040002E3 RID: 739
		VECTOR4,
		// Token: 0x040002E4 RID: 740
		FLOAT,
		// Token: 0x040002E5 RID: 741
		INT
	}

	// Token: 0x02000081 RID: 129
	public class CustomPropertys
	{
		// Token: 0x040002E6 RID: 742
		public string propertyName;

		// Token: 0x040002E7 RID: 743
		public int nPropertyID;

		// Token: 0x040002E8 RID: 744
		public MaterialExtensions.PropertyType propertyType;

		// Token: 0x040002E9 RID: 745
		public Texture property;

		// Token: 0x040002EA RID: 746
		public Color propertyColor;

		// Token: 0x040002EB RID: 747
		public Vector4 propertyVector;

		// Token: 0x040002EC RID: 748
		public float propertyFloat;

		// Token: 0x040002ED RID: 749
		public int propertyInt;
	}
}
