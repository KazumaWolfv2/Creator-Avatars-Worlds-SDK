using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class RuleParticle : RuleBase
{
	// Token: 0x06000116 RID: 278 RVA: 0x00008C0C File Offset: 0x00006E0C
	public RuleParticle(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRatingParticleCount = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_ParticleCount];
		this.mRuleRatingParticleSystemCount = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_ParticleSystemCount];
		this.mRuleRatingMeshEmitterCount = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_ParticleMeshEmiiterCount];
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00008C58 File Offset: 0x00006E58
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_ParticleCount;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00008C5C File Offset: 0x00006E5C
	public override void Reset()
	{
		base.Reset();
		this.ParticleCount = 0;
		this.ParticleSystemCount = 0;
		this.ParticleCountOtherExtra = 0;
		this.ResultParticleSystemCount = RuleResultType.RuleResultType_Prefect;
		this.ResultParticleMeshCount = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00008C88 File Offset: 0x00006E88
	public override void Check(GameObject obj)
	{
		ParticleSystem[] componentsInChildren = obj.GetComponentsInChildren<ParticleSystem>(false);
		this.ParticleSystemCount += componentsInChildren.Length;
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			this.ParticleCount += particleSystem.particleCount;
		}
		foreach (ParticleSystemRenderer particleSystemRenderer in obj.GetComponentsInChildren<ParticleSystemRenderer>(false))
		{
			if (particleSystemRenderer != null && particleSystemRenderer.renderMode == 4)
			{
				this.ParticleCountOtherExtra++;
			}
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00008D0F File Offset: 0x00006F0F
	private int GetResultValue()
	{
		return this.ParticleCount;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00008D17 File Offset: 0x00006F17
	private int GetResultJudgment()
	{
		return this.mRuleRatingParticleCount.Poor;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00008D24 File Offset: 0x00006F24
	private int GetResultValueExtra()
	{
		return this.ParticleSystemCount;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00008D2C File Offset: 0x00006F2C
	private int GetResultJudgmentExtra()
	{
		return this.mRuleRatingParticleSystemCount.Poor;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008D39 File Offset: 0x00006F39
	private RuleResultType GetResultParticleSystemCount()
	{
		return this.ResultParticleSystemCount;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00008D41 File Offset: 0x00006F41
	private RuleResultType GetResultParticleMeshCount()
	{
		return this.ResultParticleMeshCount;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00008D49 File Offset: 0x00006F49
	private int GetResultValueOtherExtra()
	{
		return this.ParticleCountOtherExtra;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00008D51 File Offset: 0x00006F51
	private int GetResultJudgmentOtherExtra()
	{
		return this.mRuleRatingMeshEmitterCount.Poor;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00008D60 File Offset: 0x00006F60
	public override void CalResult()
	{
		int particleCount = this.ParticleCount;
		if (particleCount <= this.mRuleRatingParticleCount.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
		}
		else if (particleCount <= this.mRuleRatingParticleCount.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
		}
		else if (particleCount <= this.mRuleRatingParticleCount.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
		}
		else if (particleCount <= this.mRuleRatingParticleCount.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
		}
		else if (particleCount <= this.mRuleRatingParticleCount.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
		}
		else
		{
			this.Result = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_ParticleActiveCount, null, "");
		}
		int particleSystemCount = this.ParticleSystemCount;
		if (particleSystemCount <= this.mRuleRatingParticleSystemCount.Perfect)
		{
			this.ResultParticleSystemCount = RuleResultType.RuleResultType_Prefect;
		}
		else if (particleSystemCount <= this.mRuleRatingParticleSystemCount.Excellent)
		{
			this.ResultParticleSystemCount = RuleResultType.RuleResultType_Excellent;
		}
		else if (particleSystemCount <= this.mRuleRatingParticleSystemCount.Good)
		{
			this.ResultParticleSystemCount = RuleResultType.RuleResultType_Good;
		}
		else if (particleSystemCount <= this.mRuleRatingParticleSystemCount.Average)
		{
			this.ResultParticleSystemCount = RuleResultType.RuleResultType_Normal;
		}
		else if (particleSystemCount <= this.mRuleRatingParticleSystemCount.Poor)
		{
			this.ResultParticleSystemCount = RuleResultType.RuleResultType_Poor;
		}
		else
		{
			this.ResultParticleSystemCount = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_ParticleSystemCount, null, "");
		}
		if (this.ParticleCountOtherExtra <= this.mRuleRatingMeshEmitterCount.Poor)
		{
			this.ResultParticleMeshCount = RuleResultType.RuleResultType_Prefect;
			return;
		}
		this.ResultParticleMeshCount = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_ParticleSystemMesh, null, "");
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00008EC0 File Offset: 0x000070C0
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleParticle_system_count"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType resultParticleSystemCount = this.GetResultParticleSystemCount();
		GUIStyle checkResultStyle = CheckResultRenderManager.Instance.GetCheckResultStyle(resultParticleSystemCount);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetResultValueExtra(), this.GetResultJudgmentExtra(), RuleResultDefine.GetRuleResultType(resultParticleSystemCount)), checkResultStyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleParticle_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType result = this.GetResult();
		GUIStyle checkResultStyle2 = CheckResultRenderManager.Instance.GetCheckResultStyle(result);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetResultValue(), this.GetResultJudgment(), RuleResultDefine.GetRuleResultType(result)), checkResultStyle2, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleParticle_text1"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType resultParticleMeshCount = this.GetResultParticleMeshCount();
		GUIStyle checkResultStyle3 = CheckResultRenderManager.Instance.GetCheckResultStyle(resultParticleMeshCount);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetResultValueOtherExtra(), this.GetResultJudgmentOtherExtra(), RuleResultDefine.GetRuleResultType(resultParticleMeshCount)), checkResultStyle3, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000CF RID: 207
	private RuleRating mRuleRatingParticleCount;

	// Token: 0x040000D0 RID: 208
	private RuleRating mRuleRatingParticleSystemCount;

	// Token: 0x040000D1 RID: 209
	private RuleRating mRuleRatingMeshEmitterCount;

	// Token: 0x040000D2 RID: 210
	private int ParticleCount;

	// Token: 0x040000D3 RID: 211
	private int ParticleSystemCount;

	// Token: 0x040000D4 RID: 212
	private int ParticleCountOtherExtra;

	// Token: 0x040000D5 RID: 213
	private RuleResultType ResultParticleSystemCount;

	// Token: 0x040000D6 RID: 214
	private RuleResultType ResultParticleMeshCount;
}
