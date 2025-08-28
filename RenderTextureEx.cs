using System;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000066 RID: 102
	public static class RenderTextureEx
	{
		// Token: 0x06000210 RID: 528 RVA: 0x0000AA78 File Offset: 0x00008C78
		public static RenderTexture GetTemporary(RenderTexture renderTexture)
		{
			return RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
		}
	}
}
