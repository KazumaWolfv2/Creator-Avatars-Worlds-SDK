using System;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000079 RID: 121
	[Serializable]
	public class TextureOutput
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0000FFED File Offset: 0x0000E1ED
		public TextureOutput()
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00010024 File Offset: 0x0000E224
		public TextureOutput(bool a, string n, TextureScale s, bool sr, TextureChannels c, TextureCompression nc, ImageFormat i)
		{
			this.Active = a;
			this.Name = n;
			this.Scale = s;
			this.SRGB = sr;
			this.Channels = c;
			this.Compression = nc;
			this.ImageFormat = i;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0001009A File Offset: 0x0000E29A
		public TextureOutput Clone()
		{
			return (TextureOutput)base.MemberwiseClone();
		}

		// Token: 0x040002C3 RID: 707
		[SerializeField]
		public int Index = -1;

		// Token: 0x040002C4 RID: 708
		[SerializeField]
		public OverrideMask OverrideMask;

		// Token: 0x040002C5 RID: 709
		public bool Active = true;

		// Token: 0x040002C6 RID: 710
		public string Name = string.Empty;

		// Token: 0x040002C7 RID: 711
		public TextureScale Scale = TextureScale.Full;

		// Token: 0x040002C8 RID: 712
		public bool SRGB;

		// Token: 0x040002C9 RID: 713
		public TextureChannels Channels;

		// Token: 0x040002CA RID: 714
		public TextureCompression Compression = TextureCompression.Normal;

		// Token: 0x040002CB RID: 715
		public ImageFormat ImageFormat = ImageFormat.TGA;
	}
}
