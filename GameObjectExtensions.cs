using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public static class GameObjectExtensions
{
	// Token: 0x0600007B RID: 123 RVA: 0x00004CAC File Offset: 0x00002EAC
	public static void SetLayer(this GameObject go, int layer)
	{
		go.layer = layer;
		foreach (object obj in go.transform)
		{
			((Transform)obj).gameObject.SetLayer(layer);
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004D10 File Offset: 0x00002F10
	public static bool IsActiveRecursively(this GameObject go)
	{
		GameObject gameObject = go;
		while (gameObject.activeSelf)
		{
			if (!(gameObject.transform.parent != null))
			{
				return true;
			}
			gameObject = gameObject.transform.parent.gameObject;
			if (!gameObject.activeSelf)
			{
				return false;
			}
		}
		return false;
	}
}
