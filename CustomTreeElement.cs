using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[Serializable]
public class CustomTreeElement : TreeElement
{
	// Token: 0x060001BC RID: 444 RVA: 0x0000BC01 File Offset: 0x00009E01
	public CustomTreeElement(string name, int depth, int id)
		: base(name, depth, id)
	{
		this.enabled = true;
	}

	// Token: 0x0400011D RID: 285
	public Material material;

	// Token: 0x0400011E RID: 286
	public GameObject refObject;

	// Token: 0x0400011F RID: 287
	public Mesh mesh;

	// Token: 0x04000120 RID: 288
	public Texture texture;

	// Token: 0x04000121 RID: 289
	public string text = "";

	// Token: 0x04000122 RID: 290
	public bool enabled;

	// Token: 0x04000123 RID: 291
	public long Size;
}
