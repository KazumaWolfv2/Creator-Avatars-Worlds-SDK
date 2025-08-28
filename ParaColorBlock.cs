using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[Serializable]
public struct ParaColorBlock : IEquatable<ParaColorBlock>
{
	// Token: 0x0600002B RID: 43 RVA: 0x00003144 File Offset: 0x00001344
	public override bool Equals(object obj)
	{
		if (obj is ParaColorBlock)
		{
			ParaColorBlock paraColorBlock = (ParaColorBlock)obj;
			return this.Equals(paraColorBlock);
		}
		return false;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x0000316B File Offset: 0x0000136B
	public bool Equals(ParaColorBlock other)
	{
		return this.normalColor == other.normalColor && this.pressedColor == other.pressedColor && this.disabledColor == other.disabledColor;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000031A6 File Offset: 0x000013A6
	public static bool operator ==(ParaColorBlock point1, ParaColorBlock point2)
	{
		return point1.Equals(point2);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000031B0 File Offset: 0x000013B0
	public static bool operator !=(ParaColorBlock point1, ParaColorBlock point2)
	{
		return !point1.Equals(point2);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000031BD File Offset: 0x000013BD
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x04000071 RID: 113
	public Color normalColor;

	// Token: 0x04000072 RID: 114
	public Color pressedColor;

	// Token: 0x04000073 RID: 115
	public Color disabledColor;

	// Token: 0x04000074 RID: 116
	public static ParaColorBlock defaultColorBlock = new ParaColorBlock
	{
		normalColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue),
		pressedColor = new Color32(200, 200, 200, byte.MaxValue),
		disabledColor = new Color32(200, 200, 200, 128)
	};
}
