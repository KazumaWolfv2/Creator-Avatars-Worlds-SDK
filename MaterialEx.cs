using System;
using UnityEditor;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000062 RID: 98
	public static class MaterialEx
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000A37C File Offset: 0x0000857C
		public static void CopyPropertiesFrom(this Material to, Material from)
		{
			int propertyCount = ShaderUtil.GetPropertyCount(from.shader);
			for (int i = 0; i < propertyCount; i++)
			{
				ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(from.shader, i);
				string propertyName = ShaderUtil.GetPropertyName(from.shader, i);
				switch (propertyType)
				{
				case 0:
					to.SetColor(propertyName, from.GetColor(propertyName));
					break;
				case 1:
					to.SetVector(propertyName, from.GetVector(propertyName));
					break;
				case 2:
					to.SetFloat(propertyName, from.GetFloat(propertyName));
					break;
				case 3:
					to.SetFloat(propertyName, from.GetFloat(propertyName));
					break;
				case 4:
					to.SetTexture(propertyName, from.GetTexture(propertyName));
					to.SetTextureOffset(propertyName, from.GetTextureOffset(propertyName));
					to.SetTextureScale(propertyName, from.GetTextureScale(propertyName));
					break;
				}
			}
			to.renderQueue = from.renderQueue;
			to.globalIlluminationFlags = from.globalIlluminationFlags;
			to.shaderKeywords = from.shaderKeywords;
			foreach (string text in to.shaderKeywords)
			{
				to.EnableKeyword(text);
			}
			to.enableInstancing = from.enableInstancing;
			EditorUtility.SetDirty(to);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A4A8 File Offset: 0x000086A8
		public static void CopyPropertiesPara(this Material to, Material from)
		{
			if (from.HasProperty(MaterialEx.BaseTex))
			{
				to.SetTexture(MaterialEx.BaseMap, from.GetTexture(MaterialEx.BaseTex));
			}
			if (from.HasProperty(MaterialEx.MainTex))
			{
				to.SetTexture(MaterialEx.BaseMap, from.GetTexture(MaterialEx.MainTex));
			}
			if (from.HasProperty(MaterialEx.BaseColor))
			{
				to.SetColor(MaterialEx.BaseColor, from.GetColor(MaterialEx.BaseColor));
			}
		}

		// Token: 0x04000216 RID: 534
		private static readonly int MainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x04000217 RID: 535
		private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

		// Token: 0x04000218 RID: 536
		private static readonly int BaseTex = Shader.PropertyToID("_BaseTex");

		// Token: 0x04000219 RID: 537
		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
	}
}
