using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class RuleFix
{
	// Token: 0x040000A8 RID: 168
	public RuleErrorType ErrorType;

	// Token: 0x040000A9 RID: 169
	public RuleErrorRuleType Type;

	// Token: 0x040000AA RID: 170
	public Action<GameObject> FixFunc;

	// Token: 0x040000AB RID: 171
	public string ErrorInfo;

	// Token: 0x040000AC RID: 172
	public bool IsValid;
}
