using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine.SceneManagement;

// Token: 0x02000053 RID: 83
[InitializeOnLoad]
public class ShaderProxy
{
	// Token: 0x06000289 RID: 649 RVA: 0x000104A8 File Offset: 0x0000E6A8
	public static string SavePreloadShader(string scenePath)
	{
		SceneManager.GetActiveScene();
		string text = Path.ChangeExtension(scenePath, ShaderProxy.SUFFIX);
		MethodInfo method = typeof(ShaderUtil).GetMethod("SaveCurrentShaderVariantCollection", BindingFlags.Static | BindingFlags.NonPublic);
		if (method != null)
		{
			string text2 = text.Replace("\\", "/");
			method.Invoke(null, new object[] { text2 });
		}
		return text;
	}

	// Token: 0x0400016F RID: 367
	public static string SUFFIX = ".shadervariants";
}
