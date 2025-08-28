using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
public static class LODUtil
{
	// Token: 0x0600007D RID: 125 RVA: 0x00004D5C File Offset: 0x00002F5C
	public static void ForceLOD(GameObject root, int lod)
	{
		LODGroup componentInChildren = root.GetComponentInChildren<LODGroup>(true);
		if (componentInChildren == null)
		{
			return;
		}
		LOD[] lods = componentInChildren.GetLODs();
		for (int i = 0; i < lods.Length; i++)
		{
			foreach (Renderer component in lods[i].renderers)
			{
				bool flag = lod == -1 || lod == i;
				component.gameObject.SetActive(flag);
			}
		}
		componentInChildren.SetLODs(lods);
		componentInChildren.enabled = lod == -1;
	}
}
