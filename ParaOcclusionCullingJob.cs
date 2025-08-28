using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

// Token: 0x0200002D RID: 45
public struct ParaOcclusionCullingJob : IJob
{
	// Token: 0x06000065 RID: 101
	[DllImport("libSDOC.qti.dll")]
	private unsafe static extern bool sdocStartNewFrame(float* viewPos, float* viewDir, float* viewProj);

	// Token: 0x06000066 RID: 102
	[DllImport("libSDOC.qti.dll")]
	private unsafe static extern void sdocRenderOccluder(float* vertices, ushort* indices, uint nVert, uint nIdx, float* modelAABB, float* localToWorld);

	// Token: 0x06000067 RID: 103
	[DllImport("libSDOC.qti.dll")]
	private unsafe static extern bool sdocQueryOccludees(float* bbox, uint nMesh, byte* results);

	// Token: 0x06000068 RID: 104 RVA: 0x0000432C File Offset: 0x0000252C
	public unsafe void Execute()
	{
		void* unsafeReadOnlyPtr = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.viewPos);
		void* unsafeReadOnlyPtr2 = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.viewDir);
		void* unsafeReadOnlyPtr3 = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.viewProj);
		void* unsafeReadOnlyPtr4 = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.vertices);
		void* unsafeReadOnlyPtr5 = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<ushort>(this.indices);
		void* unsafeReadOnlyPtr6 = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.modelAABB);
		void* unsafeReadOnlyPtr7 = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.localToWorld);
		float* unsafeReadOnlyPtr8 = (float*)NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<float>(this.bbox);
		void* unsafePtr = NativeArrayUnsafeUtility.GetUnsafePtr<byte>(this.results);
		if (!ParaOcclusionCullingJob.sdocStartNewFrame((float*)unsafeReadOnlyPtr, (float*)unsafeReadOnlyPtr2, (float*)unsafeReadOnlyPtr3))
		{
			Debug.LogError("SDOC sdocStartNewFrame failed.");
		}
		ParaOcclusionCullingJob.sdocRenderOccluder((float*)unsafeReadOnlyPtr4, (ushort*)unsafeReadOnlyPtr5, this.nVert, this.nIdx, (float*)unsafeReadOnlyPtr6, (float*)unsafeReadOnlyPtr7);
		if (!ParaOcclusionCullingJob.sdocQueryOccludees(unsafeReadOnlyPtr8, this.nMesh, (byte*)unsafePtr))
		{
			Debug.LogError("SDOC sdocQueryOccludees failed.");
		}
	}

	// Token: 0x04000106 RID: 262
	public const string libSDOC = "libSDOC.qti.dll";

	// Token: 0x04000107 RID: 263
	[ReadOnly]
	public NativeArray<float> viewPos;

	// Token: 0x04000108 RID: 264
	[ReadOnly]
	public NativeArray<float> viewDir;

	// Token: 0x04000109 RID: 265
	[ReadOnly]
	public NativeArray<float> viewProj;

	// Token: 0x0400010A RID: 266
	[ReadOnly]
	public NativeArray<float> vertices;

	// Token: 0x0400010B RID: 267
	[ReadOnly]
	public NativeArray<ushort> indices;

	// Token: 0x0400010C RID: 268
	public uint nVert;

	// Token: 0x0400010D RID: 269
	public uint nIdx;

	// Token: 0x0400010E RID: 270
	[ReadOnly]
	public NativeArray<float> modelAABB;

	// Token: 0x0400010F RID: 271
	[ReadOnly]
	public NativeArray<float> localToWorld;

	// Token: 0x04000110 RID: 272
	[ReadOnly]
	public NativeArray<float> bbox;

	// Token: 0x04000111 RID: 273
	public uint nMesh;

	// Token: 0x04000112 RID: 274
	public NativeArray<byte> results;
}
