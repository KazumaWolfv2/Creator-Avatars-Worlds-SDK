using System;
using System.Runtime.CompilerServices;
using System.Text;

// Token: 0x02000049 RID: 73
public static class XXHash
{
	// Token: 0x06000094 RID: 148 RVA: 0x000051AE File Offset: 0x000033AE
	public static uint Hash32(string text)
	{
		return XXHash.Hash32(text, Encoding.UTF8);
	}

	// Token: 0x06000095 RID: 149 RVA: 0x000051BB File Offset: 0x000033BB
	public static uint Hash32(string text, Encoding encoding)
	{
		return XXHash.Hash32(encoding.GetBytes(text));
	}

	// Token: 0x06000096 RID: 150 RVA: 0x000051CC File Offset: 0x000033CC
	public unsafe static uint Hash32(byte[] buffer)
	{
		byte* ptr;
		if (buffer == null || buffer.Length == 0)
		{
			ptr = null;
		}
		else
		{
			ptr = &buffer[0];
		}
		return XXHash.Hash32(ptr, buffer.Length, 0U);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x000051FC File Offset: 0x000033FC
	public unsafe static uint Hash32(byte* buffer, int bufferLength, uint seed = 0U)
	{
		int num = bufferLength;
		byte* ptr = buffer;
		uint num6;
		if (bufferLength >= 16)
		{
			uint num2 = seed + 2654435761U + 2246822519U;
			uint num3 = seed + 2246822519U;
			uint num4 = seed;
			uint num5 = seed - 2654435761U;
			do
			{
				num6 = XXHash.processStripe32(ref ptr, ref num2, ref num3, ref num4, ref num5);
				num -= 16;
			}
			while (num >= 16);
		}
		else
		{
			num6 = seed + 374761393U;
		}
		num6 += (uint)bufferLength;
		num6 = XXHash.processRemaining32(ptr, num6, num);
		return XXHash.avalanche32(num6);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0000526F File Offset: 0x0000346F
	public static ulong Hash64(string text)
	{
		return XXHash.Hash64(text, Encoding.UTF8);
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0000527C File Offset: 0x0000347C
	public static ulong Hash64(string text, Encoding encoding)
	{
		return XXHash.Hash64(encoding.GetBytes(text));
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0000528C File Offset: 0x0000348C
	public unsafe static ulong Hash64(byte[] buffer)
	{
		byte* ptr;
		if (buffer == null || buffer.Length == 0)
		{
			ptr = null;
		}
		else
		{
			ptr = &buffer[0];
		}
		return XXHash.Hash64(ptr, buffer.Length, 0UL);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000052BC File Offset: 0x000034BC
	public unsafe static ulong Hash64(byte* buffer, int bufferLength, ulong seed = 0UL)
	{
		int num = bufferLength;
		byte* ptr = buffer;
		ulong num6;
		if (bufferLength >= 32)
		{
			ulong num2 = seed + 11400714785074694791UL + 14029467366897019727UL;
			ulong num3 = seed + 14029467366897019727UL;
			ulong num4 = seed;
			ulong num5 = seed - 11400714785074694791UL;
			do
			{
				num6 = XXHash.processStripe64(ref ptr, ref num2, ref num3, ref num4, ref num5);
				num -= 32;
			}
			while (num >= 32);
		}
		else
		{
			num6 = seed + 2870177450012600261UL;
		}
		num6 += (ulong)((long)bufferLength);
		num6 = XXHash.processRemaining64(ptr, num6, num);
		return XXHash.avalanche64(num6);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00005344 File Offset: 0x00003544
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static ulong processStripe64(ref byte* pInput, ref ulong acc1, ref ulong acc2, ref ulong acc3, ref ulong acc4)
	{
		XXHash.processLane64(ref acc1, ref pInput);
		XXHash.processLane64(ref acc2, ref pInput);
		XXHash.processLane64(ref acc3, ref pInput);
		XXHash.processLane64(ref acc4, ref pInput);
		ulong num = XXHash.Bits.RotateLeft(acc1, 1) + XXHash.Bits.RotateLeft(acc2, 7) + XXHash.Bits.RotateLeft(acc3, 12) + XXHash.Bits.RotateLeft(acc4, 18);
		XXHash.mergeAccumulator64(ref num, acc1);
		XXHash.mergeAccumulator64(ref num, acc2);
		XXHash.mergeAccumulator64(ref num, acc3);
		XXHash.mergeAccumulator64(ref num, acc4);
		return num;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000053BC File Offset: 0x000035BC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void processLane64(ref ulong accn, ref byte* pInput)
	{
		ulong num = (ulong)(*pInput);
		accn = XXHash.round64(accn, num);
		pInput += 8;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000053E0 File Offset: 0x000035E0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static ulong processRemaining64(byte* pInput, ulong acc, int remainingLen)
	{
		while (remainingLen >= 8)
		{
			ulong num = (ulong)(*(long*)pInput);
			acc ^= XXHash.round64(0UL, num);
			acc = XXHash.Bits.RotateLeft(acc, 27) * 11400714785074694791UL;
			acc += 9650029242287828579UL;
			remainingLen -= 8;
			pInput += 8;
		}
		while (remainingLen >= 4)
		{
			uint num2 = *(uint*)pInput;
			acc ^= (ulong)num2 * 11400714785074694791UL;
			acc = XXHash.Bits.RotateLeft(acc, 23) * 14029467366897019727UL;
			acc += 1609587929392839161UL;
			remainingLen -= 4;
			pInput += 4;
		}
		while (remainingLen >= 1)
		{
			byte b = *pInput;
			acc ^= (ulong)b * 2870177450012600261UL;
			acc = XXHash.Bits.RotateLeft(acc, 11) * 11400714785074694791UL;
			remainingLen--;
			pInput++;
		}
		return acc;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000054A9 File Offset: 0x000036A9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong avalanche64(ulong acc)
	{
		acc ^= acc >> 33;
		acc *= 14029467366897019727UL;
		acc ^= acc >> 29;
		acc *= 1609587929392839161UL;
		acc ^= acc >> 32;
		return acc;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000054DE File Offset: 0x000036DE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong round64(ulong accn, ulong lane)
	{
		accn += lane * 14029467366897019727UL;
		return XXHash.Bits.RotateLeft(accn, 31) * 11400714785074694791UL;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00005501 File Offset: 0x00003701
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void mergeAccumulator64(ref ulong acc, ulong accn)
	{
		acc ^= XXHash.round64(0UL, accn);
		acc *= 11400714785074694791UL;
		acc += 9650029242287828579UL;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000552C File Offset: 0x0000372C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static uint processStripe32(ref byte* pInput, ref uint acc1, ref uint acc2, ref uint acc3, ref uint acc4)
	{
		XXHash.processLane32(ref pInput, ref acc1);
		XXHash.processLane32(ref pInput, ref acc2);
		XXHash.processLane32(ref pInput, ref acc3);
		XXHash.processLane32(ref pInput, ref acc4);
		return XXHash.Bits.RotateLeft(acc1, 1) + XXHash.Bits.RotateLeft(acc2, 7) + XXHash.Bits.RotateLeft(acc3, 12) + XXHash.Bits.RotateLeft(acc4, 18);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x0000557C File Offset: 0x0000377C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void processLane32(ref byte* pInput, ref uint accn)
	{
		uint num = *pInput;
		accn = XXHash.round32(accn, num);
		pInput += 4;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000055A0 File Offset: 0x000037A0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static uint processRemaining32(byte* pInput, uint acc, int remainingLen)
	{
		while (remainingLen >= 4)
		{
			uint num = *(uint*)pInput;
			acc += num * 3266489917U;
			acc = XXHash.Bits.RotateLeft(acc, 17) * 668265263U;
			remainingLen -= 4;
			pInput += 4;
		}
		while (remainingLen >= 1)
		{
			byte b = *pInput;
			acc += (uint)b * 374761393U;
			acc = XXHash.Bits.RotateLeft(acc, 11) * 2654435761U;
			remainingLen--;
			pInput++;
		}
		return acc;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000560A File Offset: 0x0000380A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint round32(uint accn, uint lane)
	{
		accn += lane * 2246822519U;
		accn = XXHash.Bits.RotateLeft(accn, 13);
		accn *= 2654435761U;
		return accn;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000562B File Offset: 0x0000382B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint avalanche32(uint acc)
	{
		acc ^= acc >> 15;
		acc *= 2246822519U;
		acc ^= acc >> 13;
		acc *= 3266489917U;
		acc ^= acc >> 16;
		return acc;
	}

	// Token: 0x04000186 RID: 390
	private const ulong k_Prime64v1 = 11400714785074694791UL;

	// Token: 0x04000187 RID: 391
	private const ulong k_Prime64v2 = 14029467366897019727UL;

	// Token: 0x04000188 RID: 392
	private const ulong k_Prime64v3 = 1609587929392839161UL;

	// Token: 0x04000189 RID: 393
	private const ulong k_Prime64v4 = 9650029242287828579UL;

	// Token: 0x0400018A RID: 394
	private const ulong k_Prime64v5 = 2870177450012600261UL;

	// Token: 0x0400018B RID: 395
	private const uint k_Prime32v1 = 2654435761U;

	// Token: 0x0400018C RID: 396
	private const uint k_Prime32v2 = 2246822519U;

	// Token: 0x0400018D RID: 397
	private const uint k_Prime32v3 = 3266489917U;

	// Token: 0x0400018E RID: 398
	private const uint k_Prime32v4 = 668265263U;

	// Token: 0x0400018F RID: 399
	private const uint k_Prime32v5 = 374761393U;

	// Token: 0x0200008A RID: 138
	private static class Bits
	{
		// Token: 0x06000279 RID: 633 RVA: 0x0001083E File Offset: 0x0000EA3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ulong RotateLeft(ulong value, int bits)
		{
			return (value << bits) | (value >> 64 - bits);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00010850 File Offset: 0x0000EA50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint RotateLeft(uint value, int bits)
		{
			return (value << bits) | (value >> 32 - bits);
		}
	}
}
