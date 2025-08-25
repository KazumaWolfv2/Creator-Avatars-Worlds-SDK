using System;

// Token: 0x02000061 RID: 97
public class AssetFileInfo
{
	// Token: 0x060002D3 RID: 723 RVA: 0x00011EAC File Offset: 0x000100AC
	public AssetFileInfo(string path, string realPath, string guid)
	{
		this.Path = path;
		this.RealPath = realPath;
		this.Guid = guid;
	}

	// Token: 0x04000182 RID: 386
	public string Path;

	// Token: 0x04000183 RID: 387
	public string RealPath;

	// Token: 0x04000184 RID: 388
	public string Guid;
}
