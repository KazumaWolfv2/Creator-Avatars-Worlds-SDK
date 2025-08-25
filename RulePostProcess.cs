using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class RulePostProcess : RuleBase
{
	// Token: 0x06000124 RID: 292 RVA: 0x00009091 File Offset: 0x00007291
	public RulePostProcess(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleRating = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_PostProcess];
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000090AC File Offset: 0x000072AC
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_PostProcess;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000090B0 File Offset: 0x000072B0
	public override void Reset()
	{
		base.Reset();
		this.Count = 0;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00003394 File Offset: 0x00001594
	public override void Check(GameObject obj)
	{
	}

	// Token: 0x06000128 RID: 296 RVA: 0x000090BF File Offset: 0x000072BF
	public override void CalResult()
	{
		if (this.Count > this.mRuleRating.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Bad;
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_PostProcessCount, null, "");
		}
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000090EA File Offset: 0x000072EA
	private int GetResultValue()
	{
		return this.Count;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000090F2 File Offset: 0x000072F2
	private int GetResultJudgment()
	{
		return this.mRuleRating.Poor;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00003394 File Offset: 0x00001594
	public override void RenderResult()
	{
	}

	// Token: 0x040000D7 RID: 215
	private RuleRating mRuleRating;

	// Token: 0x040000D8 RID: 216
	private int Count;
}
