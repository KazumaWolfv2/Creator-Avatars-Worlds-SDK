using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

// Token: 0x02000010 RID: 16
public static class EditorTextureUtil
{
	// Token: 0x0600007E RID: 126 RVA: 0x00006951 File Offset: 0x00004B51
	static EditorTextureUtil()
	{
		Assert.IsNotNull<Type>(EditorTextureUtil.cType);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00006978 File Offset: 0x00004B78
	public static int GetMipmapCount(Texture texture)
	{
		if (EditorTextureUtil.mMethod_GetMipmapCount == null)
		{
			EditorTextureUtil.mMethod_GetMipmapCount = EditorTextureUtil.cType.GetMethod("GetMipmapCount", BindingFlags.Static | BindingFlags.Public);
		}
		Assert.IsNotNull<MethodInfo>(EditorTextureUtil.mMethod_GetMipmapCount);
		MethodBase methodBase = EditorTextureUtil.mMethod_GetMipmapCount;
		object obj = null;
		object[] array = new Texture[] { texture };
		return (int)methodBase.Invoke(obj, array);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000069D0 File Offset: 0x00004BD0
	public static TextureFormat GetTextureFormat(Texture texture)
	{
		if (EditorTextureUtil.mMethod_GetTextureFormat == null)
		{
			EditorTextureUtil.mMethod_GetTextureFormat = EditorTextureUtil.cType.GetMethod("GetTextureFormat", BindingFlags.Static | BindingFlags.Public);
		}
		Assert.IsNotNull<MethodInfo>(EditorTextureUtil.mMethod_GetTextureFormat);
		MethodBase methodBase = EditorTextureUtil.mMethod_GetTextureFormat;
		object obj = null;
		object[] array = new Texture[] { texture };
		return (TextureFormat)methodBase.Invoke(obj, array);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00006A28 File Offset: 0x00004C28
	public static long GetRuntimeMemorySize(Texture texture)
	{
		if (EditorTextureUtil.mMethod_GetRuntimeMemorySizeLong == null)
		{
			EditorTextureUtil.mMethod_GetRuntimeMemorySizeLong = EditorTextureUtil.cType.GetMethod("GetRuntimeMemorySizeLong", BindingFlags.Static | BindingFlags.Public);
		}
		Assert.IsNotNull<MethodInfo>(EditorTextureUtil.mMethod_GetRuntimeMemorySizeLong);
		MethodBase methodBase = EditorTextureUtil.mMethod_GetRuntimeMemorySizeLong;
		object obj = null;
		object[] array = new Texture[] { texture };
		return (long)methodBase.Invoke(obj, array);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00006A80 File Offset: 0x00004C80
	public static long GetStorageMemorySize(Texture texture)
	{
		if (EditorTextureUtil.mMethod_GetStorageMemorySizeLong == null)
		{
			EditorTextureUtil.mMethod_GetStorageMemorySizeLong = EditorTextureUtil.cType.GetMethod("GetStorageMemorySizeLong", BindingFlags.Static | BindingFlags.Public);
		}
		Assert.IsNotNull<MethodInfo>(EditorTextureUtil.mMethod_GetStorageMemorySizeLong);
		MethodBase methodBase = EditorTextureUtil.mMethod_GetStorageMemorySizeLong;
		object obj = null;
		object[] array = new Texture[] { texture };
		return (long)methodBase.Invoke(obj, array);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00006AD8 File Offset: 0x00004CD8
	public static bool IsNonPowerOfTwo(Texture2D texture)
	{
		if (EditorTextureUtil.mMethod_IsNonPowerOfTwo == null)
		{
			EditorTextureUtil.mMethod_IsNonPowerOfTwo = EditorTextureUtil.cType.GetMethod("IsNonPowerOfTwo", BindingFlags.Static | BindingFlags.Public);
		}
		Assert.IsNotNull<MethodInfo>(EditorTextureUtil.mMethod_IsNonPowerOfTwo);
		MethodBase methodBase = EditorTextureUtil.mMethod_IsNonPowerOfTwo;
		object obj = null;
		object[] array = new Texture2D[] { texture };
		return (bool)methodBase.Invoke(obj, array);
	}

	// Token: 0x04000046 RID: 70
	private static readonly Type cType = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");

	// Token: 0x04000047 RID: 71
	private static MethodInfo mMethod_GetMipmapCount;

	// Token: 0x04000048 RID: 72
	private static MethodInfo mMethod_GetTextureFormat;

	// Token: 0x04000049 RID: 73
	private static MethodInfo mMethod_GetRuntimeMemorySizeLong;

	// Token: 0x0400004A RID: 74
	private static MethodInfo mMethod_GetStorageMemorySizeLong;

	// Token: 0x0400004B RID: 75
	private static MethodInfo mMethod_IsNonPowerOfTwo;
}
