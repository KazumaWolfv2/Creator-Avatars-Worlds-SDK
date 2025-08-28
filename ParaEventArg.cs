using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
public struct ParaEventArg
{
	// Token: 0x040000AE RID: 174
	public ParaEventType EventType;

	// Token: 0x040000AF RID: 175
	public GameObject Go;

	// Token: 0x040000B0 RID: 176
	public InteractiveButtonType TypeButton;

	// Token: 0x040000B1 RID: 177
	public Vector2 Pointer;

	// Token: 0x040000B2 RID: 178
	public Vector2 PointerDelta;

	// Token: 0x040000B3 RID: 179
	public HandType HandType;

	// Token: 0x040000B4 RID: 180
	public Matrix4x4 HoldMatrix;
}
