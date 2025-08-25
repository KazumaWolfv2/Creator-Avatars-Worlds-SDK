using System;

// Token: 0x0200006F RID: 111
public class AvatarJobData
{
	// Token: 0x060003ED RID: 1005 RVA: 0x0001B299 File Offset: 0x00019499
	public AvatarJobData(AvatarData data, string status, int errorCode, string jobId, bool isOwnDetailInfo, string solution, string reason)
	{
		this.Data = data;
		this.Status = status;
		this.ErrorCode = errorCode;
		this.IsOwnDetailInfo = isOwnDetailInfo;
		this.Solution = solution;
		this.Reason = reason;
		this.JobId = jobId;
	}

	// Token: 0x04000219 RID: 537
	public AvatarData Data;

	// Token: 0x0400021A RID: 538
	public string Status;

	// Token: 0x0400021B RID: 539
	public int ErrorCode;

	// Token: 0x0400021C RID: 540
	public bool IsOwnDetailInfo;

	// Token: 0x0400021D RID: 541
	public string Solution;

	// Token: 0x0400021E RID: 542
	public string Reason;

	// Token: 0x0400021F RID: 543
	public string JobId;
}
