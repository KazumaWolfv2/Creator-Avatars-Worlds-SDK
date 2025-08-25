using System;

// Token: 0x0200004F RID: 79
internal class HexConverter
{
	// Token: 0x06000268 RID: 616 RVA: 0x0000FC5F File Offset: 0x0000DE5F
	public static char ToCharUpper(int value)
	{
		value &= 15;
		value += 48;
		if (value > 57)
		{
			value += 7;
		}
		return (char)value;
	}
}
