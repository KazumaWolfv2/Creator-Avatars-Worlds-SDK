using System;

namespace LitJson
{
	// Token: 0x02000060 RID: 96
	internal enum ParserToken
	{
		// Token: 0x04000203 RID: 515
		None = 65536,
		// Token: 0x04000204 RID: 516
		Number,
		// Token: 0x04000205 RID: 517
		True,
		// Token: 0x04000206 RID: 518
		False,
		// Token: 0x04000207 RID: 519
		Null,
		// Token: 0x04000208 RID: 520
		CharSeq,
		// Token: 0x04000209 RID: 521
		Char,
		// Token: 0x0400020A RID: 522
		Text,
		// Token: 0x0400020B RID: 523
		Object,
		// Token: 0x0400020C RID: 524
		ObjectPrime,
		// Token: 0x0400020D RID: 525
		Pair,
		// Token: 0x0400020E RID: 526
		PairRest,
		// Token: 0x0400020F RID: 527
		Array,
		// Token: 0x04000210 RID: 528
		ArrayPrime,
		// Token: 0x04000211 RID: 529
		Value,
		// Token: 0x04000212 RID: 530
		ValueRest,
		// Token: 0x04000213 RID: 531
		String,
		// Token: 0x04000214 RID: 532
		End,
		// Token: 0x04000215 RID: 533
		Epsilon
	}
}
