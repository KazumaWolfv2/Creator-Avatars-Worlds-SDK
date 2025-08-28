using System;
using UnityEngine;

namespace ParaImpostors
{
	// Token: 0x02000063 RID: 99
	public static class SkinEx
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000A55C File Offset: 0x0000875C
		public static Bounds ComputeSkinBounds(SkinnedMeshRenderer renderer)
		{
			Bounds bounds = default(Bounds);
			if (renderer == null || renderer.sharedMesh == null)
			{
				return bounds;
			}
			Mesh mesh = new Mesh();
			renderer.BakeMesh(mesh);
			mesh.RecalculateBounds();
			return mesh.bounds;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A5A4 File Offset: 0x000087A4
		public static Bounds ComputeRendererBounds(Renderer renderer, Matrix4x4 rootWorldToLocal)
		{
			if (renderer == null)
			{
				return default(Bounds);
			}
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer == null)
			{
				if (!(renderer is MeshRenderer))
				{
					return default(Bounds);
				}
				MeshFilter component = renderer.gameObject.GetComponent<MeshFilter>();
				if (component == null || component.sharedMesh == null)
				{
					return default(Bounds);
				}
				return component.sharedMesh.bounds.Transform(rootWorldToLocal * renderer.localToWorldMatrix);
			}
			else
			{
				SkinnedMeshRenderer skinnedMeshRenderer2 = skinnedMeshRenderer;
				if (skinnedMeshRenderer2.sharedMesh == null)
				{
					return default(Bounds);
				}
				Mesh mesh = new Mesh();
				skinnedMeshRenderer2.BakeMesh(mesh);
				mesh.RecalculateBounds();
				Bounds bounds = mesh.bounds;
				Object.DestroyImmediate(mesh);
				Matrix4x4 matrix4x = Matrix4x4.TRS(skinnedMeshRenderer2.transform.localPosition, skinnedMeshRenderer2.transform.localRotation, Vector3.one);
				return bounds.Transform(rootWorldToLocal * matrix4x);
			}
		}
	}
}
