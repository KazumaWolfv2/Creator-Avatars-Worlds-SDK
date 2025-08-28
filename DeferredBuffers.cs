using System;

namespace ParaImpostors
{
	// Token: 0x02000070 RID: 112
	[Flags]
	public enum DeferredBuffers
	{
		// Token: 0x0400028F RID: 655
		AlbedoAlpha = 1,
		// Token: 0x04000290 RID: 656
		SpecularSmoothness = 2,
		// Token: 0x04000291 RID: 657
		NormalDepth = 4,
		// Token: 0x04000292 RID: 658
		EmissionOcclusion = 8
	}
}
