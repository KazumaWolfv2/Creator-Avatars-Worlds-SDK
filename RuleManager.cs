using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class RuleManager
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000089 RID: 137 RVA: 0x00006C0E File Offset: 0x00004E0E
	// (set) Token: 0x0600008A RID: 138 RVA: 0x00006C16 File Offset: 0x00004E16
	public RuleRatingConfig mRuleRatingConfig { get; set; }

	// Token: 0x0600008B RID: 139 RVA: 0x00006C1F File Offset: 0x00004E1F
	public void ClearRules()
	{
		this.Rules.Clear();
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00006C2C File Offset: 0x00004E2C
	public void AddRule(RuleBase rule)
	{
		this.Rules.Add(rule);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00006C3C File Offset: 0x00004E3C
	public void Check(GameObject[] rootObjects)
	{
		this.InvalidRules.Clear();
		foreach (RuleBase ruleBase in this.Rules)
		{
			ruleBase.Reset();
			foreach (GameObject gameObject in rootObjects)
			{
				ruleBase.Check(gameObject);
			}
		}
		GameObject[] array = rootObjects;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (object obj in array[i].transform)
			{
				Transform transform = (Transform)obj;
				this.RecursiveCheckNode(transform.gameObject);
			}
		}
		this.CheckResult();
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00006D24 File Offset: 0x00004F24
	private void RecursiveCheckNode(GameObject obj)
	{
		this.Rules.ForEach(delegate(RuleBase rule)
		{
			rule.CheckNode(obj);
		});
		foreach (object obj2 in obj.transform)
		{
			Transform transform = (Transform)obj2;
			this.RecursiveCheckNode(transform.gameObject);
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00006DAC File Offset: 0x00004FAC
	public void CheckResult()
	{
		foreach (RuleBase ruleBase in this.Rules)
		{
			ruleBase.CalResult();
			if (ruleBase.GetResult() == RuleResultType.RuleResultType_Bad)
			{
				string text = "Error:============>>>>";
				Type type = ruleBase.GetType();
				ParaLog.Log(text + ((type != null) ? type.ToString() : null));
			}
			else if (ruleBase.GetResult() == RuleResultType.RuleResultType_Nil)
			{
				string text2 = "Warning:============>>>>";
				Type type2 = ruleBase.GetType();
				ParaLog.Log(text2 + ((type2 != null) ? type2.ToString() : null));
			}
			if (ruleBase.FixList.Count > 0)
			{
				this.InvalidRules.Add(ruleBase);
			}
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00006E70 File Offset: 0x00005070
	public RuleResultType GetResult()
	{
		RuleResultType ruleResultType = RuleResultType.RuleResultType_Prefect;
		foreach (RuleBase ruleBase in this.Rules)
		{
			if (ruleBase.GetResult() != RuleResultType.RuleResultType_Nil && ruleBase.GetResult() < ruleResultType)
			{
				ruleResultType = ruleBase.GetResult();
			}
		}
		return ruleResultType;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00006ED8 File Offset: 0x000050D8
	public JsonData GetResultDescription()
	{
		JsonData jsonData = new JsonData();
		foreach (RuleBase ruleBase in this.Rules)
		{
			string text = ruleBase.GetRuleType().ToString();
			jsonData[text] = ruleBase.GetResult().ToString();
		}
		return jsonData;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00006F60 File Offset: 0x00005160
	public List<RuleBase> GetRuleList()
	{
		return this.Rules;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00006F68 File Offset: 0x00005168
	public List<RuleBase> GetInvalidRuleList()
	{
		return this.InvalidRules;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00006F70 File Offset: 0x00005170
	public bool HasErrorFixType()
	{
		using (List<RuleBase>.Enumerator enumerator = this.InvalidRules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasErrorFixTypeInRule())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0400009F RID: 159
	private List<RuleBase> Rules = new List<RuleBase>();

	// Token: 0x040000A0 RID: 160
	private List<RuleBase> InvalidRules = new List<RuleBase>();
}
