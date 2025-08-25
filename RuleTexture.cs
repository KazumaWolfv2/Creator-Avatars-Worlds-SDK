using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class RuleTexture : RuleBase
{
	// Token: 0x06000182 RID: 386 RVA: 0x0000A698 File Offset: 0x00008898
	public RuleTexture(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRatingTextureCount = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_TextureCount];
		this.mRuleRatingTextureSize = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_TextureSize];
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000A6D0 File Offset: 0x000088D0
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_TextureCount;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000A6D3 File Offset: 0x000088D3
	public override void Reset()
	{
		base.Reset();
		this.Count = 0;
		this.Size = 0;
		this.mTextures.Clear();
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000A6F4 File Offset: 0x000088F4
	public override void Check(GameObject obj)
	{
		foreach (Material material in EditorUtil.GetMaterials(obj, false))
		{
			if (!(material == null))
			{
				foreach (Texture texture in MaterialExtensions.GetTextures(material))
				{
					if (!(texture == null))
					{
						Texture2D texture2D = texture as Texture2D;
						if (texture2D != null)
						{
							string assetPath = AssetDatabase.GetAssetPath(texture);
							if (!this.mTextures.Contains(assetPath))
							{
								this.mTextures.Add(assetPath);
								this.Count++;
								int num = texture2D.GetRawTextureData().Length;
								this.Size += num;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000A7F0 File Offset: 0x000089F0
	private void FixTextureSize(GameObject go)
	{
		foreach (string text in this.mTextures)
		{
			TextureImporter textureImporter = AssetImporter.GetAtPath(text) as TextureImporter;
			if (!(textureImporter == null))
			{
				if (textureImporter.maxTextureSize > 1024)
				{
					textureImporter.maxTextureSize = 1024;
				}
				TextureImporterPlatformSettings platformTextureSettings = textureImporter.GetPlatformTextureSettings("iOS");
				if (platformTextureSettings.maxTextureSize > 1024)
				{
					platformTextureSettings.maxTextureSize = 1024;
				}
				if (platformTextureSettings.format != 50)
				{
					platformTextureSettings.format = 50;
				}
				textureImporter.SetPlatformTextureSettings(platformTextureSettings);
				TextureImporterPlatformSettings platformTextureSettings2 = textureImporter.GetPlatformTextureSettings("Android");
				if (platformTextureSettings2.maxTextureSize > 1024)
				{
					platformTextureSettings2.maxTextureSize = 1024;
				}
				if (textureImporter.DoesSourceTextureHaveAlpha())
				{
					if (platformTextureSettings2.format != 65)
					{
						platformTextureSettings2.format = 65;
					}
				}
				else if (platformTextureSettings2.format != 64)
				{
					platformTextureSettings2.format = 64;
				}
				textureImporter.SetPlatformTextureSettings(platformTextureSettings2);
				textureImporter.SaveAndReimport();
			}
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A90C File Offset: 0x00008B0C
	public override void CalResult()
	{
		if (this.Count <= this.mRuleRatingTextureCount.Perfect)
		{
			this.mTexturesCountResult = RuleResultType.RuleResultType_Prefect;
		}
		else if (this.Count <= this.mRuleRatingTextureCount.Excellent)
		{
			this.mTexturesCountResult = RuleResultType.RuleResultType_Excellent;
		}
		else if (this.Count <= this.mRuleRatingTextureCount.Good)
		{
			this.mTexturesCountResult = RuleResultType.RuleResultType_Good;
		}
		else if (this.Count <= this.mRuleRatingTextureCount.Average)
		{
			this.mTexturesCountResult = RuleResultType.RuleResultType_Normal;
		}
		else if (this.Count <= this.mRuleRatingTextureCount.Poor)
		{
			this.mTexturesCountResult = RuleResultType.RuleResultType_Poor;
		}
		else
		{
			this.mTexturesCountResult = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_TextureCount, null, "");
		}
		float num = (float)this.Size / 1024f / 1024f;
		if (num <= (float)this.mRuleRatingTextureSize.Perfect)
		{
			this.ResultExtra = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (num <= (float)this.mRuleRatingTextureSize.Excellent)
		{
			this.ResultExtra = RuleResultType.RuleResultType_Excellent;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_TextureSizeLowerLimit, null, "");
			return;
		}
		if (num <= (float)this.mRuleRatingTextureSize.Good)
		{
			this.ResultExtra = RuleResultType.RuleResultType_Good;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_TextureSizeLowerLimit, null, "");
			return;
		}
		if (num <= (float)this.mRuleRatingTextureSize.Average)
		{
			this.ResultExtra = RuleResultType.RuleResultType_Normal;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_TextureSizeLowerLimit, null, "");
			return;
		}
		if (num <= (float)this.mRuleRatingTextureSize.Poor)
		{
			this.ResultExtra = RuleResultType.RuleResultType_Poor;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Warning, RuleErrorRuleType.RuleErrorRuleType_TextureSizeLowerLimit, null, "");
			return;
		}
		this.ResultExtra = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_TextureSizeHightLimit, new Action<GameObject>(this.FixTextureSize), "");
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000AAA0 File Offset: 0x00008CA0
	public override RuleResultType GetResult()
	{
		RuleResultType resultExtra = this.mTexturesCountResult;
		if (this.ResultExtra < resultExtra)
		{
			resultExtra = this.ResultExtra;
		}
		return resultExtra;
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000AAC5 File Offset: 0x00008CC5
	private int GetTexturesCount()
	{
		return this.Count;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000AACD File Offset: 0x00008CCD
	private int GetResultJudgmentTexturesCount()
	{
		return this.mRuleRatingTextureCount.Poor;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000AADA File Offset: 0x00008CDA
	private RuleResultType GetTexturesCountResult()
	{
		return this.mTexturesCountResult;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000AAE2 File Offset: 0x00008CE2
	private float GetResultValueExtra()
	{
		return (float)this.Size / 1024f / 1024f;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000AAF7 File Offset: 0x00008CF7
	private int GetResultJudgmentExtra()
	{
		return this.mRuleRatingTextureSize.Poor;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000AB04 File Offset: 0x00008D04
	private RuleResultType GetResultExtra()
	{
		return this.ResultExtra;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000AB0C File Offset: 0x00008D0C
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleTexture_text2"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUIStyle styleRed = CheckResultRenderManager.Instance.StyleRed;
		GUIStyle styleRed2 = CheckResultRenderManager.Instance.StyleRed;
		RuleResultType resultExtra = this.GetResultExtra();
		GUIStyle checkResultStyle = CheckResultRenderManager.Instance.GetCheckResultStyle(resultExtra);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetResultValueExtra(), this.GetResultJudgmentExtra(), RuleResultDefine.GetRuleResultType(resultExtra)), checkResultStyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleTexture_text3"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType texturesCountResult = this.GetTexturesCountResult();
		GUIStyle checkResultStyle2 = CheckResultRenderManager.Instance.GetCheckResultStyle(texturesCountResult);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetTexturesCount(), this.GetResultJudgmentTexturesCount(), RuleResultDefine.GetRuleResultType(texturesCountResult)), checkResultStyle2, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000F5 RID: 245
	private int Count;

	// Token: 0x040000F6 RID: 246
	private int Size;

	// Token: 0x040000F7 RID: 247
	private RuleResultType mTexturesCountResult;

	// Token: 0x040000F8 RID: 248
	private RuleResultType ResultExtra;

	// Token: 0x040000F9 RID: 249
	private List<string> mTextures = new List<string>();

	// Token: 0x040000FA RID: 250
	private RuleRating mRuleRatingTextureSize;

	// Token: 0x040000FB RID: 251
	private RuleRating mRuleRatingTextureCount;
}
