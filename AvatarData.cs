using System;
using System.Collections.Generic;

// Token: 0x0200006E RID: 110
public class AvatarData
{
	// Token: 0x060003EC RID: 1004 RVA: 0x0001B220 File Offset: 0x00019420
	public AvatarData(string name, string contentID, bool isTakeDown, string des, List<string> tags, int sharingStauts, List<int> sharingChannel, string remoteTexturePath, string textureUri, string createTime, string updateTime)
	{
		this.Name = name;
		this.SharingStauts = sharingStauts;
		this.ContentID = contentID;
		this.IsTakeDown = isTakeDown;
		this.Description = des;
		this.HashTags = tags;
		this.SharingChannel = sharingChannel;
		this.RemoteTexturePath = remoteTexturePath;
		this.TexturePath = textureUri;
		this.CreatTime = EditorUtil.TransformTimeStamp(createTime);
		this.UpdateTime = EditorUtil.TransformTimeStamp(updateTime);
		this.IsShow = false;
	}

	// Token: 0x0400020D RID: 525
	public string Name;

	// Token: 0x0400020E RID: 526
	public string ContentID;

	// Token: 0x0400020F RID: 527
	public bool IsTakeDown;

	// Token: 0x04000210 RID: 528
	public List<string> HashTags;

	// Token: 0x04000211 RID: 529
	public int SharingStauts;

	// Token: 0x04000212 RID: 530
	public List<int> SharingChannel;

	// Token: 0x04000213 RID: 531
	public string RemoteTexturePath;

	// Token: 0x04000214 RID: 532
	public string TexturePath;

	// Token: 0x04000215 RID: 533
	public string CreatTime;

	// Token: 0x04000216 RID: 534
	public string UpdateTime;

	// Token: 0x04000217 RID: 535
	public bool IsShow;

	// Token: 0x04000218 RID: 536
	public string Description;
}
