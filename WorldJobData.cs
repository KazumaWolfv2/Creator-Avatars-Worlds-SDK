using System;

// Token: 0x02000071 RID: 113
public class WorldJobData
{
	// Token: 0x060003F2 RID: 1010 RVA: 0x0001B329 File Offset: 0x00019529
	public WorldJobData(WorldData data, string status, int errorCode, string jobId, bool isOwnDetailInfo, string solution, string reason)
	{
		this.Data = data;
		this.Status = status;
		this.ErrorCode = errorCode;
		this.IsOwnDetailInfo = isOwnDetailInfo;
		this.Solution = solution;
		this.Reason = reason;
		this.JobId = jobId;
	}

	// Token: 0x04000222 RID: 546
	public WorldData Data;

	// Token: 0x04000223 RID: 547
	public string Status;

	// Token: 0x04000224 RID: 548
	public int ErrorCode;

	// Token: 0x04000225 RID: 549
	public string JobId;

	// Token: 0x04000226 RID: 550
	public bool IsOwnDetailInfo;

	// Token: 0x04000227 RID: 551
	public string Solution;

	// Token: 0x04000228 RID: 552
	public string Reason;
}
