using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class ParaPathDefine
{
	// Token: 0x04000125 RID: 293
	public static string kGeneratePath = Application.dataPath.Replace("Assets", "Generate");

	// Token: 0x04000126 RID: 294
	public static string kTemGeneratePath = Application.dataPath.Replace("Assets", "TmpGenerate");

	// Token: 0x04000127 RID: 295
	public static string kSdkDownLoadPath = Application.dataPath.Replace("Assets", "ParaDownLoad/Resources");

	// Token: 0x04000128 RID: 296
	public static string kPackageShaderFileBasePath = "Assets/ParaSDK";

	// Token: 0x04000129 RID: 297
	public static string kPackageShaderFilePath = "Assets/ParaSDK/ParaPackageJson/";

	// Token: 0x0400012A RID: 298
	public static string kPackageShaderFileName = "para_package.json";

	// Token: 0x0400012B RID: 299
	public static string kParaAvatarPath = Application.dataPath + "/ParaAvatar/";

	// Token: 0x0400012C RID: 300
	public static string kParaWorldPath = Application.dataPath + "/ParaWorld/";

	// Token: 0x0400012D RID: 301
	public static string kEditorWarningTexturePath = Application.dataPath.Replace("Assets", "Packages/com.para.common/Config/Texture/EditorWarningFrame.png");

	// Token: 0x0400012E RID: 302
	public static string kEditorErrorTexturePath = Application.dataPath.Replace("Assets", "Packages/com.para.common/Config/Texture/EditorErrorFrame.png");

	// Token: 0x0400012F RID: 303
	public static string kAvatarPrefabPath = "Assets/ParaAvatar/Avatar_JobID.prefab";

	// Token: 0x04000130 RID: 304
	public static string kWorldPrefabPath = "Assets/ParaWorld/World_JobID.unity";

	// Token: 0x04000131 RID: 305
	public static string kWorldPrefabName = "World_JobID";
}
