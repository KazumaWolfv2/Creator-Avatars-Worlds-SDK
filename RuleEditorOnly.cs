using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class RuleEditorOnly : RuleBase
{
	// Token: 0x060000BF RID: 191 RVA: 0x000077E1 File Offset: 0x000059E1
	public override RuleEnumType GetRuleType()
	{
		return RuleEnumType.RuleEnumType_EditorOnly;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000077E5 File Offset: 0x000059E5
	public override void Reset()
	{
		base.Reset();
		this._errorNodeName.Clear();
		this.Result = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x000077FF File Offset: 0x000059FF
	public void SetCheckRule(Action<GameObject> func)
	{
		this._checkRuleFunc = func;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00007808 File Offset: 0x00005A08
	public override void Check(GameObject obj)
	{
		if (this._checkRuleFunc != null)
		{
			this._checkRuleFunc(obj);
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00003394 File Offset: 0x00001594
	public override void CheckNode(GameObject obj)
	{
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00007820 File Offset: 0x00005A20
	public void CheckEditorOnly(List<string> typeList, GameObject obj)
	{
		MonoBehaviour[] componentsInChildren = obj.GetComponentsInChildren<MonoBehaviour>(false);
		ParaBehaviour[] componentsInChildren2 = obj.GetComponentsInChildren<ParaBehaviour>(false);
		this._checkEditorOnly<MonoBehaviour>(typeList, componentsInChildren);
		this._checkEditorOnly<ParaBehaviour>(typeList, componentsInChildren2);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00007850 File Offset: 0x00005A50
	private void _checkEditorOnly<T>(List<string> typeList, T[] ts) where T : MonoBehaviour
	{
		for (int i = 0; i < ts.Length; i++)
		{
			if (!(ts[i] == null))
			{
				string name = ts[i].GetType().Name;
				bool flag = false;
				for (int j = 0; j < typeList.Count; j++)
				{
					if (string.Equals(name, typeList[j]))
					{
						flag = true;
						break;
					}
				}
				if (flag && this.RecursionForEditorOnly(ts[i].gameObject) && !this._errorNodeName.Contains(ts[i].name))
				{
					this._errorNodeName.Add(ts[i].name);
				}
			}
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0000791C File Offset: 0x00005B1C
	public void CheckEditorOnly<T>(GameObject obj, List<T> checkList = null) where T : Component
	{
		if (checkList == null)
		{
			T[] componentsInChildren = obj.GetComponentsInChildren<T>(false);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (this.RecursionForEditorOnly(componentsInChildren[i].gameObject) && !this._errorNodeName.Contains(componentsInChildren[i].name))
				{
					this._errorNodeName.Add(componentsInChildren[i].name);
				}
			}
			return;
		}
		for (int j = 0; j < checkList.Count; j++)
		{
			if (checkList[j] != null && this.RecursionForEditorOnly(checkList[j].gameObject) && !this._errorNodeName.Contains(checkList[j].name))
			{
				this._errorNodeName.Add(checkList[j].name);
			}
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00007A10 File Offset: 0x00005C10
	private bool RecursionForEditorOnly(GameObject obj)
	{
		return string.Equals("EditorOnly", obj.tag) || (obj.transform.parent != null && this.RecursionForEditorOnly(obj.transform.parent.gameObject));
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00007A5C File Offset: 0x00005C5C
	public override void CalResult()
	{
		if (this._errorNodeName.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this._errorNodeName.Count; i++)
			{
				stringBuilder.Append(this._errorNodeName[i]);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			base.AddRuleFix(RuleErrorType.RuleErrorType_Error, RuleErrorRuleType.RuleErrorRuleType_EditorOnly, null, stringBuilder.ToString());
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00003394 File Offset: 0x00001594
	public override void RenderResult()
	{
	}

	// Token: 0x040000B6 RID: 182
	private List<string> _errorNodeName = new List<string>();

	// Token: 0x040000B7 RID: 183
	private Action<GameObject> _checkRuleFunc;
}
