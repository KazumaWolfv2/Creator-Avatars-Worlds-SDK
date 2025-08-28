using System;
using System.IO;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000064 RID: 100
	public static class Texture2DEx
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0000A69C File Offset: 0x0000889C
		public static byte[] EncodeToTGA(this Texture2D tex, Texture2DEx.Compression compression = Texture2DEx.Compression.RLE)
		{
			int num = ((tex.format == 5 || tex.format == 4) ? 4 : 3);
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream(18 + tex.width * tex.height * num))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write((compression == Texture2DEx.Compression.None) ? 2 : 10);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write((short)tex.width);
					binaryWriter.Write((short)tex.height);
					binaryWriter.Write((byte)(num * 8));
					binaryWriter.Write(8);
					Color32[] pixels = tex.GetPixels32();
					if (compression == Texture2DEx.Compression.None)
					{
						foreach (Color32 color in pixels)
						{
							binaryWriter.Write(color.r);
							binaryWriter.Write(color.g);
							binaryWriter.Write(color.b);
							if (num == 4)
							{
								binaryWriter.Write(color.a);
							}
						}
					}
					else
					{
						int k;
						for (int j = 0; j < pixels.Length; j = k)
						{
							Color32 color2 = pixels[j];
							bool flag = j != pixels.Length - 1 && Texture2DEx.Equals(pixels[j], pixels[j + 1]);
							int num2 = (j / tex.width + 1) * tex.width;
							int num3 = Mathf.Min(new int[]
							{
								j + 128,
								pixels.Length,
								num2
							});
							for (k = j + 1; k < num3; k++)
							{
								bool flag2 = Texture2DEx.Equals(pixels[k - 1], pixels[k]);
								if ((!flag && flag2) || (flag && !flag2))
								{
									break;
								}
							}
							int num4 = k - j;
							if (flag)
							{
								binaryWriter.Write((byte)((num4 - 1) | 128));
								binaryWriter.Write(color2.r);
								binaryWriter.Write(color2.g);
								binaryWriter.Write(color2.b);
								if (num == 4)
								{
									binaryWriter.Write(color2.a);
								}
							}
							else
							{
								binaryWriter.Write((byte)(num4 - 1));
								for (int l = j; l < k; l++)
								{
									Color32 color3 = pixels[l];
									binaryWriter.Write(color3.r);
									binaryWriter.Write(color3.g);
									binaryWriter.Write(color3.b);
									if (num == 4)
									{
										binaryWriter.Write(color3.a);
									}
								}
							}
						}
					}
					binaryWriter.Write(0);
					binaryWriter.Write(0);
					binaryWriter.Write(Texture2DEx.Footer);
				}
				array = memoryStream.ToArray();
			}
			return array;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000A990 File Offset: 0x00008B90
		private static bool Equals(Color32 first, Color32 second)
		{
			return first.r == second.r && first.g == second.g && first.b == second.b && first.a == second.a;
		}

		// Token: 0x0400021A RID: 538
		private static readonly byte[] Footer = new byte[]
		{
			84, 82, 85, 69, 86, 73, 83, 73, 79, 78,
			45, 88, 70, 73, 76, 69, 46, 0
		};

		// Token: 0x0200008F RID: 143
		public enum Compression
		{
			// Token: 0x0400032A RID: 810
			None,
			// Token: 0x0400032B RID: 811
			RLE
		}
	}
}
