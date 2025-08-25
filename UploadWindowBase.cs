using System;
using UnityEditor;

// Token: 0x02000081 RID: 129
public class UploadWindowBase : EditorWindow
{
	// Token: 0x020000D9 RID: 217
	public struct ErrorInfo
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x000265A7 File Offset: 0x000247A7
		public ErrorInfo(int code, string message)
		{
			this.code = code;
			this.message = message;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000265B7 File Offset: 0x000247B7
		public string GetErrorMessage()
		{
			if (string.IsNullOrEmpty(this.message))
			{
				return this.code.ToString() + ": Unknown error";
			}
			return this.message;
		}

		// Token: 0x04000406 RID: 1030
		private int code;

		// Token: 0x04000407 RID: 1031
		private string message;
	}
}
