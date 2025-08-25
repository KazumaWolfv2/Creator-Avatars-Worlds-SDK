using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

// Token: 0x02000059 RID: 89
public class MD4 : HashAlgorithm
{
	// Token: 0x0600029F RID: 671 RVA: 0x00010C4A File Offset: 0x0000EE4A
	public MD4()
	{
		this._x = new uint[16];
		this.Initialize();
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00010C65 File Offset: 0x0000EE65
	public override void Initialize()
	{
		this._a = 1732584193U;
		this._b = 4023233417U;
		this._c = 2562383102U;
		this._d = 271733878U;
		this._bytesProcessed = 0;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00010C9A File Offset: 0x0000EE9A
	protected override void HashCore(byte[] array, int offset, int length)
	{
		this.ProcessMessage(MD4.Bytes(array, offset, length));
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00010CAC File Offset: 0x0000EEAC
	protected override byte[] HashFinal()
	{
		byte[] array;
		try
		{
			this.ProcessMessage(this.Padding());
			array = new uint[] { this._a, this._b, this._c, this._d }.SelectMany((uint word) => this.Bytes(word)).ToArray<byte>();
		}
		finally
		{
			this.Initialize();
		}
		return array;
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00010D20 File Offset: 0x0000EF20
	private void ProcessMessage(IEnumerable<byte> bytes)
	{
		foreach (byte b in bytes)
		{
			int num = this._bytesProcessed & 63;
			int num2 = num >> 2;
			int num3 = (num & 3) << 3;
			this._x[num2] = (this._x[num2] & ~(255U << num3)) | (uint)((uint)b << num3);
			if (num == 63)
			{
				this.Process16WordBlock();
			}
			this._bytesProcessed++;
		}
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00010DB0 File Offset: 0x0000EFB0
	private static IEnumerable<byte> Bytes(byte[] bytes, int offset, int length)
	{
		int num;
		for (int i = offset; i < length; i = num + 1)
		{
			yield return bytes[i];
			num = i;
		}
		yield break;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00010DCE File Offset: 0x0000EFCE
	private IEnumerable<byte> Bytes(uint word)
	{
		yield return (byte)(word & 255U);
		yield return (byte)((word >> 8) & 255U);
		yield return (byte)((word >> 16) & 255U);
		yield return (byte)((word >> 24) & 255U);
		yield break;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00010DDE File Offset: 0x0000EFDE
	private IEnumerable<byte> Repeat(byte value, int count)
	{
		int num;
		for (int i = 0; i < count; i = num + 1)
		{
			yield return value;
			num = i;
		}
		yield break;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00010DF8 File Offset: 0x0000EFF8
	private IEnumerable<byte> Padding()
	{
		return this.Repeat(128, 1).Concat(this.Repeat(0, ((this._bytesProcessed + 8) & 2147483584) + 55 - this._bytesProcessed)).Concat(this.Bytes((uint)((uint)this._bytesProcessed << 3)))
			.Concat(this.Repeat(0, 4));
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00010E58 File Offset: 0x0000F058
	private void Process16WordBlock()
	{
		uint num = this._a;
		uint num2 = this._b;
		uint num3 = this._c;
		uint num4 = this._d;
		foreach (int num5 in new int[] { 0, 4, 8, 12 })
		{
			num = MD4.Round1Operation(num, num2, num3, num4, this._x[num5], 3);
			num4 = MD4.Round1Operation(num4, num, num2, num3, this._x[num5 + 1], 7);
			num3 = MD4.Round1Operation(num3, num4, num, num2, this._x[num5 + 2], 11);
			num2 = MD4.Round1Operation(num2, num3, num4, num, this._x[num5 + 3], 19);
		}
		foreach (int num6 in new int[] { 0, 1, 2, 3 })
		{
			num = MD4.Round2Operation(num, num2, num3, num4, this._x[num6], 3);
			num4 = MD4.Round2Operation(num4, num, num2, num3, this._x[num6 + 4], 5);
			num3 = MD4.Round2Operation(num3, num4, num, num2, this._x[num6 + 8], 9);
			num2 = MD4.Round2Operation(num2, num3, num4, num, this._x[num6 + 12], 13);
		}
		foreach (int num7 in new int[] { 0, 2, 1, 3 })
		{
			num = MD4.Round3Operation(num, num2, num3, num4, this._x[num7], 3);
			num4 = MD4.Round3Operation(num4, num, num2, num3, this._x[num7 + 8], 9);
			num3 = MD4.Round3Operation(num3, num4, num, num2, this._x[num7 + 4], 11);
			num2 = MD4.Round3Operation(num2, num3, num4, num, this._x[num7 + 12], 15);
		}
		this._a += num;
		this._b += num2;
		this._c += num3;
		this._d += num4;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0001104B File Offset: 0x0000F24B
	private static uint ROL(uint value, int numberOfBits)
	{
		return (value << numberOfBits) | (value >> 32 - numberOfBits);
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0001105D File Offset: 0x0000F25D
	private static uint Round1Operation(uint a, uint b, uint c, uint d, uint xk, int s)
	{
		return MD4.ROL(a + ((b & c) | (~b & d)) + xk, s);
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00011073 File Offset: 0x0000F273
	private static uint Round2Operation(uint a, uint b, uint c, uint d, uint xk, int s)
	{
		return MD4.ROL(a + ((b & c) | (b & d) | (c & d)) + xk + 1518500249U, s);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00011092 File Offset: 0x0000F292
	private static uint Round3Operation(uint a, uint b, uint c, uint d, uint xk, int s)
	{
		return MD4.ROL(a + (b ^ c ^ d) + xk + 1859775393U, s);
	}

	// Token: 0x04000176 RID: 374
	private uint _a;

	// Token: 0x04000177 RID: 375
	private uint _b;

	// Token: 0x04000178 RID: 376
	private uint _c;

	// Token: 0x04000179 RID: 377
	private uint _d;

	// Token: 0x0400017A RID: 378
	private uint[] _x;

	// Token: 0x0400017B RID: 379
	private int _bytesProcessed;
}
