using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public abstract class ParaBehaviour : MonoBehaviour
{
	// Token: 0x06000026 RID: 38 RVA: 0x000030AA File Offset: 0x000012AA
	public bool IsSpawned()
	{
		return this.eNetState == NetState.Spawned;
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000027 RID: 39
	public abstract ParaBehaviourType ParaBehaviourType { get; }

	// Token: 0x06000028 RID: 40 RVA: 0x000030B5 File Offset: 0x000012B5
	public virtual void Spawned()
	{
	}

	// Token: 0x04000061 RID: 97
	[HideInInspector]
	public object NetObjectBind;

	// Token: 0x04000062 RID: 98
	[HideInInspector]
	public NetState eNetState;
}
