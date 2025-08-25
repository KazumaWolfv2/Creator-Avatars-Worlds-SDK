using System;
using System.Diagnostics;
using System.Text;

// Token: 0x0200004E RID: 78
public static class HttpUtil
{
	// Token: 0x06000260 RID: 608 RVA: 0x0000FA64 File Offset: 0x0000DC64
	public static string UrlEncode(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		int num = 0;
		int num2 = 0;
		foreach (char c in value)
		{
			if (HttpUtil.IsUrlSafeChar(c))
			{
				num++;
			}
			else if (c == ' ')
			{
				num2++;
			}
		}
		int num3 = num + num2;
		if (num3 != value.Length)
		{
			int byteCount = Encoding.UTF8.GetByteCount(value);
			int num4 = (byteCount - num3) * 2;
			byte[] array = new byte[byteCount + num4];
			Encoding.UTF8.GetBytes(value, 0, value.Length, array, num4);
			HttpUtil.GetEncodedBytes(array, num4, byteCount, array);
			return Encoding.UTF8.GetString(array);
		}
		if (num2 != 0)
		{
			return value.Replace(' ', '+');
		}
		return value;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000FB24 File Offset: 0x0000DD24
	private static bool IsUrlSafeChar(char ch)
	{
		return HttpUtil.IsAsciiLetter(ch) || (ch - ' ' <= '\u0019' && ((1 << (int)(ch - ' ')) & 67069698) != 0) || ch == '_';
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000FB5C File Offset: 0x0000DD5C
	private static void GetEncodedBytes(byte[] originalBytes, int offset, int count, byte[] expandedBytes)
	{
		int num = 0;
		int num2 = offset + count;
		Debug.Assert(offset < num2 && num2 <= originalBytes.Length);
		for (int i = offset; i < num2; i++)
		{
			if (originalBytes == expandedBytes)
			{
				Debug.Assert(i >= num);
			}
			byte b = originalBytes[i];
			char c = (char)b;
			if (HttpUtil.IsUrlSafeChar(c))
			{
				expandedBytes[num++] = b;
			}
			else if (c == ' ')
			{
				expandedBytes[num++] = 43;
			}
			else
			{
				expandedBytes[num++] = 37;
				expandedBytes[num++] = (byte)HexConverter.ToCharUpper(b >> 4);
				expandedBytes[num++] = (byte)HexConverter.ToCharUpper((int)b);
			}
		}
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000FBF2 File Offset: 0x0000DDF2
	public static bool IsAsciiLetter(char c)
	{
		return (c | ' ') - 'a' <= '\u0019';
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000FC02 File Offset: 0x0000DE02
	public static bool HasSettedServerTimestamp()
	{
		return HttpUtil._serverTimestamp > 0L;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000FC0D File Offset: 0x0000DE0D
	public static void SetServerTimestamp(long serverTimestamp)
	{
		HttpUtil._serverTimestamp = serverTimestamp;
		HttpUtil._timestampDiffWithServer = serverTimestamp - HttpUtil.GetCurrentTimestamp();
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000FC21 File Offset: 0x0000DE21
	public static long GetServerCurrentTimestamp()
	{
		return HttpUtil.GetCurrentTimestamp() + HttpUtil._timestampDiffWithServer;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000FC30 File Offset: 0x0000DE30
	public static long GetCurrentTimestamp()
	{
		return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	// Token: 0x04000169 RID: 361
	private static long _timestampDiffWithServer;

	// Token: 0x0400016A RID: 362
	private static long _serverTimestamp;
}
