using System;
using System.Collections.Generic;

// Token: 0x02000073 RID: 115
public class WorldData
{
	// Token: 0x060003F3 RID: 1011 RVA: 0x0001B368 File Offset: 0x00019568
	public WorldData(string name, string contentID, bool isTakeDown, string des, int capacity, bool isPublic, List<string> tags, string remoteTexturePath, string textureUri, string remoteIconPath, string iconUri, string createTime, string updateTime)
	{
		this.Name = name;
		this.ContentID = contentID;
		this.IsTakeDown = isTakeDown;
		this.Description = des;
		this.RemoteTexturePath = remoteTexturePath;
		this.RemoteIconPath = remoteIconPath;
		this.TexturePath = textureUri;
		this.IconPath = iconUri;
		this.Capacity = capacity;
		this.HashTags = tags;
		this.IsPublic = isPublic;
		this.CreatTime = EditorUtil.TransformTimeStamp(createTime);
		this.UpdateTime = EditorUtil.TransformTimeStamp(updateTime);
		this.IsShow = false;
	}

	// Token: 0x0400022E RID: 558
	public string Name;

	// Token: 0x0400022F RID: 559
	public string ContentID;

	// Token: 0x04000230 RID: 560
	public bool IsTakeDown;

	// Token: 0x04000231 RID: 561
	public string TexturePath;

	// Token: 0x04000232 RID: 562
	public string IconPath;

	// Token: 0x04000233 RID: 563
	public string RemoteTexturePath;

	// Token: 0x04000234 RID: 564
	public string RemoteIconPath;

	// Token: 0x04000235 RID: 565
	public int Capacity;

	// Token: 0x04000236 RID: 566
	public string CreatTime;

	// Token: 0x04000237 RID: 567
	public string UpdateTime;

	// Token: 0x04000238 RID: 568
	public List<string> HashTags;

	// Token: 0x04000239 RID: 569
	public string Description;

	// Token: 0x0400023A RID: 570
	public bool IsPublic;

	// Token: 0x0400023B RID: 571
	public bool IsShow;
}
