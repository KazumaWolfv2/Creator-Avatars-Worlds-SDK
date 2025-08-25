using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class RuleRealLight : RuleBase
{
	// Token: 0x0600012C RID: 300 RVA: 0x000090FF File Offset: 0x000072FF
	public RuleRealLight(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRatingRealTimeLight = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_RealLight];
		this.mRuleRatingDirectionalLight = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_DirectionalLight];
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000912D File Offset: 0x0000732D
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_RealLight;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00009131 File Offset: 0x00007331
	public override void Reset()
	{
		base.Reset();
		this.mRealTimeLightsCount = 0;
		this.mDirectionalLightsCount = 0;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00009148 File Offset: 0x00007348
	public override void Check(GameObject obj)
	{
		foreach (Light light in obj.GetComponentsInChildren<Light>(false))
		{
			if (light.type == 1 && light.lightmapBakeType != 2)
			{
				this.mDirectionalLightsCount++;
			}
			else if (light.lightmapBakeType != 2)
			{
				this.mRealTimeLightsCount++;
			}
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000091A8 File Offset: 0x000073A8
	public override void CalResult()
	{
		if (this.mDirectionalLightsCount <= this.mRuleRatingDirectionalLight.Perfect)
		{
			this.mDirectionalLightsCountResult = RuleResultType.RuleResultType_Prefect;
		}
		else if (this.mDirectionalLightsCount <= this.mRuleRatingDirectionalLight.Excellent)
		{
			this.mDirectionalLightsCountResult = RuleResultType.RuleResultType_Excellent;
		}
		else if (this.mDirectionalLightsCount <= this.mRuleRatingDirectionalLight.Good)
		{
			this.mDirectionalLightsCountResult = RuleResultType.RuleResultType_Good;
		}
		else if (this.mDirectionalLightsCount <= this.mRuleRatingDirectionalLight.Average)
		{
			this.mDirectionalLightsCountResult = RuleResultType.RuleResultType_Normal;
		}
		else if (this.mDirectionalLightsCount <= this.mRuleRatingDirectionalLight.Poor)
		{
			this.mDirectionalLightsCountResult = RuleResultType.RuleResultType_Poor;
		}
		else
		{
			this.mDirectionalLightsCountResult = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_DirectionalLightCount, null, "");
		}
		if (this.mRealTimeLightsCount <= this.mRuleRatingRealTimeLight.Perfect)
		{
			this.mRealTimeLightsCountResult = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.mRealTimeLightsCount <= this.mRuleRatingRealTimeLight.Excellent)
		{
			this.mRealTimeLightsCountResult = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.mRealTimeLightsCount <= this.mRuleRatingRealTimeLight.Good)
		{
			this.mRealTimeLightsCountResult = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.mRealTimeLightsCount <= this.mRuleRatingRealTimeLight.Average)
		{
			this.mRealTimeLightsCountResult = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.mRealTimeLightsCount <= this.mRuleRatingRealTimeLight.Poor)
		{
			this.mRealTimeLightsCountResult = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.mRealTimeLightsCountResult = RuleResultType.RuleResultType_Bad;
		base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_RealLightCount, null, "");
	}

	// Token: 0x06000131 RID: 305 RVA: 0x000092F8 File Offset: 0x000074F8
	public override RuleResultType GetResult()
	{
		RuleResultType ruleResultType = this.mRealTimeLightsCountResult;
		if (this.mDirectionalLightsCountResult < ruleResultType)
		{
			ruleResultType = this.mDirectionalLightsCountResult;
		}
		return ruleResultType;
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000931D File Offset: 0x0000751D
	private int GetLightsCount()
	{
		return this.mRealTimeLightsCount;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00009325 File Offset: 0x00007525
	private int GetResultJudgmentLightsCount()
	{
		return this.mRuleRatingRealTimeLight.Poor;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00009332 File Offset: 0x00007532
	private RuleResultType GetLightsCountResult()
	{
		return this.mRealTimeLightsCountResult;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000933A File Offset: 0x0000753A
	private int GetDirectionalLightsCount()
	{
		return this.mDirectionalLightsCount;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00009342 File Offset: 0x00007542
	private int GetResultJudgmentDirectionalLightsCount()
	{
		return this.mRuleRatingDirectionalLight.Poor;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000934F File Offset: 0x0000754F
	private RuleResultType GetDirectionalLightsCountResult()
	{
		return this.mDirectionalLightsCountResult;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00009358 File Offset: 0x00007558
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleRealLight_text0"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType ruleResultType = this.GetDirectionalLightsCountResult();
		GUIStyle guistyle = CheckResultRenderManager.Instance.GetCheckResultStyle(ruleResultType);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetDirectionalLightsCount(), this.GetResultJudgmentDirectionalLightsCount(), RuleResultDefine.GetRuleResultType(ruleResultType)), guistyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(SdkLangManager.Get("str_sdk_ruleRealLight_text1"), new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		ruleResultType = this.GetLightsCountResult();
		guistyle = CheckResultRenderManager.Instance.GetCheckResultStyle(ruleResultType);
		GUILayout.Label(string.Format("{0}/{1}({2})", this.GetLightsCount(), this.GetResultJudgmentLightsCount(), RuleResultDefine.GetRuleResultType(ruleResultType)), guistyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x040000D9 RID: 217
	private RuleRating mRuleRatingRealTimeLight;

	// Token: 0x040000DA RID: 218
	private RuleRating mRuleRatingDirectionalLight;

	// Token: 0x040000DB RID: 219
	private int mRealTimeLightsCount;

	// Token: 0x040000DC RID: 220
	private int mDirectionalLightsCount;

	// Token: 0x040000DD RID: 221
	private RuleResultType mRealTimeLightsCountResult;

	// Token: 0x040000DE RID: 222
	private RuleResultType mDirectionalLightsCountResult;
}
