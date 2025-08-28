using System;

namespace LitJson
{
	// Token: 0x02000059 RID: 89
	public enum JsonToken
	{
		// Token: 0x040001BE RID: 446
		None,
		// Token: 0x040001BF RID: 447
		ObjectStart,
		// Token: 0x040001C0 RID: 448
		PropertyName,
		// Token: 0x040001C1 RID: 449
		ObjectEnd,
		// Token: 0x040001C2 RID: 450
		ArrayStart,
		// Token: 0x040001C3 RID: 451
		ArrayEnd,
		// Token: 0x040001C4 RID: 452
		Int,
		// Token: 0x040001C5 RID: 453
		Long,
		// Token: 0x040001C6 RID: 454
		Double,
		// Token: 0x040001C7 RID: 455
		String,
		// Token: 0x040001C8 RID: 456
		Boolean,
		// Token: 0x040001C9 RID: 457
		Null
	}
}
