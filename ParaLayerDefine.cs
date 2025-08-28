using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class ParaLayerDefine
{
	// Token: 0x040000B8 RID: 184
	public static string ActorLayerName = "Actor";

	// Token: 0x040000B9 RID: 185
	public static string PhyActorLayerName = "PhyActor";

	// Token: 0x040000BA RID: 186
	public static string PropsLayerName = "Props";

	// Token: 0x040000BB RID: 187
	public static string StationLayerName = "Station";

	// Token: 0x040000BC RID: 188
	public static string MirrorLayerName = "Mirror";

	// Token: 0x040000BD RID: 189
	public static string CacheLayerName = "Cache";

	// Token: 0x040000BE RID: 190
	public static string RTLayerName = "RT";

	// Token: 0x040000BF RID: 191
	public static string PortalLayerName = "Portal";

	// Token: 0x040000C0 RID: 192
	public static string InvisibleWallName = "InvisibleWall";

	// Token: 0x040000C1 RID: 193
	public static string UILayerName = "UI";

	// Token: 0x040000C2 RID: 194
	public static int ActorLayer = LayerMask.NameToLayer(ParaLayerDefine.ActorLayerName);

	// Token: 0x040000C3 RID: 195
	public static int PhyActorLayer = LayerMask.NameToLayer(ParaLayerDefine.PhyActorLayerName);

	// Token: 0x040000C4 RID: 196
	public static int PropsLayer = LayerMask.NameToLayer(ParaLayerDefine.PropsLayerName);

	// Token: 0x040000C5 RID: 197
	public static int StationLayer = LayerMask.NameToLayer(ParaLayerDefine.StationLayerName);

	// Token: 0x040000C6 RID: 198
	public static int MirrorLayer = LayerMask.NameToLayer(ParaLayerDefine.MirrorLayerName);

	// Token: 0x040000C7 RID: 199
	public static int CacheLayer = LayerMask.NameToLayer(ParaLayerDefine.CacheLayerName);

	// Token: 0x040000C8 RID: 200
	public static int RTLayer = LayerMask.NameToLayer(ParaLayerDefine.RTLayerName);

	// Token: 0x040000C9 RID: 201
	public static int PortalLayer = LayerMask.NameToLayer(ParaLayerDefine.PortalLayerName);

	// Token: 0x040000CA RID: 202
	public static int InvisibleWallLayer = LayerMask.NameToLayer(ParaLayerDefine.InvisibleWallName);

	// Token: 0x040000CB RID: 203
	public static int UILayer = LayerMask.NameToLayer(ParaLayerDefine.UILayerName);

	// Token: 0x040000CC RID: 204
	public static LayerMask ActorLayerMask = ParaLayerDefine.ActorLayer;

	// Token: 0x040000CD RID: 205
	public static LayerMask PhyActorLayerMask = ParaLayerDefine.PhyActorLayer;

	// Token: 0x040000CE RID: 206
	public static LayerMask PropsLayerMask = ParaLayerDefine.PropsLayer;

	// Token: 0x040000CF RID: 207
	public static LayerMask StationLayerMask = ParaLayerDefine.StationLayer;

	// Token: 0x040000D0 RID: 208
	public static LayerMask CacheLayerMask = ParaLayerDefine.CacheLayer;

	// Token: 0x040000D1 RID: 209
	public static LayerMask RTLayerMask = ParaLayerDefine.RTLayer;

	// Token: 0x040000D2 RID: 210
	public static LayerMask PortalLayerMask = ParaLayerDefine.PortalLayer;

	// Token: 0x040000D3 RID: 211
	public static LayerMask InvisibleWallMask = ParaLayerDefine.InvisibleWallLayer;
}
