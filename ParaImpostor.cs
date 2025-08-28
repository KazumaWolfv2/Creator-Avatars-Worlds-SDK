using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace ParaImpostors
{
	// Token: 0x0200006E RID: 110
	public class ParaImpostor : MonoBehaviour
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000B9F1 File Offset: 0x00009BF1
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000B9F9 File Offset: 0x00009BF9
		public ParaImpostorAsset Data
		{
			get
			{
				return this.m_data;
			}
			set
			{
				this.m_data = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000BA02 File Offset: 0x00009C02
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000BA0A File Offset: 0x00009C0A
		public Transform RootTransform
		{
			get
			{
				return this.m_rootTransform;
			}
			set
			{
				this.m_rootTransform = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000BA13 File Offset: 0x00009C13
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000BA1B File Offset: 0x00009C1B
		public LODGroup LodGroup
		{
			get
			{
				return this.m_lodGroup;
			}
			set
			{
				this.m_lodGroup = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000BA24 File Offset: 0x00009C24
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000BA2C File Offset: 0x00009C2C
		public Renderer[] Renderers
		{
			get
			{
				return this.m_renderers;
			}
			set
			{
				this.m_renderers = value;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000BA38 File Offset: 0x00009C38
		private void GenerateTextures(List<TextureOutput> outputList, bool standardRendering)
		{
			this.m_rtGBuffers = new RenderTexture[outputList.Count];
			if (standardRendering && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
			{
				GraphicsFormat graphicsFormat = 4;
				GraphicsFormat graphicsFormat2 = 8;
				GraphicsFormat graphicsFormat3 = 48;
				this.m_rtGBuffers[0] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat);
				this.m_rtGBuffers[0].Create();
				this.m_rtGBuffers[1] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat2);
				this.m_rtGBuffers[1].Create();
				this.m_rtGBuffers[2] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat2);
				this.m_rtGBuffers[2].Create();
				this.m_rtGBuffers[3] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat3);
				this.m_rtGBuffers[3].Create();
				this.m_rtGBuffers[4] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat2);
				this.m_rtGBuffers[4].Create();
			}
			else
			{
				for (int i = 0; i < this.m_rtGBuffers.Length; i++)
				{
					this.m_rtGBuffers[i] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, outputList[i].SRGB ? 0 : 2);
					this.m_rtGBuffers[i].Create();
				}
			}
			this.m_trueDepth = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, 1);
			this.m_trueDepth.Create();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000BC54 File Offset: 0x00009E54
		private void GenerateAlphaTextures(List<TextureOutput> outputList)
		{
			this.m_alphaGBuffers = new RenderTexture[outputList.Count];
			for (int i = 0; i < this.m_alphaGBuffers.Length; i++)
			{
				this.m_alphaGBuffers[i] = new RenderTexture(256, 256, 16, outputList[i].SRGB ? 0 : 2);
				this.m_alphaGBuffers[i].Create();
			}
			this.m_trueDepth = new RenderTexture(256, 256, 16, 1);
			this.m_trueDepth.Create();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000BCE4 File Offset: 0x00009EE4
		private void ClearBuffers()
		{
			RenderTexture.active = null;
			RenderTexture[] rtGBuffers = this.m_rtGBuffers;
			for (int i = 0; i < rtGBuffers.Length; i++)
			{
				rtGBuffers[i].Release();
			}
			this.m_rtGBuffers = null;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000BD1C File Offset: 0x00009F1C
		private void ClearAlphaBuffers()
		{
			RenderTexture.active = null;
			RenderTexture[] alphaGBuffers = this.m_alphaGBuffers;
			for (int i = 0; i < alphaGBuffers.Length; i++)
			{
				alphaGBuffers[i].Release();
			}
			this.m_alphaGBuffers = null;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000BD53 File Offset: 0x00009F53
		public static AISRPBaseline CurrentURPBaseline
		{
			get
			{
				return ParaImpostor.m_currentURPBaseline;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000BD5A File Offset: 0x00009F5A
		public static AISRPBaseline CurrentHDRPBaseline
		{
			get
			{
				return ParaImpostor.m_currentHDRPBaseline;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000BD61 File Offset: 0x00009F61
		public static int PackageURPVersion
		{
			get
			{
				return ParaImpostor.m_packageURPVersion;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000BD68 File Offset: 0x00009F68
		public static int PackageHDRPVersion
		{
			get
			{
				return ParaImpostor.m_packageHDRPVersion;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000BD70 File Offset: 0x00009F70
		private static int PackageVersionStringToCode(string version, out int major, out int minor, out int patch)
		{
			MatchCollection matchCollection = Regex.Matches(version, ParaImpostor.SemVerPattern, RegexOptions.Multiline);
			bool flag = matchCollection.Count > 0 && matchCollection[0].Groups.Count >= 4;
			major = (flag ? int.Parse(matchCollection[0].Groups[1].Value) : 99);
			minor = (flag ? int.Parse(matchCollection[0].Groups[2].Value) : 99);
			patch = (flag ? int.Parse(matchCollection[0].Groups[3].Value) : 99);
			return major * 10000 + minor * 100 + patch;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000BE2E File Offset: 0x0000A02E
		private static int PackageVersionElementsToCode(int major, int minor, int patch)
		{
			return major * 10000 + minor * 100 + patch;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000BE3E File Offset: 0x0000A03E
		public void CheckSRPVerionAndApply()
		{
			this.m_packageListRequest = Client.List(true);
			EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.delayCall, new EditorApplication.CallbackFunction(this.ApplySRP));
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000BE6C File Offset: 0x0000A06C
		public void ApplySRP()
		{
			if (this.m_packageListRequest == null || !this.m_packageListRequest.IsCompleted)
			{
				EditorApplication.delayCall = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.delayCall, new EditorApplication.CallbackFunction(this.ApplySRP));
				return;
			}
			ParaImpostor.m_packageURPVersion = 0;
			ParaImpostor.m_packageHDRPVersion = 0;
			foreach (PackageInfo packageInfo in this.m_packageListRequest.Result)
			{
				int num2;
				int num3;
				int num4;
				int num = ParaImpostor.PackageVersionStringToCode(packageInfo.version, out num2, out num3, out num4);
				int num5 = ParaImpostor.PackageVersionElementsToCode(num2, 0, 0);
				if (packageInfo.name.Equals("com.unity.render-pipelines.universal"))
				{
					ParaImpostor.m_currentURPBaseline = (AISRPBaseline)num5;
					ParaImpostor.m_packageURPVersion = (ParaImpostor.m_srpPackageSupport.Contains(num5) ? num : 0);
				}
				if (packageInfo.name.Equals("com.unity.render-pipelines.high-definition"))
				{
					ParaImpostor.m_currentHDRPBaseline = (AISRPBaseline)num5;
					ParaImpostor.m_packageHDRPVersion = (ParaImpostor.m_srpPackageSupport.Contains(num5) ? num : 0);
				}
			}
			string text = AssetDatabase.GUIDToAssetPath("806d6cc0f22ee994f8cd901b6718f08d");
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(text) && File.Exists(text))
			{
				StreamReader streamReader = null;
				try
				{
					streamReader = new StreamReader(text);
					text2 = streamReader.ReadToEnd();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
			}
			bool flag = false;
			Match match = Regex.Match(text2, "#define AI_HDRP_VERSION (\\d*)", RegexOptions.Multiline);
			if (match.Success)
			{
				string value = match.Groups[1].Value;
				int num6 = ParaImpostor.m_packageHDRPVersion;
				if (value != num6.ToString())
				{
					string text3 = text2;
					string value2 = match.Groups[0].Value;
					string text4 = "#define AI_HDRP_VERSION ";
					num6 = ParaImpostor.m_packageHDRPVersion;
					text2 = text3.Replace(value2, text4 + num6.ToString());
					flag = true;
				}
			}
			match = Regex.Match(text2, "#define AI_URP_VERSION (\\d*)", RegexOptions.Multiline);
			if (match.Success)
			{
				string value3 = match.Groups[1].Value;
				int num6 = ParaImpostor.m_packageURPVersion;
				if (value3 != num6.ToString())
				{
					string text5 = text2;
					string value4 = match.Groups[0].Value;
					string text6 = "#define AI_URP_VERSION ";
					num6 = ParaImpostor.m_packageURPVersion;
					text2 = text5.Replace(value4, text6 + num6.ToString());
					flag = true;
				}
			}
			if (flag)
			{
				this.SaveCginc(text2, text);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		private void SaveCginc(string file, string path)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			try
			{
				streamWriter.Write(file);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				streamWriter.Close();
			}
			AssetDatabase.Refresh();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C120 File Offset: 0x0000A320
		public void CheckHDRPMaterial()
		{
			if (this.m_renderPipelineInUse != RenderPipelineInUse.HDRP)
			{
				return;
			}
			if (this.m_data == null || this.m_data.Preset == null || this.m_data.Material == null)
			{
				return;
			}
			foreach (KeyValuePair<string, int> keyValuePair in ParaImpostor.m_hdrpStencilCheck)
			{
				if (this.m_data.Material.HasProperty(keyValuePair.Key) && this.m_data.Material.GetInt(keyValuePair.Key) != keyValuePair.Value)
				{
					this.m_data.Material.SetInt(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C204 File Offset: 0x0000A404
		public void RenderToTexture(ref RenderTexture tex, string path, ImageFormat imageFormat, int resizeScale, TextureChannels channels)
		{
			Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
			if (imageFormat == ImageFormat.EXR)
			{
				texture2D = new Texture2D((int)this.m_data.TexSize.x / resizeScale, (int)this.m_data.TexSize.y / resizeScale, 20, false);
			}
			else
			{
				texture2D = new Texture2D((int)this.m_data.TexSize.x / resizeScale, (int)this.m_data.TexSize.y / resizeScale, (channels == TextureChannels.RGB) ? 3 : 4, true);
			}
			texture2D.name = Path.GetFileNameWithoutExtension(path);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = tex;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)((int)this.m_data.TexSize.x / resizeScale), (float)((int)this.m_data.TexSize.y / resizeScale)), 0, 0);
			RenderTexture.active = active;
			texture2D.Apply();
			byte[] array;
			switch (imageFormat)
			{
			case ImageFormat.PNG:
				array = ImageConversion.EncodeToPNG(texture2D);
				goto IL_0109;
			case ImageFormat.EXR:
				array = ImageConversion.EncodeToEXR(texture2D, 2);
				goto IL_0109;
			}
			array = texture2D.EncodeToTGA(Texture2DEx.Compression.RLE);
			IL_0109:
			if (imageFormat == ImageFormat.EXR)
			{
				File.WriteAllBytes(path, array);
				Object.DestroyImmediate(texture2D);
				return;
			}
			int num = array.Length;
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 65536, false);
			int num2 = 0;
			do
			{
				int num3 = Math.Min(65536, num - num2);
				fileStream.Write(array, num2, num3);
				num2 += num3;
			}
			while (num2 < num);
			fileStream.Close();
			fileStream.Dispose();
			Object.DestroyImmediate(texture2D);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C37C File Offset: 0x0000A57C
		public void ChangeTextureImporter(ref RenderTexture tex, string path, bool sRGB = true, bool changeResolution = false, TextureCompression compression = TextureCompression.Normal, bool alpha = true)
		{
			Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			if (textureImporter != null && ((textureImporter.alphaSource == 1 && !alpha) || textureImporter.textureCompression != compression || textureImporter.sRGBTexture != sRGB || (changeResolution && textureImporter.maxTextureSize != (int)this.m_data.TexSize.x)))
			{
				textureImporter.sRGBTexture = sRGB;
				textureImporter.alphaSource = (alpha ? 1 : 0);
				textureImporter.textureCompression = compression;
				if (changeResolution)
				{
					textureImporter.maxTextureSize = (int)this.m_data.TexSize.x;
				}
				EditorUtility.SetDirty(textureImporter);
				EditorUtility.SetDirty(texture2D);
				textureImporter.SaveAndReimport();
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C430 File Offset: 0x0000A630
		public void CalculateSheetBounds(ImpostorType impostorType)
		{
			this.m_xyFitSize = 0f;
			this.m_depthFitSize = 0f;
			int horizontalFrames = this.m_data.HorizontalFrames;
			int num = this.m_data.HorizontalFrames;
			if (impostorType == ImpostorType.Spherical)
			{
				num = this.m_data.HorizontalFrames - 1;
				if (this.m_data.DecoupleAxisFrames)
				{
					num = this.m_data.VerticalFrames - 1;
				}
			}
			for (int i = 0; i < horizontalFrames; i++)
			{
				for (int j = 0; j <= num; j++)
				{
					Bounds bounds = default(Bounds);
					Matrix4x4 cameraRotationMatrix = this.GetCameraRotationMatrix(impostorType, horizontalFrames, num, i, j);
					for (int k = 0; k < this.Renderers.Length; k++)
					{
						if (!(this.Renderers[k] == null) && this.Renderers[k].enabled && this.Renderers[k].shadowCastingMode != 3)
						{
							Bounds bounds2 = SkinEx.ComputeRendererBounds(this.Renderers[k], this.m_rootTransform.worldToLocalMatrix);
							if (bounds.size == Vector3.zero)
							{
								bounds = bounds2;
							}
							else
							{
								bounds.Encapsulate(bounds2);
							}
						}
					}
					if (i == 0 && j == 0)
					{
						this.m_originalBound = bounds;
					}
					bounds = bounds.Transform(cameraRotationMatrix);
					this.m_xyFitSize = Mathf.Max(new float[]
					{
						this.m_xyFitSize,
						bounds.size.x,
						bounds.size.y
					});
					this.m_depthFitSize = Mathf.Max(this.m_depthFitSize, bounds.size.z);
				}
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		public void DilateRenderTextureUsingMask(ref RenderTexture mainTex, ref RenderTexture maskTex, int pixelBleed, bool alpha, Material dilateMat = null)
		{
			if (pixelBleed == 0)
			{
				return;
			}
			bool flag = false;
			if (dilateMat == null)
			{
				flag = true;
				dilateMat = new Material(AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("57c23892d43bc9f458360024c5985405")));
			}
			RenderTexture temporary = RenderTexture.GetTemporary(mainTex.width, mainTex.height, mainTex.depth, mainTex.format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(maskTex.width, maskTex.height, maskTex.depth, maskTex.format);
			RenderTexture temporary3 = RenderTexture.GetTemporary(maskTex.width, maskTex.height, maskTex.depth, maskTex.format);
			Graphics.Blit(maskTex, temporary3);
			for (int i = 0; i < pixelBleed; i++)
			{
				dilateMat.SetTexture("_MaskTex", temporary3);
				Graphics.Blit(mainTex, temporary, dilateMat, alpha ? 1 : 0);
				Graphics.Blit(temporary, mainTex);
				Graphics.Blit(temporary3, temporary2, dilateMat, 1);
				Graphics.Blit(temporary2, temporary3);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
			if (flag)
			{
				Object.DestroyImmediate(dilateMat);
				dilateMat = null;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
		public void PackingRemapping(ref RenderTexture src, ref RenderTexture dst, int passIndex, Material packerMat = null, Texture extraTex = null, string texName = null)
		{
			bool flag = false;
			if (packerMat == null)
			{
				flag = true;
				packerMat = new Material(AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("31bd3cd74692f384a916d9d7ea87710d")));
			}
			if (extraTex != null)
			{
				if (string.IsNullOrEmpty(texName))
				{
					packerMat.SetTexture("_A", extraTex);
				}
				else
				{
					packerMat.SetTexture(texName, extraTex);
				}
			}
			if (src == dst)
			{
				int width = src.width;
				int height = src.height;
				int depth = src.depth;
				RenderTextureFormat format = src.format;
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, depth, format);
				Graphics.Blit(src, temporary, packerMat, passIndex);
				Graphics.Blit(temporary, dst);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else
			{
				Graphics.Blit(src, dst, packerMat, passIndex);
			}
			if (flag)
			{
				Object.DestroyImmediate(packerMat);
				packerMat = null;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
		private void CopyTransform()
		{
			this.m_oriPos = this.RootTransform.position;
			this.m_oriRot = this.RootTransform.rotation;
			this.m_oriSca = this.RootTransform.localScale;
			this.RootTransform.position = Vector3.zero;
			this.RootTransform.rotation = Quaternion.identity;
			this.RootTransform.localScale = Vector3.one;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000C810 File Offset: 0x0000AA10
		private void PasteTransform()
		{
			this.RootTransform.position = this.m_oriPos;
			this.RootTransform.rotation = this.m_oriRot;
			this.RootTransform.localScale = this.m_oriSca;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000C848 File Offset: 0x0000AA48
		public void CalculatePixelBounds(int targetAmount)
		{
			bool sRGBWrite = GL.sRGBWrite;
			this.CalculateSheetBounds(this.m_data.ImpostorType);
			this.GenerateAlphaTextures(this.m_data.Preset.Output);
			GL.sRGBWrite = true;
			this.m_pixelOffset = Vector2.zero;
			this.CopyTransform();
			try
			{
				this.RenderImpostor(this.m_data.ImpostorType, this.m_data.Preset.Output.Count, false, true, true, this.m_data.Preset.BakeShader);
				this.PasteTransform();
			}
			catch (Exception ex)
			{
				this.PasteTransform();
				EditorUtility.ClearProgressBar();
				throw ex;
			}
			GL.sRGBWrite = sRGBWrite;
			bool flag = this.m_data.Preset.BakeShader == null;
			int num = this.m_data.Preset.AlphaIndex;
			if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
			{
				num = 3;
			}
			else if (flag)
			{
				num = 2;
			}
			Material material = new Material(AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("31bd3cd74692f384a916d9d7ea87710d")));
			if (this.m_renderPipelineInUse == RenderPipelineInUse.HDRP && flag)
			{
				RenderTexture temporary = RenderTextureEx.GetTemporary(this.m_alphaGBuffers[3]);
				Graphics.Blit(this.m_alphaGBuffers[3], temporary);
				material.SetTexture("_A", temporary);
				Graphics.Blit(this.m_trueDepth, this.m_alphaGBuffers[3], material, 11);
				RenderTexture.ReleaseTemporary(temporary);
				this.m_trueDepth.Release();
				this.m_trueDepth = null;
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(256, 256, this.m_alphaGBuffers[num].depth, this.m_alphaGBuffers[num].format);
			this.PackingRemapping(ref this.m_alphaGBuffers[num], ref temporary2, 8, material, null, null);
			Object.DestroyImmediate(material);
			this.ClearAlphaBuffers();
			RenderTexture.active = temporary2;
			Texture2D texture2D = new Texture2D(temporary2.width, temporary2.height, 20, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary2.width, (float)temporary2.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(temporary2);
			Rect rect;
			rect..ctor(0f, 0f, (float)texture2D.width, (float)texture2D.height);
			Vector2[][] array;
			SpriteUtilityEx.GenerateOutline(texture2D, rect, 0.2f, 0, false, out array);
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num2 += array[i].Length;
			}
			Vector2[] array2 = new Vector2[num2];
			int num3 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				for (int k = 0; k < array[j].Length; k++)
				{
					array2[num3] = array[j][k] + new Vector2((float)texture2D.width * 0.5f, (float)texture2D.height * 0.5f);
					array2[num3] = Vector2.Scale(array2[num3], new Vector2(1f / (float)texture2D.width, 1f / (float)texture2D.height));
					num3++;
				}
			}
			Vector2 one = Vector2.one;
			Vector2 zero = Vector2.zero;
			for (int l = 0; l < array2.Length; l++)
			{
				one.x = Mathf.Min(array2[l].x, one.x);
				one.y = Mathf.Min(array2[l].y, one.y);
				zero.x = Mathf.Max(array2[l].x, zero.x);
				zero.y = Mathf.Max(array2[l].y, zero.y);
			}
			Vector2 vector = zero - one;
			float num4 = Mathf.Max(vector.x, vector.y);
			Vector2 vector2 = one + vector * 0.5f;
			this.m_pixelOffset = (vector2 - Vector2.one * 0.5f) * this.m_xyFitSize;
			this.m_xyFitSize *= num4;
			this.m_depthFitSize *= num4;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000CC94 File Offset: 0x0000AE94
		public void RenderCombinedAlpha(ParaImpostorAsset data = null)
		{
			ParaImpostorAsset data2 = this.m_data;
			if (data != null)
			{
				this.m_data = data;
			}
			this.CalculatePixelBounds(this.m_data.Preset.Output.Count);
			this.GenerateAlphaTextures(this.m_data.Preset.Output);
			bool sRGBWrite = GL.sRGBWrite;
			GL.sRGBWrite = true;
			this.CopyTransform();
			try
			{
				this.RenderImpostor(this.m_data.ImpostorType, this.m_data.Preset.Output.Count, false, true, false, this.m_data.Preset.BakeShader);
				this.PasteTransform();
			}
			catch (Exception ex)
			{
				this.PasteTransform();
				EditorUtility.ClearProgressBar();
				throw ex;
			}
			GL.sRGBWrite = sRGBWrite;
			bool flag = this.m_data.Preset.BakeShader == null;
			int num = this.m_data.Preset.AlphaIndex;
			if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
			{
				num = 3;
			}
			else if (flag)
			{
				num = 2;
			}
			Material material = new Material(AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("31bd3cd74692f384a916d9d7ea87710d")));
			if (this.m_renderPipelineInUse == RenderPipelineInUse.HDRP && flag)
			{
				RenderTexture temporary = RenderTextureEx.GetTemporary(this.m_alphaGBuffers[3]);
				Graphics.Blit(this.m_alphaGBuffers[3], temporary);
				material.SetTexture("_A", temporary);
				Graphics.Blit(this.m_trueDepth, this.m_alphaGBuffers[3], material, 11);
				RenderTexture.ReleaseTemporary(temporary);
				this.m_trueDepth.Release();
				this.m_trueDepth = null;
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(256, 256, this.m_alphaGBuffers[num].depth, this.m_alphaGBuffers[num].format);
			this.PackingRemapping(ref this.m_alphaGBuffers[num], ref temporary2, 8, material, null, null);
			Object.DestroyImmediate(material);
			this.ClearAlphaBuffers();
			RenderTexture.active = temporary2;
			this.m_alphaTex = new Texture2D(temporary2.width, temporary2.height, 20, false);
			this.m_alphaTex.ReadPixels(new Rect(0f, 0f, (float)temporary2.width, (float)temporary2.height), 0, 0);
			this.m_alphaTex.Apply();
			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(temporary2);
			this.m_data = data2;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000CEDC File Offset: 0x0000B0DC
		public void CreateAssetFile(ParaImpostorAsset data = null)
		{
			string text = this.OpenFolderForImpostor();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string text2 = this.m_impostorName;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = this.m_rootTransform.name + "_Impostor";
			}
			text = text.TrimEnd(new char[] { '/', '*', '.', ' ' });
			text += "/";
			text = text.TrimStart(new char[] { '/', '*', '.', ' ' });
			if (this.m_data == null)
			{
				Undo.RegisterCompleteObjectUndo(this, "Create Impostor Asset");
				ParaImpostorAsset paraImpostorAsset = AssetDatabase.LoadAssetAtPath<ParaImpostorAsset>(text + text2 + ".asset");
				if (paraImpostorAsset != null)
				{
					this.m_data = paraImpostorAsset;
					return;
				}
				this.m_data = ScriptableObject.CreateInstance<ParaImpostorAsset>();
				AssetDatabase.CreateAsset(this.m_data, text + text2 + ".asset");
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		private void DisplayProgress(float progress, string message)
		{
			if (!Application.isPlaying)
			{
				EditorUtility.DisplayProgressBar("Baking Impostor", message, progress);
				if (progress >= 1f)
				{
					EditorUtility.ClearProgressBar();
				}
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		public void DetectRenderPipeline()
		{
			string text = string.Empty;
			try
			{
				text = RenderPipelineManager.currentPipeline.ToString();
			}
			catch (Exception)
			{
				text = "";
			}
			if (text.Contains("UniversalRenderPipeline"))
			{
				this.m_renderPipelineInUse = RenderPipelineInUse.URP;
			}
			else if (text.Contains("HDRenderPipeline"))
			{
				this.m_renderPipelineInUse = RenderPipelineInUse.HDRP;
			}
			else if (text.Equals(""))
			{
				this.m_renderPipelineInUse = RenderPipelineInUse.None;
			}
			else
			{
				this.m_renderPipelineInUse = RenderPipelineInUse.Custom;
			}
			this.m_renderPipelineInUse = RenderPipelineInUse.URP;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D068 File Offset: 0x0000B268
		public GameObject RenderAllDeferredGroups(ParaImpostorAsset data = null)
		{
			string text = this.m_folderPath;
			if (this.m_data == null)
			{
				text = this.OpenFolderForImpostor();
			}
			else
			{
				this.m_impostorName = this.m_data.name;
				text = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this.m_data)).Replace("\\", "/") + "/";
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			this.DisplayProgress(0f, "Please Wait... Setting up");
			string text2 = this.m_impostorName;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = this.m_rootTransform.name + "_Impostor";
			}
			this.m_folderPath = text;
			text = text.TrimEnd(new char[] { '/', '*', '.', ' ' });
			text += "/";
			text = text.TrimStart(new char[] { '/', '*', '.', ' ' });
			this.m_impostorName = text2;
			Undo.RegisterCompleteObjectUndo(this, "Create Impostor");
			this.DetectRenderPipeline();
			if (this.m_data == null)
			{
				AssetDatabase.LoadAssetAtPath<ParaImpostorAsset>(text + text2 + ".asset");
				this.m_data = ScriptableObject.CreateInstance<ParaImpostorAsset>();
				AssetDatabase.CreateAsset(this.m_data, text + text2 + ".asset");
				if (data != null)
				{
					this.m_data.ShapePoints = data.ShapePoints;
				}
			}
			else if (data != null)
			{
				this.m_data = data;
			}
			bool sRGBWrite = GL.sRGBWrite;
			GL.sRGBWrite = true;
			if (!this.m_data.DecoupleAxisFrames)
			{
				this.m_data.HorizontalFrames = this.m_data.VerticalFrames;
			}
			if (this.m_data.Preset == null)
			{
				if (this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
				{
					this.m_data.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("47b6b3dcefe0eaf4997acf89caf8c75e"));
				}
				else if (this.m_renderPipelineInUse == RenderPipelineInUse.URP)
				{
					this.m_data.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("0403878495ffa3c4e9d4bcb3eac9b559"));
				}
				else
				{
					this.m_data.Preset = AssetDatabase.LoadAssetAtPath<ParaImpostorBakePreset>(AssetDatabase.GUIDToAssetPath("e4786beb7716da54dbb02a632681cc37"));
				}
			}
			bool flag = false;
			if (this.m_data.Preset.BakeShader == null)
			{
				flag = true;
			}
			List<TextureOutput> list = new List<TextureOutput>();
			for (int i2 = 0; i2 < this.m_data.Preset.Output.Count; i2++)
			{
				list.Add(this.m_data.Preset.Output[i2].Clone());
			}
			int num = 0;
			while (num < this.m_data.OverrideOutput.Count && num < this.m_data.Preset.Output.Count)
			{
				if ((this.m_data.OverrideOutput[num].OverrideMask & OverrideMask.OutputToggle) == OverrideMask.OutputToggle)
				{
					list[this.m_data.OverrideOutput[num].Index].Active = this.m_data.OverrideOutput[num].Active;
				}
				if ((this.m_data.OverrideOutput[num].OverrideMask & OverrideMask.NameSuffix) == OverrideMask.NameSuffix)
				{
					list[this.m_data.OverrideOutput[num].Index].Name = this.m_data.OverrideOutput[num].Name;
				}
				if ((this.m_data.OverrideOutput[num].OverrideMask & OverrideMask.RelativeScale) == OverrideMask.RelativeScale)
				{
					list[this.m_data.OverrideOutput[num].Index].Scale = this.m_data.OverrideOutput[num].Scale;
				}
				if ((this.m_data.OverrideOutput[num].OverrideMask & OverrideMask.ColorSpace) == OverrideMask.ColorSpace)
				{
					list[this.m_data.OverrideOutput[num].Index].SRGB = this.m_data.OverrideOutput[num].SRGB;
				}
				if ((this.m_data.OverrideOutput[num].OverrideMask & OverrideMask.QualityCompression) == OverrideMask.QualityCompression)
				{
					list[this.m_data.OverrideOutput[num].Index].Compression = this.m_data.OverrideOutput[num].Compression;
				}
				if ((this.m_data.OverrideOutput[num].OverrideMask & OverrideMask.FileFormat) == OverrideMask.FileFormat)
				{
					list[this.m_data.OverrideOutput[num].Index].ImageFormat = this.m_data.OverrideOutput[num].ImageFormat;
				}
				num++;
			}
			this.m_fileNames = new string[list.Count];
			string text3 = string.Empty;
			if (this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
			{
				text3 = ((this.m_data.ImpostorType == ImpostorType.Spherical) ? "175c951fec709c44fa2f26b8ab78b8dd" : "56236dc63ad9b7949b63a27f0ad180b3");
			}
			else if (this.m_renderPipelineInUse == RenderPipelineInUse.URP)
			{
				text3 = ((this.m_data.ImpostorType == ImpostorType.Spherical) ? "da79d698f4bf0164e910ad798d07efdf" : "83dd8de9a5c14874884f9012def4fdcc");
			}
			else
			{
				text3 = ((this.m_data.ImpostorType == ImpostorType.Spherical) ? "e82933f4c0eb9ba42aab0739f48efe21" : "572f9be5706148142b8da6e9de53acdb");
			}
			this.CalculatePixelBounds(list.Count);
			this.DisplayProgress(0.1f, "Please Wait... Allocating Resources");
			this.GenerateTextures(list, flag);
			this.DisplayProgress(0.2f, "Please Wait... Baking");
			this.CopyTransform();
			try
			{
				this.RenderImpostor(this.m_data.ImpostorType, list.Count, true, false, true, this.m_data.Preset.BakeShader);
				this.PasteTransform();
			}
			catch (Exception ex)
			{
				this.PasteTransform();
				EditorUtility.ClearProgressBar();
				throw ex;
			}
			this.DisplayProgress(0.5f, "Please Wait... Remapping");
			Material material = new Material(AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("31bd3cd74692f384a916d9d7ea87710d")));
			int alphaIndex = this.m_data.Preset.AlphaIndex;
			if (flag)
			{
				if (this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
				{
					this.PackingRemapping(ref this.m_rtGBuffers[2], ref this.m_rtGBuffers[4], 13, material, null, null);
					this.PackingRemapping(ref this.m_rtGBuffers[1], ref this.m_rtGBuffers[1], 10, material, null, null);
					RenderTexture temporary = RenderTextureEx.GetTemporary(this.m_rtGBuffers[3]);
					Graphics.Blit(this.m_rtGBuffers[3], temporary);
					material.SetTexture("_A", temporary);
					Graphics.Blit(this.m_trueDepth, this.m_rtGBuffers[3], material, 11);
					RenderTexture.ReleaseTemporary(temporary);
					RenderTexture temporary2 = RenderTextureEx.GetTemporary(this.m_rtGBuffers[0]);
					RenderTexture temporary3 = RenderTextureEx.GetTemporary(this.m_rtGBuffers[3]);
					material.SetTexture("_A", this.m_rtGBuffers[3]);
					Graphics.Blit(this.m_rtGBuffers[0], temporary2, material, 4);
					material.SetTexture("_A", this.m_rtGBuffers[0]);
					Graphics.Blit(this.m_rtGBuffers[3], temporary3, material, 4);
					Graphics.Blit(temporary2, this.m_rtGBuffers[0]);
					Graphics.Blit(temporary3, this.m_rtGBuffers[3]);
					RenderTexture.ReleaseTemporary(temporary2);
					RenderTexture.ReleaseTemporary(temporary3);
					RenderTexture temporary4 = RenderTextureEx.GetTemporary(this.m_rtGBuffers[1]);
					Graphics.Blit(this.m_rtGBuffers[1], temporary4);
					Graphics.Blit(this.m_rtGBuffers[2], this.m_rtGBuffers[1]);
					Graphics.Blit(temporary4, this.m_rtGBuffers[2]);
					RenderTexture.ReleaseTemporary(temporary4);
					RenderTexture temporary5 = RenderTextureEx.GetTemporary(this.m_rtGBuffers[1]);
					RenderTexture temporary6 = RenderTextureEx.GetTemporary(this.m_rtGBuffers[2]);
					material.SetTexture("_A", this.m_rtGBuffers[2]);
					Graphics.Blit(this.m_rtGBuffers[1], temporary5, material, 4);
					material.SetTexture("_A", this.m_rtGBuffers[1]);
					Graphics.Blit(this.m_rtGBuffers[2], temporary6, material, 4);
					Graphics.Blit(temporary5, this.m_rtGBuffers[1]);
					Graphics.Blit(temporary6, this.m_rtGBuffers[2]);
					RenderTexture.ReleaseTemporary(temporary5);
					RenderTexture.ReleaseTemporary(temporary6);
					this.PackingRemapping(ref this.m_rtGBuffers[2], ref this.m_rtGBuffers[2], 0, material, this.m_trueDepth, null);
					this.m_trueDepth.Release();
					this.m_trueDepth = null;
				}
				else
				{
					RenderTexture temporary7 = RenderTexture.GetTemporary(this.m_rtGBuffers[0].width, this.m_rtGBuffers[0].height, this.m_rtGBuffers[0].depth, this.m_rtGBuffers[0].format);
					RenderTexture temporary8 = RenderTexture.GetTemporary(this.m_rtGBuffers[3].width, this.m_rtGBuffers[3].height, this.m_rtGBuffers[3].depth, this.m_rtGBuffers[3].format);
					material.SetTexture("_A", this.m_rtGBuffers[2]);
					Graphics.Blit(this.m_rtGBuffers[0], temporary7, material, 4);
					material.SetTexture("_A", this.m_rtGBuffers[0]);
					Graphics.Blit(this.m_rtGBuffers[3], temporary8, material, 4);
					Graphics.Blit(temporary7, this.m_rtGBuffers[0]);
					Graphics.Blit(temporary8, this.m_rtGBuffers[3]);
					RenderTexture.ReleaseTemporary(temporary7);
					RenderTexture.ReleaseTemporary(temporary8);
					this.PackingRemapping(ref this.m_rtGBuffers[2], ref this.m_rtGBuffers[2], 0, material, this.m_trueDepth, null);
					this.m_trueDepth.Release();
					this.m_trueDepth = null;
					this.PackingRemapping(ref this.m_rtGBuffers[0], ref this.m_rtGBuffers[0], 5, material, this.m_rtGBuffers[1], null);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].ImageFormat == ImageFormat.TGA)
				{
					this.PackingRemapping(ref this.m_rtGBuffers[j], ref this.m_rtGBuffers[j], 6, material, null, null);
				}
			}
			if (this.m_data.PixelPadding > 0)
			{
				this.DisplayProgress(0.55f, "Please Wait... Dilating");
			}
			Material material2 = new Material(AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath("57c23892d43bc9f458360024c5985405")));
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].Active)
				{
					this.DilateRenderTextureUsingMask(ref this.m_rtGBuffers[k], ref this.m_rtGBuffers[alphaIndex], this.m_data.PixelPadding, alphaIndex != k, material2);
				}
			}
			Object.DestroyImmediate(material2);
			this.DisplayProgress(0.575f, "Please Wait... Resizing");
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Scale != TextureScale.Full)
				{
					RenderTexture temporary9 = RenderTexture.GetTemporary(this.m_rtGBuffers[l].width / (int)list[l].Scale, this.m_rtGBuffers[l].height / (int)list[l].Scale, this.m_rtGBuffers[l].depth, this.m_rtGBuffers[l].graphicsFormat);
					if (l == 4 && flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
					{
						Graphics.Blit(this.m_rtGBuffers[l], temporary9, material, 14);
					}
					else
					{
						Graphics.Blit(this.m_rtGBuffers[l], temporary9);
					}
					this.m_rtGBuffers[l].Release();
					this.m_rtGBuffers[l] = new RenderTexture(temporary9.width, temporary9.height, this.m_rtGBuffers[l].depth, this.m_rtGBuffers[l].graphicsFormat);
					this.m_rtGBuffers[l].Create();
					Graphics.Blit(temporary9, this.m_rtGBuffers[l]);
					RenderTexture.ReleaseTemporary(temporary9);
				}
			}
			Object.DestroyImmediate(material);
			this.DisplayProgress(0.6f, "Please Wait... Creating Asset and Textures");
			bool flag2 = false;
			if (PrefabUtility.GetPrefabAssetType(base.gameObject) == 1 && PrefabUtility.GetPrefabInstanceHandle(base.gameObject) == null)
			{
				flag2 = true;
			}
			Shader shader;
			if (this.m_data.Preset.RuntimeShader != null)
			{
				shader = this.m_data.Preset.RuntimeShader;
			}
			else
			{
				shader = AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath(text3));
			}
			Material material3 = this.m_data.Material;
			if (material3 == null)
			{
				material3 = new Material(shader);
				material3.name = text2;
				material3.enableInstancing = true;
				this.m_data.Material = material3;
				EditorUtility.SetDirty(material3);
			}
			else
			{
				material3.shader = shader;
				material3.name = text2;
				EditorUtility.SetDirty(material3);
			}
			AssetDatabase.CreateAsset(material3, Path.Join(Path.GetDirectoryName(AssetDatabase.GetAssetPath(this.m_data)), text2 + ".mat"));
			bool flag3 = false;
			this.m_standardFileNames[0] = ImpostorBakingTools.GlobalAlbedoAlpha;
			this.m_standardFileNames[1] = ImpostorBakingTools.GlobalSpecularSmoothness;
			this.m_standardFileNames[2] = ImpostorBakingTools.GlobalNormalDepth;
			this.m_standardFileNames[3] = ImpostorBakingTools.GlobalEmissionOcclusion;
			for (int m = 0; m < list.Count; m++)
			{
				Texture2D texture2D = null;
				this.m_fileNames[m] = string.Empty;
				if (material3.HasProperty(list[m].Name))
				{
					texture2D = material3.GetTexture(list[m].Name) as Texture2D;
				}
				if (texture2D != null)
				{
					this.m_fileNames[m] = AssetDatabase.GetAssetPath(texture2D);
					if (texture2D.width != (int)((TextureScale)this.m_data.TexSize.x / list[m].Scale))
					{
						flag3 = true;
					}
				}
				else
				{
					this.m_fileNames[m] = text;
					ref string ptr = ref this.m_fileNames[m];
					ptr = string.Concat(new string[]
					{
						ptr,
						text2,
						list[m].Name,
						".",
						list[m].ImageFormat.ToString().ToLower()
					});
				}
			}
			int i;
			int n;
			for (i = 0; i < this.m_propertyNames.Length; i = n + 1)
			{
				if (material3.HasProperty(this.m_propertyNames[i]))
				{
					Texture2D texture2D = material3.GetTexture(this.m_propertyNames[i]) as Texture2D;
					if (texture2D != null)
					{
						int num2 = list.FindIndex((TextureOutput x) => x.Name == this.m_standardFileNames[i]);
						if (num2 > -1)
						{
							this.m_fileNames[num2] = AssetDatabase.GetAssetPath(texture2D);
							if (texture2D.width != (int)((TextureScale)this.m_data.TexSize.x / list[num2].Scale))
							{
								flag3 = true;
							}
						}
					}
				}
				n = i;
			}
			bool flag4;
			if (flag3 && EditorPrefs.GetInt(ImpostorBakingTools.PrefGlobalTexImport, 0) == 0)
			{
				flag4 = EditorUtility.DisplayDialog("Resize Textures?", "Do you wish to override the Texture Import settings to match the provided Impostor Texture Size?", "Yes", "No");
			}
			else
			{
				flag4 = EditorPrefs.GetInt(ImpostorBakingTools.PrefGlobalTexImport, 0) == 1;
			}
			if (!Application.isPlaying)
			{
				for (int num3 = 0; num3 < list.Count; num3++)
				{
					if (list[num3].Active)
					{
						this.RenderToTexture(ref this.m_rtGBuffers[num3], this.m_fileNames[num3], list[num3].ImageFormat, (int)list[num3].Scale, list[num3].Channels);
					}
				}
			}
			GL.sRGBWrite = sRGBWrite;
			this.DisplayProgress(0.65f, "Please Wait... Generating Mesh and Material");
			Vector4 vector;
			vector..ctor(this.m_originalBound.center.x, this.m_originalBound.center.y, this.m_originalBound.center.z, 1f);
			Vector4 vector2;
			vector2..ctor(vector.x, vector.y, vector.z, -this.m_pixelOffset.y / this.m_xyFitSize);
			Vector4 vector3;
			vector3..ctor(this.m_xyFitSize, this.m_depthFitSize, this.m_pixelOffset.x / this.m_xyFitSize / (float)this.m_data.HorizontalFrames, this.m_pixelOffset.y / this.m_xyFitSize / (float)this.m_data.VerticalFrames);
			bool flag5 = false;
			Mesh mesh = this.m_data.Mesh;
			if (mesh == null)
			{
				mesh = this.GenerateMesh(this.m_data.ShapePoints, vector2, this.m_xyFitSize, this.m_xyFitSize, true);
				mesh.name = text2;
				this.m_data.Mesh = mesh;
				EditorUtility.SetDirty(mesh);
			}
			else
			{
				Mesh mesh2 = this.GenerateMesh(this.m_data.ShapePoints, vector2, this.m_xyFitSize, this.m_xyFitSize, true);
				EditorUtility.CopySerialized(mesh2, mesh);
				mesh.vertices = mesh2.vertices;
				mesh.triangles = mesh2.triangles;
				mesh.uv = mesh2.uv;
				mesh.normals = mesh2.normals;
				mesh.bounds = mesh2.bounds;
				mesh.name = text2;
				EditorUtility.SetDirty(mesh);
			}
			AssetDatabase.CreateAsset(mesh, Path.Join(Path.GetDirectoryName(AssetDatabase.GetAssetPath(this.m_data)), text2 + "_mesh.asset"));
			GameObject gameObject;
			if (flag2)
			{
				if (this.m_lastImpostor != null && PrefabUtility.GetPrefabAssetType(this.m_lastImpostor) == 1)
				{
					gameObject = this.m_lastImpostor;
				}
				else
				{
					gameObject = new GameObject("Impostor", new Type[]
					{
						typeof(MeshFilter),
						typeof(MeshRenderer)
					});
					flag5 = true;
				}
			}
			else if (this.m_lastImpostor != null)
			{
				gameObject = this.m_lastImpostor;
			}
			else
			{
				gameObject = new GameObject("Impostor", new Type[]
				{
					typeof(MeshFilter),
					typeof(MeshRenderer)
				});
				Undo.RegisterCreatedObjectUndo(gameObject, "Create Impostor");
				gameObject.transform.position = this.m_rootTransform.position;
				gameObject.transform.rotation = this.m_rootTransform.rotation;
				flag5 = true;
			}
			this.m_lastImpostor = gameObject;
			gameObject.transform.localScale = Vector3.one;
			gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
			if (flag5)
			{
				if (this.LodGroup != null)
				{
					if (flag2)
					{
						Object prefabInstanceHandle = PrefabUtility.GetPrefabInstanceHandle((Selection.activeObject as GameObject).transform.root.gameObject);
						GameObject gameObject2 = AssetDatabase.LoadAssetAtPath(text + (Selection.activeObject as GameObject).transform.root.gameObject.name + ".prefab", typeof(GameObject)) as GameObject;
						GameObject gameObject3 = PrefabUtility.InstantiatePrefab(gameObject2) as GameObject;
						ParaImpostor paraImpostor = gameObject3.GetComponentInChildren<ParaImpostor>();
						gameObject.transform.SetParent(paraImpostor.LodGroup.transform);
						paraImpostor.m_lastImpostor = gameObject;
						PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject3, AssetDatabase.GetAssetPath(prefabInstanceHandle), 0);
						paraImpostor = gameObject2.GetComponentInChildren<ParaImpostor>();
						gameObject = paraImpostor.m_lastImpostor;
						Object.DestroyImmediate(gameObject3);
					}
					else
					{
						gameObject.transform.SetParent(this.LodGroup.transform, true);
						gameObject.transform.localScale = Vector3.one;
					}
					switch (this.m_lodReplacement)
					{
					case LODReplacement.ReplaceCulled:
					{
						LOD[] lods = this.LodGroup.GetLODs();
						Array.Resize<LOD>(ref lods, lods.Length + 1);
						LOD lod = default(LOD);
						lod.screenRelativeTransitionHeight = 0f;
						lod.renderers = gameObject.GetComponents<Renderer>();
						lods[lods.Length - 1] = lod;
						this.LodGroup.SetLODs(lods);
						break;
					}
					case LODReplacement.ReplaceLast:
					{
						LOD[] lods2 = this.LodGroup.GetLODs();
						foreach (Renderer renderer in lods2[lods2.Length - 1].renderers)
						{
							if (renderer)
							{
								renderer.enabled = false;
							}
						}
						lods2[lods2.Length - 1].renderers = gameObject.GetComponents<Renderer>();
						this.LodGroup.SetLODs(lods2);
						break;
					}
					case LODReplacement.ReplaceAllExceptFirst:
					{
						LOD[] lods3 = this.LodGroup.GetLODs();
						for (int num4 = lods3.Length - 1; num4 > 0; num4--)
						{
							foreach (Renderer renderer2 in lods3[num4].renderers)
							{
								if (renderer2)
								{
									renderer2.enabled = false;
								}
							}
						}
						float screenRelativeTransitionHeight = lods3[lods3.Length - 1].screenRelativeTransitionHeight;
						Array.Resize<LOD>(ref lods3, 2);
						lods3[lods3.Length - 1].screenRelativeTransitionHeight = screenRelativeTransitionHeight;
						lods3[lods3.Length - 1].renderers = gameObject.GetComponents<Renderer>();
						this.LodGroup.SetLODs(lods3);
						break;
					}
					case LODReplacement.ReplaceSpecific:
					{
						LOD[] lods4 = this.LodGroup.GetLODs();
						foreach (Renderer renderer3 in lods4[this.m_insertIndex].renderers)
						{
							if (renderer3)
							{
								renderer3.enabled = false;
							}
						}
						lods4[this.m_insertIndex].renderers = gameObject.GetComponents<Renderer>();
						this.LodGroup.SetLODs(lods4);
						break;
					}
					case LODReplacement.ReplaceAfterSpecific:
					{
						LOD[] lods5 = this.LodGroup.GetLODs();
						for (int num5 = lods5.Length - 1; num5 > this.m_insertIndex; num5--)
						{
							foreach (Renderer renderer4 in lods5[num5].renderers)
							{
								if (renderer4)
								{
									renderer4.enabled = false;
								}
							}
						}
						float num6 = lods5[lods5.Length - 1].screenRelativeTransitionHeight;
						if (this.m_insertIndex == lods5.Length - 1)
						{
							num6 = 0f;
						}
						Array.Resize<LOD>(ref lods5, 2 + this.m_insertIndex);
						lods5[lods5.Length - 1].screenRelativeTransitionHeight = num6;
						lods5[lods5.Length - 1].renderers = gameObject.GetComponents<Renderer>();
						this.LodGroup.SetLODs(lods5);
						break;
					}
					case LODReplacement.InsertAfter:
					{
						LOD[] lods6 = this.LodGroup.GetLODs();
						Array.Resize<LOD>(ref lods6, lods6.Length + 1);
						for (int num7 = lods6.Length - 1; num7 > this.m_insertIndex; num7--)
						{
							lods6[num7].screenRelativeTransitionHeight = lods6[num7 - 1].screenRelativeTransitionHeight;
							lods6[num7].fadeTransitionWidth = lods6[num7 - 1].fadeTransitionWidth;
							lods6[num7].renderers = lods6[num7 - 1].renderers;
						}
						float num8 = 1f;
						if (this.m_insertIndex > 0)
						{
							num8 = lods6[this.m_insertIndex - 1].screenRelativeTransitionHeight;
						}
						lods6[this.m_insertIndex + 1].renderers = gameObject.GetComponents<Renderer>();
						lods6[this.m_insertIndex].screenRelativeTransitionHeight = (lods6[this.m_insertIndex + 1].screenRelativeTransitionHeight + num8) * 0.5f;
						this.LodGroup.SetLODs(lods6);
						break;
					}
					}
					Undo.RegisterCompleteObjectUndo(this.LodGroup, "Create Impostor");
				}
				else if (!flag2)
				{
					gameObject.transform.SetParent(this.m_rootTransform.parent);
					int siblingIndex = this.m_rootTransform.GetSiblingIndex();
					gameObject.transform.SetSiblingIndex(siblingIndex + 1);
					this.m_rootTransform.SetSiblingIndex(siblingIndex);
					gameObject.transform.localScale = Vector3.one;
				}
			}
			if (this.LodGroup == null)
			{
				Transform parent = gameObject.transform.parent;
				int siblingIndex2 = gameObject.transform.GetSiblingIndex();
				gameObject.transform.SetParent(this.m_rootTransform, true);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.SetParent(parent, true);
				gameObject.transform.SetSiblingIndex(siblingIndex2);
			}
			EditorUtility.SetDirty(this.m_data);
			if (this.m_lastImpostor == null)
			{
				gameObject.name = text2;
			}
			gameObject.GetComponent<Renderer>().sharedMaterial = material3;
			EditorUtility.SetDirty(gameObject);
			this.DisplayProgress(0.7f, "Please Wait... Saving and Importing");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			this.DisplayProgress(0.8f, "Please Wait... Changing Texture Import Settings");
			flag3 = false;
			if (flag)
			{
				for (int num9 = 0; num9 < list.Count; num9++)
				{
					Texture2D texture2D = null;
					if (list[num9].Active)
					{
						if (material3.HasProperty(this.m_propertyNames[num9]))
						{
							texture2D = material3.GetTexture(this.m_propertyNames[num9]) as Texture2D;
						}
						if (texture2D == null)
						{
							texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(this.m_fileNames[num9]);
						}
						if (texture2D != null)
						{
							material3.SetTexture(this.m_propertyNames[num9], texture2D);
						}
						if (texture2D != null && (float)texture2D.width != this.m_data.TexSize.x / (float)list[num9].Scale)
						{
							flag3 = true;
						}
					}
				}
			}
			else
			{
				for (int num10 = 0; num10 < list.Count; num10++)
				{
					Texture2D texture2D = null;
					if (list[num10].Active)
					{
						if (material3.HasProperty(list[num10].Name))
						{
							texture2D = material3.GetTexture(list[num10].Name) as Texture2D;
						}
						if (texture2D == null)
						{
							texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(this.m_fileNames[num10]);
						}
						if (texture2D != null)
						{
							material3.SetTexture(list[num10].Name, texture2D);
						}
						if (texture2D != null && (float)texture2D.width != this.m_data.TexSize.x / (float)list[num10].Scale)
						{
							flag3 = true;
						}
					}
				}
				int i3;
				for (i3 = 0; i3 < this.m_propertyNames.Length; i3 = n + 1)
				{
					Texture2D texture2D = null;
					if (material3.HasProperty(this.m_propertyNames[i3]))
					{
						texture2D = material3.GetTexture(this.m_propertyNames[i3]) as Texture2D;
					}
					if (texture2D == null)
					{
						texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(text + text2 + this.m_standardFileNames[i3] + ".tga");
					}
					if (texture2D == null)
					{
						texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(text + text2 + this.m_standardFileNames[i3] + ".png");
					}
					if (texture2D == null)
					{
						texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(text + text2 + this.m_standardFileNames[i3] + ".exr");
					}
					if (texture2D != null)
					{
						material3.SetTexture(this.m_propertyNames[i3], texture2D);
					}
					if (texture2D != null)
					{
						int num11 = list.FindIndex((TextureOutput x) => x.Name == this.m_standardFileNames[i3]);
						if (num11 > -1 && texture2D.width != (int)((TextureScale)this.m_data.TexSize.x / list[num11].Scale))
						{
							flag3 = true;
						}
					}
					n = i3;
				}
			}
			if (this.m_data.ImpostorType == ImpostorType.HemiOctahedron)
			{
				material3.SetFloat("_Hemi", 1f);
				material3.EnableKeyword("_HEMI_ON");
			}
			else
			{
				material3.SetFloat("_Hemi", 0f);
				material3.DisableKeyword("_HEMI_ON");
			}
			material3.SetFloat("_Frames", (float)this.m_data.HorizontalFrames);
			material3.SetFloat("_ImpostorSize", this.m_xyFitSize);
			material3.SetVector("_Offset", vector2);
			material3.SetFloat("_DepthSize", this.m_depthFitSize);
			material3.SetFloat("_FramesX", (float)this.m_data.HorizontalFrames);
			material3.SetFloat("_FramesY", (float)this.m_data.VerticalFrames);
			material3.SetFloat("_AI_Frames", (float)this.m_data.HorizontalFrames);
			material3.SetFloat("_AI_ImpostorSize", this.m_xyFitSize);
			material3.SetVector("_AI_Offset", vector2);
			material3.SetVector("_AI_SizeOffset", vector3);
			material3.SetFloat("_AI_DepthSize", this.m_depthFitSize);
			material3.SetFloat("_AI_FramesX", (float)this.m_data.HorizontalFrames);
			material3.SetFloat("_AI_FramesY", (float)this.m_data.VerticalFrames);
			this.CheckHDRPMaterial();
			if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
			{
				material3.SetShaderPassEnabled("MotionVectors", true);
			}
			EditorUtility.SetDirty(material3);
			flag4 = flag3 && flag4;
			this.DisplayProgress(1f, "Complete!");
			for (int num12 = 0; num12 < list.Count; num12++)
			{
				if (list[num12].Active)
				{
					this.ChangeTextureImporter(ref this.m_rtGBuffers[num12], this.m_fileNames[num12], list[num12].SRGB, flag4, list[num12].Compression, list[num12].Channels == TextureChannels.RGBA);
				}
			}
			this.ClearBuffers();
			return gameObject;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public void RenderImpostor(ImpostorType impostorType, int targetAmount, bool impostorMaps = true, bool combinedAlphas = false, bool useMinResolution = false, Shader customShader = null)
		{
			if (!impostorMaps && !combinedAlphas)
			{
				return;
			}
			if (targetAmount <= 0)
			{
				return;
			}
			bool flag = customShader == null;
			Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
			CommandBuffer commandBuffer = new CommandBuffer();
			if (impostorMaps)
			{
				commandBuffer.name = "GBufferCatcher";
				RenderTargetIdentifier[] array = new RenderTargetIdentifier[targetAmount];
				for (int i = 0; i < targetAmount; i++)
				{
					array[i] = this.m_rtGBuffers[i];
				}
				commandBuffer.SetRenderTarget(array, this.m_trueDepth);
				commandBuffer.ClearRenderTarget(true, true, Color.clear, 1f);
			}
			CommandBuffer commandBuffer2 = new CommandBuffer();
			if (combinedAlphas)
			{
				commandBuffer2.name = "DepthAlphaCatcher";
				RenderTargetIdentifier[] array2 = new RenderTargetIdentifier[targetAmount];
				for (int j = 0; j < targetAmount; j++)
				{
					array2[j] = this.m_alphaGBuffers[j];
				}
				commandBuffer2.SetRenderTarget(array2, this.m_trueDepth);
				commandBuffer2.ClearRenderTarget(true, true, Color.clear, 1f);
			}
			int horizontalFrames = this.m_data.HorizontalFrames;
			int num = this.m_data.HorizontalFrames;
			if (impostorType == ImpostorType.Spherical)
			{
				num = this.m_data.HorizontalFrames - 1;
				if (this.m_data.DecoupleAxisFrames)
				{
					num = this.m_data.VerticalFrames - 1;
				}
			}
			for (int k = 0; k < horizontalFrames; k++)
			{
				for (int l = 0; l <= num; l++)
				{
					Bounds bounds = default(Bounds);
					Matrix4x4 cameraRotationMatrix = this.GetCameraRotationMatrix(impostorType, horizontalFrames, num, k, l);
					for (int m = 0; m < this.Renderers.Length; m++)
					{
						if (!(this.Renderers[m] == null))
						{
							Bounds bounds2 = SkinEx.ComputeRendererBounds(this.Renderers[m], this.m_rootTransform.worldToLocalMatrix);
							if (bounds.size == Vector3.zero)
							{
								bounds = bounds2;
							}
							else
							{
								bounds.Encapsulate(bounds2);
							}
						}
					}
					if (k == 0 && l == 0)
					{
						this.m_originalBound = bounds;
					}
					bounds = bounds.Transform(cameraRotationMatrix);
					Matrix4x4 matrix4x = cameraRotationMatrix.inverse * Matrix4x4.LookAt(bounds.center - new Vector3(0f, 0f, this.m_depthFitSize * 0.5f), bounds.center, Vector3.up);
					float num2 = this.m_xyFitSize * 0.5f;
					Matrix4x4 matrix4x2 = Matrix4x4.Ortho(-num2 + this.m_pixelOffset.x, num2 + this.m_pixelOffset.x, -num2 + this.m_pixelOffset.y, num2 + this.m_pixelOffset.y, 0f, -this.m_depthFitSize);
					matrix4x = matrix4x.inverse * this.m_rootTransform.worldToLocalMatrix;
					if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
					{
						matrix4x2 = GL.GetGPUProjectionMatrix(matrix4x2, true);
					}
					if (impostorMaps)
					{
						commandBuffer.SetViewProjectionMatrices(matrix4x, matrix4x2);
						commandBuffer.SetViewport(new Rect(this.m_data.TexSize.x / (float)horizontalFrames * (float)k, this.m_data.TexSize.y / (float)(num + ((impostorType == ImpostorType.Spherical) ? 1 : 0)) * (float)l, this.m_data.TexSize.x / (float)this.m_data.HorizontalFrames, this.m_data.TexSize.y / (float)this.m_data.VerticalFrames));
						if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
						{
							commandBuffer.SetGlobalMatrix("_ViewMatrix", matrix4x);
							commandBuffer.SetGlobalMatrix("_InvViewMatrix", matrix4x.inverse);
							commandBuffer.SetGlobalMatrix("_ProjMatrix", matrix4x2);
							commandBuffer.SetGlobalMatrix("_ViewProjMatrix", matrix4x2 * matrix4x);
							commandBuffer.SetGlobalVector("_WorldSpaceCameraPos", Vector4.zero);
						}
					}
					if (combinedAlphas)
					{
						commandBuffer2.SetViewProjectionMatrices(matrix4x, matrix4x2);
						commandBuffer2.SetViewport(new Rect(0f, 0f, 256f, 256f));
						if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HDRP)
						{
							commandBuffer2.SetGlobalMatrix("_ViewMatrix", matrix4x);
							commandBuffer2.SetGlobalMatrix("_InvViewMatrix", matrix4x.inverse);
							commandBuffer2.SetGlobalMatrix("_ProjMatrix", matrix4x2);
							commandBuffer2.SetGlobalMatrix("_ViewProjMatrix", matrix4x2 * matrix4x);
							commandBuffer2.SetGlobalVector("_WorldSpaceCameraPos", Vector4.zero);
						}
					}
					for (int n = 0; n < this.Renderers.Length; n++)
					{
						if (!(this.Renderers[n] == null))
						{
							Material[] sharedMaterials = this.Renderers[n].sharedMaterials;
							for (int num3 = 0; num3 < sharedMaterials.Length; num3++)
							{
								if (!(sharedMaterials[num3] == null))
								{
									Material material = null;
									int num4 = 0;
									int num5;
									if (flag)
									{
										material = sharedMaterials[num3];
										num4 = material.FindPass("DEFERRED");
										if (num4 == -1)
										{
											num4 = material.FindPass("Deferred");
										}
										if (num4 == -1)
										{
											num4 = material.FindPass("GBuffer");
										}
										num5 = material.FindPass("DepthOnly");
										if (num4 == -1)
										{
											num4 = 0;
											for (int num6 = 0; num6 < material.passCount; num6++)
											{
												if (material.GetTag("LightMode", true).Equals("Deferred"))
												{
													num4 = num6;
													break;
												}
											}
										}
										commandBuffer.EnableShaderKeyword("UNITY_HDR_ON");
									}
									else
									{
										num5 = -1;
										if (!dictionary.TryGetValue(sharedMaterials[num3], out material))
										{
											material = new Material(customShader)
											{
												hideFlags = 61
											};
											material.CopyPropertiesPara(sharedMaterials[num3]);
											dictionary.Add(sharedMaterials[num3], material);
										}
									}
									bool flag2 = this.Renderers[n].lightmapIndex > -1;
									bool flag3 = this.Renderers[n].realtimeLightmapIndex > -1;
									if ((flag2 || flag3) && !flag)
									{
										commandBuffer.EnableShaderKeyword("LIGHTMAP_ON");
										if (flag2)
										{
											commandBuffer.SetGlobalVector("unity_LightmapST", this.Renderers[n].lightmapScaleOffset);
										}
										if (flag3)
										{
											commandBuffer.EnableShaderKeyword("DYNAMICLIGHTMAP_ON");
											commandBuffer.SetGlobalVector("unity_DynamicLightmapST", this.Renderers[n].realtimeLightmapScaleOffset);
										}
										else
										{
											commandBuffer.DisableShaderKeyword("DYNAMICLIGHTMAP_ON");
										}
										if (flag2 && flag3)
										{
											commandBuffer.EnableShaderKeyword("DIRLIGHTMAP_COMBINED");
										}
										else
										{
											commandBuffer.DisableShaderKeyword("DIRLIGHTMAP_COMBINED");
										}
									}
									else
									{
										commandBuffer.DisableShaderKeyword("LIGHTMAP_ON");
										commandBuffer.DisableShaderKeyword("DYNAMICLIGHTMAP_ON");
										commandBuffer.DisableShaderKeyword("DIRLIGHTMAP_COMBINED");
									}
									commandBuffer.DisableShaderKeyword("LIGHTPROBE_SH");
									if (impostorMaps)
									{
										if (num5 > -1)
										{
											commandBuffer.DrawRenderer(this.Renderers[n], material, num3, num5);
										}
										commandBuffer.DrawRenderer(this.Renderers[n], material, num3, num4);
									}
									if (combinedAlphas)
									{
										if (num5 > -1)
										{
											commandBuffer2.DrawRenderer(this.Renderers[n], material, num3, num5);
										}
										commandBuffer2.DrawRenderer(this.Renderers[n], material, num3, num4);
									}
								}
							}
						}
					}
					if (impostorMaps)
					{
						Graphics.ExecuteCommandBuffer(commandBuffer);
					}
					if (combinedAlphas)
					{
						Graphics.ExecuteCommandBuffer(commandBuffer2);
					}
				}
			}
			foreach (KeyValuePair<Material, Material> keyValuePair in dictionary)
			{
				Material value = keyValuePair.Value;
				if (value != null)
				{
					if (!Application.isPlaying)
					{
						Object.DestroyImmediate(value);
					}
				}
			}
			dictionary.Clear();
			commandBuffer.Release();
			commandBuffer = null;
			commandBuffer2.Release();
			commandBuffer2 = null;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000F604 File Offset: 0x0000D804
		private Matrix4x4 GetCameraRotationMatrix(ImpostorType impostorType, int hframes, int vframes, int x, int y)
		{
			Matrix4x4 matrix4x = Matrix4x4.identity;
			if (impostorType == ImpostorType.Spherical)
			{
				float num = 0f;
				if (vframes > 0)
				{
					num = -(180f / (float)vframes);
				}
				Quaternion quaternion = Quaternion.Euler(num * (float)y + 90f, 0f, 0f);
				Quaternion quaternion2 = Quaternion.Euler(0f, 360f / (float)hframes * (float)x + -90f, 0f);
				matrix4x = Matrix4x4.Rotate(quaternion * quaternion2);
			}
			else if (impostorType == ImpostorType.Octahedron)
			{
				Vector3 vector = this.OctahedronToVector((float)x / ((float)hframes - 1f) * 2f - 1f, (float)y / ((float)vframes - 1f) * 2f - 1f);
				matrix4x = Matrix4x4.Rotate(Quaternion.LookRotation(new Vector3(vector.x * -1f, vector.z * -1f, vector.y * -1f), Vector3.up)).inverse;
			}
			else if (impostorType == ImpostorType.HemiOctahedron)
			{
				Vector3 vector2 = this.HemiOctahedronToVector((float)x / ((float)hframes - 1f) * 2f - 1f, (float)y / ((float)vframes - 1f) * 2f - 1f);
				matrix4x = Matrix4x4.Rotate(Quaternion.LookRotation(new Vector3(vector2.x * -1f, vector2.z * -1f, vector2.y * -1f), Vector3.up)).inverse;
			}
			return matrix4x;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000F780 File Offset: 0x0000D980
		private Vector3 OctahedronToVector(Vector2 oct)
		{
			Vector3 vector;
			vector..ctor(oct.x, oct.y, 1f - Mathf.Abs(oct.x) - Mathf.Abs(oct.y));
			float num = Mathf.Clamp01(-vector.z);
			vector.Set(vector.x + ((vector.x >= 0f) ? (-num) : num), vector.y + ((vector.y >= 0f) ? (-num) : num), vector.z);
			vector = Vector3.Normalize(vector);
			return vector;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000F814 File Offset: 0x0000DA14
		private Vector3 OctahedronToVector(float x, float y)
		{
			Vector3 vector;
			vector..ctor(x, y, 1f - Mathf.Abs(x) - Mathf.Abs(y));
			float num = Mathf.Clamp01(-vector.z);
			vector.Set(vector.x + ((vector.x >= 0f) ? (-num) : num), vector.y + ((vector.y >= 0f) ? (-num) : num), vector.z);
			vector = Vector3.Normalize(vector);
			return vector;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000F894 File Offset: 0x0000DA94
		private Vector3 HemiOctahedronToVector(float x, float y)
		{
			float num = x;
			float num2 = y;
			x = (num + num2) * 0.5f;
			y = (num - num2) * 0.5f;
			return Vector3.Normalize(new Vector3(x, y, 1f - Mathf.Abs(x) - Mathf.Abs(y)));
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
		public void GenerateAutomaticMesh(ParaImpostorAsset data)
		{
			Rect rect;
			rect..ctor(0f, 0f, (float)this.m_alphaTex.width, (float)this.m_alphaTex.height);
			Vector2[][] array;
			SpriteUtilityEx.GenerateOutline(this.m_alphaTex, rect, data.Tolerance, 254, false, out array);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num += array[i].Length;
			}
			data.ShapePoints = new Vector2[num];
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				for (int k = 0; k < array[j].Length; k++)
				{
					data.ShapePoints[num2] = array[j][k] + new Vector2((float)this.m_alphaTex.width * 0.5f, (float)this.m_alphaTex.height * 0.5f);
					data.ShapePoints[num2] = Vector2.Scale(data.ShapePoints[num2], new Vector2(1f / (float)this.m_alphaTex.width, 1f / (float)this.m_alphaTex.height));
					num2++;
				}
			}
			data.ShapePoints = Vector2Ex.ConvexHull(data.ShapePoints);
			data.ShapePoints = Vector2Ex.ReduceVertices(data.ShapePoints, data.MaxVertices);
			data.ShapePoints = Vector2Ex.ScaleAlongNormals(data.ShapePoints, data.NormalScale);
			for (int l = 0; l < data.ShapePoints.Length; l++)
			{
				data.ShapePoints[l].x = Mathf.Clamp01(data.ShapePoints[l].x);
				data.ShapePoints[l].y = Mathf.Clamp01(data.ShapePoints[l].y);
			}
			data.ShapePoints = Vector2Ex.ConvexHull(data.ShapePoints);
			for (int m = 0; m < data.ShapePoints.Length; m++)
			{
				data.ShapePoints[m] = new Vector2(data.ShapePoints[m].x, 1f - data.ShapePoints[m].y);
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000FB24 File Offset: 0x0000DD24
		public Mesh GenerateMesh(Vector2[] points, Vector3 offset, float width = 1f, float height = 1f, bool invertY = true)
		{
			Vector2[] array = new Vector2[points.Length];
			Vector2[] array2 = new Vector2[points.Length];
			Array.Copy(points, array, points.Length);
			float num = width * 0.5f;
			float num2 = height * 0.5f;
			if (invertY)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Vector2(array[i].x, 1f - array[i].y);
				}
			}
			Array.Copy(array, array2, array.Length);
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = new Vector2(array[j].x * width - num + this.m_pixelOffset.x, array[j].y * height - num2 + this.m_pixelOffset.y);
			}
			Triangulator triangulator = new Triangulator(array);
			int[] array3 = triangulator.Triangulate();
			Vector3[] array4 = new Vector3[triangulator.Points.Count];
			for (int k = 0; k < array4.Length; k++)
			{
				array4[k] = new Vector3(triangulator.Points[k].x, triangulator.Points[k].y, 0f);
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array4;
			mesh.uv = array2;
			mesh.triangles = array3;
			mesh.RecalculateNormals();
			mesh.bounds = new Bounds(offset, this.m_originalBound.size);
			return mesh;
		}

		// Token: 0x04000259 RID: 601
		public const string Preset = "e4786beb7716da54dbb02a632681cc37";

		// Token: 0x0400025A RID: 602
		public const string Shader = "e82933f4c0eb9ba42aab0739f48efe21";

		// Token: 0x0400025B RID: 603
		public const string ShaderOcta = "572f9be5706148142b8da6e9de53acdb";

		// Token: 0x0400025C RID: 604
		public const string PresetHDRP = "47b6b3dcefe0eaf4997acf89caf8c75e";

		// Token: 0x0400025D RID: 605
		public const string ShaderHDRP = "175c951fec709c44fa2f26b8ab78b8dd";

		// Token: 0x0400025E RID: 606
		public const string ShaderOctaHDRP = "56236dc63ad9b7949b63a27f0ad180b3";

		// Token: 0x0400025F RID: 607
		public const string PresetURP = "0403878495ffa3c4e9d4bcb3eac9b559";

		// Token: 0x04000260 RID: 608
		public const string ShaderOctaURP = "83dd8de9a5c14874884f9012def4fdcc";

		// Token: 0x04000261 RID: 609
		public const string ShaderURP = "da79d698f4bf0164e910ad798d07efdf";

		// Token: 0x04000262 RID: 610
		public const string DilateGUID = "57c23892d43bc9f458360024c5985405";

		// Token: 0x04000263 RID: 611
		public const string PackerGUID = "31bd3cd74692f384a916d9d7ea87710d";

		// Token: 0x04000264 RID: 612
		[SerializeField]
		private ParaImpostorAsset m_data;

		// Token: 0x04000265 RID: 613
		[SerializeField]
		private Transform m_rootTransform;

		// Token: 0x04000266 RID: 614
		[SerializeField]
		private LODGroup m_lodGroup;

		// Token: 0x04000267 RID: 615
		[SerializeField]
		private Renderer[] m_renderers;

		// Token: 0x04000268 RID: 616
		public LODReplacement m_lodReplacement = LODReplacement.ReplaceLast;

		// Token: 0x04000269 RID: 617
		[SerializeField]
		public RenderPipelineInUse m_renderPipelineInUse;

		// Token: 0x0400026A RID: 618
		public int m_insertIndex = 1;

		// Token: 0x0400026B RID: 619
		[SerializeField]
		public GameObject m_lastImpostor;

		// Token: 0x0400026C RID: 620
		[SerializeField]
		public string m_folderPath;

		// Token: 0x0400026D RID: 621
		[NonSerialized]
		public string m_impostorName = string.Empty;

		// Token: 0x0400026E RID: 622
		[SerializeField]
		public CutMode m_cutMode;

		// Token: 0x0400026F RID: 623
		[NonSerialized]
		private const float StartXRotation = -90f;

		// Token: 0x04000270 RID: 624
		[NonSerialized]
		private const float StartYRotation = 90f;

		// Token: 0x04000271 RID: 625
		[NonSerialized]
		private const int MinAlphaResolution = 256;

		// Token: 0x04000272 RID: 626
		[NonSerialized]
		private RenderTexture[] m_rtGBuffers;

		// Token: 0x04000273 RID: 627
		[NonSerialized]
		private RenderTexture[] m_alphaGBuffers;

		// Token: 0x04000274 RID: 628
		[NonSerialized]
		private RenderTexture m_trueDepth;

		// Token: 0x04000275 RID: 629
		[NonSerialized]
		public Texture2D m_alphaTex;

		// Token: 0x04000276 RID: 630
		[NonSerialized]
		private float m_xyFitSize;

		// Token: 0x04000277 RID: 631
		[NonSerialized]
		private float m_depthFitSize;

		// Token: 0x04000278 RID: 632
		[NonSerialized]
		private Vector2 m_pixelOffset = Vector2.zero;

		// Token: 0x04000279 RID: 633
		[NonSerialized]
		private Bounds m_originalBound;

		// Token: 0x0400027A RID: 634
		[NonSerialized]
		private Vector3 m_oriPos = Vector3.zero;

		// Token: 0x0400027B RID: 635
		[NonSerialized]
		private Quaternion m_oriRot = Quaternion.identity;

		// Token: 0x0400027C RID: 636
		[NonSerialized]
		private Vector3 m_oriSca = Vector3.one;

		// Token: 0x0400027D RID: 637
		[NonSerialized]
		private const int BlockSize = 65536;

		// Token: 0x0400027E RID: 638
		[NonSerialized]
		private readonly string[] m_propertyNames = new string[] { "_Albedo", "_Specular", "_Normals", "_Emission", "_Features" };

		// Token: 0x0400027F RID: 639
		[NonSerialized]
		private string[] m_standardFileNames = new string[]
		{
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty
		};

		// Token: 0x04000280 RID: 640
		[NonSerialized]
		private string[] m_fileNames = new string[]
		{
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty
		};

		// Token: 0x04000281 RID: 641
		private const string ImpostorsGCincGUID = "806d6cc0f22ee994f8cd901b6718f08d";

		// Token: 0x04000282 RID: 642
		private ListRequest m_packageListRequest;

		// Token: 0x04000283 RID: 643
		private static AISRPBaseline m_currentURPBaseline = AISRPBaseline.AI_SRP_INVALID;

		// Token: 0x04000284 RID: 644
		private static AISRPBaseline m_currentHDRPBaseline = AISRPBaseline.AI_SRP_INVALID;

		// Token: 0x04000285 RID: 645
		private static int m_packageURPVersion = 0;

		// Token: 0x04000286 RID: 646
		private static int m_packageHDRPVersion = 0;

		// Token: 0x04000287 RID: 647
		private static HashSet<int> m_srpPackageSupport = new HashSet<int> { 100000, 110000, 120000, 130000, 140000 };

		// Token: 0x04000288 RID: 648
		private static readonly string SemVerPattern = "^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$";

		// Token: 0x04000289 RID: 649
		private static readonly List<KeyValuePair<string, int>> m_hdrpStencilCheck = new List<KeyValuePair<string, int>>
		{
			new KeyValuePair<string, int>("_StencilForwardRef", 0),
			new KeyValuePair<string, int>("_StencilForwardMask", 6),
			new KeyValuePair<string, int>("_StencilMotionRef", 40),
			new KeyValuePair<string, int>("_StencilMotionMask", 40),
			new KeyValuePair<string, int>("_StencilDepthRef", 8),
			new KeyValuePair<string, int>("_StencilDepthMask", 8),
			new KeyValuePair<string, int>("_StencilGBufferRef", 10),
			new KeyValuePair<string, int>("_StencilGBufferMask", 14)
		};
	}
}
