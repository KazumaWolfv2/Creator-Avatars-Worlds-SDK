using System;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x02000044 RID: 68
public class ProfilerExtensions
{
	// Token: 0x06000082 RID: 130 RVA: 0x00004E98 File Offset: 0x00003098
	public static void BeginSample(string name)
	{
		Profiler.BeginSample(name);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00004EA0 File Offset: 0x000030A0
	public static void BeginSample(string name, Object targetObject)
	{
		Profiler.BeginSample(name, targetObject);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00004EA9 File Offset: 0x000030A9
	public static void EndSample()
	{
		Profiler.EndSample();
	}
}
