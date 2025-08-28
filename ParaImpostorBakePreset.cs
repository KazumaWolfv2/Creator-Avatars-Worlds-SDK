using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x0200007A RID: 122
	[CreateAssetMenu(fileName = "New Bake Preset", menuName = "ParaSpace/Impostor Bake Preset", order = 86)]
	public class ParaImpostorBakePreset : ScriptableObject
	{
		// Token: 0x040002CC RID: 716
		[SerializeField]
		public Shader BakeShader;

		// Token: 0x040002CD RID: 717
		[SerializeField]
		public Shader RuntimeShader;

		// Token: 0x040002CE RID: 718
		[SerializeField]
		public int AlphaIndex;

		// Token: 0x040002CF RID: 719
		[SerializeField]
		public List<TextureOutput> Output = new List<TextureOutput>();
	}
}
