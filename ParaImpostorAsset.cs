using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000072 RID: 114
	[CreateAssetMenu(fileName = "New Impostor", menuName = "ParaSpace/Impostor Asset", order = 85)]
	public class ParaImpostorAsset : ScriptableObject
	{
		// Token: 0x04000296 RID: 662
		[SerializeField]
		public Material Material;

		// Token: 0x04000297 RID: 663
		[SerializeField]
		public Mesh Mesh;

		// Token: 0x04000298 RID: 664
		[HideInInspector]
		[SerializeField]
		public int Version;

		// Token: 0x04000299 RID: 665
		[SerializeField]
		public ImpostorType ImpostorType = ImpostorType.Octahedron;

		// Token: 0x0400029A RID: 666
		[HideInInspector]
		[SerializeField]
		public bool LockedSizes = true;

		// Token: 0x0400029B RID: 667
		[HideInInspector]
		[SerializeField]
		public int SelectedSize = 2048;

		// Token: 0x0400029C RID: 668
		[SerializeField]
		public Vector2 TexSize = new Vector2(2048f, 2048f);

		// Token: 0x0400029D RID: 669
		[HideInInspector]
		[SerializeField]
		public bool DecoupleAxisFrames;

		// Token: 0x0400029E RID: 670
		[SerializeField]
		[Range(1f, 32f)]
		public int HorizontalFrames = 16;

		// Token: 0x0400029F RID: 671
		[SerializeField]
		[Range(1f, 33f)]
		public int VerticalFrames = 16;

		// Token: 0x040002A0 RID: 672
		[SerializeField]
		[Range(0f, 64f)]
		public int PixelPadding = 32;

		// Token: 0x040002A1 RID: 673
		[SerializeField]
		[Range(4f, 16f)]
		public int MaxVertices = 8;

		// Token: 0x040002A2 RID: 674
		[SerializeField]
		[Range(0f, 0.2f)]
		public float Tolerance = 0.15f;

		// Token: 0x040002A3 RID: 675
		[SerializeField]
		[Range(0f, 1f)]
		public float NormalScale = 0.01f;

		// Token: 0x040002A4 RID: 676
		[SerializeField]
		public Vector2[] ShapePoints = new Vector2[]
		{
			new Vector2(0.15f, 0f),
			new Vector2(0.85f, 0f),
			new Vector2(1f, 0.15f),
			new Vector2(1f, 0.85f),
			new Vector2(0.85f, 1f),
			new Vector2(0.15f, 1f),
			new Vector2(0f, 0.85f),
			new Vector2(0f, 0.15f)
		};

		// Token: 0x040002A5 RID: 677
		[SerializeField]
		public ParaImpostorBakePreset Preset;

		// Token: 0x040002A6 RID: 678
		[SerializeField]
		public List<TextureOutput> OverrideOutput = new List<TextureOutput>();
	}
}
