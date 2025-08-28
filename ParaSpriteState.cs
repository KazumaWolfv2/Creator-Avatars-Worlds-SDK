using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
[Serializable]
public struct ParaSpriteState : IEquatable<ParaSpriteState>
{
	// Token: 0x06000030 RID: 48 RVA: 0x000031CF File Offset: 0x000013CF
	public bool Equals(ParaSpriteState other)
	{
		return this.normalImage == other.normalImage && this.pressedImage == other.pressedImage && this.disabledImage == other.disabledImage;
	}

	// Token: 0x04000075 RID: 117
	public Sprite normalImage;

	// Token: 0x04000076 RID: 118
	public Sprite pressedImage;

	// Token: 0x04000077 RID: 119
	public Sprite disabledImage;
}
