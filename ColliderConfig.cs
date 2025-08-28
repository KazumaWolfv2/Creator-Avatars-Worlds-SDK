using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
[Serializable]
public struct ColliderConfig
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000034 RID: 52 RVA: 0x0000321D File Offset: 0x0000141D
	public Vector3 axis
	{
		get
		{
			return this.rotation * Vector3.up;
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003230 File Offset: 0x00001430
	public static ColliderConfig Create()
	{
		return new ColliderConfig
		{
			rotation = Quaternion.identity,
			isMirrored = true
		};
	}

	// Token: 0x04000085 RID: 133
	public bool isMirrored;

	// Token: 0x04000086 RID: 134
	public ColliderConfig.State state;

	// Token: 0x04000087 RID: 135
	public Transform transform;

	// Token: 0x04000088 RID: 136
	public float radius;

	// Token: 0x04000089 RID: 137
	public float height;

	// Token: 0x0400008A RID: 138
	public Vector3 position;

	// Token: 0x0400008B RID: 139
	public Quaternion rotation;

	// Token: 0x02000083 RID: 131
	public enum State
	{
		// Token: 0x040002F1 RID: 753
		Automatic,
		// Token: 0x040002F2 RID: 754
		Custom,
		// Token: 0x040002F3 RID: 755
		Disabled
	}
}
