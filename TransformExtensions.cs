using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
public static class TransformExtensions
{
	// Token: 0x0600008B RID: 139 RVA: 0x00004ED4 File Offset: 0x000030D4
	public static Matrix4x4 LocalMatrix(this Transform transform)
	{
		Transform parent = transform.parent;
		if (parent != null)
		{
			return Matrix4x4.TRS(Vector3.Scale(parent.lossyScale, transform.localPosition), transform.localRotation, Vector3.one);
		}
		return Matrix4x4.TRS(transform.localPosition, transform.localRotation, Vector3.one);
	}
}
