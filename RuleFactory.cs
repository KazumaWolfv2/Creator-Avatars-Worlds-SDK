using System;

// Token: 0x02000016 RID: 22
public abstract class RuleFactory
{
	// Token: 0x06000086 RID: 134
	public abstract void Register();

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000087 RID: 135 RVA: 0x00006C06 File Offset: 0x00004E06
	protected RuleManager mRuleManager { get; }
}
