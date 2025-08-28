using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
[SerializeField]
public class ParaOccludee : MonoBehaviour
{
	// Token: 0x0600004A RID: 74 RVA: 0x00003728 File Offset: 0x00001928
	public void Start()
	{
		Renderer renderer = base.GetComponent<MeshRenderer>();
		if (renderer == null)
		{
			renderer = base.GetComponent<SkinnedMeshRenderer>();
		}
		if (renderer == null || !renderer.enabled)
		{
			return;
		}
		this.Occludee = Singleton<ParaOcclusionCullingManager>.instance.AddObject(renderer, this.staticOccluee) > -1;
		if (this.staticOccluee && this.isOpaque())
		{
			base.gameObject.SetLayer(LayerMask.NameToLayer("SceneStatic"));
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000037A0 File Offset: 0x000019A0
	private bool isOpaque()
	{
		Renderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return false;
		}
		int num = 2450;
		Material[] sharedMaterials = component.sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			if (ParaOccludee.GetShaderRenderQueue(sharedMaterials[i]) > num)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000037E8 File Offset: 0x000019E8
	private static int GetShaderRenderQueue(Material material)
	{
		if (material == null)
		{
			return 3000;
		}
		string tag = material.GetTag("Queue", true, "Geometry");
		if (tag.StartsWith("Transparent") || tag.StartsWith("Overlay"))
		{
			return 3000;
		}
		return 2000;
	}

	// Token: 0x040000D6 RID: 214
	[SerializeField]
	public bool staticOccluee;

	// Token: 0x040000D7 RID: 215
	public bool Occludee;
}
