using System;
using System.Collections.Generic;

// Token: 0x02000051 RID: 81
public class ParaLog
{
	// Token: 0x06000278 RID: 632 RVA: 0x0000FE7A File Offset: 0x0000E07A
	public static void Init(byte type)
	{
		ParaLog.SetType(type);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000FE84 File Offset: 0x0000E084
	public static void SetType(byte type)
	{
		ParaLog._Type = type;
		foreach (object obj in Enum.GetValues(typeof(ParaLog.ParaLogType)))
		{
			ParaLog.ParaLogType paraLogType = (ParaLog.ParaLogType)obj;
			ParaLog._LogType[paraLogType] = (type & (byte)paraLogType) > 0;
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
	private static bool IsShow(ParaLog.ParaLogType type)
	{
		return ParaLog._LogType.ContainsKey(type) && ParaLog._LogType[type];
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00003394 File Offset: 0x00001594
	public static void Log(object message)
	{
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00003394 File Offset: 0x00001594
	public static void LogFormat(string format, params object[] args)
	{
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00003394 File Offset: 0x00001594
	public static void LogError(object message)
	{
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00003394 File Offset: 0x00001594
	public static void LogErrorFormat(string format, params object[] args)
	{
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00003394 File Offset: 0x00001594
	public static void LogWarning(object message)
	{
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00003394 File Offset: 0x00001594
	public static void LogWarningFormat(string format, params object[] args)
	{
	}

	// Token: 0x0400016D RID: 365
	private static byte _Type = 4;

	// Token: 0x0400016E RID: 366
	private static Dictionary<ParaLog.ParaLogType, bool> _LogType = new Dictionary<ParaLog.ParaLogType, bool>();

	// Token: 0x020000AC RID: 172
	public enum ParaLogType
	{
		// Token: 0x04000382 RID: 898
		kLog = 1,
		// Token: 0x04000383 RID: 899
		kWarning,
		// Token: 0x04000384 RID: 900
		kError = 4
	}
}
