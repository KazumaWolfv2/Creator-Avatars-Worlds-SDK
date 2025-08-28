using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
[AddComponentMenu("ParaSpace/Para Object Sync")]
[RequireComponent(typeof(ParaNetworkGuid))]
public class ParaObjectSync : ParaBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003719 File Offset: 0x00001919
	public override ParaBehaviourType ParaBehaviourType
	{
		get
		{
			return ParaBehaviourType.ObjectSync;
		}
	}
}
