using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class RuleBase
{
	// Token: 0x06000099 RID: 153 RVA: 0x00006FEA File Offset: 0x000051EA
	public virtual RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_Nil;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void CalResult()
	{
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00006FED File Offset: 0x000051ED
	public virtual RuleResultType GetResult()
	{
		return this.Result;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00006FF5 File Offset: 0x000051F5
	public virtual void Reset()
	{
		this.FixList.Clear();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void Check(GameObject obj)
	{
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00007004 File Offset: 0x00005204
	public void AddRuleFix(RuleErrorType errorType, RuleErrorRuleType type, Action<GameObject> fixFunc = null, string errorInfo = "")
	{
		if (this.IsInRuleFix(type))
		{
			return;
		}
		RuleFix ruleFix = new RuleFix();
		ruleFix.ErrorType = errorType;
		ruleFix.Type = type;
		ruleFix.FixFunc = fixFunc;
		ruleFix.IsValid = false;
		ruleFix.ErrorInfo = errorInfo;
		this.FixList.Add(ruleFix);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00007054 File Offset: 0x00005254
	public bool IsInRuleFix(RuleErrorRuleType type)
	{
		using (List<RuleFix>.Enumerator enumerator = this.FixList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Type == type)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000070B0 File Offset: 0x000052B0
	public bool HasErrorFixTypeInRule()
	{
		using (List<RuleFix>.Enumerator enumerator = this.FixList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.ErrorType == RuleErrorType.RuleErrorType_Error)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000710C File Offset: 0x0000530C
	public void DelRuleFix(RuleErrorRuleType type)
	{
		for (int i = 0; i < this.FixList.Count; i++)
		{
			if (this.FixList[i].Type == type)
			{
				this.FixList[i].IsValid = true;
				return;
			}
		}
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void CheckNode(GameObject obj)
	{
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00003394 File Offset: 0x00001594
	public virtual void RenderResult()
	{
	}

	// Token: 0x040000AD RID: 173
	protected RuleEnumType Type;

	// Token: 0x040000AE RID: 174
	protected RuleResultType Result;

	// Token: 0x040000AF RID: 175
	public List<RuleFix> FixList = new List<RuleFix>();
}
