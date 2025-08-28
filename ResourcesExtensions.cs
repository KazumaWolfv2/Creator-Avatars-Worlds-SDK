using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public static class ResourcesExtensions
{
	// Token: 0x06000086 RID: 134 RVA: 0x00004EB0 File Offset: 0x000030B0
	public static Sprite LoadSprite(string path)
	{
		return Resources.Load<Sprite>(path);
	}
}
