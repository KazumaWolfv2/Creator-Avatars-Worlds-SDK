using System;
using System.Reflection;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000065 RID: 101
	public static class SpriteUtilityEx
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A9E5 File Offset: 0x00008BE5
		public static Type Type
		{
			get
			{
				if (!(SpriteUtilityEx.type == null))
				{
					return SpriteUtilityEx.type;
				}
				return SpriteUtilityEx.type = Type.GetType("UnityEditor.Sprites.SpriteUtility, UnityEditor");
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000AA0C File Offset: 0x00008C0C
		public static void GenerateOutline(Texture2D texture, Rect rect, float detail, byte alphaTolerance, bool holeDetection, out Vector2[][] paths)
		{
			Vector2[][] array = new Vector2[0][];
			object[] array2 = new object[] { texture, rect, detail, alphaTolerance, holeDetection, array };
			SpriteUtilityEx.Type.GetMethod("GenerateOutline", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, array2);
			paths = (Vector2[][])array2[5];
		}

		// Token: 0x0400021B RID: 539
		private static Type type;
	}
}
