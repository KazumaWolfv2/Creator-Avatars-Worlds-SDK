using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class RuleMemory : RuleBase
{
	// Token: 0x060000E9 RID: 233 RVA: 0x000082B9 File Offset: 0x000064B9
	public RuleMemory(RuleRatingConfig ruleRatingConfig)
	{
		this.mRuleConfig = ruleRatingConfig.mRuleRatings[RuleEnumType.RuleEnumType_PackageTotalSize];
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000082D4 File Offset: 0x000064D4
	public override void Reset()
	{
		this.UseMemorySize = 0f;
		this.Result = RuleResultType.RuleResultType_Prefect;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000082E8 File Offset: 0x000064E8
	public override void Check(GameObject go)
	{
		this.UseMemorySize = this.CalcObjectUseMemorySize(go);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000082F8 File Offset: 0x000064F8
	public override void CalResult()
	{
		if (this.UseMemorySize <= (float)this.mRuleConfig.Perfect)
		{
			this.Result = RuleResultType.RuleResultType_Prefect;
			return;
		}
		if (this.UseMemorySize <= (float)this.mRuleConfig.Excellent)
		{
			this.Result = RuleResultType.RuleResultType_Excellent;
			return;
		}
		if (this.UseMemorySize <= (float)this.mRuleConfig.Good)
		{
			this.Result = RuleResultType.RuleResultType_Good;
			return;
		}
		if (this.UseMemorySize <= (float)this.mRuleConfig.Average)
		{
			this.Result = RuleResultType.RuleResultType_Normal;
			return;
		}
		if (this.UseMemorySize <= (float)this.mRuleConfig.Poor)
		{
			this.Result = RuleResultType.RuleResultType_Poor;
			return;
		}
		this.Result = RuleResultType.RuleResultType_Bad;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00008398 File Offset: 0x00006598
	public override void RenderResult()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Memory Size:", new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		RuleResultType result = this.GetResult();
		GUIStyle checkResultStyle = CheckResultRenderManager.Instance.GetCheckResultStyle(result);
		GUILayout.Label(string.Format("{0}/{1}(MB)({2})", this.GetResultValue(), this.GetResultJudgment(), RuleResultDefine.GetRuleResultType(result)), checkResultStyle, new GUILayoutOption[]
		{
			GUILayout.Height(20f),
			GUILayout.ExpandWidth(false)
		});
		GUILayout.EndHorizontal();
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00008430 File Offset: 0x00006630
	private string GetResultValue()
	{
		return string.Format("{0:0.00}", this.UseMemorySize);
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00008447 File Offset: 0x00006647
	private float GetResultJudgment()
	{
		return (float)this.mRuleConfig.Poor;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00008458 File Offset: 0x00006658
	private float CalcObjectUseMemorySize(GameObject go)
	{
		long num = 0L;
		if (go != null)
		{
			string prefabAssetPath = this.GetPrefabAssetPath(go);
			if (!string.IsNullOrEmpty(prefabAssetPath))
			{
				List<string> list = new List<string>();
				list.Add(prefabAssetPath);
				Singleton<DependencesMgr>.instance.CollectDependenceMap(list);
				num = Singleton<DependencesMgr>.instance.CalcMemorySize();
			}
		}
		return (float)num / 1048576f;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000084AC File Offset: 0x000066AC
	public string GetPrefabAssetPath(GameObject gameobject)
	{
		if (PrefabUtility.IsPartOfPrefabAsset(gameobject))
		{
			AssetDatabase.GetAssetPath(gameobject);
			return AssetDatabase.GetAssetPath(gameobject);
		}
		if (PrefabUtility.IsPartOfPrefabInstance(gameobject))
		{
			GameObject correspondingObjectFromSource = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(gameobject);
			AssetDatabase.GetAssetPath(correspondingObjectFromSource);
			return AssetDatabase.GetAssetPath(correspondingObjectFromSource);
		}
		PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(gameobject);
		if (prefabStage != null)
		{
			return prefabStage.assetPath;
		}
		return "";
	}

	// Token: 0x040000C6 RID: 198
	private float UseMemorySize;

	// Token: 0x040000C7 RID: 199
	private RuleRating mRuleConfig;
}
